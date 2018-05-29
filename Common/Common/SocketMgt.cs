using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Common
{
    public delegate void ServerReceiveDataEventHandler(byte[] receivedData);

    public delegate string ServerProcessDataEventHandler(byte[] receivedData);

    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024; //缓存大小默认1024
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    #region Socket监听服务

    public class SocketServer
    {
        #region data

        public event ServerProcessDataEventHandler ServerProcessDataEventHandler;
        private Socket serverSocket = null;
        private int port = 0;
        private string ipAddress = "";
        private string logPath = "";

        #endregion

        #region constructor
        /// <summary>
        /// 监听服务构造器
        /// </summary>
        /// <param name="ip">监听某个终结点，若为空监控网络上所有终结点</param>
        /// <param name="port"></param>
        /// <param name="logpath">监听服务日志路径</param>
        public SocketServer(string ip, int port, string logpath)
        {
            this.port = port;
            this.ipAddress = ip;
            this.logPath = logpath;
        }
        public void Dispose()
        {
            serverSocket.Close();
            serverSocket.Dispose();

            Utility.RecordLog("AnyscSocketServer stop listening...", this.logPath);
        }

        #endregion

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="maxConnection"></param>
        public void StartListener(int maxConnection)
        {
            Utility.RecordLog("SocketServer start listening...", this.logPath);

            Thread thrClient = null;
            IPHostEntry hostEntry = null;
            // Get host related information.         
            hostEntry = Dns.GetHostEntry(this.ipAddress);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid         
            // an exception that occurs when the host IP Address is not compatible with the address family         
            // (typical in the IPv6 case).         
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, this.port);
                TcpListener listener = new TcpListener(ipe);
                //listener.Start();
                listener.Start(maxConnection);

                while (true)
                {
                    try
                    {
                        this.serverSocket = listener.AcceptSocket();
                        thrClient = new Thread(new ThreadStart(ProcessData));
                        thrClient.Start();
                    }
                    catch (Exception ex)
                    {
                        Utility.RecordLog("SocketServer occur exception, restart listening...。exception cause:" + ex.Message, this.logPath);
                        this.serverSocket.Close();
                        thrClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        private void ProcessData()
        {
            //处理数据
            string receiveData = string.Empty;
            string resData = string.Empty;

            Byte[] bytesReceived = new Byte[1024];
            string receBuf = string.Empty;
            int resCnt = this.serverSocket.Receive(bytesReceived);

            if(resCnt > 0)
            {
                receiveData = System.Text.Encoding.UTF8.GetString(bytesReceived);
                if (this.ServerProcessDataEventHandler != null)
                {
                    resData = this.ServerProcessDataEventHandler(bytesReceived);
                }

                if (string.IsNullOrEmpty(resData))
                {
                    //将最终的信息通过socket发到客户端
                    int tmp = this.serverSocket.Send(Encoding.UTF8.GetBytes(resData));
                }                
            }            
        }
    }

    #endregion


    #region 异步Socket监听服务

    public class AsyncSocketServer : IDisposable
    {
        #region data

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static List<StateObject> localClientList = new List<StateObject>();
        public static List<StateObject> remoteClientList = new List<StateObject>();
        public event ServerReceiveDataEventHandler ServerReceiveDataEventHandler;

        private Socket serverSocket = null;
        private SocketError errorCode;
        private int port = 0;
        private string ipAddress = "";
        private string logPath = "";
        private bool isDisposing = false;

        private List<string> stringRemoteReaderIpCollection = new List<string>();

        #endregion

        #region constructor
        /// <summary>
        /// 异步构造器
        /// </summary>
        /// <param name="ip">监听某个终结点，若为空监控网络上所有终结点</param>
        /// <param name="port"></param>
        /// <param name="logpath">监听服务日志路径</param>
        /// <param name="stringClientIpCollection">监听的客户端ip集合</param>
        public AsyncSocketServer(string ip, int port, string logpath, List<string> stringClientIpCollection)
        {
            this.port = port;
            this.ipAddress = ip;
            this.logPath = logpath;
            this.stringRemoteReaderIpCollection = stringClientIpCollection;
        }
        public void Dispose()
        {
            isDisposing = true;
            serverSocket.Close();
            serverSocket.Dispose();

            Utility.RecordLog("AnyscSocketServer stop listening...", this.logPath);
        }

        #endregion

        #region public Method

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="maxCnt">挂起监听数</param>
        public void StartListening(int maxCnt)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (string.IsNullOrEmpty(this.ipAddress))
                {
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                }
                else
                {
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(this.ipAddress), port);
                    serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                }
                serverSocket.Listen(maxCnt);

                Utility.RecordLog("AnyscSocketServer start listening...", this.logPath);

                while (true)
                {
                    allDone.Reset();
                    if (!isDisposing)
                    {
                        serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);                        
                    }
                    allDone.WaitOne();
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
                throw;
            }
        }

        public void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            try
            {
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
                throw;
            }
        }

        public void Send(Socket handler, byte[] byteData)
        {
            try
            {
                handler.BeginSendTo(byteData, 0, byteData.Length, 0, handler.RemoteEndPoint, new AsyncCallback(SendCallback), handler);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
                throw;
            }
        }

        #endregion

        #region private Method
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (!isDisposing)
                {
                    allDone.Set();
                    Socket listener = (Socket)ar.AsyncState;
                    Socket handler = listener.EndAccept(ar);

                    StateObject state = new StateObject();
                    state.workSocket = handler;

                    //Add client to client list
                    //直接获取服务实例监听集合
                    //System.Collections.Specialized.StringCollection stringRemoteReaderIpCollection = Settings.Default.RemoteReaderIpCollection;
                    if (stringRemoteReaderIpCollection.Contains(state.workSocket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        remoteClientList.Add(state);
                    }
                    else
                    {
                        localClientList.Add(state);
                    }
                    Utility.RecordLog(string.Format("Clint {0} connected.", handler.RemoteEndPoint), this.logPath);
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(ReceiveCallback), state);
                }

            }
            catch (System.ObjectDisposedException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
                throw;
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                int bytesRead = 0;

                bytesRead = handler.EndReceive(ar, out errorCode);

                if (errorCode == SocketError.Success)
                {
                    if (bytesRead > 0)
                    {
                        //process received data
                        if (ServerReceiveDataEventHandler != null)
                        {
                            ServerReceiveDataEventHandler(state.buffer);
                        }
                        //clear buffer
                        Array.Clear(state.buffer, 0, state.buffer.Length);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(ReceiveCallback), state);
                    }
                }
                else
                {
                    if (errorCode == SocketError.ConnectionReset)
                    {
                        if (state != null && !state.workSocket.Connected)
                        {
                            Utility.RecordLog(string.Format("Clint {0} disconnected.", handler.RemoteEndPoint), this.logPath);

                            if (localClientList.Contains(state))
                            {
                                localClientList.Remove(state);
                                state.workSocket.Dispose();
                            }
                            if (remoteClientList.Contains(state))
                            {
                                remoteClientList.Remove(state);
                                state.workSocket.Dispose();
                            }
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath);
                throw;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar, out errorCode);
                Utility.RecordLog(string.Format("Request to {0} success.", handler.RemoteEndPoint), this.logPath);
                if (errorCode != SocketError.Success)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, errorCode.ToString()), this.logPath);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
    }

    #endregion

   

    

}

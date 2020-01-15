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
    //server端委托
    public delegate void ServerReceiveDataEventHandlerAsync(string receivedData);
    public delegate void ServerReceiveDataEventHandlerSync(string receivedData);
    public delegate void ServerSendDataEventHandlerAsync(string sendData);    
    public delegate void ServerSendDataEventHandlerSync(string sendData);

    //client端委托
    public delegate void ClientReceiveDataEventHandlerAsync(string receivedData);
    public delegate void ClientReceiveDataEventHandlerSync(string receivedData);
    public delegate void ClientSendDataEventHandlerAsync(string sendData);
    public delegate void ClientSendDataEventHandlerSync(string sendData);

    public delegate void ClientReceiveByteArrayDataEventHandlerAsync(byte[] receivedByteArrayData);
    public delegate void ClientReceiveByteArrayDataEventHandlerSync(byte[] receivedByteArrayData);


    /// <summary>
    /// socket执行动作，同步还是异步标识
    /// </summary>
    public enum SocketAction
    {
        /// <summary>
        /// 同步
        /// </summary>
        Sync,
        /// <summary>
        /// 异步
        /// </summary>
        Async
    }  

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
        public bool hasHeart = false; //是否需要心跳保持长连接
        public DateTime lastKeepTime = DateTime.Now;
        public string clientIp = string.Empty; //客户端IP
    }
       
    /// <summary>
    /// socket的Server端管理
    /// </summary>
    public class SocketServerMgt
    {
        #region data

        public ManualResetEvent allDone = new ManualResetEvent(false);
        
        public List<StateObject> remoteClientList = new List<StateObject>();//接收到的客户端socket

        public List<StateObject> keepClientList = new List<StateObject>(); //接收到客户端信息的最后时间记录

        public event ServerReceiveDataEventHandlerAsync ServerReceiveDataEventHandlerAsnyc;
        public event ServerReceiveDataEventHandlerSync ServerReceiveDataEventHandlerSync;

        public event ServerSendDataEventHandlerAsync ServerSendDataEventHandlerAsync;
        public event ServerSendDataEventHandlerSync ServerSendDataEventHandlerSync;        


        
        string _eartMessage = "OK";
        /// <summary>
        /// 心跳消息内容,默认OK
        /// </summary>
        public string HeartMessage
        {
            get
            {
                return this._eartMessage;
            }
            set
            {
                this._eartMessage = value;
            }
        }
             
        int _keepAliveTime = 60;
        /// <summary>
        /// 心跳间隔时间,默认一分钟一心跳，单位秒
        /// </summary>
        public int KeepAliveTime
        {
            get
            {
                return this._keepAliveTime;
            }
            set
            {
                this._keepAliveTime = value;
            }
        }
        
        bool _hasKeepAlive = false;
        /// <summary>
        /// 是否需要保持长连接,默认不保持
        /// </summary>
        public bool HasKeepAlive
        {
            get
            {
                return this._hasKeepAlive;
            }
            set
            {
                this._hasKeepAlive = value;
            }
        }

          
        int _monitorkeepAliveTime = 30;
        /// <summary>
        /// 超时监控时间间隔，默认30秒，单位秒
        /// </summary>
        public int MonitorkeepAliveTime
        {
            get
            {
                return this._monitorkeepAliveTime;
            }
            set
            {
                this._monitorkeepAliveTime = value;
            }
        }


        int _overTime = 120;
        /// <summary>
        /// 超时时间,超时后会关闭超时连接，默认两分钟，单位秒
        /// </summary>
        public int OverTime
        {
            get
            {
                return this._overTime;
            }
            set
            {
                this._overTime = value;
            }
        }

        private Socket serverSocket = null;
        private SocketError errorCode;
        private int port = 0;
        private string ipAddress = "";
        private string logPath = "";
        private string isRecordLog = "N"; //是否记录日志 Y 记录 N 不记录
        private bool isDisposing = false;

        private List<StateObject> stringRemoteReaderIpCollection = new List<StateObject>();

        #endregion

        #region constructor

        /// <summary>
        /// 异步构造器
        /// </summary>
        /// <param name="ip">监听某个终结点，若为空监控网络上所有终结点</param>
        /// <param name="port"></param>        
        /// <param name="logpath">监听服务日志路径</param>
        /// <param name="recordLog"></param>
        /// <param name="stringClientIpCollection">监听的客户端ip集合</param>
        public SocketServerMgt(string ip, int port, string logpath, string recordLog, List<StateObject> stringClientIpCollection)
        {
            this.port = port;
            this.ipAddress = ip;
            this.logPath = logpath;
            this.isRecordLog = recordLog;
            this.stringRemoteReaderIpCollection = stringClientIpCollection;

            //启动保持长连接控制
            ThreadStart AutoMRKeepAlive = new ThreadStart(SendHeartMessage);
            Thread AutoKeepAlive = new Thread(AutoMRKeepAlive);
            AutoKeepAlive.Start();

            //启动监控超时连接控制
            ThreadStart AutoMROverTime = new ThreadStart(MonitorOverTime);
            Thread AutoOverTime = new Thread(AutoMROverTime);
            AutoOverTime.Start();

        }

        public void Dispose()
        {
            isDisposing = true;
            serverSocket.Close();
            serverSocket.Dispose();

            Utility.RecordLog("SocketServer stop listening...", this.logPath, this.isRecordLog, "ServerConnect");
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="maxCnt">挂起监听数</param>
        /// <param name="socketaction">同步还是异步</param>
        public void StartListening(int maxCnt, SocketAction socketaction)
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

                Utility.RecordLog("SocketServer start listening...", this.logPath, this.isRecordLog, "ServerConnect");

                if (socketaction == SocketAction.Async)
                {
                    //异步监听
                    new Thread(this.AsyncReceiveListen).Start(serverSocket);
                }
                else if (socketaction == SocketAction.Sync)
                {
                    //同步监听                            
                    new Thread(this.SyncReceiveListen).Start(serverSocket);
                }

                Thread.Sleep(1000);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerConnect");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerConnect");
            }
        }

        #endregion


        #region 私有方法

        /// <summary>
        /// 保存允许且已经连接成功的客户端
        /// </summary>
        /// <param name="handler"></param>
        private void SaveClient(Socket handler)
        {
            string clientIp = handler.RemoteEndPoint.ToString().Split(':')[0];
            StateObject sob = new StateObject();

            if (this.CheckClientConnected(handler, this.stringRemoteReaderIpCollection, out sob))
            {
                StateObject sobTmp = new StateObject();
                if (!this.CheckClient(handler, this.remoteClientList, out sobTmp))
                {
                    StateObject state = new StateObject();
                    state.workSocket = handler;
                    state.hasHeart = sob.hasHeart;
                    state.clientIp = sob.clientIp;                    
                    this.remoteClientList.Add(state);
                }
            }
        }

        /// <summary>
        /// 检查是否连接客户端
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="lst"></param>
        /// <param name="sob"></param>
        /// <returns></returns>
        private bool CheckClientConnected(Socket handler, List<StateObject> lst, out StateObject sob)
        {
            string clientIp = handler.RemoteEndPoint.ToString().Split(':')[0];
            sob = new StateObject();
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].clientIp == clientIp)
                {
                    sob = lst[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否存在客户端
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="lst"></param>
        /// <param name="sob"></param>
        /// <returns></returns>
        private bool CheckClient(Socket handler, List<StateObject> lst, out StateObject sob)
        {
            sob = new StateObject();

            try
            {
                string clientIp = handler.RemoteEndPoint.ToString().Split(':')[0];
                
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].workSocket.RemoteEndPoint.ToString().Split(':')[0] == clientIp)
                    {
                        if (lst[i].workSocket.Connected)
                        {
                            sob = lst[i];
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //发生异常认为不存在该连接
            }
            
            return false;
        }

        /// <summary>
        /// 移除失败的客户端连接
        /// </summary>
        /// <param name="handler"></param>
        private void RemoveClient(Socket handler)
        {
            string clientIp = handler.RemoteEndPoint.ToString().Split(':')[0];
            for (int i = 0; i < this.remoteClientList.Count; i++)
            {
                if (this.remoteClientList[i].workSocket.RemoteEndPoint.ToString().Split(':')[0] == clientIp)
                {
                    this.remoteClientList.Remove(this.remoteClientList[i]);
                }
            }
        }

        /// <summary>
        /// 保存当前连接最后通信时间
        /// </summary>
        /// <param name="handler"></param>
        private void SaveKeepClient(Socket handler)
        {
            bool hasClient = false;
            string clientIp = handler.RemoteEndPoint.ToString().Split(':')[0];
            for (int i = 0; i < this.keepClientList.Count; i++)
            {
                if (this.keepClientList[i].workSocket.RemoteEndPoint.ToString().Split(':')[0] == clientIp)
                {
                    this.keepClientList[i].lastKeepTime = DateTime.Now;
                    hasClient = true;
                    break;
                }
            }

            if (!hasClient)
            {
                StateObject sob = new StateObject();
                sob.workSocket = handler;
                sob.lastKeepTime = DateTime.Now;
                sob.clientIp = clientIp;                

                this.keepClientList.Add(sob);
            }
        }

        /// <summary>
        /// 监控超时，超时没有交互信息，则关闭该连接并移除出以连接集合
        /// </summary>
        private void MonitorOverTime()
        {
            while (true)
            {
                for (int i = 0; i < this.keepClientList.Count; i++)
                {
                    try
                    {
                        TimeSpan ts = DateTime.Now - this.keepClientList[i].lastKeepTime;

                        string tmpClientIP = this.keepClientList[i].clientIp; //记录超时ip

                        if (ts.TotalSeconds > double.Parse(this.OverTime.ToString()))
                        {
                            //超过时间，关闭当前连接
                            this.RemoveClient(this.keepClientList[i].workSocket);
                            this.keepClientList[i].workSocket.Disconnect(true);
                            this.keepClientList.Remove(this.keepClientList[i]);

                            Utility.RecordLog(tmpClientIP + " 监控发生超时" + this.HeartMessage + DateTime.Now.ToString("yyyyMMddHHmmss"), this.logPath, this.isRecordLog, "ServerMonitorOverTimeMessage");
                        }

                        Utility.RecordLog(this.keepClientList[i].clientIp + " 监控启动" + this.HeartMessage + DateTime.Now.ToString("yyyyMMddHHmmss"), this.logPath, this.isRecordLog, "ServerMonitorMessage");
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }                    
                }

                Thread.Sleep(this.MonitorkeepAliveTime * 1000); //监控一次时间间隔
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void SendHeartMessage()
        {
            while (true)
            {
                if (this.HasKeepAlive)
                {
                    for (int i = 0; i < this.remoteClientList.Count; i++)
                    {
                        try
                        {
                            if (this.remoteClientList[i].hasHeart)
                            {
                                if (this.remoteClientList[i].workSocket.Connected)
                                {
                                    this.SyncSend(this.remoteClientList[i].workSocket, this.HeartMessage);
                                    Utility.RecordLog(this.remoteClientList[i].clientIp + " 心跳数据" + this.HeartMessage + DateTime.Now.ToString("yyyyMMddHHmmss"), this.logPath, this.isRecordLog, "ServerHeartMessage");
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }                     
                    }                    
                }               

                Thread.Sleep(this.KeepAliveTime * 1000);
            }
        }

        #endregion


        #region 同步处理

        /// <summary>
        /// 同步发送字符串
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public void SyncSend(Socket handler, string data)
        {
            StateObject sob = new StateObject();
            if(this.CheckClient(handler, this.remoteClientList, out sob))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                try
                {
                    int tmp = handler.Send(bytes);
                    if (tmp > 0)
                    {
                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if(data != this.HeartMessage)
                        if (!data.Contains(this.HeartMessage))
                        {
                            if (ServerSendDataEventHandlerSync != null)
                            {
                                ServerSendDataEventHandlerSync(data);
                            }
                        }                        
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerSyncSend");

                }
                catch (Exception ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerSyncSend");
                }
            }            
        }


        /// <summary>
        /// 同步发送字节数组
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="bytes"></param>
        public void SyncSend(Socket handler, byte[] bytes)
        {
            StateObject sob = new StateObject();
            if (this.CheckClient(handler, this.remoteClientList, out sob))
            {
                try
                {
                    int tmp = handler.Send(bytes);
                    if (tmp > 0)
                    {
                        string data = Encoding.UTF8.GetString(bytes, 0, tmp);
                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if(data != this.HeartMessage)
                        if (!data.Contains(this.HeartMessage))
                        {
                            if (ServerSendDataEventHandlerSync != null)
                            {
                                ServerSendDataEventHandlerSync(data);
                            }
                        }                       
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerSyncSend");                   
                }
                catch (Exception ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerSyncSend");
                }
            }                
        }

        /// <summary>
        /// 同步监听
        /// </summary>
        private void SyncReceiveListen(object obj)
        {
            Socket socket = (Socket)obj;
            while (true)
            {
                if (!isDisposing)
                {
                    //同步监听
                    Socket clientScoket = socket.Accept();

                    string tmpClientIP = clientScoket.RemoteEndPoint.ToString().Split(':')[0];

                    StateObject sob = new StateObject();
                    if (this.CheckClientConnected(clientScoket, this.stringRemoteReaderIpCollection, out sob))
                    {
                        this.SaveClient(clientScoket);

                        new Thread(SyncReceiveData).Start(clientScoket);
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        clientScoket.Disconnect(true);
                        //非法客户端
                        Utility.RecordLog(tmpClientIP + " 非法客户端连接" + this.HeartMessage + DateTime.Now.ToString("yyyyMMddHHmmss"), 
                            this.logPath, this.isRecordLog, "ServerMonitorIllegalClientMessage");
                    }                       
                }
            }
        }

        /// <summary>
        /// 同步接收通过客户端socket发送过来的数据
        /// </summary>
        private void SyncReceiveData(object obj)
        {
            Socket socket = (Socket)obj;

            while (true)
            {
                StateObject sob = new StateObject();
                if (this.CheckClient(socket, this.remoteClientList, out sob))
                {
                    string data = "";
                    byte[] bytes = null;
                    int len = socket.Available;
                    if (len > 0)
                    {
                        bytes = new byte[len];
                        int receiveNumber = socket.Receive(bytes);
                        data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);

                        this.SaveKeepClient(socket); //保存通信最后时间，以备检测超时用

                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if(data != this.HeartMessage)
                        if (!data.Contains(this.HeartMessage))
                        {
                            if (!string.IsNullOrEmpty(data))
                            {        
                                if (ServerReceiveDataEventHandlerSync != null)
                                {
                                    ServerReceiveDataEventHandlerSync(data);
                                }
                            }
                        }                        
                    }
                }               
            }                  
        }

        #endregion


        #region 异步处理

        /// <summary>
        /// 异步发送字符串
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public void AsyncSend(Socket handler, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            StateObject sob = new StateObject();
            if (this.CheckClient(handler, this.remoteClientList, out sob))
            {
                try
                {
                    StateObject state = new StateObject();
                    state.workSocket = handler;
                    state.sb.Append(data);

                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(AsyncSendCallback), state);
                   
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
                }
                catch (Exception ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
                }
            }
        }

        /// <summary>
        /// 异步发送字节数组
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="byteData"></param>
        public void AsyncSend(Socket handler, byte[] byteData)
        {
            StateObject sob = new StateObject();
            if (this.CheckClient(handler, this.remoteClientList, out sob))
            {
                try
                {
                    string data = Encoding.UTF8.GetString(byteData, 0, byteData.Length);

                    StateObject state = new StateObject();
                    state.workSocket = handler;
                    state.sb.Append(data);

                    handler.BeginSendTo(byteData, 0, byteData.Length, 0, handler.RemoteEndPoint, new AsyncCallback(AsyncSendCallback), state);                   
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
                }
                catch (Exception ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
                }
            }
                
        }


        /// <summary>
        /// 异步监听
        /// </summary>
        private void AsyncReceiveListen()
        {
            while (true)
            {
                allDone.Reset();
                if (!isDisposing)
                {
                    //异步监听
                    serverSocket.BeginAccept(new AsyncCallback(AsyncAcceptCallback), serverSocket);
                }
                allDone.WaitOne();
            }
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncSendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            StateObject sob = new StateObject();
            if (this.CheckClient(handler, this.remoteClientList, out sob))
            {
                try
                {
                    int bytesSent = handler.EndSend(ar, out errorCode);

                    if (errorCode == SocketError.Success)
                    {
                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if (state.sb.ToString() != this.HeartMessage)
                        if (!state.sb.ToString().Contains(this.HeartMessage))
                        {
                            if (ServerSendDataEventHandlerAsync != null)
                            {
                                ServerSendDataEventHandlerAsync(state.sb.ToString());
                            }
                        }                       
                    }
                }
                catch (Exception ex)
                {
                    Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
                }
            }              
        }

        /// <summary>
        /// 异步接受请求
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncAcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (!isDisposing)
                {
                    allDone.Set();
                    Socket listener = (Socket)ar.AsyncState;
                    Socket handler = listener.EndAccept(ar);

                    string tmpClientIP = handler.RemoteEndPoint.ToString().Split(':')[0];

                    StateObject sob = new StateObject();
                    if (this.CheckClientConnected(handler, this.stringRemoteReaderIpCollection, out sob))
                    {
                        this.SaveClient(handler);

                        if(this.CheckClient(handler, this.remoteClientList, out sob))
                        {
                            StateObject state = new StateObject();
                            state.workSocket = handler;

                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(AsyncReceiveCallback), state);
                        }
                    }
                    else
                    {
                        handler.Disconnect(true);
                        //非法客户端
                        Utility.RecordLog(tmpClientIP + " 非法客户端连接" + this.HeartMessage + DateTime.Now.ToString("yyyyMMddHHmmss"),
                            this.logPath, this.isRecordLog, "ServerMonitorIllegalClientMessage");
                    }
                }
            }
            catch (System.ObjectDisposedException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncSend");
            }

        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncReceiveCallback(IAsyncResult ar)
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
                        this.SaveKeepClient(handler); //保存通信最后时间，以备检测超时用

                        string datas = Encoding.UTF8.GetString(state.buffer, 0, bytesRead);

                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if(datas != this.HeartMessage)
                        if (!datas.Contains(this.HeartMessage))
                        {
                            //process received data
                            if (ServerReceiveDataEventHandlerAsnyc != null)
                            {
                                ServerReceiveDataEventHandlerAsnyc(datas);
                            }
                        }                        
                        //clear buffer
                        Array.Clear(state.buffer, 0, state.buffer.Length);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(AsyncReceiveCallback), state);
                    }
                }
                else
                {
                    if (errorCode == SocketError.ConnectionReset)
                    {
                        if (state != null && !state.workSocket.Connected)
                        {
                            Utility.RecordLog(string.Format("Client {0} disconnected.", handler.RemoteEndPoint), this.logPath, this.isRecordLog, "ServerAsyncReceive");    
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncReceive");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ServerAsyncReceive");
            }
        }

        #endregion

    }

    /// <summary>
    /// socket的Client端管理
    /// </summary>
    public class SocketClientMgt
    {
        #region data

        public ManualResetEvent allDone = new ManualResetEvent(false);

        public event ClientReceiveDataEventHandlerAsync ClientReceiveDataEventHandlerAsnyc;
        public event ClientReceiveDataEventHandlerSync ClientReceiveDataEventHandlerSync;

        public event ClientReceiveByteArrayDataEventHandlerAsync ClientReceiveByteArrayDataEventHandlerAsnyc;
        public event ClientReceiveByteArrayDataEventHandlerSync ClientReceiveByteArrayDataEventHandlerSync;

        public event ClientSendDataEventHandlerAsync ClientSendDataEventHandlerAsync;
        public event ClientSendDataEventHandlerSync ClientSendDataEventHandlerSync;

        public SocketAction socketAction = SocketAction.Async; //默认同步处理
        public Socket clientSocket = null;       

        string _eartMessage = "OK";
        /// <summary>
        /// 心跳消息内容,默认OK
        /// </summary>
        public string HeartMessage
        {
            get
            {
                return this._eartMessage;
            }
            set
            {
                this._eartMessage = value;
            }
        }

        int _keepAliveTime = 60;
        /// <summary>
        /// 心跳间隔时间,默认一分钟一心跳,单位秒
        /// </summary>
        public int KeepAliveTime
        {
            get
            {
                return this._keepAliveTime;
            }
            set
            {
                this._keepAliveTime = value;
            }
        }

        bool _hasKeepAlive = false;
        /// <summary>
        /// 是否需要保持长连接,默认不保持
        /// </summary>
        public bool HasKeepAlive
        {
            get
            {
                return this._hasKeepAlive;
            }
            set
            {
                this._hasKeepAlive = value;
            }
        }

        bool _hasAutoConnected = false;
        /// <summary>
        /// 是否需要保持断开后自动重连,默认不支持
        /// </summary>
        public bool HasAutoConnected
        {
            get
            {
                return this._hasAutoConnected;
            }
            set
            {
                this._hasAutoConnected = value;
            }
        }

        int _autoConnectedTime = 60;
        /// <summary>
        /// 自动重连间隔时间,默认60秒，单位秒
        /// </summary>
        public int AutoConnectedTime
        {
            get
            {
                return this._autoConnectedTime;
            }
            set
            {
                this._autoConnectedTime = value;
            }
        }

        int _bizDataLenth = 0;
        /// <summary>
        /// 业务数据长度
        /// </summary>
        public int BizDataLenth
        {
            get
            {
                return this._bizDataLenth;
            }
            set
            {
                this._bizDataLenth = value;
            }
        }

        string _bizDataStartIden = "";
        /// <summary>
        /// 业务数据起始识别
        /// </summary>
        public string BizDataStartIden
        {
            get
            {
                return this._bizDataStartIden;
            }
            set
            {
                this._bizDataStartIden = value;
            }
        }



        private SocketError errorCode;
        private int port = 0;
        private string ipAddress = "";
        private bool connected = false; // 标识当前是否连接到服务器
        private string logPath = "";
        private string isRecordLog = "";

        //mqg于20180820增加，扩展显示socket名称
        private string socketName = string.Empty;

        //mqg于20180820增加，记录最后接收心跳数据时间
        private DateTime lastHeartData = DateTime.Now;  

        #endregion

        #region constructor

        /// <summary>
        /// 异步构造器
        /// </summary>
        /// <param name="ip">监听某个终结点，若为空监控网络上所有终结点</param>
        /// <param name="port"></param>        
        /// <param name="logpath">监听服务日志路径</param>
        /// <param name="recordLog"></param>
        public SocketClientMgt(string ip, int port, string logpath, string recordLog)
        {
            this.port = port;
            this.ipAddress = ip;
            this.logPath = logpath;
            this.isRecordLog = recordLog;

            //启动监控超时自动重新接控制
            ThreadStart AutoMROverTime = new ThreadStart(MonitorOverTime);
            Thread AutoOverTime = new Thread(AutoMROverTime);
            AutoOverTime.Start();

            //启动保持长连接控制
            ThreadStart AutoMRKeepAlive = new ThreadStart(SendHeartMessage);
            Thread AutoKeepAlive = new Thread(AutoMRKeepAlive);
            AutoKeepAlive.Start();

            //监控服务端的心跳数据保持长连接控制,mqg于20180820增加
            ThreadStart AutoMRServerKeepAlive = new ThreadStart(ServerKeepAlive);
            Thread AutoServerKeepAlive = new Thread(AutoMRServerKeepAlive);
            AutoServerKeepAlive.Start();

        }

        /// <summary>
        /// 异步构造器, mqg于20180820增加，扩展socketname
        /// </summary>
        /// <param name="ip">监听某个终结点，若为空监控网络上所有终结点</param>
        /// <param name="port"></param>        
        /// <param name="logpath">监听服务日志路径</param>
        /// <param name="recordLog"></param>
        public SocketClientMgt(string ip, int port, string logpath, string recordLog, string socketName)
        {
            this.port = port;
            this.ipAddress = ip;
            this.logPath = logpath;
            this.isRecordLog = recordLog;
            this.socketName = socketName;


            //启动监控超时自动重新接控制
            ThreadStart AutoMROverTime = new ThreadStart(MonitorOverTime);
            Thread AutoOverTime = new Thread(AutoMROverTime);
            AutoOverTime.Start();

            //启动保持长连接控制
            ThreadStart AutoMRKeepAlive = new ThreadStart(SendHeartMessage);
            Thread AutoKeepAlive = new Thread(AutoMRKeepAlive);
            AutoKeepAlive.Start();

            //监控服务端的心跳数据保持长连接控制,mqg于20180820增加
            ThreadStart AutoMRServerKeepAlive = new ThreadStart(ServerKeepAlive);
            Thread AutoServerKeepAlive = new Thread(AutoMRServerKeepAlive);
            AutoServerKeepAlive.Start();

        }

        public void Dispose()
        {
            connected = false;
            clientSocket.Close();
            clientSocket.Dispose();

            //mqg于20180820增加，区分IP记录连接状态日志文件
            Utility.RecordLog("停止连接！", this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
        }

        #endregion    

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="errInfo">连接失败提示信息</param>
        public void ConnectServer(string errInfo)
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse(this.ipAddress);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(new IPEndPoint(ip, port));
                connected = true;

                this.lastHeartData = DateTime.Now;

                //mqg于20180820增加，记录连接成功日志
                Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + errInfo + "成功！",
                    this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
            }
            catch (Exception ex)
            {
                //mqg于20180820增加，调整日志记录格式
                //Utility.RecordLog(errInfo + "发生异常：" + ex.ToString(), this.logPath, this.isRecordLog, "ClientConnect");
                Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + errInfo + "失败！发生异常：" + ex.Message, 
                    this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
                connected = false;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReceiveData()
        {
            try
            {
                if (!this.clientSocket.Connected)
                {
                    this.ConnectServer("重新建立连接");
                }

                if (this.socketAction == SocketAction.Async)
                {
                    //异步接收
                    StateObject state = new StateObject();
                    state.workSocket = clientSocket;
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(AsyncReceiveCallback), state);
                }
                else if (this.socketAction == SocketAction.Sync)
                {
                    //同步接收
                    this.SyncReceiveData(clientSocket);
                }
            }
            catch (Exception ex)
            {
                Utility.RecordLog("接收数据失败。发生异常： " + ex.ToString(), this.logPath, this.isRecordLog, "ClientReceieveData");
            }
        }
        
        /// <summary>
        /// 监控超时，发生超时自动进行重连
        /// </summary>
        private void MonitorOverTime()
        {
            while (true)
            {
                if (this.HasAutoConnected)
                {
                    if (!this.clientSocket.Connected)
                    {
                        this.ConnectServer("自动重新建立连接");
                    }
                }

                Thread.Sleep(this.AutoConnectedTime * 1000); //监控一次时间间隔
            }
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void SendHeartMessage()
        {
            while (true)
            {
                if (this.HasKeepAlive)
                {
                    if (this.clientSocket.Connected)
                    {
                        this.SyncSend(this.clientSocket, this.HeartMessage);

                        //mqg于20180820增加，记录连接成功日志
                        Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + "client发送心跳数据包成功",
                            this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
                    }
                }              

                Thread.Sleep(this.KeepAliveTime * 1000);
            }
        }

        /// <summary>
        /// 监控服务端的心跳数据,mqg于20180820增加
        /// </summary>
        private void ServerKeepAlive()
        {
            while (true)
            {
                TimeSpan ts = DateTime.Now - this.lastHeartData;
                if (ts.TotalSeconds > this.KeepAliveTime * 2)
                {
                    //mqg于20180820增加，记录连接成功日志
                    Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + "服务端已经断开，启动重新连接服务端",
                        this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));

                    this.ConnectServer("监控服务端断开进行重连");
                }

                Thread.Sleep(15 * 1000); 
            }
        }


        #region 同步处理

        /// <summary>
        /// 同步发送字符串
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        public void SyncSend(Socket handler, string data)
        {
            if (!this.clientSocket.Connected)
            {
                this.ConnectServer("重新建立连接失败！");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                int tmp = handler.Send(bytes);
                if (tmp > 0)
                {
                    //mqg于20180820增加，调整过滤心跳数据
                    //不处理心跳数据
                    //if(data != this.HeartMessage)
                    if (!data.Contains(this.HeartMessage))
                    {
                        if (ClientSendDataEventHandlerSync != null)
                        {
                            ClientSendDataEventHandlerSync(data);
                        }
                    }           
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientSyncSend");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientSyncSend");
            }
        }


        /// <summary>
        /// 同步发送字节数组
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="bytes"></param>
        public void SyncSend(Socket handler, byte[] bytes)
        {
            if (!this.clientSocket.Connected)
            {
                this.ConnectServer("重新建立连接失败！");
            }

            try
            {
                int tmp = handler.Send(bytes);
                if (tmp > 0)
                {
                    string data = Encoding.UTF8.GetString(bytes, 0, tmp);
                    //mqg于20180820增加，调整过滤心跳数据
                    //不处理心跳数据
                    //if(data != this.HeartMessage)
                    if (!data.Contains(this.HeartMessage))
                    {
                        if (ClientSendDataEventHandlerSync != null)
                        {
                            ClientSendDataEventHandlerSync(data);
                        }
                    }
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientSyncSend");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientSyncSend");
            }
        }


        /// <summary>
        /// 同步处理接收数据
        /// </summary>
        private void SyncReceiveData(object obj)
        {
            try
            {
                Socket socket = (Socket)obj;

                if (!this.clientSocket.Connected)
                {
                    this.ConnectServer("重新建立连接失败！");
                }

                string data = "";
                byte[] bytes = null;
                int len = socket.Available;
                if (len > 0)
                {
                    bytes = new byte[len];
                    int receiveNumber = socket.Receive(bytes);
                    data = Encoding.UTF8.GetString(bytes, 0, receiveNumber);

                    //mqg于20180820增加，调整过滤心跳数据
                    //不处理心跳数据
                    //if(data != this.HeartMessage)
                    if (!data.Contains(this.HeartMessage))
                    {
                        if (ClientReceiveDataEventHandlerSync != null)
                        {
                            ClientReceiveDataEventHandlerSync(data);
                        }

                        if(ClientReceiveByteArrayDataEventHandlerSync != null)
                        {
                            ClientReceiveByteArrayDataEventHandlerSync(bytes);
                        }
                    }
                    else
                    {
                        this.lastHeartData = DateTime.Now;

                        //mqg20180820增加，若心跳数据和业务数据一起发来时，要处理仅发回业务数据
                        int bizDataIndex = data.IndexOf(this.BizDataStartIden);
                        string bizCode = string.Empty; 
                        if (bizDataIndex != -1 && data.Length >= (bizDataIndex + this.BizDataLenth))
                        {
                            bizCode = data.Substring(bizDataIndex, this.BizDataLenth);
                        }    

                        if(!string.IsNullOrEmpty(bizCode))
                        {
                            if (ClientReceiveDataEventHandlerSync != null)
                            {
                                ClientReceiveDataEventHandlerSync(data);
                            }

                            if (ClientReceiveByteArrayDataEventHandlerSync != null)
                            {
                                ClientReceiveByteArrayDataEventHandlerSync(bytes);
                            }
                        }

                        //mqg于20180820增加，记录服务端发来的心跳数据
                        Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + "服务端发来心跳数据：" + data,
                            this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.RecordLog(string.Format("处理接收数据失败！发生异常：:{0}", ex.Message), this.logPath, this.isRecordLog, "ClientSyncRecieve");
            }
        }

        #endregion

        #region 异步处理

        /// <summary>
        /// 异步发送字符串
        /// </summary>
        /// <param name="data"></param>
        public void AsyncSend(string data)
        {
            if (!this.clientSocket.Connected)
            {
                this.ConnectServer("重新建立连接失败！");
            }

            byte[] byteData = Encoding.UTF8.GetBytes(data);
            try
            {
                StateObject state = new StateObject();
                state.workSocket = clientSocket;
                state.sb.Append(data);

                clientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(AsyncSendCallback), state);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncSend");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncSend");
            }
        }

        /// <summary>
        /// 异步发送字节数组
        /// </summary>
        /// <param name="byteData"></param>
        public void AsyncSend(byte[] byteData)
        {
            if (!this.clientSocket.Connected)
            {
                this.ConnectServer("重新建立连接失败！");
            }

            try
            {
                string data = Encoding.UTF8.GetString(byteData, 0, byteData.Length);
                StateObject state = new StateObject();
                state.workSocket = clientSocket;
                state.sb.Append(data);

                clientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(AsyncSendCallback), state);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncSend");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncSend");
            }
        }
                       

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncSendCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesSent = handler.EndSend(ar, out errorCode);

                if (errorCode == SocketError.Success)
                {
                    //mqg于20180820增加，调整过滤心跳数据
                    //不处理心跳数据
                    //if (state.sb.ToString() != this.HeartMessage)
                    if (!state.sb.ToString().Contains(this.HeartMessage))
                    {
                        if (ClientSendDataEventHandlerAsync != null)
                        {
                            ClientSendDataEventHandlerAsync(state.sb.ToString());
                        }
                    }                    
                }
                else
                {
                    Utility.RecordLog(string.Format("{0} 异步发送数据失败！", handler.RemoteEndPoint), this.logPath, this.isRecordLog);
                }
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncSend");
            }
        }


        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncReceiveCallback(IAsyncResult ar)
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
                        string datas = Encoding.UTF8.GetString(state.buffer, 0, bytesRead);

                        //mqg于20180820增加，调整过滤心跳数据
                        //不处理心跳数据
                        //if(data != this.HeartMessage)
                        if (!datas.Contains(this.HeartMessage))
                        {
                            //process received data
                            if (ClientReceiveDataEventHandlerAsnyc != null)
                            {
                                ClientReceiveDataEventHandlerAsnyc(datas);
                            }

                            if(ClientReceiveByteArrayDataEventHandlerAsnyc != null)
                            {
                                ClientReceiveByteArrayDataEventHandlerAsnyc(state.buffer);
                            }
                        }
                        else
                        {
                            this.lastHeartData = DateTime.Now;

                            //mqg20180820增加，若心跳数据和业务数据一起发来时，要发回业务数据 haeartZ6031001
                            int bizDataIndex = datas.IndexOf(this.BizDataStartIden);
                            string bizCode = string.Empty;
                            if (bizDataIndex != -1 && datas.Length >= (bizDataIndex + this._bizDataLenth))
                            {
                                bizCode = datas.Substring(bizDataIndex, this.BizDataLenth);
                            }

                            if (!string.IsNullOrEmpty(bizCode))
                            {
                                if (ClientReceiveDataEventHandlerSync != null)
                                {
                                    ClientReceiveDataEventHandlerSync(datas);
                                }

                                if (ClientReceiveByteArrayDataEventHandlerSync != null)
                                {
                                    ClientReceiveByteArrayDataEventHandlerSync(state.buffer);
                                }
                            }

                            //mqg于20180820增加，记录服务端发来的心跳数据
                            Utility.RecordLog("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "连接IP：" + this.ipAddress + "服务端发来心跳数据：" + datas,
                                this.logPath, this.isRecordLog, "ClientConnect_" + this.socketName + "_" + this.ipAddress.Replace(".", "_"));
                        }

                        //clear buffer
                        Array.Clear(state.buffer, 0, state.buffer.Length);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(AsyncReceiveCallback), state);
                    }
                }
                else
                {
                    if (errorCode == SocketError.ConnectionReset)
                    {
                        if (state != null && !state.workSocket.Connected)
                        {
                            Utility.RecordLog(string.Format("Client {0} disconnected.", handler.RemoteEndPoint), this.logPath, this.isRecordLog, "ClientAsyncReceieve");
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncReceieve");
            }
            catch (Exception ex)
            {
                Utility.RecordLog(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()), this.logPath, this.isRecordLog, "ClientAsyncReceieve");
            }
        }




        #endregion     

    
    }


}

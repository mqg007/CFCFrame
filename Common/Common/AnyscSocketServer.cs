using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using BoxBatteryDataModel;
using Utility;
using ThirdParty.RFIDService.Properties;
namespace AnyscSocket
{
    public delegate void ServerReceiveDataEventHandler(byte[] receivedData);

    public class AnyscSocketServer : IDisposable
    {
        #region data
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static List<StateObject> localClientList = new List<StateObject>();
        public static List<StateObject> remoteClientList = new List<StateObject>();
        public event ServerReceiveDataEventHandler ServerReceiveDataEventHandler;
        private Socket serverSocket = null;
        private SocketError errorCode;
        private int port = 0;

        private bool isDisposing = false;

        #endregion

        #region constructor
        public AnyscSocketServer(int port = 61000)
        {
            this.port = port;
        }
        public void Dispose()
        {
            isDisposing = true;
            serverSocket.Close();
            serverSocket.Dispose();

            LogRecorder.Info("AnyscSocketServer stop listening...");
        }

        #endregion

        #region public Method

        public void StartListening()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(100);

                LogRecorder.Info("AnyscSocketServer start listening...");

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
            catch(System.Net.Sockets.SocketException ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
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
            catch(System.Net.Sockets.SocketException ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
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
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw;
            }

        }

        #endregion

        #region private Method
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if(!isDisposing)
                {
                    allDone.Set();
                    Socket listener = (Socket)ar.AsyncState;
                    Socket handler = listener.EndAccept(ar);

                    StateObject state = new StateObject();
                    state.workSocket = handler;
                    //Add client to client list
                    System.Collections.Specialized.StringCollection stringRemoteReaderIpCollection = Settings.Default.RemoteReaderIpCollection;
                    if (stringRemoteReaderIpCollection.Contains(state.workSocket.RemoteEndPoint.ToString().Split(':')[0]))
                    {
                        remoteClientList.Add(state);
                    }
                    else
                    {
                        localClientList.Add(state);
                        
                    }
                    LogRecorder.Info(string.Format("Clint {0} connected.", handler.RemoteEndPoint));

                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, out errorCode, new AsyncCallback(ReceiveCallback), state);
                }
                
            }
            catch (System.ObjectDisposedException ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
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
                            LogRecorder.Info(string.Format("Clint {0} disconnected.", handler.RemoteEndPoint));

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
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar, out errorCode);
                LogRecorder.Debug(string.Format("Request to {0} success.", handler.RemoteEndPoint));
                if (errorCode != SocketError.Success)
                {
                    LogRecorder.Error(String.Format("{0}() throw an exception:{1}", System.Reflection.MethodBase.GetCurrentMethod().Name, errorCode.ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
    }
}

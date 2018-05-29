using System;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Xml;
using System.Data;
using System.Transactions;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Globalization;


using Common;
using Entitys.ComonEnti;
using Frame.ServiceNodeInterfaces;
using FrameCommon;



namespace WebServiceManager
{
    /// <summary>
    /// WCF服务业务类 基类-需要安全令牌检查
    /// </summary>
    public class BaseService : CommonBaseService
    {
        public BaseService()
        {
            string token = this.GetToken();     
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    //检查令牌,这里需要动态调用服务
                    //节点中心安全检查服务
                    //string FrameNodeSecurityService = this.GetNodeBaseAddr(this.GetNodeCenterAddr(), ServiceType.NodeCenter) +
                    //    APPConfig.GetAPPConfig().GetConfigValue(SSY_ServiceHost.FrameNodeSecurityService, "").TrimStart('/');

                    DataRow drServ = this.GetServiceConfigOne("framenodesecu", "1.0", "normal", "frameNode", this.serviceConfig);
                    //string FrameNodeSecurityService = base.envirObj.BizNodeAddr + "/" + drServ["servcodename"].ToString().TrimStart('/');
                    string FrameNodeSecurityService = drServ["url_addr"].ToString().TrimStart('/') + "/" + drServ["servcodename"].ToString().TrimStart('/');

                    //初始化完整框架环境
                    base.GetAllSysEnvironment();

                    SSY_DYNAMICTOKEN tokenModel = new SSY_DYNAMICTOKEN();
                    tokenModel.Dynamictoken = envirObj.TokenEncrpValue;

                    this.reqdata = new ReqData();
                    this.reqdata.reqdata = json.Serialize(tokenModel);

                    //检查令牌
                    string resCheckToken = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurityService).CheckToken(json.Serialize(this.reqdata));
                    this.resdata = json.Deserialize<RespData>(resCheckToken);
                    if (this.resdata.respflag == "0")
                    {
                        throw new Exception( this.resdata.resptoolstr);
                    }                       
                }
                catch (Exception ex)
                {
                    throw new Exception(this.GetI18nLangItem("ana2CheckTokenErr", this.i18nCommonCurrLang) + ex.Message);
                }
            }
            else
            {
                throw new Exception(this.GetI18nLangItem("noFindTokenErr", this.i18nCommonCurrLang));
            }
        }        
    }

    /// <summary>
    /// WCF服务业务类 基类-不需要安全令牌检查
    /// </summary>
    public class BaseServiceNoCheck : CommonBaseService
    {
        public BaseServiceNoCheck()
        {
            string token = this.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    //初始化完整框架环境
                    base.GetAllSysEnvironment();
                }
                catch (Exception ex)
                {
                    throw new Exception(this.GetI18nLangItem("anaTokenErr", this.i18nCommonCurrLang) + ex.Message);
                }
            }
            else
            {
                throw new Exception(this.GetI18nLangItem("noFindTokenErr", this.i18nCommonCurrLang));
            }
        }
    }


    /// <summary>
    /// 开放服务
    /// </summary>
    public class OpenService : CommonBaseService
    {

    }

    /// <summary>
    /// 服务基类公共方法
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CommonBaseService
    {
        public JavaScriptSerializer json = new JavaScriptSerializer();
        public RespData resdata = new RespData();
        public ReqData reqdata = new ReqData();
        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志

        //相关配置项，后续考虑缓存处理
        public DataTable i18nCommonCurrLang = new DataTable();
        public DataTable i18nModuleCurrLang = new DataTable();
        public DataTable serviceConfig = new DataTable();
        public System.Web.Caching.Cache currCache = HttpRuntime.Cache; //当前缓存

        public string successStr = "";
        public string errorStr = "";

        public SysEnvironmentSerialize envirObj = null; //传递框架环境
        public DistributeDataNodeManagerParams distManagerParam = null; //加载可用数据节点
        public string currlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");  //默认语种 

        public CommonBaseService()
        {
            //支持CROSS访问
            if (OperationContext.Current != null)
            {
                //wcf通道不存在跨域问题      
            }
            if (WebOperationContext.Current != null)
            {
                #region 跨域访问  
                if (WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
                {
                    if (WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Methods"] == null)
                    {
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Origin, Cache-Control, X-Requested-With, Content-Type, Accept, token");
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Max-Age", "1728000");
                    }
                }
                else
                {
                    if (WebOperationContext.Current.OutgoingResponse.Headers["Access-Control-Allow-Methods"] == null)
                    {
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE");
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type");
                    }
                }

                #endregion
            }
            if(HttpContext.Current != null)
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
                if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, Cache-Control, X-Requested-With, Content-Type, Accept, token");
                    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                    HttpContext.Current.Response.End();
                }
            }            

            envirObj = new SysEnvironmentSerialize();            

            try
            {
                //获取框架环境
                string token = this.GetToken();                
                if(!string.IsNullOrEmpty(token))
                {
                    //bool temps = JsonSerializer.Deserialize<SysEnvironmentSerialize>(Encoding.Default.GetString(Convert.FromBase64String(token)), out envirObj);
                    //TODO 后续考虑js的base64处理
                    envirObj = json.Deserialize<SysEnvironmentSerialize>(token);
                }
                else
                {
                    envirObj.I18nCurrLang = currlang;
                }

                //赋值框架实例到静态框架环境
                ManagerSysEnvironment.GetSysEnvironmentSerialize2SysEnvironment(envirObj);

                //这里装载框架级语言包,具体模块在模块内装载
                DataTable comlangtmp = (DataTable)currCache.Get("i18nCommonCurrLang");
                if (comlangtmp != null)
                {
                    if(currlang == envirObj.I18nCurrLang)
                    {
                        i18nCommonCurrLang = comlangtmp;
                    }
                    else
                    {
                        string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), envirObj.I18nCurrLang);
                        i18nCommonCurrLang = this.GetI18nLang(commoni18nLangPath);
                    }                    
                }
                else
                {                    
                    string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), envirObj.I18nCurrLang);
                    i18nCommonCurrLang = this.GetI18nLang(commoni18nLangPath);
                }    
                
                //装载服务配置
                //serviceConfig = this.GetServiceConfig(APPConfig.GetAPPConfig().GetConfigValue("ServiceConfigPath", ""));
                DataTable dttmp = (DataTable)currCache.Get("serviceConfig");
                if (dttmp != null)
                {
                    serviceConfig = dttmp;
                }
                else
                {
                    serviceConfig = this.GetServiceConfig(APPConfig.GetAPPConfig().GetConfigValue("ServiceConfigPath", ""));
                }

                //装载数据配置
                //distManagerParam = this.GetDistributeDataNodeManagerParams();
                DistributeDataNodeManagerParams ddnmtmp = (DistributeDataNodeManagerParams)currCache.Get("dataNodes");
                if (ddnmtmp != null)
                {
                    distManagerParam = ddnmtmp;
                }
                //这里不需要再装载了，缓存已经装载了
                //else
                //{
                //    distManagerParam = this.GetDistributeDataNodeManagerParams();
                //}

                //设置语言运行环境 
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(envirObj.I18nCurrLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(envirObj.I18nCurrLang);
                //due to an error of freetextbox, all the cultures must use a dot as NumberDecimalSeparator			
                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";

                this.successStr = this.GetI18nLangItem("successStr", this.i18nCommonCurrLang);
                this.errorStr = this.GetI18nLangItem("errorStr", this.i18nCommonCurrLang);

            }
            catch (Exception ex)
            {
                throw new Exception("Unknown exception! Reason:" + ex.Message);
            }                   
        }

        #region 公共方法

        /// <summary>
        /// 选择节点地址
        /// </summary>
        /// <param name="nodeaddrs">服务基地址</param>
        /// <param name="servType">业务服务类型</param>
        /// <returns></returns>
        public string GetNodeBaseAddr(List<string> nodeaddrs, ServiceType servType)
        {
            return this.RandomSelectAddr(nodeaddrs, servType).TrimEnd('/') + "/";
        }

        /// <summary>
        /// 获取节点中心地址
        /// </summary>
        /// <returns></returns>
        public List<string> GetNodeCenterAddr()
        {
            List<string> nodeCenterAddr = new List<string>();
            nodeCenterAddr.Add(APPConfig.GetAPPConfig().GetConfigValue("NodeCenterMaster", ""));
            nodeCenterAddr.Add(APPConfig.GetAPPConfig().GetConfigValue("NodeCenterSlave", ""));

            return nodeCenterAddr;
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            string token = string.Empty;
            if (string.IsNullOrEmpty(token) && OperationContext.Current != null)
            {
                int index = OperationContext.Current.IncomingMessageHeaders.FindHeader("token", "");
                if (index >= 0) token = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index);
            }
            if (string.IsNullOrEmpty(token) && WebOperationContext.Current != null)
            {
                if (WebOperationContext.Current.IncomingRequest.Headers["token"] != null)
                {
                    token = WebOperationContext.Current.IncomingRequest.Headers["token"].ToString();
                }
            }

            return token;
        }

        /// <summary>
        /// 获取可用节点，包括业务及数据
        /// </summary>
        /// <param name="use_state"></param>
        /// <returns></returns>
        public UseNodeCollection GetUseNodeCollections(string use_state)
        {
            UseNodeCollection unc = new UseNodeCollection();
            List<SSY_BIZNODE_ADDR> bizNode = new List<SSY_BIZNODE_ADDR>();
            List<SSY_DATANODE_ADDR> dataNode = new List<SSY_DATANODE_ADDR>();
            unc.BizNodeList = bizNode;
            unc.DataNodeList = dataNode;

            //业务节点
            string cols = "id|url_addr|use_status|moudiden|remarks|timestampss";
            string colTypes = "String|String|String|String|String|String";
            string useNodeAddr = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_BIZNODE_ADDR.xml";
            System.Data.DataTable dtXmlDataBiz = Common.Utility.GetTableFromXml(cols, colTypes, useNodeAddr);

            if (Utility.DtHasData(dtXmlDataBiz))
            {
                DataRow[] drsBiz = null;
                if (!string.IsNullOrEmpty(use_state))
                {
                    drsBiz = dtXmlDataBiz.Select(string.Format("use_status = '{0}'", use_state));
                }
                else
                {
                    drsBiz = dtXmlDataBiz.Select(string.Format("1 = {0}", "1"));
                }

                if (drsBiz.Length > 0)
                {
                    SSY_BIZNODE_ADDR tempBizNode = null;
                    for (int i = 0; i < drsBiz.Length; i++)
                    {
                        tempBizNode = new SSY_BIZNODE_ADDR();
                        tempBizNode.ID = drsBiz[i]["id"].ToString();
                        tempBizNode.Url_addr = drsBiz[i]["Url_addr"].ToString();
                        tempBizNode.Moudiden = drsBiz[i]["Moudiden"].ToString();
                        tempBizNode.Use_status = drsBiz[i]["Use_status"].ToString();
                        bizNode.Add(tempBizNode);
                    }
                }
            }

            //数据节点
            string cols1 = @"id|url_addr|use_status|data_schema|data_user|data_password|data_conn|remarks|timestampss|dbfactoryname|systemname|isencrydbconn|isencrypwd|encryhashlenth|encrykeystr|isusepwdsecuritycheck|pwdintervalhours|pwdfirstcheck";
            string colTypes1 = @"String|String|String|String|String|String|String|String|String|String|String|String|String|String|String|String|String|String";
            string useDataNodeAddr = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_DATANODE_ADDR.xml";
            System.Data.DataTable dtXmlDataData = Common.Utility.GetTableFromXml(cols1, colTypes1, useDataNodeAddr);

            if (Utility.DtHasData(dtXmlDataData))
            {
                DataRow[] drsData = null;
                if (!string.IsNullOrEmpty(use_state))
                {
                    drsData = dtXmlDataData.Select(string.Format("use_status = '{0}'", use_state));
                }
                else
                {
                    drsData = dtXmlDataData.Select(string.Format("1 = {0}", "1"));
                }
                if (drsData.Length > 0)
                {
                    SSY_DATANODE_ADDR tempDataNode = null;
                    for (int i = 0; i < drsData.Length; i++)
                    {
                        tempDataNode = new SSY_DATANODE_ADDR();
                        tempDataNode.ID = drsData[i]["id"].ToString();

                        tempDataNode.Use_status = drsData[i]["use_status"].ToString();
                        tempDataNode.DBFactoryName = drsData[i]["dbfactoryname"].ToString();
                        tempDataNode.Systemname = drsData[i]["systemname"].ToString();
                        tempDataNode.Isencrypwd = drsData[i]["isencrypwd"].ToString();
                        tempDataNode.Encryhashlenth = drsData[i]["encryhashlenth"].ToString();
                        tempDataNode.Encrykeystr = drsData[i]["encrykeystr"].ToString();
                        tempDataNode.Isusepwdsecuritycheck = drsData[i]["isusepwdsecuritycheck"].ToString();
                        tempDataNode.Pwdintervalhours = drsData[i]["pwdintervalhours"].ToString();
                        tempDataNode.Pwdfirstcheck = drsData[i]["pwdfirstcheck"].ToString();

                        tempDataNode.Isencrydbconn = drsData[i]["isencrydbconn"].ToString();
                        tempDataNode.Url_addr = drsData[i]["url_addr"].ToString();
                        tempDataNode.Data_schema = drsData[i]["data_schema"].ToString();
                        tempDataNode.Data_user = drsData[i]["data_user"].ToString();
                        tempDataNode.Data_password = drsData[i]["data_password"].ToString();
                        tempDataNode.Data_conn = drsData[i]["data_conn"].ToString();

                        dataNode.Add(tempDataNode);
                    }
                }
            }

            return unc;
        }

        /// <summary>
        /// 更新节点运行状态
        /// </summary>
        /// <param name="servType"></param>
        /// <param name="node_addr"></param>
        /// <param name="errStr"></param>
        public void UpdateNodeRunStatus(ServiceType servType, string node_addr, string errStr)
        {
            bool tempFlag = false;
            string nodeStatusFileName = string.Empty;
            string updateNodeName = string.Empty;
            string updateNodeValue = string.Empty;
            string idName = string.Empty;
            string idValue = string.Empty;

            if (servType == ServiceType.NodeCenter)
            {
                //节点中心服务ID(NCServices) 节点中心后台工作服务ID(NCBServices)  
                //配置文件节点属性名
                //"id|nodeAddr|nodeName|runStatus|nodeCenterClass|remarks|timestampss";
                idName = "id";
                idValue = "NCServices";

                //需要更新的节点名及节点值，必须一一对应
                updateNodeName = "runStatus|timestampss";
                updateNodeValue = string.Format("{0}|{1}", "停止", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //更新节点中心状态
                nodeStatusFileName = "\\SSY_NodeCenter_RunStatus.xml";
                tempFlag = Utility.UpdateAttributeFromXmls(APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + nodeStatusFileName, idName, idValue,
                    updateNodeName, updateNodeValue, '|');
            }
            else if (servType == ServiceType.BizDueWith)
            {
                //业务节点状态
                //配置文件节点属性名
                //id="xxx" url_addr="xxx" use_status="xxx"  remarks="xxx" timestampss="xxx";
                idName = "url_addr";
                idValue = node_addr;

                //需要更新的节点名及节点值，必须一一对应
                updateNodeName = "use_status|timestampss";
                updateNodeValue = string.Format("{0}|{1}", "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //更新业务节点运行状态
                nodeStatusFileName = "\\SSY_BIZNODE_ADDR.xml";
                tempFlag = Utility.UpdateAttributeFromXmls(APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + nodeStatusFileName, idName, idValue,
                    updateNodeName, updateNodeValue, '|');
            }
            else if (servType == ServiceType.BizDueWith)
            {
                //数据库节点状态
                //配置文件节点属性名
                //id="xxx" url_addr="xxx" use_status="xxx" data_schema="xxx" data_user="xxx"  data_password="xxx" data_conn="xxx"  remarks="xxx" 
                //timestampss ="xxx" dbfactoryname="xxx" systemname="xxx" isconfigdb="" isencrydbconn="" isencrypwd="xxx" encryhashlenth="xxx" 
                //encrykeystr ="xxx" securitycode="xxx" isusepwdsecuritycheck="xxx" pwdintervalhours="xxx" pwdfirstcheck="xxx"
                idName = "url_addr";
                idValue = node_addr;

                //需要更新的节点名及节点值，必须一一对应
                updateNodeName = "use_status|timestampss";
                updateNodeValue = string.Format("{0}|{1}", "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //更新业务节点运行状态
                nodeStatusFileName = "\\SSY_DATANODE_ADDR.xml";
                tempFlag = Utility.UpdateAttributeFromXmls(APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + nodeStatusFileName, idName, idValue,
                    updateNodeName, updateNodeValue, '|');
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        public void RecordLog(string logContent, string logPath)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            StreamWriter sw = File.AppendText(logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            sw.WriteLine(logContent);
            sw.Close();
        }
                

        /// <summary>
        /// 解析请求信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ReqData AnaRequestData(string req)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Deserialize<ReqData>(req);
        }

        /// <summary>
        /// 生成html返回信息
        /// </summary>
        /// <param name="respflag"></param>
        /// <param name="resptoolstr"></param>
        /// <param name="respdata"></param>
        /// <param name="pagertottlecounts"></param>
        /// <returns></returns>
        public RespData MakeResponseData(string respflag, string resptoolstr, string respdata, string pagertottlecounts)
        {
            RespData resdata = new RespData();
            resdata.respflag = respflag;
            resdata.resptoolstr = resptoolstr;
            resdata.respdata = respdata;
            resdata.pagertottlecounts = pagertottlecounts;
            return resdata;
        }

        /// <summary>
        /// 发送http请求获取返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <param name="reqType">post get</param>
        /// <param name="reqMethord"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public RespData RequestHttp(string url, string req, string reqType, string reqMethord, string token)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            string resstr = @"";
            Req reqTemp = new Req();
            ReqData resqq = new ReqData();
            resqq.reqdata = req;
            resqq.reqtype = reqType;

            reqTemp.req = json.Serialize(resqq);

            #region 原生态方式

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = reqMethord.ToUpper();
            request.Timeout = 60000;
            request.ReadWriteTimeout = request.Timeout;
            request.Headers.Add("token", token);

            byte[] sendData = Encoding.UTF8.GetBytes(json.Serialize(reqTemp));
            if (sendData != null && sendData.Length > 0)
            {
                using (var streamRequst = request.GetRequestStream())
                {
                    streamRequst.Write(sendData, 0, sendData.Length);
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            try
            {
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    resstr = reader.ReadToEnd().TrimStart("\u005C\u0022".ToCharArray()).TrimEnd("\u005C\u0022".ToCharArray()).Replace("\\", "");
                    reader.Close();
                }
            }
            catch (Exception exs)
            {
                throw exs;
            }

            #endregion

            #region webclient方式

            //using (System.Net.WebClient wc = new System.Net.WebClient())
            //{
            //    wc.Headers.Add("Content-Type", "application/json");
            //    wc.Headers.Add("token", token);
            //    if (reqMethord.ToUpper() == "POST")
            //    {
            //        Req reqTemp = new Req();
            //        ReqData resqq = new ReqData();
            //        resqq.reqdata = req;
            //        resqq.reqtype = reqType;
            //        reqTemp.req = json.Serialize(resqq);
            //        byte[] sendData = Encoding.UTF8.GetBytes(json.Serialize(reqTemp));
            //        wc.Headers.Add("ContentLength", sendData.Length.ToString());
            //        byte[] recData = wc.UploadData(url, reqMethord.ToUpper(), sendData);
            //        resstr = Encoding.UTF8.GetString(recData).TrimStart("\u005C\u0022".ToCharArray()).TrimEnd("\u005C\u0022".ToCharArray()).Replace("\\", "");;
            //    }
            //    else
            //    {
            //        resstr = wc.DownloadString(url);
            //    }                
            //}

            #endregion

            return json.Deserialize<RespData>(resstr);
        }

        /// <summary>
        /// 获取语言包
        /// </summary>
        /// <param name="langpath"></param>
        /// <returns></returns>
        public DataTable GetI18nLang(string langpath)
        {
            DataTable dttmp = new DataTable();

            if (!string.IsNullOrEmpty(langpath))
            {
                string cols = "id|texts";
                string colTypes = "String|String";
                dttmp = Common.Utility.GetTableFromXml(cols, colTypes, langpath);
            }           

            return dttmp;
        }

        /// <summary>
        /// 获取具体语言项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtlangs"></param>
        /// <returns></returns>
        public string GetI18nLangItem(string id, DataTable dtlangs)
        {
            for (int i = 0; i < dtlangs.Rows.Count; i++)
            {
                if(dtlangs.Rows[i]["id"].ToString() == id)
                {
                    return dtlangs.Rows[i]["texts"].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 加载服务配置文件
        /// </summary>
        /// <param name="serviceconfigpath"></param>
        /// <returns></returns>
        public DataTable GetServiceConfig(string serviceconfigpath)
        {
            DataTable dttmp = new DataTable();
            string cols = "id|url_addr|moudname|moudiden|servname|serviden|servcodename|use_status|remarks|version|servclass|servtype";
            string colTypes = "String|String|String|String|String|String|String|String|String|String|String|String";
            dttmp = Common.Utility.GetTableFromXml(cols, colTypes, serviceconfigpath);

            return dttmp;
        }

        /// <summary>
        /// 获取当前应用服务配置
        /// </summary>
        /// <param name="serviden"></param>
        /// <param name="version"></param>
        /// <param name="servclass"></param>
        /// <param name="servtype"></param>
        /// <param name="dtServConfig"></param>
        /// <returns></returns>
        public DataRow GetServiceConfigOne(string serviden, string version, string servclass, string servtype, DataTable dtServConfig)
        {
            for (int i = 0; i < dtServConfig.Rows.Count; i++)
            {
                if (dtServConfig.Rows[i]["serviden"].ToString().ToUpper() == serviden.ToUpper() &&
                    dtServConfig.Rows[i]["version"].ToString().ToUpper() == version.ToUpper() &&
                    dtServConfig.Rows[i]["servclass"].ToString().ToUpper() == servclass.ToUpper() &&
                    dtServConfig.Rows[i]["servtype"].ToString().ToUpper() == servtype.ToUpper())
                {
                    return dtServConfig.Rows[i];
                }
            }            
            return null;
        }

        /// <summary>
        /// 装载数据节点
        /// </summary>
        /// <returns></returns>
        public DistributeDataNodeManagerParams GetDistributeDataNodeManagerParams()
        {
            DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams(); //分布式管理参数

            //调用节点中心服务获取数据节点
            DataRow drServ = this.GetServiceConfigOne("framenodesecu", "1.0", "normal", "frameNode", this.serviceConfig);
            string FramenodeSecurity = drServ["url_addr"].ToString().TrimStart('/') + "/" + drServ["servcodename"].ToString().TrimStart('/');    
            this.reqdata = new ReqData();
            this.reqdata.reqdata = "1";
            string resdatanode = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FramenodeSecurity).GetDataNodeCollection(json.Serialize(this.reqdata));
            this.resdata = json.Deserialize<RespData>(resdatanode);
            if (this.resdata.respflag == "1")
            {
                distManagerParam = json.Deserialize<DistributeDataNodeManagerParams>(this.resdata.respdata);
            }         

            return distManagerParam;
        }

        /// <summary>
        /// 加载全部的环境变量
        /// </summary>
        /// <param name="envirObj"></param>
        /// <returns></returns>
        public void GetAllSysEnvironment()
        {
            this.envirObj.distManagerParam = this.distManagerParam;

            //后续若需要，考虑补全用户实体信息
        }




        #region 事务框架执行代码段
        /// <summary>
        /// 事务框架执行代码段
        /// </summary>
        /// <typeparam name="TResult">自定义操作返回的数据类型</typeparam>
        /// <param name="handler">自定义操作</param>
        /// <param name="autoComplete">是否自动提交操作</param>
        /// <returns>返回自定义操作返回值</returns>
        public TResult Invoke<TResult>(Func<TransactionScope, TResult> handler, bool autoComplete = true)
        {
            TResult result = default(TResult);
            //定义事务
            TransactionScope scope = null;
            try
            {
                //初始化事务
                scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromHours(1));

                //执行委托事件
                result = handler(scope);

                //提交事务
                if (autoComplete) this.Complete(scope);

                //
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose(scope);
            }
        }
        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 按算法选择节点地址
        /// </summary>
        /// <param name="nodeaddrs"></param>
        /// <param name="servType"></param>
        /// <returns></returns>
        private string RandomSelectAddr(List<string> nodeaddrs, ServiceType servType)
        {
            string currAddrs = string.Empty;
            List<string> nodeaddrsNo = new List<string>(); //记录不用的地址
            string errStr = string.Empty;

            for (int i = 0; i < nodeaddrs.Count; i++)
            {
                //暂时用机选代替
                currAddrs = nodeaddrs[Common.Utility.GetRandNum(0, nodeaddrs.Count)];

                if (nodeaddrsNo.Contains(currAddrs))
                {
                    continue; //若选择未可用的后，继续重新选择
                }

                if (Common.Utility.VerifyAvailable(currAddrs.TrimEnd('/'), Utility.WebRequestType.Http, ref errStr))
                {
                    break; //选中且可用  终止
                }
                else
                {
                    nodeaddrsNo.Add(currAddrs);
                    currAddrs = string.Empty;

                    //报告节点异常,区分业务类型
                    this.ReportNodeError(servType, currAddrs, errStr);
                }
            }

            return currAddrs;
        }

        /// <summary>
        /// 报告节点异常，包括节点中心服务、业务节点服务、业务节点数据库
        /// </summary>
        /// <param name="servType"></param>
        /// <param name="node_addr"></param>
        /// <param name="errStr"></param>
        private void ReportNodeError(ServiceType servType, string node_addr,  string errStr)
        {
            //报告节点异常
            string FrameManagerNodeService = this.GetNodeBaseAddr(this.GetNodeCenterAddr(), servType) +
               APPConfig.GetAPPConfig().GetConfigValue(SSY_ServiceHost.FrameManagerNodeService, "").TrimStart('/');

            List<SSY_NODE_ERRORS> model = new List<SSY_NODE_ERRORS>();
            SSY_NODE_ERRORS modelOne = new SSY_NODE_ERRORS();

            string timestr = DateTime.Now.ToString("yyyyMMddHHmmss");
            modelOne.ID = timestr  + Utility.GetRandNum(3);
            modelOne.Node_typs = servType.ToString();
            modelOne.Url_addr = node_addr;
            modelOne.Error_desc = errStr;
            modelOne.Remarks = "";
            modelOne.Timestampss = timestr;

            model.Add(modelOne);            

            //报告节点异常
            var res = DynamicInvokeWCF.Create<IFrameManagerNode>(FrameManagerNodeService).RecordNodeErrorLogN(this.json.Serialize(model));            
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="scope">分布式事务</param>
        private void Complete(TransactionScope scope)
        {
            if (scope != null)
            {
                lock (scope)
                {
                    if (scope != null) scope.Complete();
                }
            }
        }

        /// <summary>
        /// 释放事务
        /// </summary>
        /// <param name="scope">分布式事务</param>
        private void Dispose(TransactionScope scope)
        {
            if (scope != null)
            {
                lock (scope)
                {
                    if (scope != null) scope.Dispose();
                }
            }
        }

       

        #endregion


    }

    




}
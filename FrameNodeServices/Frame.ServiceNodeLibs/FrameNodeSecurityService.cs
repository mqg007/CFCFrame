using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Caching;

using Frame.ServiceNodeInterfaces;
using Frame.ServiceInterfaces;
using WebServiceManager;
using Entitys.ComonEnti;
using Common;
using FrameCommon;


namespace Frame.ServiceNodeLibs
{
    public class FrameNodeSecurityService : OpenService, IFrameNodeSecurity
    {
        public FrameNodeSecurityService()
        {
            //这里具体服务语言包
            DataTable servlangtmp = (DataTable)currCache.Get("i18nFrameNodei18nLang");
            if (servlangtmp != null)
            {
                if (currlang == envirObj.I18nCurrLang)
                {
                    i18nModuleCurrLang = servlangtmp;
                }
                else
                {
                    string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), envirObj.I18nCurrLang);
                    i18nModuleCurrLang = this.GetI18nLang(FrameNodei18nLang);
                }
            }
            else
            {
                string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), envirObj.I18nCurrLang);
                i18nModuleCurrLang = this.GetI18nLang(FrameNodei18nLang);                
            }
        }
              

        #region 升级接口实现

        /// <summary>
        /// 获取框架环境
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>        
        public string GetFrameParams(string req)
        {
            try
            {
                //解析参数实体 
                reqdata = this.AnaRequestData(req);
                //string reqobj = this.json.Deserialize<string>(reqdata.reqdata);
                //SysEnvironmentSerialize tokenObjBase = null; //初始化框架参数  
                ////获取空序列化环境变量 
                //tokenObjBase = ManagerSysEnvironment.GetSysEnvironmentSerializeEmpty();
                //tokenObjBase.TokenEncrpValue = string.Empty; //这里还没有令牌
                //string biznodeaddr = this._GetBizNodeAddr();
                //tokenObjBase.BizNodeAddr = biznodeaddr;
                ////获取分布式管理参数
                //tokenObjBase.distManagerParam = this.InitNodeDistManagerParams();

                //直接使用框架环境变量
                envirObj.TokenEncrpValue = string.Empty;//这里还没有令牌
                string biznodeaddr = this._GetBizNodeAddr(); //获取业务节点
                envirObj.BizNodeAddr = biznodeaddr;
                //获取分布式管理参数
                //envirObj.distManagerParam = this.InitNodeDistManagerParams();

                if (string.IsNullOrEmpty(biznodeaddr))
                {
                    resdata = this.MakeResponseData("0", base.errorStr + this.GetI18nLangItem("noFoundBizNode", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
                //不能传递数据节点
                //else if(envirObj.distManagerParam == null || envirObj.distManagerParam.DistributeDataNodes.Count <= 0)
                //{
                //    resdata = this.MakeResponseData("0", base.errorStr + this.GetI18nLangItem("noFoundDataNode", this.i18nModuleCurrLang), string.Empty, string.Empty);
                //}
                else
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(envirObj), string.Empty);
                }                
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("LoadFrameEnvErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }            

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetToken(string req)
        {
            try
            {
                //解析参数实体 
                reqdata = this.AnaRequestData(req);
                SSY_DYNAMICTOKEN model = this.json.Deserialize<SSY_DYNAMICTOKEN>(reqdata.reqdata);

                string bizErrStr = ""; //具体失败的业务提示
                string FrameSecurity = "";
                //获取业务可用节点
                UseNodeCollection unc = new UseNodeCollection();
                SSY_ResponseResult<UseNodeCollection> res = this.GetUseNodeCollection("1");
                if (res.IsCompleted)
                {
                    unc = res.Result;
                    //获取业务节点地址
                    List<string> bizNodes = new List<string>();
                    for (int i = 0; i < unc.BizNodeList.Count; i++)
                    {
                        bizNodes.Add(unc.BizNodeList[i].Url_addr);
                    }
                    //获取业务服务
                    //string bizSvcName = this._GetSingleBizSvc(SSY_ServiceHost.Frame_FrameSecurityService);
                    //FrameSecurity = this.GetNodeBaseAddr(bizNodes, ServiceType.BizDueWith) + bizSvcName.TrimStart('/');

                    //调用普通登陆验证服务
                    DataRow drServ = this.GetServiceConfigOne("framesecurity", "1.0", "normal", "frameMgt", this.serviceConfig);
                    //FrameSecurity = base.envirObj.BizNodeAddr + "/" + drServ["servcodename"].ToString().TrimStart('/');

                    //获取本地配置业务节点
                    List<SSY_BIZNODE_ADDR> tmpBizNode = base.json.Deserialize<List<SSY_BIZNODE_ADDR>>(this._GetBizNodeAddr());
                    for (int i = 0; i < tmpBizNode.Count; i++)
                    {
                        if (tmpBizNode[i].Moudiden.ToUpper() == "FrameMgt".ToUpper())
                        {
                            FrameSecurity = tmpBizNode[i].Url_addr + "/" + drServ["servcodename"].ToString().TrimStart('/');
                            break;
                        }
                    }

                    //二次登陆验证
                    SSY_USER_DICT ud = new SSY_USER_DICT();
                    ud.USERID = model.Remarks;
                    ud.PASSWORD = model.Timestampss;

                    this.reqdata = new ReqData();
                    this.reqdata.reqdata = json.Serialize(ud);

                    string reslogin = DynamicInvokeWCF.Create<IFrameSecurity>(FrameSecurity).GetUserForLogin2N(json.Serialize(this.reqdata));
                    List<SSY_USER_DICT> resuds = new List<SSY_USER_DICT>();
                    this.resdata = json.Deserialize<RespData>(reslogin);
                    if(this.resdata.respflag == "1")
                    {
                        #region 登陆成功

                        string cols = "id|dynamictoken|remarks|timestampss";
                        string colTypes = "String|String|String|String";
                        string tokenFilePath = APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + "\\SSY_DYNAMICTOKEN.xml";
                        System.Data.DataTable dtToken = Common.Utility.GetTableFromXml(cols, colTypes, tokenFilePath);

                        if (Utility.DtHasData(dtToken))
                        {
                            //随机获取令牌
                            model.Dynamictoken = dtToken.Rows[Utility.GetRandNum(1, 99)]["dynamictoken"].ToString();
                            if (string.IsNullOrEmpty(model.Dynamictoken))
                            {
                                bizErrStr = this.GetI18nLangItem("noFoundTokenText", this.i18nModuleCurrLang); 
                            }
                            else
                            {
                                resdata = this.MakeResponseData("1", base.successStr, model.Dynamictoken, string.Empty);
                            }
                        }
                        else
                        {
                            bizErrStr = this.GetI18nLangItem("noFoundTokens", this.i18nModuleCurrLang); 
                        }

                        #endregion
                    }
                    else
                    {
                        bizErrStr =  this.resdata.resptoolstr;
                    }                   
                }
                else
                {
                    bizErrStr = res.Exception;
                }
                if (!string.IsNullOrEmpty(bizErrStr))
                {
                    resdata = this.MakeResponseData("0", base.errorStr + bizErrStr, string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("GetTokenErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 检查令牌
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string CheckToken(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_DYNAMICTOKEN model = this.json.Deserialize<SSY_DYNAMICTOKEN>(reqdata.reqdata);

                //检查节点令牌, TODO后续优化考虑缓冲令牌
                string cols = "id|dynamictoken|remarks|timestampss";
                string colTypes = "String|String|String|String";
                string tokenFilePath = APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + "\\SSY_DYNAMICTOKEN.xml";
                System.Data.DataTable dtToken = Common.Utility.GetTableFromXml(cols, colTypes, tokenFilePath);

                if (Utility.DtHasData(dtToken))
                {
                    DataRow[] drs = dtToken.Select(string.Format("dynamictoken = '{0}'", model.Dynamictoken));
                    if (drs.Length > 0)
                    {
                        resdata = this.MakeResponseData("1", this.GetI18nLangItem("tokenOk", this.i18nModuleCurrLang), string.Empty, string.Empty);
                        return json.Serialize(resdata);
                    }
                }

                resdata = this.MakeResponseData("0", this.GetI18nLangItem("tokenErr", this.i18nModuleCurrLang), string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("tokenCheckErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 加载节点服务地址,仅仅加载目前可用节点(包含业务和数据)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetDataNodeCollection(string req)
        {
            DistributeDataNodeManagerParams distManagerParam = null; //分布式管理参数

            try
            {
                reqdata = this.AnaRequestData(req);
                string usestate = reqdata.reqdata;
                DistributeDataNodeManagerParams ddnmtmp = (DistributeDataNodeManagerParams)currCache.Get("dataNodes");
                if (ddnmtmp != null)
                {
                    distManagerParam = ddnmtmp;                    
                }
                else
                {
                    FrameNodeBizCommon fnodecom = new FrameNodeBizCommon();
                    distManagerParam = fnodecom.GetDistributeDataNodeManager(usestate);                   
                }                  
                if(distManagerParam.DistributeDataNodes.Count > 0)
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(distManagerParam), string.Empty);
                }  
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindDataNodes", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("initDataNodeErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 加载节点服务地址,仅仅加载目前可用节点
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetNodeCollection(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                string usestate = reqdata.reqdata;
                UseNodeCollection usenodes = new UseNodeCollection();
                usenodes = this.GetUseNodeCollections(usestate);

                if(usenodes.BizNodeList.Count > 0 && usenodes.DataNodeList.Count > 0)
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(usenodes), string.Empty);
                }
                else if(usenodes.BizNodeList.Count <= 0 || usenodes.DataNodeList.Count <= 0)
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindDataNodesOrBizNodes", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindDataNodesAndBizNodes", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }                
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("getDataNodeAndBizNodeErr", this.i18nModuleCurrLang) + ex.Message, 
                    string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 获取可用节点
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public SSY_ResponseResult<UseNodeCollection> GetUseNodeCollection(string temp)
        {
            return new SSY_ResponseResult<UseNodeCollection>(this.GetUseNodeCollections("1"));
        }



        /// <summary>
        /// 获取节点中心服务站点服务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SSY_ResponseResult<IEnumerable<SSY_SERVICESITE_SERVICES>> GetNodeServiceSiteService(SSY_SERVICESITE_SERVICES model)
        {
            string cols = "id|sitecode|servicecode|servicename|service_relaUrl|remarks|timestampss";
            string colTypes = "String|String|String|String|String|String|String";
            string serviceSiteServicePath = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_SERVICESITE_SERVICES.xml";
            System.Data.DataTable dtserviceSiteService = Common.Utility.GetTableFromXml(cols, colTypes, serviceSiteServicePath);

            List<SSY_SERVICESITE_SERVICES> serviceSiteServiceList = new List<SSY_SERVICESITE_SERVICES>();
            if (Utility.DtHasData(dtserviceSiteService))
            {
                serviceSiteServiceList = UtilitysForT<SSY_SERVICESITE_SERVICES>.GetListsObj(dtserviceSiteService);
            }

            return new SSY_ResponseResult<IEnumerable<SSY_SERVICESITE_SERVICES>>(serviceSiteServiceList);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取单个业务服务名称
        /// </summary>
        /// <param name="serviceIden"></param>
        /// <returns></returns>
        private string _GetSingleBizSvc(string serviceIden)
        {
            string serviceName = string.Empty;

            List<SSY_SERVICESITE_SERVICES> svcList = new List<SSY_SERVICESITE_SERVICES>();
            SSY_SERVICESITE_SERVICES modeltemp = new SSY_SERVICESITE_SERVICES();
            SSY_ResponseResult<IEnumerable<SSY_SERVICESITE_SERVICES>> resSvc = this.GetNodeServiceSiteService(modeltemp);
            svcList = resSvc.Result.ToList<SSY_SERVICESITE_SERVICES>();

            if (UtilitysForT<SSY_SERVICESITE_SERVICES>.ListHasData(svcList))
            {
                SSY_SERVICESITE_SERVICES svcTar = svcList.Find(delegate (SSY_SERVICESITE_SERVICES t) { return t.Servicecode == serviceIden; });
                serviceName = svcTar.Service_relaUrl;
            }

            return serviceName;
        }

        /// <summary>
        /// 获取可用业务节点地址
        /// </summary>
        /// <returns></returns>
        private string _GetBizNodeAddr()
        {
            string resBizNodeListStr = "";
            UseNodeCollection ff = this.GetUseNodeCollections("1");
            //目前 随机获取各平台所管的各模块的业务地址
            //return ff.BizNodeList[Common.Utility.GetRandNum(0, ff.BizNodeList.Count)].Url_addr;

            List<SSY_BIZNODE_ADDR> toAppBizNodeAddr = new List<SSY_BIZNODE_ADDR>(); //分发给应用的随机可用业务节点

            if (ff != null)
            {
                if (ff.BizNodeList.Count > 0)
                {
                    //获取当前可用模块
                    List<string> currUseMoudiden = new List<string>();
                    for (int i = 0; i < ff.BizNodeList.Count; i++)
                    {
                        if (!currUseMoudiden.Contains(ff.BizNodeList[i].Moudiden))
                        {
                            currUseMoudiden.Add(ff.BizNodeList[i].Moudiden);
                        }
                    }

                    //目前随机分配, 后面根据应用可用扩展算法来分配
                    for (int i = 0; i < currUseMoudiden.Count; i++)
                    {
                        List<SSY_BIZNODE_ADDR> tmp = ff.BizNodeList.FindAll(p => p.Moudiden.ToUpper() == currUseMoudiden[i].ToUpper());
                        toAppBizNodeAddr.Add(tmp[Common.Utility.GetRandNum(0, tmp.Count)]);
                    }
                }
            }

            if (toAppBizNodeAddr.Count > 0)
            {
                resBizNodeListStr = base.json.Serialize(toAppBizNodeAddr);
            }

            return resBizNodeListStr;
        }

        /// <summary>
        /// 初始化分布式节点管理参数
        /// </summary>
        private DistributeDataNodeManagerParams InitNodeDistManagerParams()
        {
            DistributeDataNodeManagerParams distManagerParam = null; //分布式管理参数
            List<SSY_DATANODE_ADDR> dataNodes = new List<SSY_DATANODE_ADDR>();

            UseNodeCollection unc = new UseNodeCollection();
            SSY_ResponseResult<UseNodeCollection> res = this.GetUseNodeCollection("1");
            if (res.IsCompleted)
            {
                #region 初始化数据节点

                unc = res.Result;
                //分布式参数  
                distManagerParam = new DistributeDataNodeManagerParams();
                distManagerParam.DistributeDataNode = new DistributeDataNode();
                distManagerParam.DistributeActionIden = DistributeActionIden.Query; //默认给查询，需要在具体业务工作方法重新设定
                distManagerParam.DistriActionSqlParams = new List<DistActionSql>(); //由于支持单点操作延后执行，需要在具体业务工作方法重新设定
                distManagerParam.DistributeDataNode = new DistributeDataNode(); //只需处理工厂配置参数，其他参数系统框架会自动根据数据节点及配置文件设定

                if (unc.DataNodeList.Count > 0)
                {
                    foreach (var item in unc.DataNodeList)
                    {
                        //若配置加密数据库相关参数，这里需要先进行解密
                        item.Url_addr = Common.Security.DeEncryptInfo(item.Url_addr.ToString(), item.Encrykeystr,
                                int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                        item.Data_schema = Common.Security.DeEncryptInfo(item.Data_schema.ToString(), item.Encrykeystr,
                            int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());
                        item.Data_user = Common.Security.DeEncryptInfo(item.Data_user.ToString(), item.Encrykeystr,
                            int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());
                        item.Data_password = Common.Security.DeEncryptInfo(item.Data_password.ToString(), item.Encrykeystr,
                            int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                        item.Data_conn = Common.Security.DeEncryptInfo(item.Data_conn.ToString(), item.Encrykeystr,
                            int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                        dataNodes.Add(item);
                    }
                    distManagerParam.DistributeDataNodes = dataNodes;
                    distManagerParam.DistributeDataNode.DbFactoryName = unc.DataNodeList[0].DBFactoryName;
                }

                #endregion
            }

            return distManagerParam;
        }


        #endregion     
    }

    /// <summary>
    /// 节点中心公共类
    /// </summary>
    public class FrameNodeBizCommon
    {
        #region 公共方法

        /// <summary>
        /// 加载节点服务地址,仅仅加载目前可用节点(包含业务和数据)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DistributeDataNodeManagerParams GetDistributeDataNodeManager(string usestate)
        {
            DistributeDataNodeManagerParams distManagerParam = null; //分布式管理参数

            List<SSY_DATANODE_ADDR> dataNodes = new List<SSY_DATANODE_ADDR>();
            UseNodeCollection unc = this.GetUseNodeCollections(usestate);

            //分布式参数  
            distManagerParam = new DistributeDataNodeManagerParams();
            distManagerParam.DistributeDataNode = new DistributeDataNode();
            distManagerParam.DistributeActionIden = DistributeActionIden.Query; //默认给查询，需要在具体业务工作方法重新设定
            distManagerParam.DistriActionSqlParams = new List<DistActionSql>(); //由于支持单点操作延后执行，需要在具体业务工作方法重新设定
            distManagerParam.DistributeDataNode = new DistributeDataNode(); //只需处理工厂配置参数，其他参数系统框架会自动根据数据节点及配置文件设定

            if (unc.DataNodeList.Count > 0)
            {
                foreach (var item in unc.DataNodeList)
                {
                    //若配置加密数据库相关参数，这里需要先进行解密
                    item.Url_addr = Common.Security.DeEncryptInfo(item.Url_addr.ToString(), item.Encrykeystr,
                            int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                    item.Data_schema = Common.Security.DeEncryptInfo(item.Data_schema.ToString(), item.Encrykeystr,
                        int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());
                    item.Data_user = Common.Security.DeEncryptInfo(item.Data_user.ToString(), item.Encrykeystr,
                        int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());
                    item.Data_password = Common.Security.DeEncryptInfo(item.Data_password.ToString(), item.Encrykeystr,
                        int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                    item.Data_conn = Common.Security.DeEncryptInfo(item.Data_conn.ToString(), item.Encrykeystr,
                        int.Parse(item.Encryhashlenth), item.Isencrydbconn.ToUpper());

                    dataNodes.Add(item);
                }
                distManagerParam.DistributeDataNodes = dataNodes;
                distManagerParam.DistributeDataNode.DbFactoryName = unc.DataNodeList[0].DBFactoryName;
            }

            return distManagerParam;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取可用节点，包括业务及数据
        /// </summary>
        /// <param name="use_state"></param>
        /// <returns></returns>
        private UseNodeCollection GetUseNodeCollections(string use_state)
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

        #endregion



    }
}

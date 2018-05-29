using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Configuration;
using System.Web.Caching;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using System.IO;

using Common;
using WebServiceManager;
using Frame.ServiceNodeInterfaces;
using FrameCommon;
using Entitys.ComonEnti;

/// <summary>
/// CacheDataMgt 的摘要说明
/// </summary>
public class CacheDataMgt
{
    private System.Threading.Timer  cacheDataMgtTimer; //管理缓存数据    

    public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
    public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志
    public string baseXmlPath = APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "");
    //延迟leaveTime分钟后开始执行,默认1分钟      
    int leaveTime = 1;
    //间隔internalTime分钟执行一次，默认3分钟
    int internalTime = 3;

    public CacheDataMgt(HttpServerUtility currservers)
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
               
        try
        {
            leaveTime = int.Parse(APPConfig.GetAPPConfig().GetConfigValue("NodeCenterDataLeaveTime", ""));
            internalTime =int.Parse(WebConfigurationManager.AppSettings["NodeCenterDataInternalTime"].ToString());
        }
        catch (Exception ex)
        {
            leaveTime = 1;
            internalTime = 3;
        }

        //cacheDataMgtTimer = new System.Threading.Timer(new TimerCallback(ExecUpdateCacheData), currservers, leaveTime * 60000, internalTime * 60000);

        //定时同步缓存数据
        ThreadStart AutoSynCacheData = new ThreadStart(ExecUpdateCacheData);
        Thread AutoThread = new Thread(AutoSynCacheData);
        AutoThread.Start();
    }

    /// <summary>
    /// 启动更新数据缓存
    /// </summary>
    /// <param name="obj"></param>
    public void ExecUpdateCacheData(object obj)
    {
        System.Web.Caching.Cache currCache = HttpRuntime.Cache;
        int cacheMinute = 50;        

        //加载缓存服务配置
        DataTable dtservconfig = BaseServiceUtility.GetServiceConfig(APPConfig.GetAPPConfig().GetConfigValue("ServiceConfigPath", ""));
        currCache.Insert("serviceConfig", dtservconfig, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

        //缓存默认公共语言包
        string defaultlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");
        string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), defaultlang);
        DataTable i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
        currCache.Insert("i18nCommonCurrLang", i18nCommonCurrLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
       
        //当前模块语言包
        string Xxxi18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), defaultlang);
        DataTable i18nXxxi18nLang = BaseServiceUtility.GetI18nLang(Xxxi18nLang);
        currCache.Insert("i18nXxxi18nLang", i18nXxxi18nLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

      
        //加载缓存数据节点
        //调用普通节点中心服务加载数据节点
        DataRow drServ = BaseServiceUtility.GetServiceConfigOne("framenodesecu", "1.0", "normal", "frameNode", dtservconfig);
        string FrameNodeSecurity = drServ["url_addr"].ToString().TrimStart('/') + "/" + drServ["servcodename"].ToString().TrimStart('/');
        JavaScriptSerializer json = new JavaScriptSerializer();
        ReqData reqdata = new ReqData();
        reqdata.reqdata = "1";
        RespData resdata = new RespData();
        //获取节点中心数据节点,
        string datanodes = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurity).GetDataNodeCollection(json.Serialize(reqdata));
        DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams(); //分布式管理参数
        resdata = json.Deserialize<RespData>(datanodes);
        if (resdata.respflag == "1")
        {
            distManagerParam = json.Deserialize<DistributeDataNodeManagerParams>(resdata.respdata);
            if(distManagerParam.DistributeDataNodes.Count > 0)
            {
                currCache.Insert("dataNodes", distManagerParam, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
            }            
        }

        //获取业务模块节点
        string nodelists = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurity).GetNodeCollection(json.Serialize(reqdata));
        UseNodeCollection nodes = new UseNodeCollection(); //节点集合
        resdata = json.Deserialize<RespData>(nodelists);
        if (resdata.respflag == "1")
        {
            nodes = json.Deserialize<UseNodeCollection>(resdata.respdata);
            if (nodes.BizNodeList.Count > 0)
            {
                currCache.Insert("bizNodes", nodes.BizNodeList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
            }
        }

        Common.Utility.RecordLog("完成Somebiz模块配置缓存！", this.logpathForDebug, this.isLogpathForDebug);
    }


    /// <summary>
    /// 启动更新数据缓存
    /// </summary>
    /// <param name="obj"></param>
    public void ExecUpdateCacheData()
    {
        System.Web.Caching.Cache currCache = HttpRuntime.Cache;
        int cacheMinute = 50;
        bool executeFlag = true;

        while (executeFlag)
        {
            executeFlag = false;

            try
            {
                #region 缓存处理

                //加载缓存服务配置
                DataTable dtservconfig = BaseServiceUtility.GetServiceConfig(APPConfig.GetAPPConfig().GetConfigValue("ServiceConfigPath", ""));
                currCache.Insert("serviceConfig", dtservconfig, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

                //缓存默认公共语言包
                string defaultlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");
                string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), defaultlang);
                DataTable i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
                currCache.Insert("i18nCommonCurrLang", i18nCommonCurrLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

                //当前模块语言包
                string Xxxi18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), defaultlang);
                DataTable i18nXxxi18nLang = BaseServiceUtility.GetI18nLang(Xxxi18nLang);
                currCache.Insert("i18nXxxi18nLang", i18nXxxi18nLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));


                //加载缓存数据节点
                //调用普通节点中心服务加载数据节点
                DataRow drServ = BaseServiceUtility.GetServiceConfigOne("framenodesecu", "1.0", "normal", "frameNode", dtservconfig);
                string FrameNodeSecurity = drServ["url_addr"].ToString().TrimStart('/') + "/" + drServ["servcodename"].ToString().TrimStart('/');
                JavaScriptSerializer json = new JavaScriptSerializer();
                ReqData reqdata = new ReqData();
                reqdata.reqdata = "1";
                RespData resdata = new RespData();
                //获取节点中心数据节点,
                string datanodes = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurity).GetDataNodeCollection(json.Serialize(reqdata));
                DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams(); //分布式管理参数
                resdata = json.Deserialize<RespData>(datanodes);
                if (resdata.respflag == "1")
                {
                    distManagerParam = json.Deserialize<DistributeDataNodeManagerParams>(resdata.respdata);
                    if (distManagerParam.DistributeDataNodes.Count > 0)
                    {
                        currCache.Insert("dataNodes", distManagerParam, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
                    }
                }

                //获取业务模块节点
                string nodelists = DynamicInvokeWCF.Create<IFrameNodeSecurity>(FrameNodeSecurity).GetNodeCollection(json.Serialize(reqdata));
                UseNodeCollection nodes = new UseNodeCollection(); //节点集合
                resdata = json.Deserialize<RespData>(nodelists);
                if (resdata.respflag == "1")
                {
                    nodes = json.Deserialize<UseNodeCollection>(resdata.respdata);
                    if (nodes.BizNodeList.Count > 0)
                    {
                        currCache.Insert("bizNodes", nodes.BizNodeList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
                    }
                }

                Common.Utility.RecordLog("完成Somebiz模块配置缓存！", this.logpathForDebug, this.isLogpathForDebug);

                #endregion
            }
            catch (Exception ex)
            {
                Common.Utility.RecordLog("配置Somebiz模块缓存，发生异常！原因：" + ex.Message, this.logpathForDebug, this.isLogpathForDebug);
            }

            Thread.Sleep(this.internalTime * 60000);//延迟秒级别
            executeFlag = true;
        }

    }

}
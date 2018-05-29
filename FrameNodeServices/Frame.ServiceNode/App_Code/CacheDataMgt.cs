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
using FrameCommon;
using Frame.ServiceNodeLibs;

/// <summary>
/// CacheDataMgt 的摘要说明
/// </summary>
public class CacheDataMgt
{
    private System.Threading.Timer cacheDataMgtTimer; //管理缓存数据    

    public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
    public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志

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
            internalTime = int.Parse(WebConfigurationManager.AppSettings["NodeCenterDataInternalTime"].ToString());            
        }
        catch (Exception ex)
        {
            leaveTime = 1;
            internalTime = 3;            
        }

        //cacheDataMgtTimer = new System.Threading.Timer(new TimerCallback(ExecUpdateCacheData), currservers, leaveTime * 1000, internalTime * 60000);

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
        try
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

            //缓存默认各模块语言包,多个模块独立累加
            string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), defaultlang);
            DataTable i18nFrameNodei18nLang = BaseServiceUtility.GetI18nLang(FrameNodei18nLang);
            currCache.Insert("i18nFrameNodei18nLang", i18nFrameNodei18nLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));


            //缓存数据节点  
            DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams(); //分布式管理参数   
            FrameNodeBizCommon fnodecom = new FrameNodeBizCommon();
            distManagerParam = fnodecom.GetDistributeDataNodeManager("1");
            if (distManagerParam.DistributeDataNodes.Count > 0)
            {
                currCache.Insert("dataNodes", distManagerParam, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
            }

            //缓存业务节点
            DataTable dtbiznodeaddrconfig = BaseServiceUtility.GetBizNodesConfig(APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + "\\SSY_BIZNODE_ADDR.xml");
            currCache.Insert("bizNodeConfig", dtbiznodeaddrconfig, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

            Common.Utility.RecordLog("完成节点中心配置缓存！", this.logpathForDebug, this.isLogpathForDebug);
        }
        catch (Exception ex)
        {
            Common.Utility.RecordLog("配置节点中心缓存，发生异常！原因：" + ex.Message, this.logpathForDebug, this.isLogpathForDebug);
        }        
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

                //缓存默认各模块语言包,多个模块独立累加
                string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), defaultlang);
                DataTable i18nFrameNodei18nLang = BaseServiceUtility.GetI18nLang(FrameNodei18nLang);
                currCache.Insert("i18nFrameNodei18nLang", i18nFrameNodei18nLang, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));


                //缓存数据节点  
                DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams(); //分布式管理参数   
                FrameNodeBizCommon fnodecom = new FrameNodeBizCommon();
                distManagerParam = fnodecom.GetDistributeDataNodeManager("1");
                if (distManagerParam.DistributeDataNodes.Count > 0)
                {
                    currCache.Insert("dataNodes", distManagerParam, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));
                }

                //缓存业务节点
                DataTable dtbiznodeaddrconfig = BaseServiceUtility.GetBizNodesConfig(APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "") + "\\SSY_BIZNODE_ADDR.xml");
                currCache.Insert("bizNodeConfig", dtbiznodeaddrconfig, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, cacheMinute, 0));

                Common.Utility.RecordLog("完成节点中心配置缓存！", this.logpathForDebug, this.isLogpathForDebug);

                #endregion
            }
            catch (Exception ex)
            {
                //记录结果
                Common.Utility.RecordLog("配置节点中心缓存，发生异常！原因：" + ex.Message, this.logpathForDebug, this.isLogpathForDebug);
            }

            Thread.Sleep(this.internalTime * 60000);//延迟秒级别
            executeFlag = true;
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Web.Caching;

using Entitys.ComonEnti;
using Common;
using FrameCommon;
using Xxx.Entities;

namespace Xxx.Biz
{
    public abstract class Somebiz : System.MarshalByRefObject
    {
        public DataTable i18nCommonCurrLang = new DataTable(); //通用语言包
        public DataTable i18nXxxManageri18nLang = new DataTable(); //Somebiz语言包        
        public SysEnvironmentSerialize envirObj = null; //传递框架环境

        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志

        public string baseXmlPath = APPConfig.GetAPPConfig().GetConfigValue("XmldataPath", "");



        //若包含其他需要累加

        public Somebiz(SysEnvironmentSerialize _envirObj)
        {
            string _currlang = _envirObj.I18nCurrLang;
            System.Web.Caching.Cache currCache = HttpRuntime.Cache; //当前缓存
            string defaultlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");  //默认语种 
            this.envirObj = _envirObj;

            #region 通用语言包

            DataTable comlangtmp = (DataTable)currCache.Get("i18nCommonCurrLang");
            if (comlangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nCommonCurrLang = comlangtmp;
                }
                else
                {
                    string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), _currlang);
                    i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
                }
            }
            else
            {
                string commoni18nLangPath = string.Format(APPConfig.GetAPPConfig().GetConfigValue("Commoni18nLang", ""), _currlang);
                i18nCommonCurrLang = BaseServiceUtility.GetI18nLang(commoni18nLangPath);
            }

            #endregion

            #region Somebiz语言包

            DataTable servFrameSecuriylangtmp = (DataTable)currCache.Get("i18nXxxi18nLang");
            if (servFrameSecuriylangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nXxxManageri18nLang = servFrameSecuriylangtmp;
                }
                else
                {
                    string Somebizi18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), _currlang);
                    i18nXxxManageri18nLang = BaseServiceUtility.GetI18nLang(Somebizi18nLang);
                }
            }
            else
            {
                string Somebizi18nLang = string.Format(this.baseXmlPath + APPConfig.GetAPPConfig().GetConfigValue("XxxManageri18nLang", ""), _currlang);
                i18nXxxManageri18nLang = BaseServiceUtility.GetI18nLang(Somebizi18nLang);
            }

            #endregion

        }

        #region Somebiz

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictAll(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictPager(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictAll(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictPager(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">字典设别</param>
        /// <param name="dicttype">字典类型(公共 业务)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams);

        #endregion    

    }
}

using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;

using Entitys.ComonEnti;
using Common;
using FrameCommon;

namespace Biz
{
    public abstract class CommonBiz : System.MarshalByRefObject
    {
        public DataTable i18nCommonCurrLang = new DataTable(); //通用语言包
        public DataTable i18nFrameSecurityi18nLang = new DataTable(); //框架安全语言包        
        public DataTable i18nFrameManageri18nLang = new DataTable(); //框架管理语言包
        public SysEnvironmentSerialize envirObj = null; //传递框架环境
        public string permitMaxLoginFailtCnt = ""; //允许最大错误登录次数

        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志



        //若包含其他需要累加

        public CommonBiz(SysEnvironmentSerialize _envirObj)
        {
            string _currlang = _envirObj.I18nCurrLang;
            System.Web.Caching.Cache currCache = HttpRuntime.Cache; //当前缓存
            string defaultlang = APPConfig.GetAPPConfig().GetConfigValue("currlang", "");  //默认语种 
            this.envirObj = _envirObj;
            this.permitMaxLoginFailtCnt = APPConfig.GetAPPConfig().GetConfigValue("permitMaxLoginFailtCnt", "5");  //允许最大错误登录次数, 默认5次

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

            #region 框架安全语言包

            DataTable servFrameSecuriylangtmp = (DataTable)currCache.Get("i18nFrameSecurityi18nLang");
            if (servFrameSecuriylangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nFrameSecurityi18nLang = servFrameSecuriylangtmp;
                }
                else
                {
                    string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), _currlang);
                    i18nFrameSecurityi18nLang = BaseServiceUtility.GetI18nLang(FrameSecurityi18nLang);
                }
            }
            else
            {
                string FrameSecurityi18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameSecurityi18nLang", ""), _currlang);
                i18nFrameSecurityi18nLang = BaseServiceUtility.GetI18nLang(FrameSecurityi18nLang);
            }

            #endregion

            #region 框架管理语言包

            DataTable servFrameManagerlangtmp = (DataTable)currCache.Get("i18nFrameManageri18nLang");
            if (servFrameManagerlangtmp != null)
            {
                if (defaultlang == _currlang)
                {
                    i18nFrameManageri18nLang = servFrameManagerlangtmp;
                }
                else
                {
                    string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), _currlang);
                    i18nFrameManageri18nLang = BaseServiceUtility.GetI18nLang(FrameManageri18nLang);
                }
            }
            else
            {
                string FrameManageri18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameManageri18nLang", ""), _currlang);
                i18nFrameManageri18nLang = BaseServiceUtility.GetI18nLang(FrameManageri18nLang);
            }

            #endregion
        }

        #region 系统权限相关

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams">分布式管理参数，必须存在有框架赋值</param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public abstract DataTable GetUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY> ListBizLog);

        /// <summary>
        /// 用户安全退出
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams">分布式管理参数，必须存在有框架赋值</param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public abstract string QuitUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY> ListBizLog);

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetAllUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取功能
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetPages(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取用户组(带分页)
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams,  SSY_PagingParam pager);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetUserdict(SSY_USER_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// 获取页面
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataSet GetPage(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取用户组(带分页)
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetPagePager(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// 获取用户组与用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>        
        public abstract DataTable GetGroupUserPager(SSY_USER_GROUP_DICT model, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager);


        /// <summary>
        /// 获取角色权限配置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract SSY_PAGE_GROUP_MQT GetGroupPageMgt(SSY_GROUP_PAGE_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public abstract bool ResetUserPWD(SSY_USER_DICT model, DistributeDataNodeManagerParams ddnmParams, List<SSY_LOGENTITY> ListBizLog);

        #endregion

        #region 系统框架相关

        /// <summary>
        /// 获取系统框架参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameParam(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取系统框架参数明细
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameParamDetail(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取系统树结构风格参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetTreeViewConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取系统菜单风格参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetMenuConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract DataTable GetDataEntity(string  dataEntName, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 判断某类日志是否记录
        /// </summary>
        /// <param name="logtypeDomain"></param>
        /// <param name="loglevelOption"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract bool CheckIsRecord(string logtypeDomain, string loglevelOption, DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetLogDataPager(SSY_LOGENTITY model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);

        #endregion

        #region 字典相关

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictAll(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetFrameDictPager(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictAll(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract DataTable GetBizDictPager(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager);

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">字典设别</param>
        /// <param name="dicttype">字典类型(公共 业务)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams);

        #endregion

        #region 公共部分

        /// <summary>
        /// 获取系统ID字符串
        /// </summary>
        /// <param name="makeIDType"></param>
        /// <param name="formmat"></param>
        /// <param name="makeCustemTypeID"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public virtual string GetID(MakeIDType makeIDType, string formmat, MakeCustemTypeID makeCustemTypeID, DistributeDataNodeManagerParams ddnmParams)
        {
            string retId = string.Empty;

            if (makeIDType == MakeIDType.YMDHMM)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss");
            }
            else if (makeIDType == MakeIDType.GUID)
            {
                if(string.IsNullOrEmpty(formmat))
                {
                    return System.Guid.NewGuid().ToString();
                }
                else
                {
                    System.Guid guid = new System.Guid(formmat);
                    return guid.ToString();
                }
            }
            else if (makeIDType == MakeIDType.YMDHMS_1)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(1).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_2)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(2).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_3)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(3).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_4)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(4).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_5)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(5).ToString();
            }
            else if (makeIDType == MakeIDType.YMDHMS_10)
            {
                retId = System.Convert.ToDateTime(this.GetSystemDateTime(ddnmParams)).ToString("yyyyMMddHHmmss") + Utility.GetRandNum(10).ToString();
            }
            else if (makeIDType == MakeIDType.CUSTEMTYPE)
            {
                return makeCustemTypeID();
            }

            return retId;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public virtual string GetSystemDateTime(DistributeDataNodeManagerParams ddnmParams)
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 统一操作业务实体泛型,记录日志用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">业务实体List</param>
        /// <param name="opObjPropertyL">要操作的业务实体属性名称，对应于数据库字段，若确认都是删除时，可入空字符串</param>
        /// <param name="wherePropertyL">业务实体where语句属性名称，对应于数据库标示数据字段</param>
        /// <param name="mainPropertyL">主键字段说明</param>
        /// <param name="errStr"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public abstract bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams);

        /// <summary>
        /// 统一操作业务实体泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">业务实体List</param>
        /// <param name="opObjPropertyL">要操作的业务实体属性名称，对应于数据库字段，若确认都是删除时，可入空字符串</param>
        /// <param name="wherePropertyL">业务实体where语句属性名称，对应于数据库标示数据字段</param>
        /// <param name="mainPropertyL">主键字段说明</param>
        /// <param name="errStr"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public abstract bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams, List<SSY_LOGENTITY> ListBizLog);


        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="bizobjectname">数据库中的表名</param>
        /// <param name="wherePropertyL">字段|字段值,例如：prop1|value1</param>
        /// <param name="splitchar">wherePropertyL中的分割符号，默认|</param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        public abstract bool CheckBizObjectRepat(string bizobjectname, List<string> wherePropertyL, string splitchar,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams, List<string> errStr);

        /// <summary>
        /// 通用查询数据实体是否存在数据库中,批量查询，支持分别查询一个字段、一组字段是否存在,可查询多条记录
        /// </summary>
        /// <param name="bizObjectName">数据实体表名</param>
        /// <param name="fields">要查询的字段组合，支持多组查询。例如：field1|field2|field3|field4;field5|field6,表示的分别查询字段1、字段2、字段3、字段6分别单独不重复，字段4和字段5组合不重复</param>
        /// <param name="fieldsValue">每个字段对应的数据值，字符串集合</param>
        /// <param name="splitChar">要查询的重复单元分隔符号。对应例子中的竖杠 |  可自定义其他，但要确保不和数据内容冲突</param>
        /// <param name="splitCharSub">要查询的组合分隔符号。对应例子中的分号 ; 可自定义其他 但要确保不和数据内容冲突</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract List<string> CheckBizObjectsRepat(string bizObjectName, string fields, List<string> fieldsValue, string splitChar, string splitCharSub,
            DistributeDataNodeManagerParams ddnmParams);

        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public abstract DataTable GetEntityAllDataForCommon(string tablename, DistributeDataNodeManagerParams ddnmParams);

        #endregion
               


    }

   
}

using System;
using System.Data;
using System.Collections.Generic;

using Biz;
using BizFactory;
using Entitys.ComonEnti;
using Common;
using FrameCommon;

namespace BizExecFacade
{
    public class CommonBizFacade
    {
        CommonFactory comBizeF = new CommonFactory();
        CommonBiz comBize = null;

        public string err = string.Empty;

        public CommonBizFacade(SysEnvironmentSerialize _envirObj)
        {
            string _currlang = _envirObj.I18nCurrLang;
            try
            {
                this.comBize = this.comBizeF.Create("one", _envirObj);                 
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }             
        }

        #region 系统权限相关

        /// <summary>
        /// 获取用户list
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams">分布式管理参数，必须存在有框架赋值</param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public List<SSY_USER_DICT> GetUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY>  ListBizLog)
        {
            List<SSY_USER_DICT> listReturn = new List<SSY_USER_DICT>();
            DataTable dt = this.comBize.GetUserForLogin(ud, distributeDataNodeManagerParams, ListBizLog);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_USER_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 用户安全退出
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public string QuitUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams,
            List<SSY_LOGENTITY> ListBizLog)
        {
            return this.comBize.QuitUserForLogin(ud, distributeDataNodeManagerParams, ListBizLog);
        }

        /// <summary>
        /// 获取用户list
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public List<SSY_USER_DICT> GetUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            List<SSY_USER_DICT> listReturn = new List<SSY_USER_DICT>();
            DataTable dt = this.comBize.GetUsers(ud, distributeDataNodeManagerParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_USER_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取所有用户list
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public List<SSY_USER_DICT> GetAllUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            List<SSY_USER_DICT> listReturn = new List<SSY_USER_DICT>();
            DataTable dt = this.comBize.GetAllUsers(ud, distributeDataNodeManagerParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_USER_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取功能
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public DataSet GetPages(SSY_USER_DICT ud, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetPages(ud, distributeDataNodeManagerParams);
        }

        /// <summary>
        /// 获取用户组名
        /// </summary>
        /// <returns></returns>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        public List<SSY_GROUP_DICT> GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            List<SSY_GROUP_DICT> listReturn = new List<SSY_GROUP_DICT>();

            DataSet ds = this.comBize.GetGroup(gd, distributeDataNodeManagerParams);
            if (Common.Utility.DsHasData(ds))
            {
                listReturn = Common.UtilitysForT<SSY_GROUP_DICT>.GetListsObj(ds.Tables[0]);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取用户组(带分页)
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_GROUP_DICT> GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams distributeDataNodeManagerParams, SSY_PagingParam pager)
        {
            List<SSY_GROUP_DICT> listReturn = new List<SSY_GROUP_DICT>();

            DataTable dt = this.comBize.GetGroup(gd, distributeDataNodeManagerParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_GROUP_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_USER_DICT> GetUserdict(SSY_USER_DICT bizobj, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_USER_DICT> listReturn = new List<SSY_USER_DICT>();

            DataTable dt = this.comBize.GetUserdict(bizobj, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_USER_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }


        /// <summary>
        /// 获取页面
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<SSY_PAGE_DICT> GetPage(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams ddnmParams)
        {
            List<SSY_PAGE_DICT> listReturn = new List<SSY_PAGE_DICT>();

            DataSet ds = this.comBize.GetPage(bizobj, ddnmParams);
            if (Common.Utility.DsHasData(ds))
            {
                listReturn = Common.UtilitysForT<SSY_PAGE_DICT>.GetListsObj(ds.Tables[0]);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取页面(带分页)
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_PAGE_DICT> GetPagePager(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_PAGE_DICT> listReturn = new List<SSY_PAGE_DICT>();

            DataTable dt = this.comBize.GetPagePager(bizobj, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_PAGE_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取用户组与用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_USER_GROUP_DICT> GetGroupUserPager(SSY_USER_GROUP_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_USER_GROUP_DICT> listReturn = new List<SSY_USER_GROUP_DICT>();

            DataTable dt = this.comBize.GetGroupUserPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_USER_GROUP_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取角色权限配置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public SSY_PAGE_GROUP_MQT GetGroupPageMgt(SSY_GROUP_PAGE_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            return this.comBize.GetGroupPageMgt(model, ddnmParams);
        }

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public bool ResetUserPWD(SSY_USER_DICT model, DistributeDataNodeManagerParams ddnmParams, List<SSY_LOGENTITY> ListBizLog)
        {
            return this.comBize.ResetUserPWD(model, ddnmParams, ListBizLog);
        }

        #endregion

        #region 系统框架相关

        /// <summary>
        /// 获取系统框架参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public DataTable GetFrameParam(DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetFrameParam(distributeDataNodeManagerParams);
        }

        /// <summary>
        /// 获取系统框架参数明细
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public DataTable GetFrameParamDetail(DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetFrameParamDetail(distributeDataNodeManagerParams);
        }

        /// <summary>
        /// 获取系统树结构风格参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public DataTable GetTreeViewConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetTreeViewConfig(distributeDataNodeManagerParams);
        }

        /// <summary>
        /// 获取系统菜单风格参数
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public DataTable GetMenuConfig(DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetMenuConfig(distributeDataNodeManagerParams);
        }

        #endregion

        #region 公共部分

        /// <summary>
        /// 获取系统ID字符串
        /// </summary>
        /// <param name="makeIDType"></param>
        /// <param name="formmat">GUID格式</param>
        /// <param name="makeCustemTypeID"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public string GetID(MakeIDType makeIDType, string formmat, MakeCustemTypeID makeCustemTypeID, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetID(makeIDType, formmat, makeCustemTypeID, distributeDataNodeManagerParams);
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public string GetSystemDateTime(DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.GetSystemDateTime(distributeDataNodeManagerParams);
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
        public bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            return this.comBize.OpBizObjectSingle<T>(objLists, opObjPropertyL, wherePropertyL, mainPropertyL, errStr, distributeDataNodeManagerParams);
        }

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
        public bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams distributeDataNodeManagerParams, List<SSY_LOGENTITY> ListBizLog)
        {
            return this.comBize.OpBizObjectSingle<T>(objLists, opObjPropertyL, wherePropertyL, mainPropertyL, errStr, distributeDataNodeManagerParams,
                ListBizLog);
        }

        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="bizobjectname">数据库中的表名</param>
        /// <param name="wherePropertyL">字段|字段值,例如：prop1|value1</param>
        /// <param name="splitchar">wherePropertyL中的分割符号，默认|</param>
        /// <param name="ddnmParams"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        public bool CheckBizObjectRepat(string bizobjectname, List<string> wherePropertyL, string splitchar,
            DistributeDataNodeManagerParams ddnmParams, List<string> errStr)
        {
            return this.comBize.CheckBizObjectRepat(bizobjectname, wherePropertyL, splitchar, ddnmParams, errStr);
        }

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
        public List<string> CheckBizObjectsRepat(string bizObjectName, string fields, List<string> fieldsValue, string splitChar, string splitCharSub,
            DistributeDataNodeManagerParams ddnmParams)
        {
            return this.comBize.CheckBizObjectsRepat(bizObjectName, fields, fieldsValue, splitChar, splitCharSub, ddnmParams);
        }

        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public DataTable GetEntityAllDataForCommon(string tablename, DistributeDataNodeManagerParams ddnmParams)
        {
            return this.comBize.GetEntityAllDataForCommon(tablename, ddnmParams); 
        }

        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <param name="distributeDataNodeManagerParams"></param>
        /// <returns></returns>
        public List<SSY_DATAENTITY> GetDataEntity(string dataEntName, DistributeDataNodeManagerParams distributeDataNodeManagerParams)
        {
            List<SSY_DATAENTITY> listReturn = new List<SSY_DATAENTITY>();
            DataTable dt = this.comBize.GetDataEntity(dataEntName, distributeDataNodeManagerParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_DATAENTITY>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_LOGENTITY> GetLogDataPager(SSY_LOGENTITY model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_LOGENTITY> listReturn = new List<SSY_LOGENTITY>();

            DataTable dt = this.comBize.GetLogDataPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_LOGENTITY>.GetListsObj(dt);
            }

            return listReturn;
        }

        #endregion

        #region 字典相关

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<SSY_FRAME_DICT> GetFrameDictAll(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            List<SSY_FRAME_DICT> listReturn = new List<SSY_FRAME_DICT>();

            DataTable dt = this.comBize.GetFrameDictAll(model, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_FRAME_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_FRAME_DICT> GetFrameDictPager(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_FRAME_DICT> listReturn = new List<SSY_FRAME_DICT>();

            DataTable dt = this.comBize.GetFrameDictPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_FRAME_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<SSY_BIZ_DICT> GetBizDictAll(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            List<SSY_BIZ_DICT> listReturn = new List<SSY_BIZ_DICT>();

            DataTable dt = this.comBize.GetBizDictAll(model, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<SSY_BIZ_DICT> GetBizDictPager(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<SSY_BIZ_DICT> listReturn = new List<SSY_BIZ_DICT>();

            DataTable dt = this.comBize.GetBizDictPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">字典设别</param>
        /// <param name="dicttype">字典类型(公共、业务)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<SSY_BIZ_DICT> GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams)
        {
            List<SSY_BIZ_DICT> listReturn = new List<SSY_BIZ_DICT>();

            DataTable dt = this.comBize.GetDicts(DOMAINNAMEIDEN, dicttype, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        #endregion





        }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;

using Entitys.ComonEnti;
using Common;
using FrameCommon;


namespace Frame.ServiceInterfaces
{
    [ServiceContract(ConfigurationName = "Frame.ServiceInterfaces.FrameManagerService")]
    public interface IFrameManager
    {
        #region 升级接口

        #region 系统权限相关

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetUsersN(string req);

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetAllUsersN(string req);

        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string QuitUserForLoginN(string req);


        /// <summary>
        /// 获取功能信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPagesN(string req);

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetGroupN(string req);

        /// <summary>
        /// 获取用户组信息(带分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetGroupPagerN(string req);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetUserdictPagerN(string req);


        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPageN(string req);

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPagePagerN(string req);

        /// <summary>
        /// 操作页面，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_PAGE_DICTN(string req);

        /// <summary>
        /// 操作用户组，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_GROUP_DICTN(string req);

        /// <summary>
        /// 操作用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_USER_DICTN(string req);



        /// <summary>
        /// 获取用户组与用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetGroupUserPagerN(string req);

        /// <summary>
        /// 操作用户组与用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_GROUP_USERN(string req);


        /// <summary>
        /// 获取角色权限配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetGroupPageMgtN(string req);

        /// <summary>
        /// 操作用户组与功能权限，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_GROUP_PAGE_DICTN(string req);

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string ResetUserPWDN(string req);

        #endregion

        #region 字典相关        

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetFrameDictAllN(string req);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetFrameDictPagerN(string req);

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_FRAME_DICTN(string req);


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetBizDictAllN(string req);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetBizDictPagerN(string req);

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_BIZ_DICTN(string req);

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetDictsN(string req);

        #endregion

        #region 公共部分

        /// <summary>
        /// 获取系统ID字符串
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetIDN(string req);

        /// <summary>
        /// 获取系统服务器时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetSystemDateTimesN(string req);

        
        /// <summary>
        /// 操作通用实体，动作有增、改、删(目前仅用于日志记录)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string OpBizObjectSingle_SSY_LOGENTITYN(string req);     

        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string CheckBizObjectRepatN(string req);

        /// <summary>
        /// 通用查询数据实体是否存在数据库中,批量查询，支持分别查询一个字段、一组字段是否存在,可查询多条记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string CheckBizObjectsRepat(string req);

        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetEntityAllDataForComN(string req);



        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetLogDataPagerN(string req);

        #endregion

        #endregion


    }
}

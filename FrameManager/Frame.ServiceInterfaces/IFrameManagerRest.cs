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
    [ServiceContract(ConfigurationName = "Frame.ServiceInterfaces.FrameManagerServiceRest")]
    public interface IFrameManagerRest
    {
        #region 升级接口

        #region 系统权限相关

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetUsersN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetUsersN(string req);

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetAllUsersN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetAllUsersN(string req);

        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/QuitUserForLoginN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string QuitUserForLoginN(string req);

        /// <summary>
        /// 操作用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_USER_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_USER_DICTN(string req);


        /// <summary>
        /// 获取功能信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetPagesN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetPagesN(string req);

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetGroupN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetGroupN(string req);

        /// <summary>
        /// 获取用户组信息(带分页)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetGroupPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetGroupPagerN(string req);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetUserdictPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetUserdictPagerN(string req);


        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetPageN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetPageN(string req);

        /// <summary>
        /// 获取页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetPagePagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetPagePagerN(string req);

        /// <summary>
        /// 操作页面，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_PAGE_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_PAGE_DICTN(string req);


        /// <summary>
        /// 获取用户组与用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetGroupUserPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetGroupUserPagerN(string req);

        /// <summary>
        /// 操作用户组与用户，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_GROUP_USERN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_GROUP_USERN(string req);


        /// <summary>
        /// 操作用户组，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_GROUP_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_GROUP_DICTN(string req);


        /// <summary>
        /// 获取角色权限配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetGroupPageMgtN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetGroupPageMgtN(string req);

        /// <summary>
        /// 操作用户组与功能权限，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_GROUP_PAGE_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_GROUP_PAGE_DICTN(string req);

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/ResetUserPWDN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string ResetUserPWDN(string req);

        #endregion

        #region 字典相关        

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetFrameDictAllN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetFrameDictAllN(string req);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetFrameDictPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetFrameDictPagerN(string req);

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_FRAME_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_FRAME_DICTN(string req);


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetBizDictAllN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetBizDictAllN(string req);

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetBizDictPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetBizDictPagerN(string req);

        /// <summary>
        /// 操作业务表，动作有增、改、删
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_BIZ_DICTN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_BIZ_DICTN(string req);

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetDictsN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetDictsN(string req);

        #endregion

        #region 公共部分

        /// <summary>
        /// 获取系统ID字符串
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetIDN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetIDN(string req);

        /// <summary>
        /// 获取系统服务器时间
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetSystemDateTimesN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetSystemDateTimesN(string req);

        /// <summary>
        /// 操作通用实体，动作有增、改、删(目前仅用于日志记录)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/OpBizObjectSingle_SSY_LOGENTITYN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string OpBizObjectSingle_SSY_LOGENTITYN(string req);      


        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/CheckBizObjectRepatN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CheckBizObjectRepatN(string req);


        /// <summary>
        /// 通用查询数据实体是否存在数据库中,批量查询，支持分别查询一个字段、一组字段是否存在,可查询多条记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/CheckBizObjectsRepat", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string CheckBizObjectsRepat(string req);
       



        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetEntityAllDataForComN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetEntityAllDataForComN(string req);



        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetLogDataPagerN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetLogDataPagerN(string req);

        #endregion

        #endregion
    }
}

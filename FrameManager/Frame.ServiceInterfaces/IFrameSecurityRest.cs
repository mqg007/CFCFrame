using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;
using System.IO;

using Entitys.ComonEnti;
using Common;

namespace Frame.ServiceInterfaces
{
    [ServiceContract(ConfigurationName = "Frame.ServiceInterfaces.FrameSecurityServiceRest")]
    public interface IFrameSecurityRest
    {
        #region 升级接口

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetUserForLoginN", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetUserForLoginN(string req);

        /// <summary>
        /// 获取用户信息仅仅为获取令牌验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetUserForLogin2N", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string GetUserForLogin2N(string model);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FrameApi/GetVerifyCodePic", BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        byte[] GetVerifyCodePic(string req);

        #endregion



    }
}

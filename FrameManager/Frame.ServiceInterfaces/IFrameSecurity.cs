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
    [ServiceContract(ConfigurationName = "Frame.ServiceInterfaces.FrameSecurityService")]
    public interface IFrameSecurity
    {
        #region 升级接口

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        string GetUserForLoginN(string req);

        /// <summary>
        /// 获取用户信息仅仅为获取令牌验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        string GetUserForLogin2N(string req);

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] GetVerifyCodePic(string req);

        #endregion


    }
}

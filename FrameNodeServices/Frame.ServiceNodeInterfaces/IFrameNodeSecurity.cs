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

namespace Frame.ServiceNodeInterfaces
{
    [ServiceContract(ConfigurationName = "Frame.ServiceNodeInterfaces.FrameNodeSecurityService")]
    public interface IFrameNodeSecurity
    {
        #region 升级接口

        /// <summary>
        /// 获取框架环境
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>        
        [OperationContract]
        string GetFrameParams(string req);

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetToken(string req);

        /// <summary>
        /// 检查令牌
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string CheckToken(string req);


        /// <summary>
        /// 加载节点服务地址,仅仅加载目前可用节点(包含业务和数据)
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        [OperationContract]
        string GetDataNodeCollection(string req);

        /// <summary>
        /// 加载节点服务地址,仅仅加载目前可用节点
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetNodeCollection(string req);

        #endregion
    }

    
}

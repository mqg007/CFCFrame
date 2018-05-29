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
    [ServiceContract(ConfigurationName = "Frame.ServiceNodeInterfaces.FrameManagerNodeService")]
    public interface IFrameManagerNode
    {
        #region 升级接口

        /// <summary>
        /// 向中心节点报告同源单点数据操作失败任务
        /// 节点中心后续会继续进行补充完成数据操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns>0 成功  其他失败</returns>
        [OperationContract]
        string ReportSameDataActionTaskN(string req);

        /// <summary>
        /// 获取节点中心同源单点数据操作任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetSameDataActionTaskStatusN(string req);

        /// <summary>
        /// 向节点中心报告业务节点、数据节点异常情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns>0 成功  其他失败</returns>
        [OperationContract]
        string RecordNodeErrorLogN(string req);

        /// <summary>
        /// 查询节点异常情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetNodeErrorLogsN(string req);

        /// <summary>
        /// 加载节点运行状态，包括全部节点（可用或不可用）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        string GetNodeRunStatuN(string req);

        #endregion



    }
}

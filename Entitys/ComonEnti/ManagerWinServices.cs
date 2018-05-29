using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 管理windows服务
    /// </summary>
    [DataContract]
    public class ManagerWinServices
    {
        [DataMember]
        public string WinServiceName
        {
            get;
            set;
        }

        [DataMember]
        public string WinServiceDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 服务操作 S 启动  E 停止
        /// </summary>
        [DataMember]
        public string OpAction
        {
            get;
            set;
        }

        /// <summary>
        /// 服务路径
        /// </summary>
        [DataMember]
        public string WinServicePath
        {
            get;
            set;
        }

    }
}

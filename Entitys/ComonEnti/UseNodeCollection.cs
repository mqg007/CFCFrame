using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 可用节点集合，包括业务节点和数据节点
    /// </summary>
    [DataContract]
    public class UseNodeCollection
    {
        [DataMember]
        public List<SSY_BIZNODE_ADDR> BizNodeList
        {
            get;
            set;
        }

        [DataMember]
        public List<SSY_DATANODE_ADDR> DataNodeList
        {
            get;
            set;
        }
    }
}

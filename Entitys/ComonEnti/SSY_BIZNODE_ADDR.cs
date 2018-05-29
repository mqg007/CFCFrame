using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 业务节点地址
    /// </summary>
    [DataContract]
    public class SSY_BIZNODE_ADDR
    {
        [DataMember]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Url_addr
        {
            get;
            set;
        }

        [DataMember]
        public string Moudiden
        {
            get;
            set;
        }        

        [DataMember]
        public string Use_status
        {
            get;
            set;
        }

        [DataMember]
        public string Remarks
        {
            get;
            set;
        }

        [DataMember]
        public string Timestampss
        {
            get;
            set;
        }
    }
}

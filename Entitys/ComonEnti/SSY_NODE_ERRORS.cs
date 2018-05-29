using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 节点异常日志
    /// </summary>
    [DataContract]
    public class SSY_NODE_ERRORS
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
        public string Node_typs
        {
            get;
            set;
        }

        [DataMember]
        public string Error_desc
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

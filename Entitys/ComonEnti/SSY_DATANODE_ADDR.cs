using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 数据节点地址
    /// </summary>
    [DataContract]
    public class SSY_DATANODE_ADDR
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
        public string Use_status
        {
            get;
            set;
        }

        [DataMember]
        public string Data_schema
        {
            get;
            set;
        }

        [DataMember]
        public string Data_user
        {
            get;
            set;
        }

        [DataMember]
        public string Data_password
        {
            get;
            set;
        }

        [DataMember]
        public string Data_conn
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

        [DataMember]
        public string DBFactoryName
        {
            get;
            set;
        }

        [DataMember]
        public string Systemname { get; set;}     

        [DataMember]
        public string Isencrydbconn { get; set; }

        [DataMember]
        public string Isencrypwd { get; set; }

        [DataMember]
        public string Encryhashlenth { get; set; }

        [DataMember]
        public string Encrykeystr { get; set; }

        [DataMember]
        public string Isusepwdsecuritycheck { get; set; }

        [DataMember]
        public string Pwdintervalhours { get; set; }

        [DataMember]
        public string Pwdfirstcheck { get; set; }
    }
}

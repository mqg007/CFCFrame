using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 站点下服务
    /// </summary>
    [DataContract]
    public class SSY_SERVICESITE_SERVICES
    {
        [DataMember]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Sitecode
        {
            get;
            set;
        }

        [DataMember]
        public string Servicecode
        {
            get;
            set;
        }

        [DataMember]
        public string Servicename
        {
            get;
            set;
        }

        [DataMember]
        public string Service_relaUrl
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 服务站点
    /// </summary>
    [DataContract]
    public class SSY_SERVICESITE
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
        public string Sitename
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

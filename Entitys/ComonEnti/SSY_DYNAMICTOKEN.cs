using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 动态令牌
    /// </summary>
    [DataContract]
    public class SSY_DYNAMICTOKEN
    {
        [DataMember]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Dynamictoken
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

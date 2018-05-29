using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Xxx.Entities
{
    [DataContract]
    public class SSY_FRAME_DICT
    {
        [DataMember]
        public object SSY_FRAME_DICTID { get; set; }

        [DataMember]
        public object DOMAINNAMEIDEN { get; set; }

        [DataMember]
        public object DOMAINNAMES { get; set; }

        [DataMember]
        public object OPTIONIDEN { get; set; }

        [DataMember]
        public object OPTIONNAMES { get; set; }

        [DataMember]
        public object OPTIONIDEN_CUT { get; set; }

        [DataMember]
        public object OPTIONNAMES_CUT { get; set; }

        [DataMember]
        public object PYM { get; set; }

        [DataMember]
        public object REMARKS { get; set; }

        [DataMember]
        public object TIMESTAMPSS { get; set; }

        [DataMember]
        public string SequenceXXX { get; set; }

        /// <summary>
        /// OPFlag(I 增，U 改, D删)
        /// </summary>
        [DataMember]
        public string OPFlag { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 同源数据节点任务
    /// </summary>
    [DataContract]
    public class SSY_DATA_ACTION_TASK
    {
        [DataMember]
        public string ID
        {
            get;
            set;
        }

        [DataMember]
        public string Data_real_conn
        {
            get;
            set;
        }

        [DataMember]
        public string Action_sql
        {
            get;
            set;
        }

        [DataMember]
        public string Action_sql_params
        {
            get;
            set;
        }


        [DataMember]
        public string Action_status
        {
            get;
            set;
        }

        [DataMember]
        public string Execute_cnt
        {
            get;
            set;
        }

        [DataMember]
        public string Max_execute_cnt
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

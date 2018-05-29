using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    /// <summary>
    /// 系统公共数据表结构
    /// </summary>
    public class SSY_DATAENTITY
    {
        /// <summary>
        /// 数据表名
        /// </summary>
        public object TABLE_NAME { set; get; }

        /// <summary>
        /// 列名
        /// </summary>
        public object COLUMN_NAME { set; get; }

        /// <summary>
        /// 列数据类型
        /// </summary>
        public object COLTYPE { set; get; }

        /// <summary>
        /// 列注释
        /// </summary>
        public object COMMENTS { set; get; }

        /// <summary>
        /// 列长度
        /// </summary>
        public object DATA_LENGTH { set; get; }

        /// <summary>
        /// 列是否允许空
        /// </summary>
        public object NULLABLE { set; get; }


    }
}

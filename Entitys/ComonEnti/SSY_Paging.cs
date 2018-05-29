using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Entitys.ComonEnti
{            
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]    
    public class SSY_PagingResult<T>
    {
        [DataMember]
        public int TotalSize
        {
            get;
            set;
        }
        [DataMember]
        public T Obj
        {
            get;
            set;
        }

        [DataMember]
        public int PageIndex
        {
            get;
            set;
        }

        [DataMember]
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 分页集合对象
        /// </summary>
        /// <param name="obj">集合对象</param>
        /// <param name="pageIndex">页大小</param>
        /// <param name="totalSize">集合对象在数据库中条数</param>
        public SSY_PagingResult(T obj, int pageIndex, int totalSize)
        {
            this.Obj = obj;
            this.PageIndex = pageIndex;
            this.TotalSize = totalSize;
        }
    }

    
}

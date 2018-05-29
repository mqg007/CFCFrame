using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    [DataContract]
    public  class SSY_ControlPage
    {
        #region 页大小
        [DataMember]
        /// <summary>
        /// 页大小
        /// <summary>
        private int _pageSize = 0;
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }
        #endregion

        #region 记录总数
        [DataMember]
        /// <summary>
        /// 记录总数
        /// <summary>
        private int _inRecordCnt = 0;
        public int InRecordCnt
        {
            get
            {
                return this._inRecordCnt;
            }
            set
            {
                this._inRecordCnt = value;
            }
        }
        #endregion

        #region 当前页数
        [DataMember]
        /// <summary>
        /// 当前页数
        /// <summary>
        private int _pageIndex = 0;
        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }
        #endregion       


    }
}

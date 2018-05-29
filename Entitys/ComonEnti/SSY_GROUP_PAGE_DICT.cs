using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_GROUP_PAGE_DICT
    {
        #region GROUPID
        /// <summary>
        /// GROUPID
        /// <summary>
        private object _gROUPID = null;
        public object GROUPID
        {
            get
            {
                return this._gROUPID;


            }
            set
            {
                this._gROUPID = value;
            }
        }
        #endregion

        #region PAGEID
        /// <summary>
        /// PAGEID
        /// <summary>
        private object _pAGEID = null;
        public

        object PAGEID
        {
            get
            {
                return this._pAGEID;
            }
            set
            {
                this._pAGEID = value;
            }
        }
        #endregion

        #region TIMESTAMPSS
        /// <summary>
        /// TIMESTAMPSS
        /// <summary>
        private object _tIMESTAMP = null;
        public object TIMESTAMPSS
        {
            get
            {
                return this._tIMESTAMP;
            }
            set
            {


                this._tIMESTAMP = value;
            }
        }
        #endregion

        #region OPFlag
        /// <summary>
        /// OPFlag(I 增，U 改, D删)
        /// <summary>
        private string _oPFlag = string.Empty;
        public string OPFlag
        {
            get
            {
                return this._oPFlag;
            }
            set
            {
                this._oPFlag = value;
            }
        }
        #endregion


    }

    public class SSY_PAGE_GROUP_MQT
    {
        public object GROUPID { get; set; }

        public object SSY_PAGE_DICT { get; set; }

        public List<SSY_GROUP_PAGE_DICT> SSY_GROUP_PAGE_DICT { get; set; }    
    }
}

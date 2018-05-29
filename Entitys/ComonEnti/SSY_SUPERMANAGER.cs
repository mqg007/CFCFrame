using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_SUPERMANAGER
    {
        #region USERID
        /// <summary>
        /// USERID
        /// <summary>
        private object _uSERID = null;
        public object USERID
        {
            get
            {
                return this._uSERID;
            }


            set
            {
                this._uSERID = value;
            }
        }
        #endregion

        #region USERNAME
        /// <summary>
        /// USERNAME
        /// <summary>
        private object _uSERNAME = null;
        public object USERNAME
        {
            get
            {
                return this._uSERNAME;
            }
            set
            {
                this._uSERNAME = value;
            }
        }
        #endregion

        #region PASSWORD
        /// <summary>
        /// PASSWORD
        /// <summary>
        private object _pASSWORD = null;
        public object PASSWORD
        {
            get
            {
                return this._pASSWORD;
            }
            set
            {


                this._pASSWORD = value;
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
}

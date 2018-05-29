using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_LOGCONTROL
    {
        #region LOGCONTROLID
        /// <summary>
        /// LOGCONTROLID
        /// <summary>
        private object _lOGCONTROLID = null;
        public object LOGCONTROLID
        {
            get
            {
                return this._lOGCONTROLID;
            }
            set
            {
                this._lOGCONTROLID = value;
            }
        }
        #endregion

        #region DOMAINIDEN
        /// <summary>
        /// DOMAINIDEN
        /// <summary>
        private object _dOMAINIDEN = null;
        public object DOMAINIDEN
        {
            get
            {
                return this._dOMAINIDEN;
            }
            set
            {
                this._dOMAINIDEN = value;
            }
        }
        #endregion

        #region OPTIONIDEN
        /// <summary>
        /// OPTIONIDEN
        /// <summary>
        private object _oPTIONIDEN = null;
        public object OPTIONIDEN
        {
            get
            {
                return this._oPTIONIDEN;
            }
            set
            {
                this._oPTIONIDEN = value;
            }
        }
        #endregion

        #region ISRECORD
        /// <summary>
        /// ISRECORD
        /// <summary>
        private object _iSRECORD = null;
        public object ISRECORD
        {
            get
            {
                return this._iSRECORD;
            }
            set
            {
                this._iSRECORD = value;
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

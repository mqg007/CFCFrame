using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_LOGCONFIG
    {
        #region LOGCONFIGID
        /// <summary>
        /// LOGCONFIGID
        /// <summary>
        private object _lOGCONFIGID = null;
        public object LOGCONFIGID
        {
            get
            {
                return this._lOGCONFIGID;
            }
            set
            {
                this._lOGCONFIGID = value;
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

        #region DOMAINNAME
        /// <summary>
        /// DOMAINNAME
        /// <summary>
        private object _dOMAINNAME = null;
        public object DOMAINNAME
        {
            get
            {
                return this._dOMAINNAME;
            }
            set
            {
                this._dOMAINNAME = value;
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

        #region OPTIONNAME
        /// <summary>
        /// OPTIONNAME
        /// <summary>
        private object _oPTIONNAME = null;
        public object OPTIONNAME
        {
            get
            {
                return this._oPTIONNAME;
            }
            set
            {
                this._oPTIONNAME = value;
            }
        }
        #endregion

        #region REMARKS
        /// <summary>
        /// REMARKS
        /// <summary>
        private object _rEMARKS = null;
        public object REMARKS
        {
            get
            {
                return this._rEMARKS;
            }
            set
            {
                this._rEMARKS = value;
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

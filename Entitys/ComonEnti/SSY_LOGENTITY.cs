using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_LOGENTITY
    {
        #region LOGID
        /// <summary>
        /// LOGID
        /// <summary>
        private object _lOGID = null;
        public object LOGID
        {
            get
            {
                return this._lOGID;
            }
            set
            {
                this._lOGID = value;
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

        #region RECORDTIME
        /// <summary>
        /// RECORDTIME
        /// <summary>
        private object _rECORDTIME = null;
        public object RECORDTIME
        {
            get
            {
                return this._rECORDTIME;
            }
            set
            {
                this._rECORDTIME = value;
            }
        }
        #endregion

        #region CLASSNAME
        /// <summary>
        /// CLASSNAME
        /// <summary>
        private object _cLASSNAME = null;
        public object CLASSNAME
        {
            get
            {
                return this._cLASSNAME;
            }
            set
            {
                this._cLASSNAME = value;
            }
        }
        #endregion

        #region METHORDNAME
        /// <summary>
        /// METHORDNAME
        /// <summary>
        private object _mETHORDNAME = null;
        public object METHORDNAME
        {
            get
            {
                return this._mETHORDNAME;
            }
            set
            {
                this._mETHORDNAME = value;
            }
        }
        #endregion

        #region OPERATERIDEN
        /// <summary>
        /// OPERATERIDEN
        /// <summary>
        private object _oPERATERIDEN = null;
        public object OPERATERIDEN
        {
            get
            {
                return this._oPERATERIDEN;
            }
            set
            {
                this._oPERATERIDEN = value;
            }
        }
        #endregion

        #region TABLENAME
        /// <summary>
        /// TABLENAME
        /// <summary>
        private object _tABLENAME = null;
        public object TABLENAME
        {
            get
            {
                return this._tABLENAME;
            }
            set
            {
                this._tABLENAME = value;
            }
        }
        #endregion

        #region RECORDIDENCOLS
        /// <summary>
        /// RECORDIDENCOLS
        /// <summary>
        private object _rECORDIDENCOLS = null;
        public object RECORDIDENCOLS
        {
            get
            {
                return this._rECORDIDENCOLS;
            }
            set
            {
                this._rECORDIDENCOLS = value;
            }
        }
        #endregion

        #region RECORDIDENCOLSVALUES
        /// <summary>
        /// RECORDIDENCOLSVALUES
        /// <summary>
        private object _rECORDIDENCOLSVALUES = null;
        public object RECORDIDENCOLSVALUES
        {
            get
            {
                return this._rECORDIDENCOLSVALUES;
            }
            set
            {
                this._rECORDIDENCOLSVALUES = value;
            }
        }
        #endregion

        #region USERNAMES
        /// <summary>
        /// USERNAMES
        /// <summary>
        private object _uSERNAMES = null;
        public object USERNAMES
        {
            get
            {
                return this._uSERNAMES;
            }
            set
            {
                this._uSERNAMES = value;
            }
        }
        #endregion

        #region IPS
        /// <summary>
        /// IPS
        /// <summary>
        private object _iPS = null;
        public object IPS
        {
            get
            {
                return this._iPS;
            }
            set
            {
                this._iPS = value;
            }
        }
        #endregion

        #region FUNCTIONNAME
        /// <summary>
        /// FUNCTIONNAME
        /// <summary>
        private object _fUNCTIONNAME = null;
        public object FUNCTIONNAME
        {
            get
            {
                return this._fUNCTIONNAME;
            }
            set
            {
                this._fUNCTIONNAME = value;
            }
        }
        #endregion

        #region DESCRIPTIONS
        /// <summary>
        /// DESCRIPTIONS
        /// <summary>
        private object _dESCRIPTIONS = null;
        public object DESCRIPTIONS
        {
            get
            {
                return this._dESCRIPTIONS;
            }
            set
            {
                this._dESCRIPTIONS = value;
            }
        }
        #endregion

        #region SYSTEMNAME
        /// <summary>
        /// SYSTEMNAME
        /// <summary>
        private object _sYSTEMNAME = null;
        public object SYSTEMNAME
        {
            get
            {
                return this._sYSTEMNAME;
            }
            set
            {
                this._sYSTEMNAME = value;
            }
        }
        #endregion

        #region SFLAG
        /// <summary>
        /// SFLAG
        /// <summary>
        private object _sFLAG = null;
        public object SFLAG
        {
            get
            {
                return this._sFLAG;
            }
            set
            {
                this._sFLAG = value;
            }
        }
        #endregion      
        
        public object TIMESTAMPSS { set; get; }

        #region SequenceXXX
        /// <summary>
        /// SequenceXXX
        /// <summary>
        private string _sequenceXXX = string.Empty;
        public string SequenceXXX
        {
            get
            {
                return this._sequenceXXX;
            }
            set
            {
                this._sequenceXXX = value;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_USER_DICT
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

        #region REGISTERDATE
        /// <summary>
        /// REGISTERDATE
        /// <summary>
        private object _rEGISTERDATE = null;
        public object REGISTERDATE
        {
            get
            {
                return this._rEGISTERDATE;
            }
            set
            {
                this._rEGISTERDATE = value;
            }
        }


        #endregion

        #region TELEPHONE
        /// <summary>
        /// TELEPHONE
        /// <summary>
        private object _tELEPHONE = null;
        public object TELEPHONE
        {
            get
            {
                return this._tELEPHONE;


            }
            set
            {
                this._tELEPHONE = value;
            }
        }
        #endregion

        #region EMAIL
        /// <summary>
        /// EMAIL
        /// <summary>
        private object _eMAIL = null;
        public object EMAIL
        {
            get
            {
                return this._eMAIL;
            }
            set
            {
                this._eMAIL = value;
            }
        }
        #endregion

        #region ISUSE
        /// <summary>
        /// ISUSE
        /// <summary>
        private object _iSUSE = null;
        public object ISUSE
        {
            get
            {
                return this._iSUSE;
            }
            set
            {
                this._iSUSE = value;


            }
        }
        #endregion

        #region ISFLAG
        /// <summary>
        /// ISFLAG
        /// <summary>
        private object _iSFLAG = null;
        public object ISFLAG
        {
            get
            {
                return this._iSFLAG;
            }
            set
            {
                this._iSFLAG = value;
            }
        }
        #endregion

        #region NOTE
        /// <summary>
        /// NOTE
        /// <summary>
        private object _nOTE = null;
        public object NOTE
        {
            get
            {
                return this._nOTE;
            }
            set
            {
                this._nOTE = value;
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

        public string ISLONIN { get; set; }


        public string FROMPLAT { get; set; }

        public string LASTLOGINTIME { get; set; }

        public string ISFIRSTLOGIN { get; set; }

        public string FAILTCNT { get; set; }

        public string LOCKED { get; set; }

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

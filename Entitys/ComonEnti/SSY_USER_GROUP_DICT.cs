using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_USER_GROUP_DICT
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

        #region GROUPNAME
        /// <summary>
        /// GROUPNAME
        /// <summary>
        private object _gROUPNAME = null;
        public object GROUPNAME
        {
            get
            {
                return this._gROUPNAME;
            }
            set
            {
                this._gROUPNAME = value;
            }
        }


        #endregion

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_GROUP_DICT
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

        #region  GROUPDESC
        /// <summary>
        /// GROUPDESC
        /// <summary>
        private object _gROUPDESC = null;
        public object GROUPDESC
        {
            get
            {
                return this._gROUPDESC;


            }
            set
            {
                this._gROUPDESC = value;
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
                this._iSFLAG =

        value;
            }
        }
        #endregion

        #region GROUPNO
        /// <summary>
        /// GROUPNO
        /// <summary>
        private object _gROUPNO = null;
        public object GROUPNO
        {
            get
            {


                return this._gROUPNO;
            }
            set
            {
                this._gROUPNO = value;
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

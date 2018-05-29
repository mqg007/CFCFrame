using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.ComonEnti
{
    public class SSY_APPFRAME_CFG
    {
        #region SSY_APPFRAME_ID
        /// <summary>
        /// SSY_APPFRAME_ID
        /// <summary>
        private object _sSY_APPFRAME_ID = null;
        public object SSY_APPFRAME_ID
        {
            get
            {
                return this._sSY_APPFRAME_ID;
            }
            set
            {
                this._sSY_APPFRAME_ID = value;
            }
        }
        #endregion

        #region OPTIONIDENTIFIED
        /// <summary>
        /// OPTIONIDENTIFIED
        /// <summary>
        private object _oPTIONIDENTIFIED = null;
        public object OPTIONIDENTIFIED
        {
            get
            {
                return this._oPTIONIDENTIFIED;
            }
            set
            {
                this._oPTIONIDENTIFIED = value;
            }
        }
        #endregion

        #region IDS
        /// <summary>
        /// IDS
        /// <summary>
        private object _iDS = null;
        public object IDS
        {
            get
            {
                return this._iDS;
            }
            set
            {
                this._iDS = value;
            }
        }


        #endregion

        #region DISPLAYTEXT
        /// <summary>
        /// DISPLAYTEXT
        /// <summary>
        private object _dISPLAYTEXT = null;
        public object DISPLAYTEXT
        {
            get
            {
                return this._dISPLAYTEXT;
            }
            set
            {
                this._dISPLAYTEXT = value;
            }
        }
        #endregion

        #region HIDEVALUE
        /// <summary>
        /// HIDEVALUE
        /// <summary>
        private object _hIDEVALUE = null;
        public object HIDEVALUE
        {
            get
            {
                return this._hIDEVALUE;
            }
            set
            {
                this._hIDEVALUE = value;
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

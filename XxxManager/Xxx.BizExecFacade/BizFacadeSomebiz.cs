using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Xxx.Biz;
using Xxx.BizFactory;
using Entitys.ComonEnti;
using Common;
using FrameCommon;
using Xxx.Entities;

namespace Xxx.BizExecFacade
{
    public class BizFacadeSomebiz
    {
        BizFactorySomebiz comSomebizF = new BizFactorySomebiz();
        Somebiz comSomebiz = null;

        public string err = string.Empty;

        public BizFacadeSomebiz(SysEnvironmentSerialize _envirObj)
        {
            string _currlang = _envirObj.I18nCurrLang;
            try
            {
                this.comSomebiz = this.comSomebizF.Create("one", _envirObj);
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }
        }

        #region 字典相关

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<Xxx.Entities.SSY_FRAME_DICT> GetFrameDictAll(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            List<Xxx.Entities.SSY_FRAME_DICT> listReturn = new List<Xxx.Entities.SSY_FRAME_DICT>();

            DataTable dt = this.comSomebiz.GetFrameDictAll(model, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<Xxx.Entities.SSY_FRAME_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<Xxx.Entities.SSY_FRAME_DICT> GetFrameDictPager(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<Xxx.Entities.SSY_FRAME_DICT> listReturn = new List<Xxx.Entities.SSY_FRAME_DICT>();

            DataTable dt = this.comSomebiz.GetFrameDictPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<Xxx.Entities.SSY_FRAME_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<Xxx.Entities.SSY_BIZ_DICT> GetBizDictAll(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            List<Xxx.Entities.SSY_BIZ_DICT> listReturn = new List<Xxx.Entities.SSY_BIZ_DICT>();

            DataTable dt = this.comSomebiz.GetBizDictAll(model, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<Xxx.Entities.SSY_BIZ_DICT> GetBizDictPager(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            List<Xxx.Entities.SSY_BIZ_DICT> listReturn = new List<Xxx.Entities.SSY_BIZ_DICT>();

            DataTable dt = this.comSomebiz.GetBizDictPager(model, ddnmParams, pager);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">字典设别</param>
        /// <param name="dicttype">字典类型(公共、业务)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public List<Xxx.Entities.SSY_BIZ_DICT> GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams)
        {
            List<Xxx.Entities.SSY_BIZ_DICT> listReturn = new List<Xxx.Entities.SSY_BIZ_DICT>();

            DataTable dt = this.comSomebiz.GetDicts(DOMAINNAMEIDEN, dicttype, ddnmParams);
            if (Common.Utility.DtHasData(dt))
            {
                listReturn = Common.UtilitysForT<Xxx.Entities.SSY_BIZ_DICT>.GetListsObj(dt);
            }

            return listReturn;
        }

        #endregion
    }


}

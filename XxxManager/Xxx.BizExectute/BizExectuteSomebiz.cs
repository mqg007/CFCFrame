using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

using Xxx.Biz;
using Entitys.ComonEnti;
using DataAccessLayer.DataBaseFactory;
using Common;
using FrameCommon;
using Xxx.Entities;

namespace Xxx.BizExectute
{
    public class BizExectuteSomebiz : Somebiz
    {
        public BizExectuteSomebiz(SysEnvironmentSerialize _envirObj): base(_envirObj)
        {
            
        }

        #region 字典相关

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetFrameDictAll(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                sbb.AppendLine(string.Format(@" select DOMAINNAMEIDEN, DOMAINNAMES, sum(1) dicts FROM {0}.SSY_FRAME_DICT group by DOMAINNAMEIDEN, DOMAINNAMES",
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));
                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, null);
            }
            catch (Exception ex)
            {
                return null;
            }

            if (Common.Utility.DsHasData(ds))
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetFrameDictPager(Xxx.Entities.SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            //分页数据获取实现
            //分页参数数据
            SSY_PagingExecuteParam pageExecute = new SSY_PagingExecuteParam();
            pageExecute.PagingParam = pager;

            //参数值，若有，增加到该集合
            List<IDataParameter> parameters = new List<IDataParameter>();
            StringBuilder sbbSqlWhere = new StringBuilder();
            if (model != null)
            {
                if (Utility.ObjHasData(model.DOMAINNAMEIDEN))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and DOMAINNAMEIDEN = {0}DOMAINNAMEIDEN ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("DOMAINNAMEIDEN",
                    DbType.String, model.DOMAINNAMEIDEN.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.SSY_FRAME_DICT ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "SSY_FRAME_DICTID";
            pageExecute.SqlWhere = " 1=1 " + sbbSqlWhere.ToString();

            StringBuilder sbbSql = new StringBuilder();
            if (pager.TotalSize == 0)
            {
                //首次计算总记录
                sbbSql.Clear();
                if (string.IsNullOrEmpty(pageExecute.SqlWhere))
                {
                    sbbSql.Append(string.Format(@"SELECT count(*) as cnt FROM {0} {1} ", pageExecute.TableNameOrView, pageExecute.Joins));
                }
                else
                {
                    sbbSql.Append(string.Format(@"SELECT count(*) as cnt FROM {0} {1} where {2} ", pageExecute.TableNameOrView, pageExecute.Joins,
                        pageExecute.SqlWhere));
                }

                DataTable dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbbSql.ToString(),
                    SqlExecType.SqlText, parameters.ToArray());

                if (Utility.DtHasData(dt))
                {
                    pager.TotalSize = int.Parse(dt.Rows[0]["cnt"].ToString());
                }
            }

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataPager(pageExecute, parameters.ToArray());
        }


        /// <summary>
        /// 获取全部业务字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetBizDictAll(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                sbb.AppendLine(string.Format(@" select DOMAINNAMEIDEN, DOMAINNAMES, sum(1) dicts FROM {0}.SSY_BIZ_DICT group by DOMAINNAMEIDEN, DOMAINNAMES",
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));
                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, null);
            }
            catch (Exception ex)
            {
                return null;
            }

            if (Common.Utility.DsHasData(ds))
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统字典(某个字典的全部字典项)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetBizDictPager(Xxx.Entities.SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            //分页数据获取实现
            //分页参数数据
            SSY_PagingExecuteParam pageExecute = new SSY_PagingExecuteParam();
            pageExecute.PagingParam = pager;

            //参数值，若有，增加到该集合
            List<IDataParameter> parameters = new List<IDataParameter>();
            StringBuilder sbbSqlWhere = new StringBuilder();
            if (model != null)
            {
                if (Utility.ObjHasData(model.DOMAINNAMEIDEN))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and DOMAINNAMEIDEN = {0}DOMAINNAMEIDEN ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("DOMAINNAMEIDEN",
                    DbType.String, model.DOMAINNAMEIDEN.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.SSY_BIZ_DICT ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "SSY_BIZ_DICTID";
            pageExecute.SqlWhere = " 1=1 " + sbbSqlWhere.ToString();

            StringBuilder sbbSql = new StringBuilder();
            if (pager.TotalSize == 0)
            {
                //首次计算总记录
                sbbSql.Clear();
                if (string.IsNullOrEmpty(pageExecute.SqlWhere))
                {
                    sbbSql.Append(string.Format(@"SELECT count(*) as cnt FROM {0} {1} ", pageExecute.TableNameOrView, pageExecute.Joins));
                }
                else
                {
                    sbbSql.Append(string.Format(@"SELECT count(*) as cnt FROM {0} {1} where {2} ", pageExecute.TableNameOrView, pageExecute.Joins,
                        pageExecute.SqlWhere));
                }

                DataTable dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbbSql.ToString(),
                    SqlExecType.SqlText, parameters.ToArray());

                if (Utility.DtHasData(dt))
                {
                    pager.TotalSize = int.Parse(dt.Rows[0]["cnt"].ToString());
                }
            }

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataPager(pageExecute, parameters.ToArray());
        }


        /// <summary>
        /// 获取某个字典
        /// </summary>
        /// <param name="DOMAINNAMEIDEN">字典设别</param>
        /// <param name="dicttype">字典类型(公共、业务)</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetDicts(string DOMAINNAMEIDEN, DictType dicttype, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            string dictname = string.Empty;

            if (dicttype == DictType.BizDict)
            {
                dictname = "SSY_BIZ_DICT";
            }
            else if (dicttype == DictType.FrameDict)
            {
                dictname = "SSY_FRAME_DICT";
            }
            else
            {
                //throw new Exception("必须设置字典类型！");
                throw new Exception(BaseServiceUtility.GetI18nLangItem("noticeinfo_setDictType", base.i18nXxxManageri18nLang));
            }

            if (string.IsNullOrEmpty(DOMAINNAMEIDEN))
            {
                //throw new Exception("必须设置字典设别！");
                throw new Exception(BaseServiceUtility.GetI18nLangItem("noticeinfo_setDictIden", base.i18nXxxManageri18nLang));
            }

            List<IDataParameter> parameters = new List<IDataParameter>();

            try
            {
                sbb.AppendLine(string.Format(@" select * FROM {0}.{1} where DOMAINNAMEIDEN = {2}DOMAINNAMEIDEN",
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                    dictname,
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("DOMAINNAMEIDEN", DbType.String, DOMAINNAMEIDEN));

                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, null);
            }
            catch (Exception ex)
            {
                return null;
            }

            if (Common.Utility.DsHasData(ds))
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}

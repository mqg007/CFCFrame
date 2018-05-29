using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;

using Biz;
using Entitys.ComonEnti;
using DataAccessLayer.DataBaseFactory;
using Common;
using FrameCommon;

namespace BizExectute
{
    public class BizExectuteCommon : CommonBiz
    {
        public BizExectuteCommon(SysEnvironmentSerialize _envirObj): base(_envirObj)
        {
            //string _currlang = _envirObj.I18nCurrLang;
        }

        #region 系统权限相关

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="ddnmParams">分布式管理参数，必须存在， 分布式动作参数由业务处理决定赋值</param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public override DataTable GetUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams ddnmParams,
            List<SSY_LOGENTITY> ListBizLog)
        {
            DataTable dt = null;
            try
            {
                StringBuilder sbb = new StringBuilder();
                if (ud.USERID.ToString().ToUpper() == "super".ToUpper())
                {
                    sbb.AppendLine(string.Format(@"SELECT USERID,
                                USERNAME,
                                PASSWORD,
                                '' REGISTERDATE,
                                '' TELEPHONE,
                                '' EMAIL,
                                '' ISUSE,
                                '' ISFLAG,
                                '' NOTE,
                                '' ISLONIN,
                                '' FROMPLAT,
                                '' LASTLOGINTIME,
                                '' FAILTCNT,
                                '' LOCKED,
                                '' ISFIRSTLOGIN,   '' LOCKED,  '' FAILTCNT
                                FROM {0}.ssy_supermanager t where 1=1 ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));
                }
                else
                {
                    sbb.AppendLine(string.Format(@"SELECT USERID,
                                                 USERNAME,
                                                 PASSWORD,
                                                 REGISTERDATE,
                                                 TELEPHONE,
                                                 EMAIL,
                                                 ISUSE,
                                                 ISFLAG,
                                                 NOTE,
                                                 ISLONIN,
                                                 FROMPLAT, LASTLOGINTIME, ISFIRSTLOGIN, LOCKED, FAILTCNT
                                            FROM {0}.ssy_user_dict t WHERE 1=1  ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

                }

                //口令后续验证，先查询用户用于判断其是否锁定、更新试错次数
                sbb.AppendLine(string.Format(@"and t.userid = {0}userid ",
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                List<IDataParameter> parameters = new List<IDataParameter>();
                if (ud != null)
                {
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                        DbType.String, ud.USERID.ToString()));
                }
                dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

                if (Common.Utility.DtHasData(dt))
                {
                    if (ud.USERID.ToString().ToUpper() != "super".ToUpper())
                    {

                        if (ud.PASSWORD.ToString() != dt.Rows[0]["PASSWORD"].ToString())
                        {
                            #region 记录试错次数,超过次数进行锁定用户

                            int failtCnt = int.Parse(dt.Rows[0]["FAILTCNT"].ToString());  //登录错误次数     
                            StringBuilder sbbErr = new StringBuilder();
                            List<IDataParameter> parametersUPErr = new List<IDataParameter>();

                            if (failtCnt < int.Parse(this.permitMaxLoginFailtCnt))
                            {
                                if (failtCnt + 1 == 5)
                                {
                                    //更新错误次数同时锁定
                                    sbbErr.AppendLine(string.Format(@"update  {0}.ssy_user_dict t  set t.FAILTCNT= {1}FAILTCNT, t.LOCKED= {1}LOCKED  WHERE t.userid = {1}userid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                                    parametersUPErr.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "FAILTCNT",
                                        DbType.String, (failtCnt + 1).ToString()));

                                    parametersUPErr.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "LOCKED",
                                        DbType.String, "1"));

                                    parametersUPErr.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                                        DbType.String, ud.USERID.ToString()));
                                }
                                else
                                {
                                    //仅更新错误次数
                                    sbbErr.AppendLine(string.Format(@"update  {0}.ssy_user_dict t  set t.FAILTCNT= {1}FAILTCNT  WHERE t.userid = {1}userid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                                               DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                                    parametersUPErr.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "FAILTCNT",
                                        DbType.String, (failtCnt + 1).ToString()));

                                    parametersUPErr.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                                        DbType.String, ud.USERID.ToString()));

                                }

                                int executeFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbbErr.ToString(), SqlExecType.SqlText, null, parametersUPErr.ToArray());

                            }

                            #endregion

                        }
                        else
                        {
                            if (dt.Rows[0]["ISUSE"].ToString() == "1" && dt.Rows[0]["ISLONIN"].ToString() == "N" && dt.Rows[0]["LOCKED"].ToString() == "0")
                            {
                                #region 记录登录时间及状态，同时清零错误次数

                                string islogin = APPConfig.GetAPPConfig().GetConfigValue("isLogin", "Y");  //是否置已经登录标识 

                                StringBuilder sbbUpdateLoginTime = new StringBuilder();
                                sbbUpdateLoginTime.AppendLine(string.Format(@"update {0}.ssy_user_dict set isfirstlogin = 'N', FAILTCNT = '0',
                                    fromplat = {1}fromplat, lastlogintime = '{2}' WHERE Isuse = '1' and userid = {1}userid and ISLONIN = '{3}'  ",
                                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign(),
                                    this.GetSystemDateTime(ddnmParams), islogin));

                                List<IDataParameter> parametersUp = new List<IDataParameter>();
                                if (ud != null)
                                {
                                    parametersUp.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter("userid",
                                        DbType.String, ud.USERID.ToString()));
                                    parametersUp.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter("fromplat",
                                        DbType.String, ddnmParams.DistributeDataNodes[0].Systemname));
                                }

                                int tempflag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbbUpdateLoginTime.ToString(),
                                    SqlExecType.SqlText, null, parametersUp.ToArray());

                                //准备记录登录日志     
                                SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, this.GetSystemDateTime(ddnmParams),
                                                    "", "", LogAction.Login, "SSY_USER_DICT", "", "",
                                                    ud.USERID.ToString(),
                                                    "",
                                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_systemLogin", this.i18nFrameManageri18nLang),
                                                    BaseServiceUtility.GetI18nLangItem("logFunctionName_loginSuccess", this.i18nFrameManageri18nLang),
                                                    ddnmParams.DistributeDataNodes[0].Systemname, "");
                                ListBizLog.Add(logenti);

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Utility.RecordLog("业务层登录逻辑异常！原因" + ex.Message + ex.Source, this.logpathForDebug, this.isLogpathForDebug);             
            }

            return dt;
        }

        /// <summary>
        /// 用户安全退出
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog">记录日志内容参数，若不记录日志可以不传入</param>
        /// <returns></returns>
        public override string QuitUserForLogin(SSY_USER_DICT ud, DistributeDataNodeManagerParams ddnmParams,
            List<SSY_LOGENTITY> ListBizLog)
        {
            //准备记录退出日志              
            SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, this.GetSystemDateTime(ddnmParams),
                                "", "", LogAction.Quit, "SSY_USER_DICT", 
                                "", "",
                                base.envirObj.SysUserDict.USERID.ToString(),
                                "",
                                BaseServiceUtility.GetI18nLangItem("logFunctionName_quitSystem", this.i18nFrameManageri18nLang),
                                BaseServiceUtility.GetI18nLangItem("logFunctionName_quitSuccess", this.i18nFrameManageri18nLang),
                                ddnmParams.DistributeDataNodes[0].Systemname, "");
            ListBizLog.Add(logenti);
                        
            //清除登录标识及系统
            StringBuilder sbb = new StringBuilder();

            if (ud != null)
            {
                if (ud.USERID.ToString().ToUpper() != "super".ToUpper())
                {                    
                    List<IDataParameter> parameters = new List<IDataParameter>();

                    sbb.AppendLine(string.Format(@"update {0}.ssy_user_dict set ISLONIN = 'N', FROMPLAT = '' where userid = {1}userid ", 
                               DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                               DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter("userid",
                        DbType.String, ud.USERID.ToString()));

                    int temp = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), 
                        SqlExecType.SqlText, null, parameters.ToArray());
                }
            }

            return "";
        }

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams ddnmParams)
        {
            DataTable dt = null;
            StringBuilder sbb = new StringBuilder();
            if (ud.USERID.ToString().ToUpper() == "super".ToUpper())
            {
                sbb.AppendLine(string.Format(@"SELECT USERID,
                                USERNAME,
                                PASSWORD,
                                '' REGISTERDATE,
                                '' TELEPHONE,
                                '' EMAIL,
                                '' ISUSE,
                                '' ISFLAG,
                                '' NOTE,
                                '' ISLONIN,
                                '' FROMPLAT,
                                '' LASTLOGINTIME,
                                '' ISFIRSTLOGIN
                                FROM {0}.ssy_supermanager t where 1=1 ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));
            }
            else
            {
                sbb.AppendLine(string.Format(@"SELECT USERID,
                                                 USERNAME,
                                                 PASSWORD,
                                                 REGISTERDATE,
                                                 TELEPHONE,
                                                 EMAIL,
                                                 ISUSE,
                                                 ISFLAG,
                                                 NOTE,
                                                 ISLONIN,
                                                 FROMPLAT, LASTLOGINTIME, ISFIRSTLOGIN
                                            FROM {0}.ssy_user_dict t
                                           WHERE t.Isuse = '1' ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            }
            sbb.AppendLine(string.Format(@"and t.userid = {0}userid and t.password = {0}password", 
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
            List<IDataParameter> parameters = new List<IDataParameter>();
            if (ud != null)
            {                
                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                    DbType.String, ud.USERID.ToString()));
                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "password",
                    DbType.String, ud.PASSWORD.ToString()));
            }
            dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());           

            return dt;
        }


        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetAllUsers(SSY_USER_DICT ud, DistributeDataNodeManagerParams ddnmParams)
        {
            DataTable dt = null;
            StringBuilder sbb = new StringBuilder();

            sbb.AppendLine(string.Format(@"SELECT USERID,
                                                 USERNAME,
                                                 PASSWORD,
                                                 REGISTERDATE,
                                                 TELEPHONE,
                                                 EMAIL,
                                                 ISUSE,
                                                 ISFLAG,
                                                 NOTE,
                                                 ISLONIN,
                                                 FROMPLAT, LASTLOGINTIME, ISFIRSTLOGIN
                                            FROM {0}.ssy_user_dict t
                                           WHERE t.Isuse = '1' ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            List<IDataParameter> parameters = new List<IDataParameter>();      
            
            if(ud.USERID != null && ud.USERID.ToString() != string.Empty)
            {
                sbb.AppendLine(string.Format(@"and t.userid = {0}userid",
               DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                   DbType.String, ud.USERID.ToString()));
            }

            if (ud.USERNAME != null && ud.USERNAME.ToString() != string.Empty)
            {
                sbb.AppendLine(string.Format(@"and t.username = {0}username ",
               DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "username",
                   DbType.String, ud.USERNAME.ToString()));
            }

            dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

            return dt;
        }

        /// <summary>
        /// 获取功能根据用户
        /// </summary>
        /// <param name="ud"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataSet GetPages(SSY_USER_DICT ud, DistributeDataNodeManagerParams ddnmParams)
        {
            try
            {
                DataSet ds = new DataSet();

                #region 过程方式

                //string spname = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema + ".Logs.getPages";
                //List<IDataParameter> parameters = new List<IDataParameter>();
                //if (ud != null)
                //{
                //    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter("v_userid", DbType.String, ud.USERID.ToString()));
                //    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter("cur_rt", DbType.String, ""));
                //}
                //ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(spname, SqlExecType.SqlProcName, parameters.ToArray());

                #endregion

                StringBuilder sbb = new StringBuilder();
                List<IDataParameter> parameters = new List<IDataParameter>();
                if (ud.USERID.ToString().ToUpper() == "super".ToUpper())
                {
                    //to_number(S.seqsort)
                    sbb.AppendLine(string.Format(@"SELECT * FROM {0}.ssy_PAGE_DICT S
                                                 WHERE S.ISUSE = '1'
                                                 ORDER BY {1} ASC ", 
                                                 DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                                                 DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.Int16,
                                                 "S.seqsort", ConvertFormat.intT, false, "", "", "")));
                }
                else
                {
                    sbb.AppendLine(string.Format(@"SELECT * FROM {0}.ssy_PAGE_DICT S
                                                     WHERE S.ISUSE = '1' ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

                    //to_number(S.seqsort)
                    sbb.AppendLine(string.Format(@"AND S.Pageid IN
                                                           (SELECT a.pageid
                                                              FROM {0}.ssy_GROUP_PAGE_DICT a
                                                             WHERE a.groupid IN
                                                                   (SELECT t.groupid
                                                                      FROM {0}.ssy_USER_GROUP_DICT T
                                                                     WHERE T.Userid = {1}userid))
                                               ORDER BY {2} ASC",
                                                   DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                                                   DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign(),
                                                   DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.Int16,
                                                     "S.seqsort", ConvertFormat.intT, false, "", "", "")));
                    
                    if (ud != null)
                    {
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).
                            Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "userid",
                            DbType.String, ud.USERID.ToString()));
                    }
                }               
                
                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取用户组名
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataSet GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                sbb.AppendLine(string.Format(@"SELECT * FROM {0}.ssy_group_dict t where 1=1", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

                List<IDataParameter> parameters = new List<IDataParameter>();

                if (gd != null)
                {
                    if (Utility.ObjHasData(gd.GROUPID))
                    {
                        sbb.AppendLine(string.Format(@"and t.groupid = {0}groupid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "groupid",
                        DbType.String, gd.GROUPID.ToString()));
                    }

                    if (Utility.ObjHasData(gd.ISUSE))
                    {
                        sbb.AppendLine(string.Format(@"and t.isuse = {0}isuse", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "isuse",
                        DbType.String, gd.ISUSE.ToString()));
                    }
                }

                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return null;
            }

            return ds;
        }

        /// <summary>
        /// 获取用户组(带分页)
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetGroup(SSY_GROUP_DICT gd, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            //分页数据获取实现
            //分页参数数据
            SSY_PagingExecuteParam pageExecute = new SSY_PagingExecuteParam();
            pageExecute.PagingParam = pager;

            //参数值，若有，增加到该集合
            List<IDataParameter> parameters = new List<IDataParameter>();   
            StringBuilder sbbSqlWhere = new StringBuilder();
            if (gd != null)
            {
                if (Utility.ObjHasData(gd.GROUPID))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and groupid = {0}groupid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("groupid",
                    DbType.String, gd.GROUPID.ToString()));
                }
                if (Utility.ObjHasData(gd.ISUSE))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and isuse = {0}isuse", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("isuse",
                    DbType.String, gd.ISUSE.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.ssy_group_dict ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "groupid";
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

                if(Utility.DtHasData(dt))
                {
                    pager.TotalSize = int.Parse(dt.Rows[0]["cnt"].ToString());
                }
            }    

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataPager(pageExecute, parameters.ToArray());
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetUserdict(SSY_USER_DICT bizobj, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            //分页数据获取实现
            //分页参数数据
            SSY_PagingExecuteParam pageExecute = new SSY_PagingExecuteParam();
            pageExecute.PagingParam = pager;

            //参数值，若有，增加到该集合
            List<IDataParameter> parameters = new List<IDataParameter>();
            StringBuilder sbbSqlWhere = new StringBuilder();
            if (bizobj != null)
            {
                if (Utility.ObjHasData(bizobj.USERNAME))
                {
                    //and USERNAMES like  '%' {0} {1}USERNAMES {0} '%'
                    //and USERNAME = {0}USERNAME DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(),


                    //sbbSqlWhere.AppendLine(string.Format(@"and (USERNAME like  '%' {0} {1}USERNAME {0} '%' or USERID like  '%' {0} {1}USERNAME {0} '%') ",
                    //DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(), 
                    //    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                    List<ConvertFlag> convertFlag_USERNAME = new List<ConvertFlag>();
                    convertFlag_USERNAME.Add(ConvertFlag.Value);
                    convertFlag_USERNAME.Add(ConvertFlag.Field);
                    convertFlag_USERNAME.Add(ConvertFlag.Value);
                    List<string> opfileds_USERNAME = new List<string>();
                    opfileds_USERNAME.Add("%");
                    opfileds_USERNAME.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "USERNAME");
                    opfileds_USERNAME.Add("%");

                    sbbSqlWhere.AppendLine(string.Format(@"and (USERNAME like  {0}  or USERID like  {0} ) ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(convertFlag_USERNAME, opfileds_USERNAME)));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("USERNAME",
                    DbType.String, bizobj.USERNAME.ToString()));
                }
                if (Utility.ObjHasData(bizobj.ISUSE))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and isuse = {0}isuse", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("isuse",
                    DbType.String, bizobj.ISUSE.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.ssy_user_dict ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "userid";
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
        /// 获取页面
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataSet GetPage(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                sbb.AppendLine(string.Format(@"SELECT * FROM {0}.ssy_page_dict t where 1=1", 
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

                List<IDataParameter> parameters = new List<IDataParameter>();

                if (bizobj != null)
                {
                    if (Utility.ObjHasData(bizobj.PAGEID))
                    {
                        sbb.AppendLine(string.Format(@"and t.pageid = {0}pageid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "pageid",
                        DbType.String, bizobj.PAGEID.ToString()));
                    }

                    if (Utility.ObjHasData(bizobj.ISUSE))
                    {
                        sbb.AppendLine(string.Format(@"and t.isuse = {0}isuse", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "isuse",
                        DbType.String, bizobj.ISUSE.ToString()));
                    }
                }

                //增加排序
                sbb.Append(" order by " + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.Int16, "pageparentid",
                    ConvertFormat.intT, false, "", "", "") +
                    ", " + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.Int16, "seqsort",
                    ConvertFormat.intT, false, "", "", ""));

                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return null;
            }

            return ds;
        }

        /// <summary>
        /// 获取页面(带分页)
        /// </summary>
        /// <param name="bizobj"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetPagePager(SSY_PAGE_DICT bizobj, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
        {
            //分页数据获取实现
            //分页参数数据
            SSY_PagingExecuteParam pageExecute = new SSY_PagingExecuteParam();
            pageExecute.PagingParam = pager;

            //参数值，若有，增加到该集合
            List<IDataParameter> parameters = new List<IDataParameter>();
            StringBuilder sbbSqlWhere = new StringBuilder();
            if (bizobj != null)
            {
                if (Utility.ObjHasData(bizobj.PAGEID))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and pageid = {0}pageid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("pageid",
                    DbType.String, bizobj.PAGEID.ToString()));
                }
                if (Utility.ObjHasData(bizobj.ISUSE))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and isuse = {0}isuse", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("isuse",
                    DbType.String, bizobj.ISUSE.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.ssy_page_dict ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "pageid";
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
        /// 获取用户组与用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>        
        public override DataTable GetGroupUserPager(SSY_USER_GROUP_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
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
                if (Utility.ObjHasData(model.USERID))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and {0}.SSY_USER_GROUP_DICT.userid = {1}userid ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("userid",
                    DbType.String, model.USERID.ToString()));
                }
                if (Utility.ObjHasData(model.GROUPID))
                {
                    sbbSqlWhere.AppendLine(string.Format(@"and {0}.SSY_USER_GROUP_DICT.groupid = {1}groupid",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("groupid",
                    DbType.String, model.GROUPID.ToString()));
                }
            }

            pageExecute.TableNameOrView = string.Format(@" {0}.SSY_USER_GROUP_DICT ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);
            string joins = string.Format(@"left join {0}.ssy_user_dict on {0}.SSY_USER_GROUP_DICT.userid={0}.ssy_user_dict.userid 
                    left join {0}.ssy_group_dict on {0}.SSY_USER_GROUP_DICT.groupid={0}.ssy_group_dict.groupid
                ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = joins;
            pageExecute.Fields = " SSY_USER_GROUP_DICT.*, ssy_user_dict.username, ssy_group_dict.groupname ";
            pageExecute.OrderField = "SSY_USER_GROUP_DICT.groupid";
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
        /// 获取角色权限配置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override SSY_PAGE_GROUP_MQT GetGroupPageMgt(SSY_GROUP_PAGE_DICT model, DistributeDataNodeManagerParams ddnmParams)
        {
            SSY_PAGE_GROUP_MQT temp = new SSY_PAGE_GROUP_MQT();

            //获取系统全部功能
            DataSet pages = this.GetPage(new SSY_PAGE_DICT(), ddnmParams);
            //组织json字符串
            string resfunctions = string.Empty;
            StringBuilder sbbjson = new StringBuilder();
            StringBuilder sbbjson1 = new StringBuilder();
            StringBuilder sbbjson2 = new StringBuilder();
            StringBuilder sbbjson3 = new StringBuilder();
            if (Common.Utility.DsHasData(pages))
            {
                DataTable tempdt = pages.Tables[0];
                #region 历史处理

                //sbbjson.Append("[");
                //for (int i = 0; i < tempdt.Rows.Count; i++)
                //{
                //    sbbjson1.Clear();
                //    if (tempdt.Rows[i]["pageid"].ToString().Length == 3)
                //    {
                //        sbbjson2.Clear();
                //        sbbjson1.Append("{id:'" + tempdt.Rows[i]["pageid"].ToString() + "',label:'" + tempdt.Rows[i]["pagename"].ToString() +
                //            "',pageurl:'" + tempdt.Rows[i]["pageurl"].ToString() + "',");
                //        sbbjson1.Append("items: [");
                //        for (int j = 0; j < tempdt.Rows.Count; j++)
                //        {
                //            if (tempdt.Rows[j]["pageparentid"].ToString() == tempdt.Rows[i]["pageid"].ToString())
                //            {
                //                sbbjson3.Clear();
                //                sbbjson2.Append("{id:'" + tempdt.Rows[j]["pageid"].ToString() + "',label:'" + tempdt.Rows[j]["pagename"].ToString() +
                //                    "',pageurl:'" + tempdt.Rows[j]["pageurl"].ToString() + "',");
                //                sbbjson2.Append("items: [");
                //                for (int m = 0; m < tempdt.Rows.Count; m++)
                //                {
                //                    if (tempdt.Rows[m]["pageparentid"].ToString() == tempdt.Rows[j]["pageid"].ToString())
                //                    {
                //                        sbbjson3.Append("{id:'" + tempdt.Rows[m]["pageid"].ToString() + "',label:'" + tempdt.Rows[m]["pagename"].ToString() +
                //                            "',pageurl:'" + tempdt.Rows[m]["pageurl"].ToString() + "'},");
                //                    }
                //                }
                //                sbbjson2.Append(sbbjson3.ToString().TrimEnd(','));
                //                sbbjson2.Append("]},");
                //            }
                //        }
                //        sbbjson1.Append(sbbjson2.ToString().TrimEnd(','));
                //        sbbjson1.Append("]},");
                //        sbbjson.Append(sbbjson1.ToString());
                //    }
                //}
                //resfunctions = sbbjson.ToString().TrimEnd(',');
                //resfunctions = resfunctions + "]";

                #endregion

                //转义"处理
                sbbjson.Append("[");
                for (int i = 0; i < tempdt.Rows.Count; i++)
                {
                    sbbjson1.Clear();
                    if (tempdt.Rows[i]["pageid"].ToString().Length == 3)
                    {
                        sbbjson2.Clear();
                        sbbjson1.Append("{\"id\":\"" + tempdt.Rows[i]["pageid"].ToString() + "\",\"label\":\"" + tempdt.Rows[i]["pagename"].ToString() +
                            "\",\"pageurl\":\"" + tempdt.Rows[i]["pageurl"].ToString() + "\",");
                        sbbjson1.Append("\"items\": [");
                        for (int j = 0; j < tempdt.Rows.Count; j++)
                        {
                            if (tempdt.Rows[j]["pageparentid"].ToString() == tempdt.Rows[i]["pageid"].ToString())
                            {
                                sbbjson3.Clear();
                                sbbjson2.Append("{\"id\":\"" + tempdt.Rows[j]["pageid"].ToString() + "\",\"label\":\"" + tempdt.Rows[j]["pagename"].ToString() +
                                    "\",\"pageurl\":\"" + tempdt.Rows[j]["pageurl"].ToString() + "\",");
                                sbbjson2.Append("\"items\": [");
                                for (int m = 0; m < tempdt.Rows.Count; m++)
                                {                                    
                                    if (tempdt.Rows[m]["pageparentid"].ToString() == tempdt.Rows[j]["pageid"].ToString())
                                    {                                        
                                        sbbjson3.Append("{\"id\":\"" + tempdt.Rows[m]["pageid"].ToString() + "\",\"label\":\"" + tempdt.Rows[m]["pagename"].ToString() +
                                            "\",\"pageurl\":\"" + tempdt.Rows[m]["pageurl"].ToString() + "\"},");
                                    }
                                }
                                sbbjson2.Append(sbbjson3.ToString().TrimEnd(','));
                                sbbjson2.Append("]},");
                            }
                        }
                        sbbjson1.Append(sbbjson2.ToString().TrimEnd(','));
                        sbbjson1.Append("]},");
                        sbbjson.Append(sbbjson1.ToString());
                    }                                    
                }
                resfunctions = sbbjson.ToString().TrimEnd(',');
                resfunctions = resfunctions + "]";
            }
            temp.SSY_PAGE_DICT = resfunctions;     

            //获取当前角色权限
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            sbb.AppendLine(string.Format(@"SELECT * FROM {0}.SSY_GROUP_PAGE_DICT t where 1=1",
                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            List<IDataParameter> parameters = new List<IDataParameter>();

            if (model != null)
            {
                if (Utility.ObjHasData(model.GROUPID))
                {
                    sbb.AppendLine(string.Format(@"and t.groupid = {0}groupid ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "groupid",
                    DbType.String, model.GROUPID.ToString()));
                }
            }
            ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

            if(Common.Utility.DsHasData(ds))
            {
                temp.SSY_GROUP_PAGE_DICT = Common.UtilitysForT<SSY_GROUP_PAGE_DICT>.GetListsObj(ds.Tables[0]);
            }
            else
            {
                temp.SSY_GROUP_PAGE_DICT = new List<SSY_GROUP_PAGE_DICT>();
            }

            temp.GROUPID = model.GROUPID.ToString(); 

            return temp;
        }

        /// <summary>
        /// 重置默认密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public override bool ResetUserPWD(SSY_USER_DICT model, DistributeDataNodeManagerParams ddnmParams, List<SSY_LOGENTITY> ListBizLog)
        {          
            bool execFlag = false;
            StringBuilder sbb = new StringBuilder();
            List<IDataParameter> parameters = new List<IDataParameter>();
            if (model != null)
            {
                if(!string.IsNullOrEmpty(model.USERID.ToString()))
                {
                    sbb.AppendLine(string.Format(@" update {0}.ssy_user_dict  set PASSWORD = {1}password  where 1=1 ", 
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                       GetDataParameter("password", DbType.String, model.PASSWORD.ToString()));

                    sbb.AppendLine(string.Format(@" and userid = {0}userid ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                      GetDataParameter("userid", DbType.String, model.USERID.ToString()));

                    execFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), 
                        SqlExecType.SqlText, null, parameters.ToArray()) > 0;

                    if (execFlag)
                    {
                        //记录操作日志        
                        SSY_LOGENTITY logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.Biz, LogLevelOption.Normal, this.GetSystemDateTime(ddnmParams),
                                            "", "", LogAction.Modify, "SSY_USER_DICT", 
                                            "", "",
                                             base.envirObj.SysUserDict.USERID.ToString(),
                                            "",
                                            BaseServiceUtility.GetI18nLangItem("logFunctionName_resetPassword", this.i18nFrameManageri18nLang),
                                            BaseServiceUtility.GetI18nLangItem("logFunctionName_resetPasswordSuccess", this.i18nFrameManageri18nLang),
                                            ddnmParams.DistributeDataNodes[0].Systemname, "");

                        ListBizLog.Add(logenti);
                    }
                }               
            }

            return execFlag;
        }

        #endregion

        #region 系统框架相关

        /// <summary>
        /// 获取系统框架参数
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetFrameParam(DistributeDataNodeManagerParams ddnmParams)
        {
            StringBuilder sbb = new StringBuilder();

            sbb.AppendLine(string.Format(@"select ssy_appframe_id, optionidentified, ids, displaytext, hidevalue from {0}.ssy_appframe_cfg ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, null);
        }

        /// <summary>
        /// 获取系统框架参数明细
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetFrameParamDetail(DistributeDataNodeManagerParams ddnmParams)
        {
            StringBuilder sbb = new StringBuilder();

            sbb.AppendLine(string.Format(@"select ssy_appframe_item_id, optionidentified, itemiden, ids, displaytext, hidevalue from {0}.ssy_appframe_item_cfg ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, null);
        }

        /// <summary>
        /// 获取系统树结构风格参数
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetTreeViewConfig(DistributeDataNodeManagerParams ddnmParams)
        {
            StringBuilder sbb = new StringBuilder();

            sbb.AppendLine(string.Format(@"select ssy_appframe_trees_id, optionidentified, ids, displaytext, minheight, maxheight, fontsize, foreground, margin, 
                                            alternationcount from {0}.ssy_appframe_trees_cfg ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, null);
        }

        /// <summary>
        /// 获取系统菜单风格参数
        /// </summary>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetMenuConfig(DistributeDataNodeManagerParams ddnmParams)
        {
            StringBuilder sbb = new StringBuilder();

            sbb.AppendLine(string.Format(@"select ssy_appframe_menus_id, optionidentified, ids, displaytext, foreground, background, margin, subforeground, 
                                            subbackground, submargin from {0}.ssy_appframe_menus_cfg ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, null);
        }

        #endregion

        #region 公共部分

        //系统时间取应用服务器时间即可，不需要取数据库服务器时间
        ///// <summary>
        ///// 获取系统时间
        ///// </summary>
        ///// <param name="ddnmParams"></param>
        ///// <returns></returns>
        //public override string GetSystemDateTime(DistributeDataNodeManagerParams ddnmParams)
        //{
        //    return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime();
        //}


        /// <summary>
        /// 统一操作业务实体泛型,记录日志用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">业务实体List</param>
        /// <param name="opObjPropertyL">要操作的业务实体属性名称，对应于数据库字段,若确认都是删除时，可入空字符串</param>
        /// <param name="wherePropertyL">业务实体where语句属性名称，对应于数据库标示数据字段</param>
        /// <param name="mainPropertyL">主键字段说明</param>
        /// <param name="errStr"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams ddnmParams)
        {
            if (!Common.UtilitysForT<T>.ListHasData(objLists))
            {
                return false;
            }

            List<string> opObjProperty = Common.Utility.GetList2Upper(opObjPropertyL);
            List<string> whereProperty = Common.Utility.GetList2Upper(wherePropertyL);
            List<string> mainProperty = Common.Utility.GetList2Upper(mainPropertyL);

            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例
            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性

            StringBuilder sbb = new StringBuilder();
            List<IDataParameter> parameters = new List<IDataParameter>();
            string columns = "";
            string columnsValues = "";
            //反射泛型类型获取操作字段标示OpFlag
            string opflag = string.Empty;
            bool execFlag = false;

            string objName = string.Empty; //对象名称，对应数据库表名
            objName = obj.ToString().Substring(obj.ToString().LastIndexOf('.') + 1,
                            obj.ToString().Length - obj.ToString().LastIndexOf('.') - 1).ToUpper();

            #region 事物处理方式

            ////开始事务处理
            //using (IDbConnection conn = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetConnection())
            //{
            //    if (conn.State != ConnectionState.Open)
            //        conn.Open();
            //    IDbTransaction trans = conn.BeginTransaction();
            //    try
            //    {
            //        //先检查数据库完整性检查,
            //        //注意这里仅仅检查数据库结构定义，业务逻辑规则需要单独在方法里定义进行检查
            //        //若业务需要双重检查，这里可以反射实体的检查方法进行检查 22.34
            //        List<string> dataEntChecksErr = new List<string>();
            //        for (int iCheck = 0; iCheck < objLists.Count; iCheck++)
            //        {
            //            bool checkDataEnt = this.CheckDataEntity(objName, objLists[iCheck], fields, opObjProperty, ref dataEntChecksErr,
            //                ddnmParams);
            //        }

            //        if (dataEntChecksErr.Count > 0)
            //        {
            //            //发生实体结构检查错误，返回到应用
            //            for (int ierr = 0; ierr < dataEntChecksErr.Count; ierr++)
            //            {
            //                //errStr += dataEntChecksErr[ierr] + "*e*";
            //                errStr.Add(dataEntChecksErr[ierr] + "*e*");
            //            }
            //            //errStr = errStr.TrimEnd("*e*".ToCharArray());
            //            return false;
            //        }

            //        //开始处理数据
            //        for (int i = 0; i < objLists.Count; i++)
            //        {
            //            sbb.Clear();
            //            parameters.Clear();
            //            columns = "";
            //            columnsValues = "";
            //            opflag = string.Empty;

            //            obj = objLists[i];
            //            foreach (PropertyInfo t in fields)
            //            {
            //                if (t.Name.ToUpper() == "OpFlag".ToUpper())
            //                {
            //                    opflag = t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString();
            //                    break;
            //                }
            //            }

            //            //组织操作sql语句
            //            if (opflag.ToUpper() == "I")
            //            {
            //                #region  新增

            //                //组织新增字段
            //                for (int j = 0; j < opObjProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper())
            //                        {
            //                            //单独处理时间戳
            //                            if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper())
            //                            {
            //                                columns = columns + "TIMESTAMPSS".ToUpper() + ",";
            //                                columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "TIMESTAMPSS".ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    "TIMESTAMPSS".ToUpper(), DbType.String, DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
            //                                break;
            //                            }
            //                            else
            //                            {
            //                                columns = columns + opObjProperty[j].ToUpper() + ",";
            //                                columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }

            //                //构建语句
            //                sbb.Append(string.Format(@" insert into {0}.{1}({2}) ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
            //                    objName, columns.TrimEnd(',')));
            //                sbb.Append(string.Format(@" values({0}) ", columnsValues.TrimEnd(',')));

            //                #endregion
            //            }
            //            else if (opflag.ToUpper() == "M")
            //            {
            //                #region 修改

            //                //组织修改字段
            //                for (int j = 0; j < opObjProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && !mainProperty.Contains(opObjProperty[j]))
            //                        {
            //                            //单独处理时间戳
            //                            if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper())
            //                            {
            //                                columns = columns + "TIMESTAMPSS".ToUpper() + " = " +
            //                                 DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
            //                                    + "TIMESTAMPSS".ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    "TIMESTAMPSS".ToUpper(), DbType.String,
            //                                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
            //                                break;
            //                            }
            //                            else
            //                            {
            //                                columns = columns + opObjProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";

            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }

            //                //组织where
            //                string andStr = "";
            //                for (int j = 0; j < whereProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
            //                        {
            //                            if (j == whereProperty.Count - 1)
            //                            {
            //                                andStr = " " + string.Empty + " ";
            //                            }
            //                            else
            //                            {
            //                                andStr = " and ";
            //                            }

            //                            columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper() + andStr;

            //                            parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                            break;
            //                        }
            //                    }
            //                }

            //                //构建语句
            //                sbb.Append(string.Format(@" update {0}.{1} set {2} ",
            //                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
            //                    objName, columns.TrimEnd(',')));
            //                sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));

            //                #endregion
            //            }
            //            else if (opflag.ToUpper() == "D")
            //            {
            //                #region 删除

            //                //组织where
            //                string andStr = "";
            //                for (int j = 0; j < whereProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
            //                        {
            //                            if (j == whereProperty.Count - 1)
            //                            {
            //                                andStr = " " + string.Empty + " ";
            //                            }
            //                            else
            //                            {
            //                                andStr = " and ";
            //                            }

            //                            columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper() + andStr;

            //                            parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                            break;
            //                        }
            //                    }
            //                }

            //                sbb.Append(string.Format(@" delete from {0}.{1} ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema, objName));
            //                sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));


            //                #endregion
            //            }

            //            execFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), SqlExecType.SqlText, trans, parameters.ToArray()) > 0;

            //            if (!execFlag)
            //            {
            //                trans.Rollback();
            //                //通用事务执行失败提示,这个情况很少出现
            //                string opsql = @"操作sql为：{0}；操作参数为：{1}";
            //                string pas = "";
            //                for (int mmm = 0; mmm < parameters.Count; mmm++)
            //                {
            //                    pas += parameters[mmm].ParameterName + "/" + parameters[mmm].Value.ToString() + "*e*";
            //                }
            //                //errStr = string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray()));
            //                errStr.Add(string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray())));
            //                return false;
            //            }
            //        }
            //        trans.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        //errStr = ex.ToString();
            //        errStr.Add(ex.Message);
            //        return false;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}

            #endregion

            #region 非事物处理方式

            try
            {
                //先检查数据库完整性检查,
                //注意这里仅仅检查数据库结构定义，业务逻辑规则需要单独在方法里定义进行检查
                //若业务需要双重检查，这里可以反射实体的检查方法进行检查 22.34
                List<string> dataEntChecksErr = new List<string>();
                for (int iCheck = 0; iCheck < objLists.Count; iCheck++)
                {
                    bool checkDataEnt = this.CheckDataEntity(objName, objLists[iCheck], fields, opObjProperty, ref dataEntChecksErr,
                        ddnmParams);
                }

                if (dataEntChecksErr.Count > 0)
                {
                    //发生实体结构检查错误，返回到应用
                    for (int ierr = 0; ierr < dataEntChecksErr.Count; ierr++)
                    {
                        //errStr += dataEntChecksErr[ierr] + "*e*";
                        errStr.Add(dataEntChecksErr[ierr] + "*e*");
                    }
                    //errStr = errStr.TrimEnd("*e*".ToCharArray());
                    return false;
                }

                //开始处理数据
                for (int i = 0; i < objLists.Count; i++)
                {
                    sbb.Clear();
                    parameters.Clear();
                    columns = "";
                    columnsValues = "";
                    opflag = string.Empty;

                    obj = objLists[i];
                    foreach (PropertyInfo t in fields)
                    {
                        if (t.Name.ToUpper() == "OpFlag".ToUpper())
                        {
                            opflag = t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString();
                            break;
                        }
                    }

                    //组织操作sql语句
                    if (opflag.ToUpper() == "I")
                    {
                        #region  新增

                        //组织新增字段
                        for (int j = 0; j < opObjProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper())
                                {
                                    //单独处理时间戳
                                    if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper())
                                    {
                                        columns = columns + "TIMESTAMPSS".ToUpper() + ",";
                                        columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "TIMESTAMPSS".ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            "TIMESTAMPSS".ToUpper(), DbType.String, DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
                                        break;
                                    }
                                    else
                                    {
                                        columns = columns + opObjProperty[j].ToUpper() + ",";
                                        columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                        break;
                                    }
                                }
                            }
                        }

                        //构建语句
                        sbb.Append(string.Format(@" insert into {0}.{1}({2}) ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                            objName, columns.TrimEnd(',')));
                        sbb.Append(string.Format(@" values({0}) ", columnsValues.TrimEnd(',')));

                        #endregion
                    }
                    else if (opflag.ToUpper() == "M")
                    {
                        #region 修改

                        //组织修改字段
                        for (int j = 0; j < opObjProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && !mainProperty.Contains(opObjProperty[j]))
                                {
                                    //单独处理时间戳
                                    if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper())
                                    {
                                        columns = columns + "TIMESTAMPSS".ToUpper() + " = " +
                                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
                                            + "TIMESTAMPSS".ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            "TIMESTAMPSS".ToUpper(), DbType.String,
                                            DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
                                        break;
                                    }
                                    else
                                    {
                                        columns = columns + opObjProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";

                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                        break;
                                    }
                                }
                            }
                        }

                        //组织where
                        string andStr = "";
                        for (int j = 0; j < whereProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
                                {
                                    if (j == whereProperty.Count - 1)
                                    {
                                        andStr = " " + string.Empty + " ";
                                    }
                                    else
                                    {
                                        andStr = " and ";
                                    }

                                    columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper() + andStr;

                                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                    break;
                                }
                            }
                        }

                        //构建语句
                        sbb.Append(string.Format(@" update {0}.{1} set {2} ",
                            DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                            objName, columns.TrimEnd(',')));
                        sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));

                        #endregion
                    }
                    else if (opflag.ToUpper() == "D")
                    {
                        #region 删除

                        //组织where
                        string andStr = "";
                        for (int j = 0; j < whereProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
                                {
                                    if (j == whereProperty.Count - 1)
                                    {
                                        andStr = " " + string.Empty + " ";
                                    }
                                    else
                                    {
                                        andStr = " and ";
                                    }

                                    columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper() + andStr;

                                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                    break;
                                }
                            }
                        }

                        sbb.Append(string.Format(@" delete from {0}.{1} ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema, objName));
                        sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));


                        #endregion
                    }

                    execFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), SqlExecType.SqlText, null, parameters.ToArray()) > 0;

                    if (!execFlag)
                    {
                        //通用执行失败提示,这个情况很少出现
                        string opsql = @"操作sql为：{0}；操作参数为：{1}";
                        string pas = "";
                        for (int mmm = 0; mmm < parameters.Count; mmm++)
                        {
                            pas += parameters[mmm].ParameterName + "/" + parameters[mmm].Value.ToString() + "*e*";
                        }
                        //errStr = string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray()));
                        errStr.Add(string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray())));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //errStr = ex.ToString();
                errStr.Add(ex.Message);
                return false;
            }

            #endregion

            return true;
        }


        /// <summary>
        /// 统一操作业务实体泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objLists">业务实体List</param>
        /// <param name="opObjPropertyL">要操作的业务实体属性名称，对应于数据库字段,若确认都是删除时，可入空字符串</param>
        /// <param name="wherePropertyL">业务实体where语句属性名称，对应于数据库标示数据字段</param>
        /// <param name="mainPropertyL">主键字段说明</param>
        /// <param name="errStr"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="ListBizLog"></param>
        /// <returns></returns>
        public override bool OpBizObjectSingle<T>(List<T> objLists, List<string> opObjPropertyL, List<string> wherePropertyL, List<string> mainPropertyL, List<string> errStr,
            DistributeDataNodeManagerParams ddnmParams, List<SSY_LOGENTITY> ListBizLog)
        {
            if(!Common.UtilitysForT<T>.ListHasData(objLists))
            {
                return false;
            }

            List<string> opObjProperty = Common.Utility.GetList2Upper(opObjPropertyL);
            List<string> whereProperty = Common.Utility.GetList2Upper(wherePropertyL);
            List<string> mainProperty = Common.Utility.GetList2Upper(mainPropertyL);
            
            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例
            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性

            StringBuilder sbb = new StringBuilder();
            List<IDataParameter> parameters = new List<IDataParameter>();
            string columns = "";
            string columnsValues = "";
            //反射泛型类型获取操作字段标示OpFlag
            string opflag = string.Empty;
            bool execFlag = false;

            string objName = string.Empty; //对象名称，对应数据库表名
            objName = obj.ToString().Substring(obj.ToString().LastIndexOf('.') + 1,
                            obj.ToString().Length - obj.ToString().LastIndexOf('.') - 1).ToUpper();

            #region 事物处理方式

            //开始事务处理
            //using (IDbConnection conn = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetConnection())
            //{
            //    if (conn.State != ConnectionState.Open)
            //        conn.Open();
            //    IDbTransaction trans = conn.BeginTransaction();
            //    try
            //    {
            //        //先检查数据库完整性检查,
            //        //注意这里仅仅检查数据库结构定义，业务逻辑规则需要单独在方法里定义进行检查
            //        //若业务需要双重检查，这里可以反射实体的检查方法进行检查 22.34
            //        List<string> dataEntChecksErr = new List<string>();
            //        for (int iCheck = 0; iCheck < objLists.Count; iCheck++)
            //        {
            //            bool checkDataEnt = this.CheckDataEntity(objName, objLists[iCheck], fields, opObjProperty, ref dataEntChecksErr,
            //                ddnmParams);
            //        }

            //        if (dataEntChecksErr.Count > 0)
            //        {
            //            //发生实体结构检查错误，返回到应用
            //            for (int ierr = 0; ierr < dataEntChecksErr.Count; ierr++)
            //            {
            //                //errStr += dataEntChecksErr[ierr] + "*e*";
            //                errStr.Add(dataEntChecksErr[ierr] + "*e*");
            //            }
            //            //errStr = errStr.TrimEnd("*e*".ToCharArray());
            //            return false;
            //        }

            //        //开始处理数据
            //        for (int i = 0; i < objLists.Count; i++)
            //        {
            //            sbb.Clear();
            //            parameters.Clear();
            //            columns = "";
            //            columnsValues = "";
            //            opflag = string.Empty;

            //            obj = objLists[i];
            //            foreach (PropertyInfo t in fields)
            //            {
            //                if (t.Name.ToUpper() == "OpFlag".ToUpper())
            //                {
            //                    opflag = t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString();
            //                    break;
            //                }
            //            }

            //            //组织操作sql语句
            //            if (opflag.ToUpper() == "I")
            //            {
            //                #region  新增

            //                //组织新增字段
            //                for (int j = 0; j < opObjProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper())
            //                        {
            //                            //单独处理时间戳
            //                            if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper() && false)
            //                            {
            //                                columns = columns + "TIMESTAMPSS".ToUpper() + ",";
            //                                columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "TIMESTAMPSS".ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    "TIMESTAMPSS".ToUpper(), DbType.String, DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
            //                                break;
            //                            }
            //                            else
            //                            {
            //                                columns = columns + opObjProperty[j].ToUpper() + ",";
            //                                columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }

            //                //构建语句
            //                sbb.Append(string.Format(@" insert into {0}.{1}({2}) ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
            //                    objName, columns.TrimEnd(',')));
            //                sbb.Append(string.Format(@" values({0}) ", columnsValues.TrimEnd(',')));

            //                #endregion
            //            }
            //            else if (opflag.ToUpper() == "M")
            //            {
            //                #region 修改

            //                //组织修改字段
            //                for (int j = 0; j < opObjProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && !mainProperty.Contains(opObjProperty[j]))
            //                        {
            //                            //单独处理时间戳
            //                            if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper() && false)
            //                            {
            //                                columns = columns + "TIMESTAMPSS".ToUpper() + " = " +
            //                                 DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
            //                                    + "TIMESTAMPSS".ToUpper() + ",";
            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    "TIMESTAMPSS".ToUpper(), DbType.String,
            //                                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
            //                                break;
            //                            }
            //                            else
            //                            {
            //                                columns = columns + opObjProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";

            //                                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                    opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }

            //                //组织where
            //                string andStr = "";
            //                for (int j = 0; j < whereProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
            //                        {
            //                            if (j == whereProperty.Count - 1)
            //                            {
            //                                andStr = " " + string.Empty + " ";
            //                            }
            //                            else
            //                            {
            //                                andStr = " and ";
            //                            }

            //                            columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper() + andStr;

            //                            parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                            break;
            //                        }
            //                    }
            //                }

            //                //构建语句
            //                sbb.Append(string.Format(@" update {0}.{1} set {2} ",
            //                    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
            //                    objName, columns.TrimEnd(',')));
            //                sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));

            //                #endregion
            //            }
            //            else if (opflag.ToUpper() == "D")
            //            {
            //                #region 删除

            //                //组织where
            //                string andStr = "";
            //                for (int j = 0; j < whereProperty.Count; j++)
            //                {
            //                    foreach (PropertyInfo t in fields)
            //                    {
            //                        if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
            //                            && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
            //                        {
            //                            if (j == whereProperty.Count - 1)
            //                            {
            //                                andStr = string.Empty;
            //                            }
            //                            else
            //                            {
            //                                andStr = " and ";
            //                            }

            //                            columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
            //                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper() + andStr;

            //                            parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
            //                                whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
            //                            break;
            //                        }
            //                    }
            //                }

            //                sbb.Append(string.Format(@" delete from {0}.{1} ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema, objName));
            //                sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));


            //                #endregion
            //            }

            //            execFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), SqlExecType.SqlText, trans, parameters.ToArray()) > 0;

            //            if (!execFlag)
            //            {
            //                trans.Rollback();
            //                //通用事务执行失败提示,这个情况很少出现
            //                string opsql = @"操作sql为：{0}；操作参数为：{1}";
            //                string pas = "";
            //                for (int mmm = 0; mmm < parameters.Count; mmm++)
            //                {
            //                    pas += parameters[mmm].ParameterName + "/" + parameters[mmm].Value.ToString() + "*e*";
            //                }
            //                //errStr = string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray()));
            //                errStr.Add(string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray())));
            //                return false;
            //            }
            //        }
            //        trans.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        //errStr = ex.Message;
            //        errStr.Add(ex.Message);
            //        return false;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}

            #endregion

            #region 非事物方式

            try
            {
                //先检查数据库完整性检查,
                //注意这里仅仅检查数据库结构定义，业务逻辑规则需要单独在方法里定义进行检查
                //若业务需要双重检查，这里可以反射实体的检查方法进行检查 22.34
                List<string> dataEntChecksErr = new List<string>();
                for (int iCheck = 0; iCheck < objLists.Count; iCheck++)
                {
                    bool checkDataEnt = this.CheckDataEntity(objName, objLists[iCheck], fields, opObjProperty, ref dataEntChecksErr,
                        ddnmParams);
                }

                if (dataEntChecksErr.Count > 0)
                {
                    //发生实体结构检查错误，返回到应用
                    for (int ierr = 0; ierr < dataEntChecksErr.Count; ierr++)
                    {
                        //errStr += dataEntChecksErr[ierr] + "*e*";
                        errStr.Add(dataEntChecksErr[ierr] + "*e*");
                    }
                    //errStr = errStr.TrimEnd("*e*".ToCharArray());
                    return false;
                }

                //开始处理数据
                for (int i = 0; i < objLists.Count; i++)
                {
                    sbb.Clear();
                    parameters.Clear();
                    columns = "";
                    columnsValues = "";
                    opflag = string.Empty;

                    obj = objLists[i];
                    foreach (PropertyInfo t in fields)
                    {
                        if (t.Name.ToUpper() == "OpFlag".ToUpper())
                        {
                            opflag = t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString();
                            break;
                        }
                    }

                    //组织操作sql语句
                    if (opflag.ToUpper() == "I")
                    {
                        #region  新增

                        //组织新增字段
                        for (int j = 0; j < opObjProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper())
                                {
                                    //单独处理时间戳
                                    if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper() && false)
                                    {
                                        columns = columns + "TIMESTAMPSS".ToUpper() + ",";
                                        columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "TIMESTAMPSS".ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            "TIMESTAMPSS".ToUpper(), DbType.String, DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
                                        break;
                                    }
                                    else
                                    {
                                        columns = columns + opObjProperty[j].ToUpper() + ",";
                                        columnsValues = columnsValues + DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                        break;
                                    }
                                }
                            }
                        }

                        //构建语句
                        sbb.Append(string.Format(@" insert into {0}.{1}({2}) ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                            objName, columns.TrimEnd(',')));
                        sbb.Append(string.Format(@" values({0}) ", columnsValues.TrimEnd(',')));

                        #endregion
                    }
                    else if (opflag.ToUpper() == "M")
                    {
                        #region 修改

                        //组织修改字段
                        for (int j = 0; j < opObjProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && !mainProperty.Contains(opObjProperty[j]))
                                {
                                    //单独处理时间戳
                                    if (t.Name.ToUpper() == "TIMESTAMPSS".ToUpper() && false)
                                    {
                                        columns = columns + "TIMESTAMPSS".ToUpper() + " = " +
                                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
                                            + "TIMESTAMPSS".ToUpper() + ",";
                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            "TIMESTAMPSS".ToUpper(), DbType.String,
                                            DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetSystemDateTime()));
                                        break;
                                    }
                                    else
                                    {
                                        columns = columns + opObjProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + opObjProperty[j].ToUpper() + ",";

                                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                            opObjProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                        break;
                                    }
                                }
                            }
                        }

                        //组织where
                        string andStr = "";
                        for (int j = 0; j < whereProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
                                {
                                    if (j == whereProperty.Count - 1)
                                    {
                                        andStr = " " + string.Empty + " ";
                                    }
                                    else
                                    {
                                        andStr = " and ";
                                    }

                                    columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper() + andStr;

                                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                    break;
                                }
                            }
                        }

                        //构建语句
                        sbb.Append(string.Format(@" update {0}.{1} set {2} ",
                            DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                            objName, columns.TrimEnd(',')));
                        sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));

                        #endregion
                    }
                    else if (opflag.ToUpper() == "D")
                    {
                        #region 删除

                        //组织where
                        string andStr = "";
                        for (int j = 0; j < whereProperty.Count; j++)
                        {
                            foreach (PropertyInfo t in fields)
                            {
                                if (t.Name.ToUpper() == whereProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                                    && t.Name.ToUpper() != "OpFlag".ToUpper() && t.Name.ToUpper() != "TIMESTAMPSS".ToUpper())
                                {
                                    if (j == whereProperty.Count - 1)
                                    {
                                        andStr = string.Empty;
                                    }
                                    else
                                    {
                                        andStr = " and ";
                                    }

                                    columnsValues = columnsValues + whereProperty[j].ToUpper() + " = " +
                                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper() + andStr;

                                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() +
                                        whereProperty[j].ToUpper(), DbType.String, t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString()));
                                    break;
                                }
                            }
                        }

                        sbb.Append(string.Format(@" delete from {0}.{1} ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema, objName));
                        sbb.Append(string.Format(@" where 1=1 and {0} ", columnsValues.TrimEnd("and".ToCharArray())));


                        #endregion
                    }

                    execFlag = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ExecuteNonQuery(sbb.ToString(), SqlExecType.SqlText, null, parameters.ToArray()) > 0;

                    if (!execFlag)
                    {
                        //通用执行失败提示,这个情况很少出现
                        string opsql = @"操作sql为：{0}；操作参数为：{1}";
                        string pas = "";
                        for (int mmm = 0; mmm < parameters.Count; mmm++)
                        {
                            pas += parameters[mmm].ParameterName + "/" + parameters[mmm].Value.ToString() + "*e*";
                        }
                        //errStr = string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray()));
                        errStr.Add(string.Format(opsql, sbb.ToString(), pas.TrimEnd("*e*".ToCharArray())));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //errStr = ex.Message;
                errStr.Add(ex.Message);
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 通用查询数据实体是否存在数据库中
        /// </summary>
        /// <param name="bizobjectname">数据库中的表名</param>
        /// <param name="wherePropertyL">字段|字段值,例如：prop1|value1</param>
        /// <param name="splitchar">wherePropertyL中的分割符号，默认|</param>
        /// <param name="ddnmParams"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        public override bool CheckBizObjectRepat(string bizobjectname, List<string> wherePropertyL, string splitchar,
            DistributeDataNodeManagerParams ddnmParams, List<string> errStr)
        {            
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            if(string.IsNullOrEmpty(splitchar))
            {
                splitchar = "|";
            }
            try
            {
                sbb.AppendLine(string.Format(@"SELECT * FROM {0}.{1} where 1=1 ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                    bizobjectname));
                List<IDataParameter> parameters = new List<IDataParameter>();

                if (wherePropertyL.Count > 0)
                {
                    for (int i = 0; i < wherePropertyL.Count; i++)
                    {
                        string[] kvs = wherePropertyL[i].Split(new string[] { splitchar }, StringSplitOptions.RemoveEmptyEntries);
                        sbb.AppendLine(string.Format(@" and {0} = {1}{0} ", kvs[0],
                            DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));

                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.
                            GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + kvs[0], DbType.String, kvs[1]));
                    }                   
                }

                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());
            }
            catch (Exception ex)
            {
                errStr.Add(ex.Message);
                return false;
            }

            return Common.Utility.DsHasData(ds);
        }


        /// <summary>
        /// 通用查询数据实体是否存在数据库中,批量查询，支持分别查询一个字段、一组字段是否存在,可查询多条记录
        /// </summary>
        /// <param name="bizObjectName">数据实体表名</param>
        /// <param name="fields">要查询的字段组合，支持多组查询。例如：field1|field2|field3|field4;field5|field6,表示的分别查询字段1、字段2、字段3、字段6分别单独不重复，字段4和字段5组合不重复</param>
        /// <param name="fieldsValue">每个字段对应的数据值，字符串集合</param>
        /// <param name="splitChar">要查询的重复单元分隔符号。对应例子中的竖杠 |  可自定义其他，但要确保不和数据内容冲突</param>
        /// <param name="splitCharSub">要查询的组合分隔符号。对应例子中的分号 ; 可自定义其他 但要确保不和数据内容冲突</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override List<string> CheckBizObjectsRepat(string bizObjectName, string fields, List<string>  fieldsValue, string splitChar, string splitCharSub,
            DistributeDataNodeManagerParams ddnmParams)
        {
            List<string> repatDataNotice = new List<string>();
            DataSet ds = null;         

            string datas = ""; //重复数据内容
            string repeatNotice = BaseServiceUtility.GetI18nLangItem("findDataRepat", this.i18nCommonCurrLang);

            StringBuilder sbb = new StringBuilder();
            List<IDataParameter> parameters = new List<IDataParameter>();
            if (string.IsNullOrEmpty(splitChar))
            {
                splitChar = "|";
            }
            if (string.IsNullOrEmpty(splitCharSub))
            {
                splitCharSub = ";";                
            }

            string mainStr = string.Format(@"SELECT * FROM {0}.{1} where 1=1 ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema,
                    bizObjectName);

            try
            {
                string[] fieldArr = fields.Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries); //要查重复字段

                for (int i = 0; i < fieldsValue.Count; i++)
                {
                    string[] fieldValueArr = fieldsValue[i].Split(new string[] { splitChar }, StringSplitOptions.RemoveEmptyEntries); //要查重复字段对应的数值

                    for (int j = 0; j < fieldArr.Length; j++)
                    {
                        string[] fieldRepatValue = fieldValueArr[j].Split(new string[] { splitCharSub }, StringSplitOptions.RemoveEmptyEntries); //要查重复字段对应值
                        string[] fieldRepat = fieldArr[j].Split(new string[] { splitCharSub }, StringSplitOptions.RemoveEmptyEntries); //要查重复字段

                        sbb.Clear();
                        parameters.Clear();
                        ds = null;
                        datas = string.Empty;

                        sbb.AppendLine(mainStr);

                        for (int m = 0; m < fieldRepat.Length; m++)
                        {  
                            sbb.AppendLine(string.Format(@" and {0} = {1}{0} ", fieldRepat[m],
                                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                            parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.
                                GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + fieldRepat[m], DbType.String, fieldRepatValue[m]));

                            datas += fieldRepatValue[m] + " ";
                        }

                        ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

                        if (Common.Utility.DsHasData(ds))
                        {
                            repatDataNotice.Add(datas + " " + repeatNotice);
                        }
                    }

                }   
            }
            catch (Exception ex)
            {
                repatDataNotice.Clear();
                repatDataNotice.Add("err" + " " + ex.Message);
            }

            return repatDataNotice;
        }


        /// <summary>
        /// 获取通用信息提示
        /// </summary>
        /// <param name="striden">消息项识别</param>
        /// <param name="strcollectioniden">消息集合识别</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public string GetNoticeCfg(string striden, string strcollectioniden, string systemName, DistributeDataNodeManagerParams ddnmParams)
        {
            string picPath = string.Empty;
            DataTable dt = this.GetFrameParamDetail(ddnmParams);

            if (Common.Utility.DtHasData(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (systemName.ToUpper().Equals(dt.Rows[i]["optionidentified"].ToString().ToUpper()) &&
                        strcollectioniden.ToUpper().Equals(dt.Rows[i]["itemiden"].ToString().ToUpper()) &&
                        striden.ToUpper().ToUpper().Equals(dt.Rows[i]["ids"].ToString().ToUpper()))
                    {
                        picPath = dt.Rows[i]["hidevalue"].ToString();
                        break;
                    }
                }
            }
            return picPath;
        }

        /// <summary>
        /// 通用获取单个实体所有数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetEntityAllDataForCommon(string tablename, DistributeDataNodeManagerParams ddnmParams)
        {
            DataTable dt = null;
            StringBuilder sbb = new StringBuilder();
            sbb.AppendLine(string.Format(@"SELECT *  FROM {0}.{1} t ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema, tablename));           
            dt = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataTable(sbb.ToString(), SqlExecType.SqlText, null);
            return dt;
        }

        #region 私有方法

        /// <summary>
        /// 统一检查数据库实体结构是否否何数据库相应定义,
        /// 目前只检查字段长度和是否为空
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <param name="obj"></param>
        /// <param name="fields"></param>
        /// <param name="opObjProperty"></param>
        /// <param name="errs">整个实体要检查的错误结果提示集合</param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        private bool CheckDataEntity(string dataEntName, object obj, PropertyInfo[] fields, List<string> opObjProperty, ref List<string> errs,
            DistributeDataNodeManagerParams ddnmParams)
        {
            DataTable dt = this.GetDataEntity(dataEntName, ddnmParams);
            if (!Common.Utility.DtHasData(dt))
            {
                return true; //没有找到该实体结构不做检查
            }

            //开始检查要操作的字段
            string objFieldValue = string.Empty;
            for (int j = 0; j < opObjProperty.Count; j++)
            {
                objFieldValue = string.Empty;
                foreach (PropertyInfo t in fields)
                {
                    if (t.Name.ToUpper() == opObjProperty[j].ToUpper() && t.Name.ToUpper() != "SequenceXXX".ToUpper()
                        && t.Name.ToUpper() != "OpFlag".ToUpper())
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["colname"].ToString().ToUpper() == t.Name.ToUpper())
                            {
                                objFieldValue = t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString();
                                //检查是否为空
                                if (dt.Rows[i]["NULLABLE"].ToString().ToUpper() == "N")
                                {
                                    //不可以为空
                                    if (string.IsNullOrEmpty(objFieldValue))
                                    {
                                        //errs.Add(string.Format("该字段{0}不能为空。请检查！", t.Name.ToUpper()));
                                        errs.Add(string.Format(BaseServiceUtility.GetI18nLangItem("noticeinfo_checkFieldEmpty", i18nFrameManageri18nLang), 
                                            t.Name.ToUpper()));
                                    }
                                }

                                //检查长度
                                if (!Common.Utility.ChangeByte(objFieldValue, int.Parse(dt.Rows[i]["DATA_LENGTH"].ToString())))
                                {
                                    //errs.Add(string.Format("该字段{0}长度超限，必须<={1}。字段值为:{2}请检查！", t.Name.ToUpper(), 
                                    //    dt.Rows[i]["DATA_LENGTH"].ToString(), objFieldValue));
                                    errs.Add(string.Format(BaseServiceUtility.GetI18nLangItem("noticeinfo_checkFieldLength", i18nFrameManageri18nLang),
                                        t.Name.ToUpper(), dt.Rows[i]["DATA_LENGTH"].ToString(), objFieldValue));
                                }

                                break; 
                            } 
                        } 
                    }

                    break;
                }
            }

            return errs.Count > 0;
        }
              

        #endregion

        #endregion

        #region 系统日志相关

        /// <summary>
        /// 获取系统表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetDataEntity(string dataEntName, DistributeDataNodeManagerParams ddnmParams)
        {
            return DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataEntity(dataEntName);            
        }

        /// <summary>
        /// 判断某类日志是否记录
        /// </summary>
        /// <param name="logtypeDomain"></param>
        /// <param name="loglevelOption"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override bool CheckIsRecord(string logtypeDomain, string loglevelOption, DistributeDataNodeManagerParams ddnmParams)
        {
            DataSet ds = null;
            StringBuilder sbb = new StringBuilder();
            try
            {
                sbb.AppendLine(string.Format(@"SELECT * FROM {0}.SSY_LOGCONTROL t where 1=1 ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema));

                List<IDataParameter> parameters = new List<IDataParameter>();
                sbb.AppendLine(string.Format(@"and t.domainiden = {0}domainiden ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "domainiden",
                DbType.String, logtypeDomain.ToString()));

                sbb.AppendLine(string.Format(@"and t.optioniden = {0}optioniden ", DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()));
                parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataParameter(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "optioniden",
                DbType.String, loglevelOption.ToString()));  

                ds = DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.GetDataSet(sbb.ToString(), SqlExecType.SqlText, parameters.ToArray());

                if(Utility.DsHasData(ds))
                {
                    if(ds.Tables[0].Rows[0]["isrecord"].ToString() == "1")
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// 获取系统日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override DataTable GetLogDataPager(SSY_LOGENTITY model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
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
                if (Utility.ObjHasData(model.USERNAMES))
                {
                    //sbbSqlWhere.AppendLine(string.Format(@"and USERNAMES like  '%' {0} {1}USERNAMES {0} '%' ",
                    //    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(null, null),
                    //    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
                    //    ));


                    List<ConvertFlag> convertFlag_USERNAMES = new List<ConvertFlag>();
                    convertFlag_USERNAMES.Add(ConvertFlag.Value);
                    convertFlag_USERNAMES.Add(ConvertFlag.Field);
                    convertFlag_USERNAMES.Add(ConvertFlag.Value);
                    List<string> opfileds_USERNAMES = new List<string>();
                    opfileds_USERNAMES.Add("%");
                    opfileds_USERNAMES.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "USERNAMES");
                    opfileds_USERNAMES.Add("%");

                    sbbSqlWhere.AppendLine(string.Format(@"and USERNAMES like  {0} ",
                        DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(convertFlag_USERNAMES, opfileds_USERNAMES)));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("USERNAMES",
                    DbType.String, model.USERNAMES.ToString()));
                }

                if (Utility.ObjHasData(model.DESCRIPTIONS))
                {
                    //sbbSqlWhere.AppendLine(string.Format(@"and DESCRIPTIONS like  '%' {0} {1}DESCRIPTIONS {0} '%' ",
                    //    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(null, null),
                    //    DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign()
                    //    ));


                    List<ConvertFlag> convertFlag_DESCRIPTIONS = new List<ConvertFlag>();
                    convertFlag_DESCRIPTIONS.Add(ConvertFlag.Value);
                    convertFlag_DESCRIPTIONS.Add(ConvertFlag.Field);
                    convertFlag_DESCRIPTIONS.Add(ConvertFlag.Value);
                    List<string> opfileds_DESCRIPTIONS = new List<string>();
                    opfileds_DESCRIPTIONS.Add("%");
                    opfileds_DESCRIPTIONS.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "DESCRIPTIONS");
                    opfileds_DESCRIPTIONS.Add("%");

                    sbbSqlWhere.AppendLine(string.Format(@"and DESCRIPTIONS like  {0} ",
                       DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConnectChar(convertFlag_DESCRIPTIONS, opfileds_DESCRIPTIONS)));

                    parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                        GetDataParameter("DESCRIPTIONS",
                    DbType.String, model.DESCRIPTIONS.ToString()));
                }

                if (Utility.ObjHasData(model.RECORDTIME))
                {
                    string[] dateQSeg = model.RECORDTIME.ToString().Split('|');

                    if (!string.IsNullOrEmpty(dateQSeg[0]))
                    {
                        sbbSqlWhere.AppendLine(string.Format(@"and {0} >= {1} ",
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.DateTime, "RECORDTIME",
                         ConvertFormat.yyyy_mm_ddohhammass, false, "", "", ""),
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.DateTime,
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "RECORDTIMESTART",
                         ConvertFormat.yyyy_mm_ddohhammass, false, "", "", "")
                         ));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                            GetDataParameter("RECORDTIMESTART", DbType.String, dateQSeg[0]));
                    }
                    if (!string.IsNullOrEmpty(dateQSeg[1]))
                    {
                        sbbSqlWhere.AppendLine(string.Format(@"and {0} <= {1} ",
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.DateTime, "RECORDTIME",
                         ConvertFormat.yyyy_mm_ddohhammass, false, "", "", ""),
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ConvertTypeStr(DbType.DateTime,
                         DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.ParamSign() + "RECORDTIMEEND",
                         ConvertFormat.yyyy_mm_ddohhammass, false, "", "", "")
                         ));
                        parameters.Add(DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.
                            GetDataParameter("RECORDTIMEEND", DbType.String, dateQSeg[1]));
                    }
                }               
            }           

            pageExecute.TableNameOrView = string.Format(@" {0}.SSY_LOGENTITY ",
                DBFactorySingleton.GetInstance(ddnmParams.DistributeDataNode).Factory.DbSchema);

            pageExecute.Joins = string.Empty;
            pageExecute.Fields = "*";
            pageExecute.OrderField = "RECORDTIME";
            pageExecute.PagingParam.SortType = AscOrDesc.Desc;

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

        #endregion

        #region 字典相关

        /// <summary>
        /// 获取全部系统字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ddnmParams"></param>
        /// <returns></returns>
        public override DataTable GetFrameDictAll(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams)
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

            if(Common.Utility.DsHasData(ds))
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
        public override DataTable GetFrameDictPager(SSY_FRAME_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
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
        public override DataTable GetBizDictAll(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams)
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
        public override DataTable GetBizDictPager(SSY_BIZ_DICT model, DistributeDataNodeManagerParams ddnmParams, SSY_PagingParam pager)
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

            if(dicttype == DictType.BizDict)
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
                throw new Exception(BaseServiceUtility.GetI18nLangItem("noticeinfo_setDictType", this.i18nFrameManageri18nLang));
            }

            if(string.IsNullOrEmpty(DOMAINNAMEIDEN))
            {
                //throw new Exception("必须设置字典设别！");
                throw new Exception(BaseServiceUtility.GetI18nLangItem("noticeinfo_setDictIden", this.i18nFrameManageri18nLang));
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

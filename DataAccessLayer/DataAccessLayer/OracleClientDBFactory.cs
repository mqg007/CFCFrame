using System;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// 实现Oracle的数据库访问工厂
    /// </summary>
    public class OracleClientFactory : AbstractDBFactory
    {
        public OracleConnection cnn = null;
        public OracleTransaction trans = null;
        public OracleCommand cmd = null;
        public OracleDataAdapter adapter = null;

        //mqg于20180928增加，扩展增加数据库连接池
        public List<OracleConnection> cnnList = new List<OracleConnection>();

        /// <summary>
        /// 获得一个数据库链接。
        /// </summary>
        /// <returns>数据库链接</returns>
        public override IDbConnection GetConnection()
        {
            //if (cnn == null || cnn.ConnectionString == null || cnn.ConnectionString == string.Empty)
            //{
            //    cnn = new OracleConnection(base.ConnectionString);
            //}
            //if (cnn.State != ConnectionState.Open)
            //{
            //    cnn.Open();
            //}
            //return cnn;

            //mqg于20180928增加，扩展连接池处理
            if (base.maxCnn == cnnList.Count)
            {
                //连接池满从连接池获取
                Random rdom = new Random();
                int tmpIndex = rdom.Next(0, cnnList.Count);
                cnn = cnnList[tmpIndex];

                if (cnn == null || cnn.ConnectionString == null || cnn.ConnectionString == string.Empty)
                {
                    cnnList.Remove(cnnList[tmpIndex]);
                    cnn = new OracleConnection(base.ConnectionString);
                    cnn.Open();
                    cnnList.Add(cnn);
                }
            }
            else
            {
                OracleConnection tmpcnn = new OracleConnection(base.ConnectionString);
                tmpcnn.Open();
                if (base.maxCnn >= cnnList.Count)
                {
                    cnnList.Add(tmpcnn);
                }
                cnn = tmpcnn;
            }

            return cnn;
        }

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        public override IDbTransaction GetIDbTransaction()
        {
            OracleConnection cnnTrans = (OracleConnection)this.GetConnection();

            try
            {
                trans = cnnTrans.BeginTransaction();
            }
            catch (Exception e)
            {
                throw new Exception("create transaction error!", e);
            }           

            return trans;
        }

        /// <summary>
        /// 获得一个数据库命令对象。
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns>命令对象</returns>
        public override IDbCommand GetCommand(string sql, SqlExecType sqlexectype, IDbConnection conn, IDbTransaction trans, params IDataParameter[] paras)
        {
            try
            {
                cmd = new OracleCommand();
                cmd.CommandText = sql;
                if (sqlexectype == SqlExecType.SqlText)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if(sqlexectype == SqlExecType.SqlProcName)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }    
                cmd.Connection = (OracleConnection)conn;

                //这里可能需要兼容处理oracle的bolb和colb数据字段，暂时先不处理，看看参数方式是否直接可以
                this.PrepareCommand(cmd, paras);

                if (trans != null)
                {
                    cmd.Transaction = (OracleTransaction) trans;
                }
            }
            catch (Exception e)
            {
                throw new Exception("create command  error, please check sql script:" + sql, e);
            }
                        
            return (IDbCommand)cmd;
        }

        /// <summary>
        /// 构造参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdParms"></param>
        private void PrepareCommand(OracleCommand cmd, IDataParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    OracleParameter OraParameter = (OracleParameter)cmdParms[i];
                    cmd.Parameters.Add(OraParameter);
                }

                //OracleParameter[] OraParameters =(OracleParameter[]) cmdParms;
                //for (int i = 0; i < (int)OraParameters.Length; i++)
                //{
                //    OracleParameter OraParameter = OraParameters[i];
                //    cmd.Parameters.Add(OraParameter);
                //}
            }
        }


        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private OracleType GetDBType(DbType type)
        {
            OracleType dbType = OracleType.VarChar;

            switch (type)
            {
                case DbType.Binary:
                    dbType = OracleType.Blob;
                    break;
                case DbType.Boolean:
                    dbType = OracleType.Byte;
                    break;
                case DbType.Date:
                    dbType = OracleType.DateTime;
                    break;
                case DbType.Time:
                    dbType = OracleType.DateTime;
                    break;
                case DbType.Single:
                    dbType = OracleType.Float;
                    break;
                case DbType.Int32:
                    dbType = OracleType.Int32;
                    break;
                case DbType.Currency:
                    dbType = OracleType.Float;
                    break;
                case DbType.Double:
                    dbType = OracleType.Double;
                    break;
                case DbType.Int64:
                    dbType = OracleType.Number;
                    break;
                case DbType.String:
                    dbType = OracleType.VarChar;
                    break;
                case DbType.Object:
                    dbType = OracleType.Blob;
                    break;
                case DbType.AnsiString:
                    dbType = OracleType.Clob;
                    break;
                case DbType.AnsiStringFixedLength:
                    dbType = OracleType.LongVarChar;
                    break;
            }

            return dbType;
        }

        /// <summary>
        /// 获取一个DataParameter对象。
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="type">若是</param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public override IDataParameter GetDataParameter(string paraName, DbType type, object paramValue)
        {
            OracleParameter op = null;
            OracleType dbType = GetDBType(type);
            //处理游标，游标参数名默认cur_rt
            if(paraName.ToUpper() == "cur_rt".ToUpper())
            {
                op = new OracleParameter(this.ParamSign() + paraName.Replace(":","").Replace("@", "").Replace("?", ""), OracleType.Cursor);
                op.Value = paramValue;
                op.Direction = ParameterDirection.Output;
            }
            else
            {
                op = new OracleParameter(this.ParamSign() + paraName.Replace(":", "").Replace("@", "").Replace("?", ""), dbType);
                op.Value = paramValue;
                op.Direction = ParameterDirection.Input;
            }
            
            return (IDataParameter)op;            
        }

        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        public override int ExecuteNonQuery(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {
            int tempint = 0;
            try
            {
                if (trans == null)
                {
                    cnn = (OracleConnection)this.GetConnection();
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {               
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempint = cmd.ExecuteNonQuery();
                //清除掉参数，以免二次使用.net取缓冲导致报错
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            }
            finally
            {
                //mqg于20180928增加，这里不在释放连接，交给连接池管理           
                //if (trans == null && cnn != null)
                //{
                //    cnn.Close();
                //    cnn.Dispose();
                //}
            }

            return tempint;
        }

        /// <summary>
        /// 执行SQL，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override Object ExecuteScalar(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {
            object tempobj = 0;

            try
            {
                if (trans == null)
                {
                    cnn = (OracleConnection)this.GetConnection();
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempobj = cmd.ExecuteScalar();
                //清除掉参数，以免二次使用.net取缓冲导致报错
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            }
            finally
            {
                //mqg于20180928增加，这里不在释放连接，交给连接池管理
                //if (trans == null && cnn != null)
                //{
                //    cnn.Close();
                //    cnn.Dispose();
                //}
            }

            return tempobj;
        }

        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到Read中返回。
        /// </summary>
        /// <param name="sql">要执行的SQL查询。</param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="paras"></param>
        /// <returns>返回一个包含查询结果的Read。</returns>
        public override IDataReader GetReader(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            try
            {
                cnn =(OracleConnection)this.GetConnection();
                cmd =(OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, null, paras);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //清除掉参数，以免二次使用.net取缓冲导致报错
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            }
            finally
            {
                //mqg于20180928增加，这里不在释放连接，交给连接池管理
                reader.Close();
                //cnn.Close();
                //cnn.Dispose();
            }

            return reader;
        }

        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到数据集中返回。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataSet GetDataSet(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            DataSet ds = new DataSet();            
            try
            {
                cnn = (OracleConnection)this.GetConnection();
                cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, cnn, null, paras);
                adapter = new OracleDataAdapter(cmd);
                adapter.Fill(ds, "tableName");
                //清除掉参数，以免二次使用.net取缓冲导致报错
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            }

            finally
            {
                //mqg于20180928增加，这里不在释放连接，交给连接池管理
                //adapter.Dispose();
                //cnn.Close();
                //cnn.Dispose();
            }

            return ds;
        }

        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到数据集中返回。
        /// 单独服务于多线程处理，多线程且高频访问的业务查询，mqg于20181106增加
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataSet GetDataSetThreadSafe(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            DataSet ds = new DataSet();

            //为了不独立实现支持多线程的处理，又能允许多线程应用，只能牺牲资源每次新建连接
            OracleDataAdapter adapterTmp = new OracleDataAdapter();
            OracleConnection connection = new OracleConnection(base.ConnectionString);
            try
            {
                connection.Open();
                OracleCommand cmdTmp = (OracleCommand)this.GetCommand(sql, sqlexectype, connection, null, paras);
                adapterTmp.SelectCommand = cmdTmp;
                adapterTmp.Fill(ds, "tableName");
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            }
            finally
            {
                adapterTmp.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return ds;
        }


        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到数据集中返回。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataTable GetDataTable(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            DataSet ds = this.GetDataSet(sql, sqlexectype, paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 处理大文本字符串。可能不完善，需要后续处理
        /// </summary>
        /// <returns></returns>
        public override void SetCLOBVlaue(string txt, IDataParameter para, IDbConnection conn, IDbTransaction tran)
        {
            cmd = new OracleCommand();
            if (tran != null)
                cmd.Transaction = tran as OracleTransaction;
            cmd.CommandText = "declare xx clob; begin dbms_lob.createtemporary(xx, false, 0); :tempclob := xx; end;";
            cmd.Connection = conn as OracleConnection;
            OracleParameter p = new OracleParameter("tempclob", OracleType.Clob);
            p.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(p);
            cmd.ExecuteNonQuery();

            OracleLob tempLob = default(OracleLob);
            tempLob = (OracleLob)cmd.Parameters[0].Value;

            byte[] cbComments = Encoding.Unicode.GetBytes(txt);

            tempLob.BeginBatch(OracleLobOpenMode.ReadWrite);
            tempLob.Write(cbComments, 0, cbComments.Length);
            tempLob.EndBatch();

            para.Value = tempLob;
        }

        /// <summary>
        /// 参数前面的符号
        /// </summary>
        /// <returns></returns>
        public override string ParamSign()
        {
            return ":";
        }

        /// <summary>
        /// 获取数据库表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <returns></returns>
        public override DataTable GetDataEntity(string dataEntName)
        {
            string sql = @"Select t1.TABLE_NAME,t1.COLUMN_NAME colname,t1.DATA_TYPE colType,t2.COMMENTS, t1.DATA_LENGTH, t1.NULLABLE
                                From USER_TAB_COLUMNS t1, USER_COL_COMMENTS t2
                                     Where t1.TABLE_NAME = t2.TABLE_NAME    And t1.COLUMN_NAME = t2.COLUMN_NAME And t1.TABLE_NAME = upper({0}TABLE_NAME)
                                     Order By t1.COLUMN_ID";
            sql = string.Format(sql, this.ParamSign());
            OracleParameter op = null;
            op = new OracleParameter(this.ParamSign() + "TABLE_NAME", OracleType.VarChar);
            op.Value = dataEntName;
            op.Direction = ParameterDirection.Input;

            DataSet ds = this.GetDataSet(sql, SqlExecType.SqlText, op);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }  
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public override string GetSystemDateTime()
        {
            string sql = @"select sysdate from dual";
            DataSet ds = this.GetDataSet(sql, SqlExecType.SqlText, null);

            if(Common.Utility.DsHasData(ds))
            {
                return Convert.ToDateTime(ds.Tables[0].Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            return "";
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="opPagerData"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataTable GetDataPager(FrameCommon.SSY_PagingExecuteParam opPagerData, params IDataParameter[] paras)
        {
            if(opPagerData.PagingParam.TotalSize > 0)
            {
                //实现获取分页数据
                StringBuilder sbbSql = new StringBuilder();
                string sortType = string.Empty; //排序类型

                if (opPagerData.PagingParam.SortType == FrameCommon.AscOrDesc.Asc)
                {
                    sortType = " asc ";
                }
                else
                {
                    sortType = " desc ";
                }

                if (string.IsNullOrEmpty(opPagerData.SqlWhere))
                {
                    sbbSql.Append(string.Format(@" Select * FROM (Select Rownum rowcnt, 
                                    QR.* from (Select  {0} from {1} {2} order by {3} {4} ", opPagerData.Fields, opPagerData.TableNameOrView,
                                        opPagerData.Joins, opPagerData.OrderField, sortType));
                }
                else
                {
                    sbbSql.Append(string.Format(@" Select * FROM (Select Rownum As rowcnt, 
                                    QR.* from (Select {0} from {1} {2} where {3} order by {4} {5} ", opPagerData.Fields,
                                        opPagerData.TableNameOrView, opPagerData.Joins, opPagerData.SqlWhere, opPagerData.OrderField, sortType));
                }

                //区间段
                int startRecord = (opPagerData.PagingParam.PageIndex - 1) * opPagerData.PagingParam.PageSize + 1;
                int endRecord = startRecord + opPagerData.PagingParam.PageSize - 1;

                sbbSql.Append(string.Format(@" ) QR) where rowcnt between {0} and {1} ", startRecord.ToString(),
                    endRecord.ToString()));

                return this.GetDataTable(sbbSql.ToString(), SqlExecType.SqlText, paras);
            }
            else
            {
                return new DataTable();
            }            
        }

        #region 相关转换函数

        /// <summary>
        /// 通用数据转换函数
        /// </summary>
        /// <param name="tarType"></param>
        /// <param name="socExpress"></param>
        /// <param name="formatp"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <param name="dateSplitChar">日期之间分割符号, 默认 -</param>
        /// <param name="date2timeSplitChar">日期与时间之间分割符号 默认 空格</param>
        /// <param name="timeSplitChar">时间之间分割符号，默认 :</param>
        /// <returns></returns>
        public override string ConvertTypeStr(DbType tarType, string socExpress, ConvertFormat formatp, bool opflag,
            string dateSplitChar, string date2timeSplitChar, string timeSplitChar)
        {
            string format = string.Empty; //格式处理
            if (string.IsNullOrEmpty(dateSplitChar)) { dateSplitChar = "-"; }
            if (string.IsNullOrEmpty(date2timeSplitChar)) { date2timeSplitChar = " "; }
            if (string.IsNullOrEmpty(timeSplitChar)) { timeSplitChar = ":"; }
            if (formatp == ConvertFormat.yyyy)
            {
                format = "yyyy";
            }
            else if (formatp == ConvertFormat.yyyy_mm)
            {
                format =  "yyyy_mm".Replace("_", dateSplitChar);
            }
            else if (formatp == ConvertFormat.yyyy_mm_dd)
            {
                format = "yyyy_mm_dd".Replace("_", dateSplitChar);
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohh)
            {
                format = "yyyy_mm_ddoHH24".Replace("_", dateSplitChar).Replace("o", date2timeSplitChar);
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhamm)
            {
                format = "yyyy_mm_ddoHH24ami".Replace("_", dateSplitChar).Replace("o", date2timeSplitChar).Replace("a", timeSplitChar);
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhammass)
            {
                format = "yyyy_mm_ddoHH24amiass".Replace("_", dateSplitChar).Replace("o", date2timeSplitChar).Replace("a", timeSplitChar);
            } 
            string resStr = string.Empty;
            if(tarType == DbType.String)
            {
                if(string.IsNullOrEmpty(format))
                {
                    if(opflag)
                    {
                        resStr = string.Format(" to_char('{0}') ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" to_char({0}) ", socExpress.ToString());
                    }                    
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" to_char('{0}', '{1}') ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" to_char({0}, '{1}') ", socExpress.ToString(), format);
                    }
                }                
            }
            else if (tarType == DbType.Int16 || tarType == DbType.Int32 || tarType == DbType.Int64)
            {
                if (string.IsNullOrEmpty(format))
                {
                    if(opflag)
                    {
                        resStr = string.Format(" to_number('{0}') ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" to_number({0}) ", socExpress.ToString());
                    }                    
                }
                else
                {
                    if(opflag)
                    {
                        resStr = string.Format(" to_number('{0}', '{1}')  ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" to_number({0}, '{1}')  ", socExpress.ToString(), format);
                    }                    
                } 
            }
            else if (tarType == DbType.DateTime)
            {
                if (string.IsNullOrEmpty(format))
                {
                    throw new Exception("transfering time format must to set format!");
                }
                else
                {
                    if(opflag)
                    {
                        resStr = string.Format(" to_date('{0}', '{1}')  ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" to_date({0}, '{1}')  ", socExpress.ToString(), format);
                    }                    
                } 
            }

            //后续继续扩展

            return resStr;
        }

        #endregion

        #region 相关关键符号

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <returns></returns>
        public override string ConnectChar()
        {
            return "||";
        }       

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="opflags">拼接内容说明，要和内容一一对应（value 数值 field 字段）</param>
        /// <param name="opfields">拼接内容, 可以是内容、字段</param>
        /// <returns></returns>
        public override string ConnectChar(List<ConvertFlag> opflags, List<string> opfields)
        {
            //return "||";

            StringBuilder resStr = new StringBuilder();

            for (int i = 0; i < opfields.Count; i++)
            {
                if (i == 0)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append(" '" + opfields[i] + "' || ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append(" " + opfields[i] + " || ");
                    }
                }
                else if (i > 0 && i < opfields.Count - 1)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append("  '" + opfields[i] + "' || ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append("  " + opfields[i] + " || ");
                    }
                }
                else if (i == opfields.Count - 1)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append(" '" + opfields[i] + "'  ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append("  " + opfields[i] + "  ");
                    }
                }
            }

            return resStr.ToString();
        }

        #endregion

        #region 相关常规应用函数

        /// <summary>
        /// 转换大写
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string ToUpper(string soc, bool opflag)
        {
            if(opflag)
            {
                return string.Format(" upper('{0}') ", soc);
            }
            else
            {
                return string.Format(" upper({0}) ", soc);
            }            
        }

        /// <summary>
        /// 转换小写
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string ToLower(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" lower('{0}') ", soc);
            }
            else
            {
                return string.Format(" lower({0}) ", soc);
            }            
        }

        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Trim(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" trim('{0}') ", soc);
            }
            else
            {
                return string.Format(" trim({0}) ", soc);
            }            
        }

        /// <summary>
        /// 去掉左空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Ltrim(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" ltrim('{0}') ", soc);
            }
            else
            {
                return string.Format(" ltrim({0}) ", soc);
            }            
        }

        /// <summary>
        /// 去掉右空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Rtrim(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" rtrim('{0}') ", soc);
            }
            else
            {
                return string.Format(" rtrim({0}) ", soc);
            }            
        }

        /// <summary>
        /// 从左边截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Left(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" substr('{0}', 0, {1}) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, 0, {1}) ", soc, n.ToString());
            }            
        }

        /// <summary>
        /// 从右边截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Right(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" substr('{0}', {1}, {2}) ", soc, (soc.Length - n + 1).ToString(), n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, {1}, {2}) ", soc, (soc.Length - n + 1).ToString(), n.ToString());
            }            
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="startIndex"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Substring(string soc, int startIndex, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" substr('{0}', {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
            }            
        }

        /// <summary>
        /// 字符串替换
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Replace(string soc, string oldStr, string newStr, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" replace('{0}', '{1}', '{2}') ", soc, oldStr, newStr);
            }
            else
            {
                return string.Format(" replace({0}, '{1}', '{2}') ", soc, oldStr, newStr);
            }            
        }

        /// <summary>
        /// Length
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Length(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" lenght('{0}') ", soc);
            }
            else
            {
                return string.Format(" lenght({0}) ", soc);
            }            
        }

        /// <summary>
        /// 字符串连接
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string ConcatString(string[] strs, bool opflag)
        {
            string tempStr = string.Empty;
            for (int i = 0; i < strs.Length; i++)
            {
                if (opflag)
                {
                    tempStr += "||" + "'" + strs[i] + "'";
                }
                else
                {
                    tempStr += "||" + strs[i];
                }                
            }          

            return string.Format(" {0} ", tempStr.TrimStart("||".ToCharArray()));
        }

        /// <summary>
        /// 左补空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Lpad(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" lpad('{0}', {1} ) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" lpad({0}, {1} ) ", soc, n.ToString());
            }            
        }

        /// <summary>
        /// 右边补空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Rpad(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" rpad('{0}', {1} ) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" rpad({0}, {1} ) ", soc, n.ToString());
            }            
        }

        /// <summary>
        /// 两个日期差
        /// </summary>
        /// <param name="datepart">仅支持yy MM dd HH mi ss</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Datediff(string datepart, string startDate, string endDate, bool opflag)
        {
            string resStr = string.Empty;
            string parts = "yy|MM|dd|HH|mi|ss";
            if(parts.IndexOf(datepart) == -1)
            {
                throw new Exception("date format error!");
            }

            string departFormat = string.Empty;
            if(datepart.ToUpper() == "YY")
            {
                departFormat = "/365";
            }
            else if (datepart.ToUpper() == "MM")
            {
                departFormat = "/12";
            }
            else if (datepart.ToUpper() == "DD")
            {
                departFormat = "*1";
            }
            else if (datepart.ToUpper() == "HH")
            {
                departFormat = "*24";
            }
            else if (datepart.ToUpper() == "MI")
            {
                departFormat = "*24*60";
            }
            else if (datepart.ToUpper() == "SS")
            {
                departFormat = "*24*60*60";
            }

            if (opflag)
            {
                resStr = string.Format(" (to_date('{0}', 'yyyy-MM-dd HH:mi:ss') - to_date('{1}', 'yyyy-MM-dd HH:mi:ss')) {2} ", startDate, endDate, departFormat);
            }
            else
            {
                resStr = string.Format(" (to_date({0}, 'yyyy-MM-dd HH:mi:ss') - to_date({1}, 'yyyy-MM-dd HH:mi:ss')) {2} ", startDate, endDate, departFormat);
            }            

            return resStr;
        }

        /// <summary>
        /// 日期部分增加
        /// </summary>
        /// <param name="datepart">仅支持yy MM dd HH mi ss</param>
        /// <param name="currDate"></param>
        /// <param name="addValue"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string DateAdd(string datepart, string currDate, string addValue, bool opflag)
        {
            string resStr = string.Empty;
            string parts = "yy|MM|dd|HH|mi|ss";
            if (parts.IndexOf(datepart) == -1)
            {
                throw new Exception("date add format error!");
            }
            string departFormat = string.Empty;
            if (datepart.ToUpper() == "YY")
            {
                departFormat = string.Format("+ {0}/365", addValue);
            }
            else if (datepart.ToUpper() == "MM")
            {
                departFormat = string.Format("+ {0}/12", addValue);
            }
            else if (datepart.ToUpper() == "DD")
            {
                departFormat = string.Format("+ {0}/1", addValue);
            }
            else if (datepart.ToUpper() == "HH")
            {
                departFormat = string.Format("+ {0}*24", addValue);
            }
            else if (datepart.ToUpper() == "MI")
            {
                departFormat = string.Format("+ {0}*24*60", addValue);
            }
            else if (datepart.ToUpper() == "SS")
            {
                departFormat = string.Format("+ {0}*24*60*60", addValue);
            }

            if (opflag)
            {
                resStr = string.Format(" to_date('{0}', 'yyyy-MM-dd HH:mi:ss') {1} ", currDate, departFormat);
            }
            else
            {
                resStr = string.Format(" to_date({0}, 'yyyy-MM-dd HH:mi:ss') {1} ", currDate, departFormat);
            }            

            return resStr;
        }

        /// <summary>
        /// 解析固定内容显示值
        /// </summary>
        /// <param name="soc">原始值</param>
        /// <param name="anaV">固定内容值 例如：value1|display1,  ...</param>
        /// <param name="defaultV"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Decode(string soc, string anaV, string defaultV, bool opflag)
        {
            //decode('a2','a1','true1','a2','true2','default') 
            string anaStr = string.Empty;
            string[] anaVs = anaV.Split(',');
            for (int i = 0; i < anaVs.Length; i++)
            {
                anaStr += string.Format("'{0}','{1}'", anaVs[i].Split('|')[0], anaVs[i].Split('|')[1]) + ",";
            }

            if (opflag)
            {
                return string.Format(" decode('{0}', {1} '{2}') ", soc, anaStr, defaultV);
            }
            else
            {
                return string.Format(" decode({0}, {1} '{2}') ", soc, anaStr, defaultV);
            }            
        }

        /// <summary>
        /// v1 is null 取v2 否则取v1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Nvl(string v1, string v2, bool opflag)
        {
            //NVL(null, '12') 
            if (opflag)
            {
                return string.Format(" nvl('{0}', '{1}') ", v1, v2);
            }
            else
            {
                return string.Format(" nvl({0}, '{1}') ", v1, v2);
            }            
        }

        /// <summary>
        /// v1 not null 取v2, v1 is null取v3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string Nvl2(string v1, string v2, string v3, bool opflag)
        {
            //nvl2('a', 'b', 'c')
            if (opflag)
            {
                return string.Format(" nvl2('{0}', '{1}', '{2}') ", v1, v2, v3);
            }
            else
            {
                return string.Format(" nvl2({0}, '{1}', '{2}') ", v1, v2, v3);
            }            
        }

        /// <summary>
        /// v1 = v2取null 否则取v1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public override string NullIf(string v1, string v2, bool opflag)
        {
            // NULLIF('a','b')
            if (opflag)
            {
                return string.Format(" nullif('{0}', '{1}') ", v1, v2);
            }
            else
            {
                return string.Format(" nullif({0}, '{1}') ", v1, v2);
            }            
        }

        /// <summary>
        /// v1 is null 取0
        /// </summary>
        /// <param name="v1">v1一般是字段，也可以取字段的聚合函数，如：sum(字段)</param>
        /// <returns></returns>
        public override string IsNull(string v1)
        {
            return string.Format("  nvl({0}, '0') ", v1);
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public override string GetDBDateTime()
        {
            return " sysdate ";
        }

        #endregion

    }
}

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Sql;
using System.Threading;

namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// 实现sql server的数据库访问工厂
    /// </summary>
    public class MSSqlServerDBFactory : AbstractDBFactory
    {
        public SqlConnection cnn = null;
        public SqlTransaction trans = null;
        public SqlCommand cmd = null;
        public SqlDataAdapter adapter = null;

        //mqg于20180928增加，扩展增加数据库连接池
        public List<SqlConnection> cnnList = new List<SqlConnection>();

        /// <summary>
        /// 获得一个数据库链接。
        /// </summary>
        /// <returns>数据库链接</returns>
        public override IDbConnection GetConnection()
        {
            //if (cnn == null || cnn.ConnectionString == null || cnn.ConnectionString == string.Empty)
            //{
            //    cnn = new SqlConnection(base.ConnectionString);
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
                    cnn = new SqlConnection(base.ConnectionString);
                    cnn.Open();
                    cnnList.Add(cnn);
                }
            }
            else
            {
                SqlConnection tmpcnn = new SqlConnection(base.ConnectionString);
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
            SqlConnection cnnTrans = (SqlConnection)this.GetConnection();

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
                cmd = new SqlCommand();
                cmd.CommandText = sql;
                if (sqlexectype == SqlExecType.SqlText)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (sqlexectype == SqlExecType.SqlProcName)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.Connection = (SqlConnection)conn;

                //这里可能需要兼容处理大数据字段，暂时先不处理，看看参数方式是否直接可以
                this.PrepareCommand(cmd, paras);

                if (trans != null)
                {
                    cmd.Transaction = (SqlTransaction)trans;
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
        private void PrepareCommand(SqlCommand cmd, IDataParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    //SqlParameter pp = (SqlParameter)((ICloneable)p).Clone();
                    //SqlParameter OraParameter = (SqlParameter)cmdParms[i];
                    //这里会发送重复添加参数问题，克隆处理下
                    SqlParameter OraParameter = (SqlParameter)((ICloneable)(SqlParameter)cmdParms[i]).Clone();                   
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
        private SqlDbType GetDBType(DbType type)
        {
            SqlDbType dbType = SqlDbType.VarChar;

            switch (type)
            {
                case DbType.Binary:
                    dbType = SqlDbType.Binary;
                    break;
                case DbType.Boolean:
                    dbType = SqlDbType.Bit;
                    break;
                case DbType.Date:
                    dbType = SqlDbType.DateTime2;
                    break;
                case DbType.Time:
                    dbType = SqlDbType.DateTime2;
                    break;
                case DbType.Single:
                    dbType = SqlDbType.Float;
                    break;
                case DbType.Int32:
                    dbType = SqlDbType.Int;
                    break;
                case DbType.Currency:
                    dbType = SqlDbType.Decimal;
                    break;
                case DbType.Double:
                    dbType = SqlDbType.Float;
                    break;
                case DbType.Int64:
                    dbType = SqlDbType.BigInt;
                    break;
                case DbType.String:
                    dbType = SqlDbType.VarChar;
                    break;
                case DbType.Object:
                    dbType = SqlDbType.Binary;
                    break;
                case DbType.AnsiString:
                    dbType = SqlDbType.VarChar;
                    break;
                case DbType.AnsiStringFixedLength:
                    dbType = SqlDbType.VarChar;
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
            SqlParameter op = null;
            SqlDbType dbType = GetDBType(type);

            op = new SqlParameter(this.ParamSign() + paraName.Replace(":", "").Replace("@", "").Replace("?", ""), dbType);
            op.Value = paramValue;
            op.Direction = ParameterDirection.Input;

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
                    cnn = (SqlConnection)this.GetConnection();    
                    cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {  
                    cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempint = cmd.ExecuteNonQuery();
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
                    cnn = (SqlConnection)this.GetConnection();
                    cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {
                    cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempobj = cmd.ExecuteScalar();
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
                cnn = (SqlConnection)this.GetConnection();
                cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, null, paras);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

            //mqg于20181104增加，单独处理MS SQL查询，不进入连接池管理, 不然多线程处理有时会报错
            //try
            //{
            //    cnn = (SqlConnection)this.GetConnection();
            //    cmd = (SqlCommand)this.GetCommand(sql, sqlexectype, cnn, null, paras);

            //    adapter = new SqlDataAdapter(cmd);
            //    adapter.Fill(ds, "tableName");
            //}
            //catch (Exception err)
            //{               
            //    throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is：" + sql, err);
            //}
            //finally
            //{               
            //    adapter.Dispose();
            //    cnn.Close();
            //    cnn.Dispose();               
            //}

            //为了不独立实现支持多线程的处理，又能允许多线程应用，只能牺牲资源每次新建连接
            //sql server需要特殊处理，必须的关闭连接才能释放command，所以不能只接用连接池的连接
            SqlDataAdapter adapterTmp = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                connection.Open();
                SqlCommand cmdTmp = (SqlCommand)this.GetCommand(sql, sqlexectype, connection, null, paras);
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

            //这种方式不满足多线程
            //using (SqlConnection connection = new SqlConnection(base.ConnectionString))
            //{
            //    SqlDataAdapter adapter = new SqlDataAdapter();
            //    SqlCommand cmdTmp = (SqlCommand)this.GetCommand(sql, sqlexectype, connection, null, paras);
            //    adapter.SelectCommand = cmdTmp;
            //    adapter.Fill(ds);
            //}

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
            SqlDataAdapter adapterTmp = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                connection.Open();
                SqlCommand cmdTmp = (SqlCommand)this.GetCommand(sql, sqlexectype, connection, null, paras);
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
            //ms sql没有
            return;            
        }

        /// <summary>
        /// 参数前面的符号
        /// </summary>
        /// <returns></returns>
        public override string ParamSign()
        {
            return "@";
        }

        /// <summary>
        /// 获取数据库表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <returns></returns>
        public override DataTable GetDataEntity(string dataEntName)
        {
            string sql = @" select  c.name TABLE_NAME, t.name colname, d.name colType,  cr.value COMMENTS,
	                         t.max_length DATA_LENGTH , t.is_nullable NULLABLE
                             from sys.all_columns t
	                         inner join sys.all_objects c 
                             on t.object_id = c.object_id 
	                         inner join sys.types d
	                         on t.user_type_id = d.user_type_id
	                         left join sys.extended_properties cr
	                         on t.object_id = cr.major_id 
	                         and t.column_id = cr.minor_id
                             where {0} = upper({1}TABLE_NAME)
                             Order By t.column_id";
            sql = string.Format(sql, this.ToUpper("c.name", false), this.ParamSign());
            SqlParameter op = null;
            op = new SqlParameter(this.ParamSign() + "TABLE_NAME", SqlDbType.VarChar);
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
            string sql = @"select getdate() ";
            DataSet ds = this.GetDataSet(sql, SqlExecType.SqlText, null);

            if (Common.Utility.DsHasData(ds))
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
            if (opPagerData.PagingParam.TotalSize > 0)
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
                    sbbSql.Append(string.Format(@" Select * FROM (select ROW_NUMBER() Over(order by {0} {1}) as rowId, {2} from {3} {4} ",
                        opPagerData.OrderField, sortType, opPagerData.Fields, opPagerData.TableNameOrView, opPagerData.Joins));
                }
                else
                {
                    sbbSql.Append(string.Format(@" Select * FROM (select ROW_NUMBER() Over(order by {0} {1}) as rowId, {2} from {3} {4} where {5} ",
                        opPagerData.OrderField, sortType, opPagerData.Fields, opPagerData.TableNameOrView, opPagerData.Joins, opPagerData.SqlWhere));
                }

                //区间段
                int startRecord = (opPagerData.PagingParam.PageIndex - 1) * opPagerData.PagingParam.PageSize + 1;
                int endRecord = startRecord + opPagerData.PagingParam.PageSize - 1;

                sbbSql.Append(string.Format(@" ) as t where rowId between {0} and {1} ", startRecord.ToString(),
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
            if (string.IsNullOrEmpty(date2timeSplitChar)) { dateSplitChar = " "; }
            if (string.IsNullOrEmpty(timeSplitChar)) { dateSplitChar = ":"; }
            if (formatp == ConvertFormat.yyyy)
            {
                format = ""; //没有相应格式
            }
            else if (formatp == ConvertFormat.yyyy_mm)
            {
                format = "";//没有相应格式
            }
            else if (formatp == ConvertFormat.yyyy_mm_dd)
            {
                format = "23";
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohh)
            {
                format = "";//没有相应格式
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhamm)
            {
                format = "";//没有相应格式
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhammass)
            {
                format = "20";
            }

            string resStr = string.Empty;
            if (tarType == DbType.String)
            {
                if (string.IsNullOrEmpty(format))
                {
                    if (opflag)
                    {
                        resStr = string.Format(" convert(varchar, '{0}') ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" convert(varchar, {0}) ", socExpress.ToString());
                    }
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" convert(varchar, '{0}', '{1}') ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" convert(varchar, {0}, '{1}') ", socExpress.ToString(), format);
                    }
                }
            }
            else if (tarType == DbType.Int16 || tarType == DbType.Int32 || tarType == DbType.Int64)
            {
                if (string.IsNullOrEmpty(format))
                {
                    if (opflag)
                    {
                        resStr = string.Format(" convert(int, '{0}') ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" convert(int, {0}) ", socExpress.ToString());
                    }
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" convert(int, '{0}', '{1}')  ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" convert(int, {0}, '{1}')  ", socExpress.ToString(), format);
                    }
                }
            }
            else if (tarType == DbType.DateTime)
            {
                format = "any"; //sql无需指定格式，只要是日期格式即可
                if (string.IsNullOrEmpty(format))
                {
                    throw new Exception("转换时间时必须指定format格式！");
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" convert(datetime, '{0}')  ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" convert(datetime, {0})  ", socExpress.ToString(), format);
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
            return "+";
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="opflags">拼接内容说明，要和内容一一对应（value 数值 field 字段）</param>
        /// <param name="opfields">拼接内容, 可以是内容、字段</param>
        /// <returns></returns>
        public override string ConnectChar(List<ConvertFlag> opflags, List<string> opfields)
        {
            //return "+";
            StringBuilder resStr = new StringBuilder();

            for (int i = 0; i < opfields.Count; i++)
            {
                if (i == 0)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append(" '" + opfields[i] + "' + ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append(" " + opfields[i] + " + ");
                    }
                }
                else if (i > 0 && i < opfields.Count - 1)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append("  '" + opfields[i] + "' + ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append("  " + opfields[i] + " + ");
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
            if (opflag)
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
                return string.Format(" replace('{0}', ' ', '') ", soc);
            }
            else
            {
                return string.Format(" replace({0}, ' ', '') ", soc);
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
                return string.Format(" left('{0}', 0, {1}) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" left({0}, 0, {1}) ", soc, n.ToString());
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
                return string.Format(" right('{0}', {1}) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" right({0}, {1}) ", soc, n.ToString());
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
                return string.Format(" substring('{0}', {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
            }
            else
            {
                return string.Format(" substring({0}, {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
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
                return string.Format(" len('{0}') ", soc);
            }
            else
            {
                return string.Format(" len({0}) ", soc);
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
                    tempStr += "+" + "'" + strs[i] + "'";
                }
                else
                {
                    tempStr += "+" + strs[i];
                }
            }

            return string.Format(" {0} ", tempStr);
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
                return string.Format(" space({1}) + '{0}' ", soc, n.ToString());
            }
            else
            {
                return string.Format(" space({1}) + '{0}' ", soc, n.ToString());
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
                return string.Format(" '{0}' + space({1}) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" {0} + space({1}) ", soc, n.ToString());
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
            if (parts.IndexOf(datepart) == -1)
            {
                throw new Exception("date format error!");
            }            

            if (opflag)
            {
                resStr = string.Format(" datediff({2}, convert(datetime, '{0}'), convert(datetime, '{1}')) ", 
                    startDate, endDate, datepart);
            }
            else
            {
                resStr = string.Format("  datediff({2}, convert(datetime, {0}), convert(datetime, {1})) ",
                    startDate, endDate, datepart);
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

            if (opflag)
            {
                resStr = string.Format(" dateadd({0}, {1}, convert(datetime, '{2}')) ", datepart, addValue, currDate);
            }
            else
            {
                resStr = string.Format(" dateadd({0}, {1}, convert(datetime, {2})) ", datepart, addValue, currDate);
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
            //select case '2'
            //when '1' then '2'
            //when '2' then '2'
            //when '3' then '2'
            //else '4'
            //end
            StringBuilder resSb = new StringBuilder();
            string[] anaVs = anaV.Split(',');
            if (opflag)
            {
                resSb.AppendLine(string.Format(" case '{0}' ", soc));                
            }
            else
            {
                resSb.AppendLine(string.Format(" case {0} ", soc));
            }

            for (int i = 0; i < anaVs.Length; i++)
            {
                resSb.AppendLine(string.Format(" when '{0}' then '{1}' ", anaVs[i].Split('|')[0], anaVs[i].Split('|')[1]));
            }

            resSb.AppendLine(string.Format(" else '{0}' ", defaultV));
            resSb.AppendLine(" end ");

            return resSb.ToString();    
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
            StringBuilder resSb = new StringBuilder();
            if (opflag)
            {
                resSb.AppendLine(string.Format(" case '{0}' ", v1));
            }
            else
            {
                resSb.AppendLine(string.Format(" case {0} ", v1));
            }

            resSb.AppendLine(string.Format(" when '{0}' then '{0}' ", v1));
            resSb.AppendLine(string.Format(" else '{0}' ", v2));
            resSb.AppendLine(" end ");

            return resSb.ToString();
            
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
            StringBuilder resSb = new StringBuilder();
            if (opflag)
            {
                resSb.AppendLine(string.Format(" case '{0}' ", v1));
            }
            else
            {
                resSb.AppendLine(string.Format(" case {0} ", v1));
            }

            resSb.AppendLine(string.Format(" when '{0}' then '{1}' ", v2));
            resSb.AppendLine(string.Format(" else '{0}' ", v3));
            resSb.AppendLine(" end ");

            return resSb.ToString();           
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
            StringBuilder resSb = new StringBuilder();
            if (opflag)
            {
                resSb.AppendLine(string.Format(" case '{0}' ", v1));
            }
            else
            {
                resSb.AppendLine(string.Format(" case {0} ", v1));
            }

            resSb.AppendLine(string.Format(" when '{0}' then null ", v2));
            resSb.AppendLine(string.Format(" else '{0}' ", v1));
            resSb.AppendLine(" end ");

            return resSb.ToString();            
        }

        /// <summary>
        /// v1 is null 取0
        /// </summary>
        /// <param name="v1">v1一般是字段，也可以取字段的聚合函数，如：sum(字段)</param>
        /// <returns></returns>
        public override string IsNull(string v1)
        {
            return string.Format(" isnull({0}) ", v1);
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public override string GetDBDateTime()
        {
            return " GETDATE() ";
        }

        #endregion
    }
}

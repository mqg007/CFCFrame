using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// ʵ��sql server�����ݿ���ʹ���
    /// </summary>
    public class MySqlDBFactory : AbstractDBFactory
    {
        public MySqlConnection cnn = null;
        public MySqlTransaction trans = null;
        public MySqlCommand cmd = null;
        public MySqlDataAdapter adapter = null;

        //mqg��20180928���ӣ���չ�������ݿ����ӳ�
        public List<MySqlConnection> cnnList = new List<MySqlConnection>();

        /// <summary>
        /// ���һ�����ݿ����ӡ�
        /// </summary>
        /// <returns>���ݿ�����</returns>
        public override IDbConnection GetConnection()
        {
            //if (cnn == null || cnn.ConnectionString == null || cnn.ConnectionString == string.Empty)
            //{
            //    cnn = new MySqlConnection(base.ConnectionString);
            //}
            //if (cnn.State != ConnectionState.Open)
            //{
            //    cnn.Open();
            //}
            //return cnn;

            //mqg��20180928���ӣ���չ���ӳش���           
            if (base.maxCnn == cnnList.Count)
            {
                //���ӳ��������ӳػ�ȡ
                Random rdom = new Random();
                int tmpIndex = rdom.Next(0, cnnList.Count);
                cnn = cnnList[tmpIndex];
               
                if (cnn == null || cnn.ConnectionString == null || cnn.ConnectionString == string.Empty)
                {
                    cnnList.Remove(cnnList[tmpIndex]);
                    cnn = new MySqlConnection(base.ConnectionString);
                    cnn.Open();
                    cnnList.Add(cnn);
                }
            }
            else
            {
                MySqlConnection tmpcnn = new MySqlConnection(base.ConnectionString);
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
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public override IDbTransaction GetIDbTransaction()
        {
            MySqlConnection cnnTrans = (MySqlConnection)this.GetConnection();

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
        /// ���һ�����ݿ��������
        /// </summary>
        /// <param name="sql">Ҫִ�е�SQL���</param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="conn">���ݿ�����</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns>�������</returns>
        public override IDbCommand GetCommand(string sql, SqlExecType sqlexectype, IDbConnection conn, IDbTransaction trans, params IDataParameter[] paras)
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.CommandText = sql;
                if (sqlexectype == SqlExecType.SqlText)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else if (sqlexectype == SqlExecType.SqlProcName)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.Connection = (MySqlConnection)conn;

                //���������Ҫ���ݴ���������ֶΣ���ʱ�Ȳ���������������ʽ�Ƿ�ֱ�ӿ���
                this.PrepareCommand(cmd, paras);

                if (trans != null)
                {
                    cmd.Transaction = (MySqlTransaction)trans;
                }
            }
            catch (Exception e)
            {
                throw new Exception("create command  error, please check sql script:" + sql, e);
            }

            return (IDbCommand)cmd;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdParms"></param>
        private void PrepareCommand(MySqlCommand cmd, IDataParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    //SqlParameter pp = (SqlParameter)((ICloneable)p).Clone();
                    //SqlParameter OraParameter = (SqlParameter)cmdParms[i];
                    //����ᷢ���ظ���Ӳ������⣬��¡������                    
                    MySqlParameter OraParameter = (MySqlParameter)((ICloneable)(MySqlParameter)cmdParms[i]).Clone();
                    cmd.Parameters.Add(OraParameter);
                }
            }
        }


        /// <summary>
        /// ��ȡ��������
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
        /// ��ȡһ��DataParameter����
        /// </summary>
        /// <param name="paraName">������</param>
        /// <param name="type">����</param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public override IDataParameter GetDataParameter(string paraName, DbType type, object paramValue)
        {
            MySqlParameter op = null;
            SqlDbType dbType = GetDBType(type);

            op = new MySqlParameter(this.ParamSign() + paraName.Replace(":", "").Replace("@", "").Replace("?", ""), dbType);
            op.Value = paramValue;
            op.DbType = type;
            op.Direction = ParameterDirection.Input;

            return (IDataParameter)op;
        }

        /// <summary>
        /// ִ��SQL��䡣
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        public override int ExecuteNonQuery(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {
            int tempint = 0;
            try
            {
                if (trans == null)
                {
                    cnn = (MySqlConnection)this.GetConnection();
                    cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {
                    cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempint = cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is��" + sql, err);
            }
            finally
            {
                //mqg��20180928���ӣ����ﲻ���ͷ����ӣ��������ӳع���
                //if (trans == null && cnn != null)
                //{
                //    cnn.Close();
                //    cnn.Dispose();
                //}
            }

            return tempint;
        }

        /// <summary>
        /// ִ��SQL�����ز�ѯ���
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
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
                    cnn = (MySqlConnection)this.GetConnection();
                    cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {
                    cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempobj = cmd.ExecuteScalar();
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is��" + sql, err);
            }
            finally
            {
                //mqg��20180928���ӣ����ﲻ���ͷ����ӣ��������ӳع���
                //if (trans == null && cnn != null)
                //{
                //    cnn.Close();
                //    cnn.Dispose();
                //}
            }

            return tempobj;
        }

        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽Read�з��ء�
        /// </summary>
        /// <param name="sql">Ҫִ�е�SQL��ѯ��</param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="paras"></param>
        /// <returns>����һ��������ѯ�����Read��</returns>
        public override IDataReader GetReader(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            try
            {
                cnn = (MySqlConnection)this.GetConnection();
                cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, null, paras);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is��" + sql, err);
            }
            finally
            {
                //mqg��20180928���ӣ����ﲻ���ͷ����ӣ��������ӳع���
                reader.Close();
                //cnn.Close();
                //cnn.Dispose();
            }

            return reader;
        }

        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataSet GetDataSet(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            DataSet ds = new DataSet();
            try
            {
                cnn = (MySqlConnection)this.GetConnection();
                cmd = (MySqlCommand)this.GetCommand(sql, sqlexectype, cnn, null, paras);

                adapter = new MySqlDataAdapter(cmd);   
                adapter.Fill(ds, "tableName");                
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is��" + sql, err);
            }
            finally
            {
                //mqg��20180928���ӣ����ﲻ���ͷ����ӣ��������ӳع���
                //adapter.Dispose();
                //cnn.Close();
                //cnn.Dispose();
            }

            return ds;
        }

        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// ���������ڶ��̴߳������߳��Ҹ�Ƶ���ʵ�ҵ���ѯ��mqg��20181106����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataSet GetDataSetThreadSafe(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            DataSet ds = new DataSet();

            //Ϊ�˲�����ʵ��֧�ֶ��̵߳Ĵ�������������߳�Ӧ�ã�ֻ��������Դÿ���½�����
            MySqlDataAdapter adapterTmp = new MySqlDataAdapter();
            MySqlConnection connection = new MySqlConnection(base.ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand cmdTmp = (MySqlCommand)this.GetCommand(sql, sqlexectype, connection, null, paras);
                adapterTmp.SelectCommand = cmdTmp;
                adapterTmp.Fill(ds, "tableName");
            }
            catch (Exception err)
            {
                throw new Exception("SQL query error!" + err.Message + "\r\n SQL script is��" + sql, err);
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
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
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
        /// ������ı��ַ��������ܲ����ƣ���Ҫ��������
        /// </summary>
        /// <returns></returns>
        public override void SetCLOBVlaue(string txt, IDataParameter para, IDbConnection conn, IDbTransaction tran)
        {
            //ms sqlû��
            return;
        }

        /// <summary>
        /// ����ǰ��ķ���
        /// </summary>
        /// <returns></returns>
        public override string ParamSign()
        {
            return "@";
        }

        /// <summary>
        /// ��ȡ���ݿ��ṹ
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <returns></returns>
        public override DataTable GetDataEntity(string dataEntName)
        {
            string sql = @" select  c.TABLE_NAME, t.column_name colname, t.column_type colType,  t.column_COMMENT COMMENTS,
	                         t.CHARACTER_MAXIMUM_LENGTH DATA_LENGTH , t.IS_NULLABLE NULLABLE
                             from information_schema.COLUMNS t
	                         inner join information_schema.tables c                              
                             on t.table_schema = c.table_schema 
                             and t.table_name = c.table_name 
                             where {0} = upper({1}TABLE_NAME)
                             Order By t.ordinal_position";
            sql = string.Format(sql, this.ToUpper("c.TABLE_NAME", false), this.ParamSign());
            MySqlParameter op = null;
            op = new MySqlParameter(this.ParamSign() + "TABLE_NAME", SqlDbType.VarChar);
            op.Value = dataEntName;
            op.DbType = DbType.String;
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
        /// ��ȡϵͳʱ��
        /// </summary>
        /// <returns></returns>
        public override string GetSystemDateTime()
        {
            string sql = @"select now() ";
            DataSet ds = this.GetDataSet(sql, SqlExecType.SqlText, null);

            if (Common.Utility.DsHasData(ds))
            {
                return Convert.ToDateTime(ds.Tables[0].Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            return "";
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="opPagerData"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataTable GetDataPager(FrameCommon.SSY_PagingExecuteParam opPagerData, params IDataParameter[] paras)
        {
            if (opPagerData.PagingParam.TotalSize > 0)
            {
                //ʵ�ֻ�ȡ��ҳ����
                StringBuilder sbbSql = new StringBuilder();
                string sortType = string.Empty; //��������

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
                    sbbSql.Append(string.Format(@" Select {0} FROM {1} {2} order by {3} {4} ",
                         opPagerData.Fields, opPagerData.TableNameOrView, opPagerData.Joins, opPagerData.OrderField, sortType));
                }
                else
                {
                    sbbSql.Append(string.Format(@" Select {0} FROM {1} {2} where {5} order by {3} {4}  ",
                        opPagerData.Fields, opPagerData.TableNameOrView, opPagerData.Joins, opPagerData.OrderField, sortType, opPagerData.SqlWhere));
                }

                //�����
                int startRecord = (opPagerData.PagingParam.PageIndex - 1) * opPagerData.PagingParam.PageSize + 1;
                int endRecord = startRecord + opPagerData.PagingParam.PageSize - 1;

                sbbSql.Append(string.Format(@" limit {0} , {1} ", startRecord.ToString(), opPagerData.PagingParam.PageSize.ToString()));

                return this.GetDataTable(sbbSql.ToString(), SqlExecType.SqlText, paras);
            }
            else
            {
                return new DataTable();
            }
        }

        #region ���ת������

        /// <summary>
        /// ͨ������ת������
        /// </summary>
        /// <param name="tarType"></param>
        /// <param name="socExpress"></param>
        /// <param name="formatp"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <param name="dateSplitChar">����֮��ָ����, Ĭ�� -</param>
        /// <param name="date2timeSplitChar">������ʱ��֮��ָ���� Ĭ�� �ո�</param>
        /// <param name="timeSplitChar">ʱ��֮��ָ���ţ�Ĭ�� :</param>
        /// <returns></returns>
        public override string ConvertTypeStr(DbType tarType, string socExpress, ConvertFormat formatp, bool opflag,
            string dateSplitChar, string date2timeSplitChar, string timeSplitChar)
        {
            string format = string.Empty; //��ʽ����
            if (string.IsNullOrEmpty(dateSplitChar)) { dateSplitChar = "-"; }
            if (string.IsNullOrEmpty(date2timeSplitChar)) { dateSplitChar = " "; }
            if (string.IsNullOrEmpty(timeSplitChar)) { dateSplitChar = ":"; }
            if (formatp == ConvertFormat.yyyy)
            {
                format = ""; //û����Ӧ��ʽ
            }
            else if (formatp == ConvertFormat.yyyy_mm)
            {
                format = "";//û����Ӧ��ʽ
            }
            else if (formatp == ConvertFormat.yyyy_mm_dd)
            {
                format = "%Y-%m-%d";
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohh)
            {
                format = "";//û����Ӧ��ʽ
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhamm)
            {
                format = "";//û����Ӧ��ʽ
            }
            else if (formatp == ConvertFormat.yyyy_mm_ddohhammass)
            {
                format = "%Y-%m-%d %H:%i:%s.%f";
            }

            string resStr = string.Empty;
            if (tarType == DbType.String)
            {
                if (string.IsNullOrEmpty(format))
                {
                    if (opflag)
                    {
                        resStr = string.Format(" CONCAT('{0}','') ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" CONCAT({0},'') ", socExpress.ToString());
                    }
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" date_format('{0}', '{1}') ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" date_format('{0}', '{1}') ", socExpress.ToString(), format);
                    }
                }
            }
            else if (tarType == DbType.Int16 || tarType == DbType.Int32 || tarType == DbType.Int64)
            {
                if (string.IsNullOrEmpty(format))
                {
                    if (opflag)
                    {
                        resStr = string.Format(" CAST('{0}' SIGNED) ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" CAST({0} AS SIGNED) ", socExpress.ToString());
                    }
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" unix_timestamp('{0}')  ", socExpress.ToString());
                    }
                    else
                    {
                        resStr = string.Format(" unix_timestamp('{0}')  ", socExpress.ToString());
                    }
                }
            }
            else if (tarType == DbType.DateTime)
            {
                format = "any"; //sql����ָ����ʽ��ֻҪ�����ڸ�ʽ����
                if (string.IsNullOrEmpty(format))
                {
                    throw new Exception("ת��ʱ��ʱ����ָ��format��ʽ��");
                }
                else
                {
                    if (opflag)
                    {
                        resStr = string.Format(" str_to_date('{0}','{1}')  ", socExpress.ToString(), format);
                    }
                    else
                    {
                        resStr = string.Format(" str_to_date('{0}','{1}')  ", socExpress.ToString(), format);
                    }
                }
            }

            //����������չ

            return resStr;
        }

        #endregion

        #region ��عؼ�����

        /// <summary>
        /// ƴ���ַ�����mysql����ʹ��
        /// </summary>
        /// <returns></returns>
        public override string ConnectChar()
        {
            return "";
        }

        /// <summary>
        /// ƴ���ַ���
        /// </summary>
        /// <param name="opflags">ƴ������˵����Ҫ������һһ��Ӧ��value ��ֵ field �ֶΣ�</param>
        /// <param name="opfields">ƴ������, ���������ݡ��ֶ�</param>
        /// <returns></returns>
        public override string ConnectChar(List<ConvertFlag> opflags, List<string> opfields)
        {
            StringBuilder  resStr = new StringBuilder();

            for (int i = 0; i < opfields.Count; i++)
            {
                if(i == 0)
                {
                    if(opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append(" concat( '" + opfields[i] + "' , ");
                    }
                    else if(opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append(" concat( " + opfields[i] + " , ");
                    }
                }
                else if(i > 0 && i < opfields.Count - 1)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append("  '" + opfields[i] + "' , ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append("  " + opfields[i] + " , ");
                    }
                }
                else  if(i == opfields.Count - 1)
                {
                    if (opflags[i] == ConvertFlag.Value)
                    {
                        resStr.Append(" '" + opfields[i] + "') ");
                    }
                    else if (opflags[i] == ConvertFlag.Field)
                    {
                        resStr.Append("  " + opfields[i] + " ) ");
                    }
                }      
            }

            return resStr.ToString();
        }

        #endregion

        #region ��س���Ӧ�ú���

        /// <summary>
        /// ת����д
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ת��Сд
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ȥ���ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ȥ����ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ȥ���ҿո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ����߽�ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ���ұ߽�ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="startIndex"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// �ַ����滻
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public override string Length(string soc, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" LENGTH('{0}') ", soc);
            }
            else
            {
                return string.Format(" LENGTH({0}) ", soc);
            }
        }

        /// <summary>
        /// �ַ�������
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public override string ConcatString(string[] strs, bool opflag)
        {
            string tempStr = string.Empty;
            for (int i = 0; i < strs.Length; i++)
            {
                if (opflag)
                {
                    tempStr += "," + "'" + strs[i] + "'";
                }
                else
                {
                    tempStr += "," + strs[i];
                }
            }
            if (!string.IsNullOrEmpty(tempStr))
            {
                tempStr = tempStr.Substring(1, tempStr.Length - 1);
            }
            return string.Format(" concat_ws('',{0}) ", tempStr);
        }

        /// <summary>
        /// �󲹿ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public override string Lpad(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" concat_ws('',space({1}),'{0}') ", soc, n.ToString());
            }
            else
            {
                return string.Format(" concat_ws('',space({1}) ,'{0}') ", soc, n.ToString());
            }
        }

        /// <summary>
        /// �ұ߲��ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public override string Rpad(string soc, int n, bool opflag)
        {
            if (opflag)
            {
                return string.Format(" concat_ws('','{0}' ,space({1})) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" concat_ws('','{0}' ,space({1})) ", soc, n.ToString());
            }
        }

        /// <summary>
        /// �������ڲ�
        /// </summary>
        /// <param name="datepart">��֧��yy MM dd HH mi ss</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
                resStr = string.Format(" datediff('{1}', '{0}') ",
                    startDate, endDate);
            }
            else
            {
                resStr = string.Format("  datediff('{1}', '{0}') ",
                    startDate, endDate);
            }

            return resStr;
        }

        /// <summary>
        /// ���ڲ�������
        /// </summary>
        /// <param name="datepart">��֧��yy MM dd HH mi ss</param>
        /// <param name="currDate"></param>
        /// <param name="addValue"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
                resStr = string.Format(" date_add({0}, interval {1} day) ", currDate, addValue);
            }
            else
            {
                resStr = string.Format(" date_add({0}, interval {1} day) ", currDate, addValue);
            }

            return resStr;
        }

        /// <summary>
        /// �����̶�������ʾֵ
        /// </summary>
        /// <param name="soc">ԭʼֵ</param>
        /// <param name="anaV">�̶�����ֵ ���磺value1|display1,  ...</param>
        /// <param name="defaultV"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 is null ȡv2 ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 not null ȡv2, v1 is nullȡv3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 = v2ȡnull ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 is null ȡ0
        /// </summary>
        /// <param name="v1">v1һ�����ֶΣ�Ҳ����ȡ�ֶεľۺϺ������磺sum(�ֶ�)</param>
        /// <returns></returns>
        public override string IsNull(string v1)
        {
            return string.Format(" isnull({0}) ", v1);
        }

        /// <summary>
        /// ��ȡ���ݿ�ʱ��
        /// </summary>
        /// <returns></returns>
        public override string GetDBDateTime()
        {
            return " now() ";
        }

        #endregion
    }
}

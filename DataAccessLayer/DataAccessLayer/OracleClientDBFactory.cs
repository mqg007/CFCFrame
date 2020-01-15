using System;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// ʵ��Oracle�����ݿ���ʹ���
    /// </summary>
    public class OracleClientFactory : AbstractDBFactory
    {
        public OracleConnection cnn = null;
        public OracleTransaction trans = null;
        public OracleCommand cmd = null;
        public OracleDataAdapter adapter = null;

        //mqg��20180928���ӣ���չ�������ݿ����ӳ�
        public List<OracleConnection> cnnList = new List<OracleConnection>();

        /// <summary>
        /// ���һ�����ݿ����ӡ�
        /// </summary>
        /// <returns>���ݿ�����</returns>
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
        /// ��ȡ����
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

                //���������Ҫ���ݴ���oracle��bolb��colb�����ֶΣ���ʱ�Ȳ���������������ʽ�Ƿ�ֱ�ӿ���
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
        /// �������
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
        /// ��ȡ��������
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
        /// ��ȡһ��DataParameter����
        /// </summary>
        /// <param name="paraName">������</param>
        /// <param name="type">����</param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public override IDataParameter GetDataParameter(string paraName, DbType type, object paramValue)
        {
            OracleParameter op = null;
            OracleType dbType = GetDBType(type);
            //�����α꣬�α������Ĭ��cur_rt
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
                    cnn = (OracleConnection)this.GetConnection();
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {               
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempint = cmd.ExecuteNonQuery();
                //������������������ʹ��.netȡ���嵼�±���
                cmd.Parameters.Clear();
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
                    cnn = (OracleConnection)this.GetConnection();
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, (IDbTransaction)trans, paras);
                }
                else
                {
                    cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, trans.Connection, trans, paras);
                }

                tempobj = cmd.ExecuteScalar();
                //������������������ʹ��.netȡ���嵼�±���
                cmd.Parameters.Clear();
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
                cnn =(OracleConnection)this.GetConnection();
                cmd =(OracleCommand)this.GetCommand(sql, sqlexectype, (IDbConnection)cnn, null, paras);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //������������������ʹ��.netȡ���嵼�±���
                cmd.Parameters.Clear();
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
                cnn = (OracleConnection)this.GetConnection();
                cmd = (OracleCommand)this.GetCommand(sql, sqlexectype, cnn, null, paras);
                adapter = new OracleDataAdapter(cmd);
                adapter.Fill(ds, "tableName");
                //������������������ʹ��.netȡ���嵼�±���
                cmd.Parameters.Clear();
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
        /// ����ǰ��ķ���
        /// </summary>
        /// <returns></returns>
        public override string ParamSign()
        {
            return ":";
        }

        /// <summary>
        /// ��ȡ���ݿ��ṹ
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
        /// ��ȡϵͳʱ��
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
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="opPagerData"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public override DataTable GetDataPager(FrameCommon.SSY_PagingExecuteParam opPagerData, params IDataParameter[] paras)
        {
            if(opPagerData.PagingParam.TotalSize > 0)
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

                //�����
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

            //����������չ

            return resStr;
        }

        #endregion

        #region ��عؼ�����

        /// <summary>
        /// ƴ���ַ���
        /// </summary>
        /// <returns></returns>
        public override string ConnectChar()
        {
            return "||";
        }       

        /// <summary>
        /// ƴ���ַ���
        /// </summary>
        /// <param name="opflags">ƴ������˵����Ҫ������һһ��Ӧ��value ��ֵ field �ֶΣ�</param>
        /// <param name="opfields">ƴ������, ���������ݡ��ֶ�</param>
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

        #region ��س���Ӧ�ú���

        /// <summary>
        /// ת����д
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
                return string.Format(" trim('{0}') ", soc);
            }
            else
            {
                return string.Format(" trim({0}) ", soc);
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
                return string.Format(" substr('{0}', 0, {1}) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, 0, {1}) ", soc, n.ToString());
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
                return string.Format(" substr('{0}', {1}, {2}) ", soc, (soc.Length - n + 1).ToString(), n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, {1}, {2}) ", soc, (soc.Length - n + 1).ToString(), n.ToString());
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
                return string.Format(" substr('{0}', {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
            }
            else
            {
                return string.Format(" substr({0}, {1}, {2}) ", soc, startIndex.ToString(), n.ToString());
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
                return string.Format(" lenght('{0}') ", soc);
            }
            else
            {
                return string.Format(" lenght({0}) ", soc);
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
                return string.Format(" lpad('{0}', {1} ) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" lpad({0}, {1} ) ", soc, n.ToString());
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
                return string.Format(" rpad('{0}', {1} ) ", soc, n.ToString());
            }
            else
            {
                return string.Format(" rpad({0}, {1} ) ", soc, n.ToString());
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
        /// �����̶�������ʾֵ
        /// </summary>
        /// <param name="soc">ԭʼֵ</param>
        /// <param name="anaV">�̶�����ֵ ���磺value1|display1,  ...</param>
        /// <param name="defaultV"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 is null ȡv2 ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 = v2ȡnull ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
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
        /// v1 is null ȡ0
        /// </summary>
        /// <param name="v1">v1һ�����ֶΣ�Ҳ����ȡ�ֶεľۺϺ������磺sum(�ֶ�)</param>
        /// <returns></returns>
        public override string IsNull(string v1)
        {
            return string.Format("  nvl({0}, '0') ", v1);
        }

        /// <summary>
        /// ��ȡ���ݿ�ʱ��
        /// </summary>
        /// <returns></returns>
        public override string GetDBDateTime()
        {
            return " sysdate ";
        }

        #endregion

    }
}

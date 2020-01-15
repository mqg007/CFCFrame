using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace DataAccessLayer.DataBaseFactory
{
   
    /// <summary>
    /// ִ��SQL����
    /// </summary>
    public enum SqlExecType
    {
        /// <summary>
        /// �ַ���
        /// </summary>
        SqlText,
        /// <summary>
        /// �洢����
        /// </summary>
        SqlProcName
    }

    /// <summary>
    /// ͨ��ת����ʽ�趨��Ŀǰֻʵ�������ڵĸ�ʽ
    /// </summary>
    public enum ConvertFormat
    {
        intT,
        yyyy,
        yyyy_mm,
        yyyy_mm_dd,
        yyyy_mm_ddohh,
        yyyy_mm_ddohhamm,
        yyyy_mm_ddohhammass
    }

    /// <summary>
    /// ת������˵������ʶ��ת�������ݣ����������ֶ�
    /// </summary>
    public enum ConvertFlag
    {
        /// <summary>
        /// ת������
        /// </summary>
        Value,
        /// <summary>
        /// ת���ֶ�
        /// </summary>
        Field
    }

    /// <summary>
    /// �������ݿ�����ݿ���� ������ 
    /// </summary>
    public abstract class AbstractDBFactory
    {
        protected int maxCnn = 10; //�����������10�����ݿ�����

        protected string connectionString;

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        string connectionKey = "";
        /// <summary>
        /// ���ݿ����ý�������Ӧ�������ļ��еļ�����
        /// </summary>
        public  string ConnectionKey
        {
            get
            {
                return connectionKey;
            }
            set
            {
                connectionKey = value;
            }
        }

        string dbSchema = "";
        /// <summary>
        /// ���ݿ����ӵ����
        /// </summary>
        public string DbSchema
        {
            get
            {
                return dbSchema;
            }
            set
            {
                dbSchema = value;
            }
        }

        public IDbConnection cnn;

        public IDbCommand cmd;

        public IDataReader reader;

        /// <summary>
        /// ���һ�����ݿ����ӡ�
        /// </summary>
        /// <returns>���ݿ�����</returns>
        public virtual IDbConnection GetConnection()
        {
            return null;
        }

        public virtual IDbTransaction GetIDbTransaction()
        {
            return null;
        }

        /// <summary>
        /// ���һ�����ݿ��������
        /// </summary>
        /// <param name="sql">Ҫִ�е�SQL����洢������</param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="conn">���ݿ�����</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns>�������</returns>
        public virtual IDbCommand GetCommand(string sql, SqlExecType sqlexectype, IDbConnection conn, IDbTransaction trans, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// ��ȡһ��DataParameter����
        /// </summary>
        /// <param name="paraName">����������oracle�α꣬��������̶�Ϊcur_rt</param>
        /// <param name="type">��Ϊoracle�α꣬��ò������ַ������ͼ���</param>
        /// <param name="paramValue">��Ϊoracle�α꣬��ò��������ַ�������</param>
        /// <returns></returns>
        public virtual IDataParameter GetDataParameter(string paraName, DbType type, object paramValue)
        {
            return null;
        }


        /// <summary>
        /// ִ��SQL��䡣
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        public virtual int ExecuteNonQuery(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {
            return -1;
        }

        /// <summary>
        /// ִ��SQL�����ز�ѯ���
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual Object ExecuteScalar(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {            
            return null;
        }


        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽Read�з��ء�
        /// </summary>
        /// <param name="sql">Ҫִ�е�SQL��ѯ��</param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="paras"></param>
        /// <returns>����һ��������ѯ�����Read��</returns>
        public virtual IDataReader GetReader(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }


        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// ���������ڶ��̴߳������߳��Ҹ�Ƶ���ʵ�ҵ���ѯ��mqg��20181106����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSetThreadSafe(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// ����SQL�����в�ѯ������ѯ������浽���ݼ��з��ء�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">ִ��SQL����</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// �����ı��ַ�����ֵ��
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="para"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public virtual void SetCLOBVlaue(string txt, IDataParameter para, IDbConnection conn, IDbTransaction tran)
        {

        }   
        
        /// <summary>
        /// ����ǰ��ķ���
        /// </summary>
        /// <returns></returns>
        public virtual string ParamSign()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݿ��ṹ
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <returns></returns>
        public virtual DataTable GetDataEntity(string dataEntName)
        {
            return null;
        }

        /// <summary>
        /// ��ȡϵͳʱ��
        /// </summary>
        /// <returns></returns>
        public virtual string GetSystemDateTime()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="opPagerData"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataTable GetDataPager(FrameCommon.SSY_PagingExecuteParam  opPagerData, params IDataParameter[] paras)
        {
            return null;
        }

        #region ���ת������

        /// <summary>
        /// ͨ������ת������
        /// </summary>
        /// <param name="tarType"></param>
        /// <param name="socExpress"></param>
        /// <param name="format"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <param name="dateSplitChar">����֮��ָ����, Ĭ�� -</param>
        /// <param name="date2timeSplitChar">������ʱ��֮��ָ���� Ĭ�� �ո�</param>
        /// <param name="timeSplitChar">ʱ��֮��ָ���ţ�Ĭ�� :</param>
        /// <returns></returns>
        public virtual string ConvertTypeStr(DbType tarType, string socExpress, ConvertFormat format, bool opflag, 
            string dateSplitChar, string date2timeSplitChar, string timeSplitChar)
        {
            return "";
        }

        #endregion

        #region ��عؼ�����

        /// <summary>
        /// ƴ���ַ���
        /// </summary>
        /// <returns></returns>
        public virtual string ConnectChar()
        {
            return "";
        }

        /// <summary>
        /// ƴ���ַ���
        /// </summary>
        /// <param name="opflags">ƴ������˵����Ҫ������һһ��Ӧ��value ��ֵ field �ֶΣ�</param>
        /// <param name="opfields">ƴ������, ���������ݡ��ֶ�</param>
        /// <returns></returns>
        public virtual string ConnectChar(List<ConvertFlag> opflags, List<string> opfields)
        {
            return "";
        }

        #endregion

        #region ��س���Ӧ�ú���

        /// <summary>
        /// ת����д
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string ToUpper(string soc, bool opflag)
        {
            return "";            
        }

        /// <summary>
        /// ת��Сд
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string ToLower(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ȥ���ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Trim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ȥ����ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Ltrim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ȥ���ҿո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Rtrim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ����߽�ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Left(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ���ұ߽�ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Right(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="startIndex"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Substring(string soc, int startIndex, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �ַ����滻
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Replace(string soc, string oldStr, string newStr, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// Length
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Length(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �ַ�������
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string ConcatString(string[] strs, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �󲹿ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Lpad(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �ұ߲��ո�
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Rpad(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �������ڲ�
        /// </summary>
        /// <param name="datepart">��֧��yy MM dd HH mi ss ms </param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Datediff(string datepart, string startDate, string endDate, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// ���ڲ�������
        /// </summary>
        /// <param name="datepart">��֧��yy MM dd HH mi ss ms</param>
        /// <param name="currDate"></param>
        /// <param name="addValue"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string DateAdd(string datepart, string currDate, string addValue, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// �����̶�������ʾֵ
        /// </summary>
        /// <param name="soc">ԭʼֵ</param>
        /// <param name="anaV">�̶�����ֵ ���磺value1|display1,  ...</param>
        /// <param name="defaultV"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Decode(string soc, string anaV, string defaultV, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 is null ȡv2 ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Nvl(string v1, string v2, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 not null ȡv2, v1 is nullȡv3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string Nvl2(string v1, string v2, string v3, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 = v2ȡnull ����ȡv1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true ��ֵ  false �ֶ�</param>
        /// <returns></returns>
        public virtual string NullIf(string v1, string v2, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 is null ȡ0
        /// </summary>
        /// <param name="v1">v1һ�����ֶΣ�Ҳ����ȡ�ֶεľۺϺ������磺sum(�ֶ�)</param>
        /// <returns></returns>
        public virtual string IsNull(string v1)
        {
            return "";
        }

        /// <summary>
        /// ��ȡ���ݿ�ʱ��
        /// </summary>
        /// <returns></returns>
        public virtual string GetDBDateTime()
        {
            return "";
        }

        #endregion
    }


}

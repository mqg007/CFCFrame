using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace DataAccessLayer.DataBaseFactory
{
   
    /// <summary>
    /// 执行SQL类型
    /// </summary>
    public enum SqlExecType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        SqlText,
        /// <summary>
        /// 存储过程
        /// </summary>
        SqlProcName
    }

    /// <summary>
    /// 通用转换格式设定，目前只实现了日期的格式
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
    /// 转换参数说明，标识是转换的内容，还是数据字段
    /// </summary>
    public enum ConvertFlag
    {
        /// <summary>
        /// 转换内容
        /// </summary>
        Value,
        /// <summary>
        /// 转换字段
        /// </summary>
        Field
    }

    /// <summary>
    /// 访问数据库的数据库访问 工厂。 
    /// </summary>
    public abstract class AbstractDBFactory
    {
        protected string connectionString;

        /// <summary>
        /// 数据库链接字符串
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
        /// 数据库配置健名。对应于配置文件中的键名。
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
        /// 数据库对象拥有者
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
        /// 获得一个数据库链接。
        /// </summary>
        /// <returns>数据库链接</returns>
        public virtual IDbConnection GetConnection()
        {
            return null;
        }

        public virtual IDbTransaction GetIDbTransaction()
        {
            return null;
        }

        /// <summary>
        /// 获得一个数据库命令对象。
        /// </summary>
        /// <param name="sql">要执行的SQL语句或存储过程名</param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns>命令对象</returns>
        public virtual IDbCommand GetCommand(string sql, SqlExecType sqlexectype, IDbConnection conn, IDbTransaction trans, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// 获取一个DataParameter对象。
        /// </summary>
        /// <param name="paraName">若参数名是oracle游标，则参数名固定为cur_rt</param>
        /// <param name="type">若为oracle游标，则该参数传字符串类型即可</param>
        /// <param name="paramValue">若为oracle游标，则该参数传空字符串即可</param>
        /// <returns></returns>
        public virtual IDataParameter GetDataParameter(string paraName, DbType type, object paramValue)
        {
            return null;
        }


        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        public virtual int ExecuteNonQuery(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {
            return -1;
        }

        /// <summary>
        /// 执行SQL，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="trans"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual Object ExecuteScalar(string sql, SqlExecType sqlexectype, IDbTransaction trans, params IDataParameter[] paras)
        {            
            return null;
        }


        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到Read中返回。
        /// </summary>
        /// <param name="sql">要执行的SQL查询。</param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="paras"></param>
        /// <returns>返回一个包含查询结果的Read。</returns>
        public virtual IDataReader GetReader(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }


        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到数据集中返回。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataSet GetDataSet(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// 根据SQL语句进行查询，将查询结果保存到数据集中返回。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlexectype">执行SQL类型</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, SqlExecType sqlexectype, params IDataParameter[] paras)
        {
            return null;
        }

        /// <summary>
        /// 给大文本字符串赋值。
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="para"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public virtual void SetCLOBVlaue(string txt, IDataParameter para, IDbConnection conn, IDbTransaction tran)
        {

        }   
        
        /// <summary>
        /// 参数前面的符号
        /// </summary>
        /// <returns></returns>
        public virtual string ParamSign()
        {
            return null;
        }

        /// <summary>
        /// 获取数据库表结构
        /// </summary>
        /// <param name="dataEntName"></param>
        /// <returns></returns>
        public virtual DataTable GetDataEntity(string dataEntName)
        {
            return null;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public virtual string GetSystemDateTime()
        {
            return null;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="opPagerData"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public virtual DataTable GetDataPager(FrameCommon.SSY_PagingExecuteParam  opPagerData, params IDataParameter[] paras)
        {
            return null;
        }

        #region 相关转换函数

        /// <summary>
        /// 通用数据转换函数
        /// </summary>
        /// <param name="tarType"></param>
        /// <param name="socExpress"></param>
        /// <param name="format"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <param name="dateSplitChar">日期之间分割符号, 默认 -</param>
        /// <param name="date2timeSplitChar">日期与时间之间分割符号 默认 空格</param>
        /// <param name="timeSplitChar">时间之间分割符号，默认 :</param>
        /// <returns></returns>
        public virtual string ConvertTypeStr(DbType tarType, string socExpress, ConvertFormat format, bool opflag, 
            string dateSplitChar, string date2timeSplitChar, string timeSplitChar)
        {
            return "";
        }

        #endregion

        #region 相关关键符号

        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="opflags">拼接内容说明，要和内容一一对应（value 数值 field 字段）</param>
        /// <param name="opfields">拼接内容, 可以是内容、字段</param>
        /// <returns></returns>
        public virtual string ConnectChar(List<ConvertFlag> opflags, List<string> opfields)
        {
            return "";
        }

        #endregion

        #region 相关常规应用函数

        /// <summary>
        /// 转换大写
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string ToUpper(string soc, bool opflag)
        {
            return "";            
        }

        /// <summary>
        /// 转换小写
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string ToLower(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Trim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 去掉左空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Ltrim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 去掉右空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Rtrim(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 从左边截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Left(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 从右边截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Right(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="startIndex"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Substring(string soc, int startIndex, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 字符串替换
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Replace(string soc, string oldStr, string newStr, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// Length
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Length(string soc, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 字符串连接
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string ConcatString(string[] strs, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 左补空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Lpad(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 右边补空格
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="n"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Rpad(string soc, int n, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 两个日期差
        /// </summary>
        /// <param name="datepart">仅支持yy MM dd HH mi ss ms </param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Datediff(string datepart, string startDate, string endDate, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 日期部分增加
        /// </summary>
        /// <param name="datepart">仅支持yy MM dd HH mi ss ms</param>
        /// <param name="currDate"></param>
        /// <param name="addValue"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string DateAdd(string datepart, string currDate, string addValue, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// 解析固定内容显示值
        /// </summary>
        /// <param name="soc">原始值</param>
        /// <param name="anaV">固定内容值 例如：value1|display1,  ...</param>
        /// <param name="defaultV"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Decode(string soc, string anaV, string defaultV, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 is null 取v2 否则取v1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Nvl(string v1, string v2, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 not null 取v2, v1 is null取v3
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string Nvl2(string v1, string v2, string v3, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 = v2取null 否则取v1
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="opflag">true 数值  false 字段</param>
        /// <returns></returns>
        public virtual string NullIf(string v1, string v2, bool opflag)
        {
            return "";
        }

        /// <summary>
        /// v1 is null 取0
        /// </summary>
        /// <param name="v1">v1一般是字段，也可以取字段的聚合函数，如：sum(字段)</param>
        /// <returns></returns>
        public virtual string IsNull(string v1)
        {
            return "";
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public virtual string GetDBDateTime()
        {
            return "";
        }

        #endregion
    }


}

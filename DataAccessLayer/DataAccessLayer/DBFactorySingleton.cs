using System;
using Common;
using FrameCommon;


namespace DataAccessLayer.DataBaseFactory
{
    /// <summary>
    /// 数据工厂管理器。
    /// </summary>
    public class DBFactorySingleton
    {
        static DBFactorySingleton dbFactorySingleton = null;

        static string connStr = string.Empty; //保存当前数据连接串

        AbstractDBFactory factory;
        /// <summary>
        /// 获得唯一实例。
        /// </summary>
        /// <returns></returns>
        public static DBFactorySingleton GetInstance(DistributeDataNode ddn)
        {
            //这里需要支持不同数据库的单例，原则是数据连接变更需要重新实例单例
            if (dbFactorySingleton == null)
            {
                dbFactorySingleton = new DBFactorySingleton(ddn);
                connStr = ddn.Connectionstring;
            }
            else
            {
                if (connStr != ddn.Connectionstring)
                {
                    dbFactorySingleton = new DBFactorySingleton(ddn);
                    connStr = ddn.Connectionstring;
                }
            }

            return dbFactorySingleton;
        }      
       
        /// <summary>
        /// 数据库访问 对象 。
        /// </summary>
        public AbstractDBFactory Factory
        {
            get
            {
                return factory;
            }
        }   

        /// <summary>
        /// 构造函数。
        /// </summary>
        private DBFactorySingleton(DistributeDataNode ddn)
        {
            //string dbFactoryName = APPConfig.GetAPPConfig().GetConfigValue("DBFactoryName", "");
            string dbFactoryName = ddn.DbFactoryName;
            if (dbFactoryName == "")
            {                
                throw new Exception("no find DBFactoryName in config file!");
                //return;
            }
            factory = (AbstractDBFactory) ClassBuilder.CreateObject(dbFactoryName);
            if(factory==null)
                throw new Exception("DBFactoryName find error in config file!");

            //string connectString = APPConfig.GetAPPConfig().GetConfigValue("ConnectionString", "");
            string connectString = ddn.Connectionstring;
            if (connectString == "")
                throw new Exception("find error for init database connection!");

            //string dbschema = APPConfig.GetAPPConfig().GetConfigValue("Dbschema", "");
            string dbschema = ddn.DbSchema;
            if (dbschema == "")
                throw new Exception("find error for init database schema!");

            try
            {                
                factory.ConnectionString = connectString;
                factory.DbSchema = dbschema;
            }
            catch (Exception e)
            {
                throw new Exception("database connection error!\n" + e.ToString());
            }            
        }

        /// <summary>
        /// 构建数据库连接器。
        /// 该方式不支持多数据节点应用
        /// </summary>
        /// <param name="dbFactoryName"></param>
        /// <param name="connectionstring"></param>
        /// <param name="dbSchema"></param>
        /// <returns></returns>
        public AbstractDBFactory BuildFactory(string dbFactoryName, string connectionstring, string dbSchema)
        {
            factory = ClassBuilder.CreateObject(dbFactoryName) as AbstractDBFactory;            
            factory.ConnectionString = connectionstring;
            factory.DbSchema = dbSchema;

            return factory;
        }
    }
}

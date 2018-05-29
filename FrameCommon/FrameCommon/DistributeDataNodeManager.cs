using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System;

using Entitys.ComonEnti;
using Common;

namespace FrameCommon
{
    /// <summary>
    /// 分布式数据节点管理识别枚举
    /// 该枚举由业务方法决定采用哪种分布式处理
    /// 目前仅支持三种，查询Query、单点操作SingleAction(包括单表的多条事务操作)、事务操作TransAction
    /// 查询：业务方法里所有操作都是查询
    /// 单点操作：业务方法里至少有一个是增或删或改操作，可以包括查询查询操作,即单表操作
    /// 事务操作：业务方法里至少有两个是增或删或改操作且夸多表并需要执行事务
    /// 要求业务方法必须传入该枚举方法，具体选择哪个数据节点启动数据库底层执行由框架来处理
    /// </summary>
    public enum DistributeActionIden
    {
        /// <summary>
        /// 仅仅是查询：业务方法里所有操作都是查询
        /// </summary>
        Query,
        /// <summary>
        /// 单点操作(包括单表的多条事务操作)
        /// 业务方法里至少有一个是增或删或改操作，可以包括查询查询操作,即单表操作
        /// </summary>
        SingleAction,
        /// <summary>
        /// 事务操作(多表事务操作)
        /// 业务方法里至少有两个个是增或删或改操作且夸多表并需要执行事务
        /// </summary>
        TransAction
    }


    /// <summary>
    /// 分布式数据节点管理中心
    /// </summary>
    public class DistributeDataNodeManager
    {
        public static DistributeDataNode GetDistributeDataNode()
        {
            DistributeDataNode ddn = new DistributeDataNode();
            ddn.DbFactoryName = APPConfig.GetAPPConfig().GetConfigValue("DBFactoryName", "");
            ddn.Connectionstring = APPConfig.GetAPPConfig().GetConfigValue("ConnectionString", "");
            ddn.DbSchema = APPConfig.GetAPPConfig().GetConfigValue("Dbschema", "");
            return ddn;
        }
    }

    /// <summary>
    /// 分布式动作管理参数
    /// </summary>
    [DataContract]
    public class DistributeDataNodeManagerParams
    {
        /// <summary>
        /// 分布式动作识别
        /// </summary>
        [DataMember(Name = "DistributeActionIden", Order = 0)]
        public DistributeActionIden DistributeActionIden { get; set; }

        /// <summary>
        /// 分布式数据节点集合
        /// </summary>
        [DataMember(Name = "DistributeDataNodes", Order = 1)]
        public List<SSY_DATANODE_ADDR> DistributeDataNodes { get; set; }

        /// <summary>
        /// 分布式数据节点参数
        /// </summary>
        [DataMember(Name = "DistributeDataNode", Order = 2)]
        public DistributeDataNode DistributeDataNode { get; set; }

        /// <summary>
        /// 分布式数据节点操作sql参数
        /// </summary>
        [DataMember(Name = "DistriActionSqlParams", Order = 3)]
        public List<DistActionSql> DistriActionSqlParams { get; set; }
    }

    /// <summary>
    /// 分布式操作SQL，包含sql文本和参数
    /// </summary>
    [DataContract]
    public class DistActionSql
    {
        /// <summary>
        /// 操作sql文本
        /// </summary>
        [DataMember(Name = "ActionSqlText", Order = 0)]
        public string ActionSqlText { get; set; }

        /// <summary>
        /// 操作SQL参数
        /// </summary>
        [DataMember(Name = "ActionSqlTextParams", Order = 1)]
        public List<IDataParameter> ActionSqlTextParams { get; set; }
    }

    /// <summary>
    /// 分布式数据节点连接参数
    /// </summary>
    [DataContract]
    public class DistributeDataNode
    {
        /// <summary>
        /// 数据库驱动工厂
        /// </summary>
        [DataMember(Name = "DbFactoryName", Order = 0)]
        public string DbFactoryName { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        [DataMember(Name = "Connectionstring", Order = 1)]
        public string Connectionstring { get; set; }

        /// <summary>
        /// 数据库用户
        /// </summary>
        [DataMember(Name = "DbSchema", Order = 2)]
        public string DbSchema { get; set; }
    }

    /// <summary>
    /// 执行分页参数,为了兼容多个数据库,需要使用ANSI92标准书写sql
    /// 函数需要使用数据操作封装的方法，已经考虑兼容多个数据库
    /// </summary>
    [DataContract]
    public class SSY_PagingExecuteParam
    {
        /// <summary>
        /// 表名或视图名，需要带结构所有者
        /// </summary>
        [DataMember]
        public string TableNameOrView
        {
            get;
            set;
        }

        /// <summary>
        /// 连接
        /// </summary>
        [DataMember]
        public string Joins
        {
            get;
            set;
        }

        /// <summary>
        /// 字段名(全部字段为*)
        /// </summary>
        [DataMember]
        public string Fields
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段(必须!支持多字段)
        /// </summary>
        [DataMember]
        public string OrderField
        {
            get;
            set;
        }

        /// <summary>
        /// 条件语句(不用加where)
        /// </summary>
        [DataMember]
        public string SqlWhere
        {
            get;
            set;
        }

        /// <summary>
        /// 分页页码参数及总记录
        /// </summary>
        [DataMember]
        public SSY_PagingParam PagingParam
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    [DataContract]
    public class SSY_PagingParam
    {
        [DataMember]
        public int TotalSize
        {
            get;
            set;
        }

        [DataMember]
        public int PageIndex
        {
            get;
            set;
        }

        [DataMember]
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 排序类型
        /// </summary>
        [DataMember]
        public AscOrDesc SortType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 数据记录排序
    /// </summary>
    public enum AscOrDesc
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc = 1,

        /// <summary>
        /// 降序
        /// </summary>
        Desc = 2
    }

    /// <summary>
    /// 通用页面处理参数
    /// </summary>
    [DataContract]
    public class LoadCommonPageParam
    {
        [DataMember]
        public string ComLoadPageUrl
        {
            get;
            set;
        }

        [DataMember]
        public string ComLoadPageName
        {
            get;
            set;
        }

        [DataMember]
        public string Id
        {
            get;
            set;
        }

        [DataMember]
        public string CurrselectContent
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 通用查询条件容器
    /// </summary>
    [DataContract]
    public class QueryItemCollection
    {
        /// <summary>
        /// 条件对象json格式化字符串值
        /// </summary>
        [DataMember]
        public string queryItemValue { set; get; }

        /// <summary>
        /// 条件识别key
        /// </summary>
        [DataMember]
        public string queryItemKey { set; get; }

        //TODO 备用解析条件

        //js: var okobj = [{ objKey: "SSY_PagingParam", objValue: JSON.stringify(SSY_PagingParam) }, { objKey: "Hospital", objValue: JSON.stringify(currqueryobj) }];
        //QueryItemCollection[] objs = json.Deserialize<QueryItemCollection[]>(req);
        //Memo2.Hospital.Hospital objQ = null;
        //        for (int i = 0; i<objs.Length; i++)
        //        {
        //            if (objs[i].queryItemKey.ToUpper() == "SSY_PagingParam".ToUpper())
        //            {
        //                pager = json.Deserialize<SSY_PagingParam>(objs[i].queryItemValue);
        //            }
        //            if (objs[i].queryItemKey.ToUpper() == "Hospital".ToUpper())
        //            {
        //                objQ = json.Deserialize<Memo2.Hospital.Hospital>(objs[i].queryItemValue);
        //            }
        //        }
    }
}

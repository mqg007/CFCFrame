using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.MetadataServices;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Web.Script.Serialization;

using Xxx.Biz;
using Xxx.BizExectute;
using Common;
using Entitys.ComonEnti;
using FrameCommon;
using BizExectute;
using Xxx.Entities;

namespace Xxx.BizFactory
{
    public class BizFactorySomebiz
    {
        public Somebiz Create(string bizType, SysEnvironmentSerialize _envirObj)
        {
            if (bizType == "one")
            {
                //return new BizExectuteCommon();
                return AOPFactory.Create<BizExectuteSomebiz>(new BizExectuteSomebiz(_envirObj));
            }
            return null;
        }
    }

    /// <summary>
    /// 泛型工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BizFactorySomebiz<T>
    {
        public T Create(string bizType, SysEnvironmentSerialize _envirObj)
        {
            return (T)this.GetBiz(bizType, _envirObj);
        }

        public object GetBiz(string bizType, SysEnvironmentSerialize _envirObj)
        {
            if (bizType == "one")
            {
                return AOPFactory.Create<BizExectuteSomebiz>(new BizExectuteSomebiz(_envirObj));
            }
            return null;
        }
    }


    #region AOP相关处理

    public class DueWithAOP<T> : RealProxy
    {
        public string logpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("logpathForDebug", "");  //调试日志路径 
        public string isLogpathForDebug = APPConfig.GetAPPConfig().GetConfigValue("isLogpathForDebug", "");  //是否记录调试日志

        public T Target
        {
            get; internal set;
        }

        public DueWithAOP(T target) : base(typeof(T))
        {
            this.Target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            #region 日志准备

            List<string> opObjPerporty = UtilitysForT<SSY_LOGENTITY>.GetAllColumns(new SSY_LOGENTITY()); //要操作的属性名
            List<string> opWherePerporty = new List<string>(); //where条件属性名 
            opWherePerporty.Add("LOGID");
            List<string> mainProperty = new List<string>(); //主键属性名 
            mainProperty.Add("LOGID");

            //string errStr = string.Empty;
            List<string> errStr = new List<string>();
            List<SSY_LOGENTITY> opList = new List<SSY_LOGENTITY>();
            SSY_LOGENTITY logenti = null;

            BizExectuteCommon recordLog = new BizExectuteCommon(ManagerSysEnvironment.GetSysEnvironmentSerialize()); //其他工厂记录日志也利用该公共方法

            //日志固定部分
            string USERNAMES = string.Empty;
            if (FrameCommon.SysEnvironment.SysUserDict != null)
            {
                if (FrameCommon.SysEnvironment.SysUserDict.USERNAME != null)
                {
                    USERNAMES = FrameCommon.SysEnvironment.SysUserDict.USERNAME.ToString();
                }
            }
            string IPS = string.Empty;
            if (!string.IsNullOrEmpty(FrameCommon.SysEnvironment.Ips))
            {
                IPS = FrameCommon.SysEnvironment.Ips;
            }
            string SYSTEMNAME = string.Empty;
            if (!string.IsNullOrEmpty(FrameCommon.SysEnvironment.distManagerParam.DistributeDataNodes[0].Systemname))
            {
                SYSTEMNAME = FrameCommon.SysEnvironment.distManagerParam.DistributeDataNodes[0].Systemname;
            }

            #endregion

            IMethodCallMessage mcall = (IMethodCallMessage)msg; //劫持方法，准备执行
            var resResult = new ReturnMessage(new Exception(), mcall);

            #region 获取必要参数

            //distributeActionIden  分布式动作识别, 必须存在
            //distributeDataNodes 分布式数据节点集合， 必须存在
            //distributeDataNode 分布式数据节点参数， 必须存在   
            //distriActionSql 分布式操作sql集合，必须存在，包括sql正文和参数
            //ddnmParams

            //singleActionList  单点操作失败集合, out参数 非必须存在，传入空的参数即可  

            //TODO 检查必须的参数，若不存在不进行执行业务方法，返回执行异常

            //获取分布式管理参数
            DistributeDataNodeManagerParams distManagerParam = new DistributeDataNodeManagerParams();
            for (int i = 0; i < mcall.InArgs.Length; i++)
            {
                if (mcall.GetInArgName(i).ToUpper() == "ddnmParams".ToUpper())
                {
                    distManagerParam = ((DistributeDataNodeManagerParams)mcall.GetInArg(i));
                    //SYSTEMNAME = distManagerParam.DistributeDataNodes[0].Systemname;
                    break;
                }
            }

            //获取分布式动作识别参数
            DistributeActionIden distBAC = distManagerParam.DistributeActionIden;

            //加载数据节点集合，然后根据节点数量及分布式动作识别初始化分布式数据节点及分布式事务处理
            //数据节点集合由服务方法传入            
            //获取数据节点集合参数
            List<SSY_DATANODE_ADDR> dataNodes = distManagerParam.DistributeDataNodes;

            //获取数据节点参数
            DistributeDataNode ddn = distManagerParam.DistributeDataNode;

            //单点操作失败集合，最后要报告给节点中心，out参数
            bool permitSingleDataOperation = false; //是否支持单点操作失败后进行报告
            List<SSY_DATA_ACTION_TASK> data_action_task = new List<SSY_DATA_ACTION_TASK>();
            for (int i = 0; i < mcall.InArgs.Length; i++)
            {
                if (mcall.GetInArgName(i).ToUpper() == "singleActionList".ToUpper())
                {
                    permitSingleDataOperation = true;
                    data_action_task = mcall.GetInArg(i) as List<SSY_DATA_ACTION_TASK>;
                    break;
                }
            }

            #endregion

            if (distBAC == DistributeActionIden.Query)
            {
                //处理数据节点
                //distManagerParam.DistributeDataNode
                distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[0].Data_conn, dataNodes[0].Url_addr,
                    dataNodes[0].Data_user, dataNodes[0].Data_password);
                distManagerParam.DistributeDataNode.DbSchema = dataNodes[0].Data_schema;

                //只执行一次即可
                #region 执行业务方法

                try
                {
                    object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                    #region 记录业务日志

                    //执行方法后记录正常业务日志，内容来自方法ListBizLog参数
                    //若要记录日志，要求该方法必须传入该参数，且名字必须为ListBizLog，内容为要记录的业务日志内容
                    SSY_LOGENTITY tempLog = null;
                    for (int i = 0; i < mcall.InArgs.Length; i++)
                    {
                        if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                        {
                            List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                            for (int j = 0; j < dictBizLog.Count; j++)
                            {
                                //遍历记录业务日志
                                tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                //获取日志控制,确定是否记录该类日志
                                if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                {
                                    tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                    //业务直接使用业务端提交的数据，不在读取框架环境变量，因为登录时这部分数据滞后，导致不能记入日志
                                    //tempLog.USERNAMES = USERNAMES;
                                    //tempLog.IPS = IPS;
                                    //tempLog.SYSTEMNAME = SYSTEMNAME;

                                    opList.Add(tempLog);
                                }
                            }
                            if (opList.Count > 0)
                            {
                                //记录日志
                                bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                            }
                            break;
                        }
                    }

                    #endregion

                    resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);
                }
                catch (Exception ex)
                {
                    #region 记录异常日志

                    if (ex.InnerException != null)
                    {
                        //获取日志控制,确定是否记录该类日志
                        if (recordLog.CheckIsRecord("ExceptionErr", "ExceptionErr", distManagerParam))
                        {
                            //处理异常类相关信息
                            string CLASSNAME = mcall.TypeName;
                            string METHORDNAME = mcall.MethodName;

                            //异常时这部分可没有内容
                            string TABLENAME = "";
                            string RECORDIDENCOLS = "";
                            string RECORDIDENCOLSVALUES = "";
                            string FUNCTIONNAME = "";

                            logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.ExceptionErr, LogLevelOption.ExecptionErr, recordLog.GetSystemDateTime(distManagerParam),
                                CLASSNAME, METHORDNAME, LogAction.ExecptionErr, TABLENAME, RECORDIDENCOLS, RECORDIDENCOLSVALUES, USERNAMES, IPS, FUNCTIONNAME,
                                ex.InnerException.Message, SYSTEMNAME, "");
                            logenti.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                            opList.Add(logenti);

                            if (opList.Count > 0)
                            {
                                //记录日志
                                bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                            }
                        }

                        resResult = new ReturnMessage(ex.InnerException, mcall);
                    }

                    #endregion

                    resResult = new ReturnMessage(ex, mcall);
                }

                #endregion
            }
            else if (distBAC == DistributeActionIden.SingleAction)
            {
                //数据节点有几个执行几次，单次提交，发现执行异常，将异常报告给节点中心，继续执行，直到完毕
                for (int m = 0; m < dataNodes.Count; m++)
                {
                    //ddn.DbFactoryName  数据库工厂取配置文件，目前不考虑同构不同种类的数据库
                    distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                        dataNodes[m].Data_password);
                    distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                    #region 执行业务方法

                    try
                    {
                        object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                        #region 记录业务日志

                        //执行方法后记录正常业务日志，内容来自方法ListBizLog参数
                        //若要记录日志，要求该方法必须传入该参数，且名字必须为ListBizLog，内容为要记录的业务日志内容
                        SSY_LOGENTITY tempLog = null;
                        for (int i = 0; i < mcall.InArgs.Length; i++)
                        {
                            if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                            {
                                List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                                for (int j = 0; j < dictBizLog.Count; j++)
                                {
                                    //遍历记录业务日志
                                    tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                    //获取日志控制,确定是否记录该类日志
                                    if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                    {
                                        tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                        //业务直接使用业务端提交的数据，不在读取框架环境变量，因为登录时这部分数据滞后，导致不能记入日志
                                        //tempLog.USERNAMES = USERNAMES;
                                        //tempLog.IPS = IPS;
                                        //tempLog.SYSTEMNAME = SYSTEMNAME;                                      

                                        opList.Add(tempLog);
                                    }
                                }
                                if (opList.Count > 0)
                                {
                                    //记录日志
                                    bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                                }
                                break;
                            }
                        }

                        #endregion

                        resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);
                    }
                    catch (Exception ex)
                    {
                        SSY_DATA_ACTION_TASK tempDataTask = null;

                        #region 记录异常日志

                        if (ex.InnerException != null)
                        {
                            //获取日志控制,确定是否记录该类日志
                            if (recordLog.CheckIsRecord("ExceptionErr", "ExceptionErr", distManagerParam))
                            {
                                //处理异常类相关信息
                                string CLASSNAME = mcall.TypeName;
                                string METHORDNAME = mcall.MethodName;

                                //异常时这部分可没有内容
                                string TABLENAME = "";
                                string RECORDIDENCOLS = "";
                                string RECORDIDENCOLSVALUES = "";
                                string FUNCTIONNAME = "";

                                logenti = LogCommon.CreateLogDataEnt(LogTypeDomain.ExceptionErr, LogLevelOption.ExecptionErr, recordLog.GetSystemDateTime(distManagerParam),
                                    CLASSNAME, METHORDNAME, LogAction.ExecptionErr, TABLENAME, RECORDIDENCOLS, RECORDIDENCOLSVALUES, USERNAMES, IPS, FUNCTIONNAME,
                                    ex.InnerException.Message, SYSTEMNAME, "");
                                logenti.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                opList.Add(logenti);

                                if (opList.Count > 0)
                                {
                                    //记录日志
                                    bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);
                                }
                            }
                            if (permitSingleDataOperation)
                            {
                                #region 获取失败操作sql

                                //获取失败记录，以便将任务报告给节点中心
                                //获取操作sql  List<DistActionSql> DistriActionSqlParams

                                for (int task = 0; task < distManagerParam.DistriActionSqlParams.Count; task++)
                                {
                                    tempDataTask = new SSY_DATA_ACTION_TASK();
                                    tempDataTask.Action_sql = distManagerParam.DistriActionSqlParams[task].ActionSqlText;
                                    string tempSqlParamSeq = string.Empty;
                                    bool temddddd = JsonSerializer.Serialize(distManagerParam.DistriActionSqlParams[task].ActionSqlTextParams, out tempSqlParamSeq);
                                    //保存sql参数序列化结果
                                    tempDataTask.Action_sql_params = tempSqlParamSeq;
                                    tempDataTask.Data_real_conn = ddn.Connectionstring;
                                    data_action_task.Add(tempDataTask);
                                }
                                //执行完毕后，清除本次的sql记录
                                distManagerParam.DistriActionSqlParams.Clear();

                                //TODO 报告单点异常给节点中心，暂时不支持，后续扩展
                                #endregion
                            }
                        }

                        #endregion

                        continue; //继续执行
                    }

                    #endregion
                }
            }
            else if (distBAC == DistributeActionIden.TransAction)
            {
                //分布式事务执行， 按数据节点数量执行，同步提交
                using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, TimeSpan.FromHours(1)))
                {
                    for (int m = 0; m < dataNodes.Count; m++)
                    {
                        //ddn.DbFactoryName  数据库工厂取配置文件，目前不考虑同构不同种类的数据库
                        distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                        dataNodes[m].Data_password);
                        distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                        #region 执行业务方法

                        object objRv = mcall.MethodBase.Invoke(this.Target, mcall.Args);

                        #region 记录业务日志

                        //执行方法后记录正常业务日志，内容来自方法ListBizLog参数
                        //若要记录日志，要求该方法必须传入该参数，且名字必须为ListBizLog，内容为要记录的业务日志内容
                        SSY_LOGENTITY tempLog = null;
                        for (int i = 0; i < mcall.InArgs.Length; i++)
                        {
                            if (mcall.GetInArgName(i).ToUpper() == "ListBizLog".ToUpper())
                            {
                                List<SSY_LOGENTITY> dictBizLog = mcall.GetInArg(i) as List<SSY_LOGENTITY>;

                                for (int j = 0; j < dictBizLog.Count; j++)
                                {
                                    //遍历记录业务日志
                                    tempLog = dictBizLog[j] as SSY_LOGENTITY;

                                    //获取日志控制,确定是否记录该类日志
                                    if (recordLog.CheckIsRecord(tempLog.DOMAINNAME.ToString(), tempLog.OPTIONNAME.ToString(), distManagerParam))
                                    {
                                        tempLog.LOGID = recordLog.GetID(MakeIDType.YMDHMS_3, string.Empty, null, distManagerParam);

                                        //业务直接使用业务端提交的数据，不在读取框架环境变量，因为登录时这部分数据滞后，导致不能记入日志
                                        //tempLog.USERNAMES = USERNAMES;
                                        //tempLog.IPS = IPS;
                                        //tempLog.SYSTEMNAME = SYSTEMNAME;                                           

                                        opList.Add(tempLog);
                                    }
                                }
                                //这里事物不能同时记录日志，需要放到业务方法提交完成后单独记录日志
                                if (opList.Count > 0)
                                {
                                    //记录日志
                                    //bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, ref errStr, distManagerParam);

                                }
                                break;
                            }
                        }

                        #endregion

                        resResult = new ReturnMessage(objRv, mcall.Args, mcall.Args.Length, mcall.LogicalCallContext, mcall);

                        #endregion
                    }

                    ts.Complete();
                    ts.Dispose();
                }

                //恢复事物默认标识
                distManagerParam.DistributeActionIden = DistributeActionIden.Query;
                //启用事物日志需要独立记录，不能和业务操作混在一个事物里
                for (int m = 0; m < dataNodes.Count; m++)
                {
                    //ddn.DbFactoryName  数据库工厂取配置文件，目前不考虑同构不同种类的数据库
                    distManagerParam.DistributeDataNode.Connectionstring = string.Format(dataNodes[m].Data_conn, dataNodes[m].Url_addr, dataNodes[m].Data_user,
                    dataNodes[m].Data_password);
                    distManagerParam.DistributeDataNode.DbSchema = dataNodes[m].Data_schema;

                    if (opList.Count > 0)
                    {
                        //记录日志
                        bool flag = recordLog.OpBizObjectSingle<SSY_LOGENTITY>(opList, opObjPerporty, opWherePerporty, mainProperty, errStr, distManagerParam);

                    }
                }

            }



            //最终返回结果,循环处理数据节点时，只返回最后一个执行成功的数据节点的执行情况
            return resResult;
        }
    }

    public static class AOPFactory
    {
        public static T Create<T>(T target)
        {
            DueWithAOP<T> dwAOP = new DueWithAOP<T>(target);
            return (T)(dwAOP.GetTransparentProxy());
        }
    }

    #endregion
}

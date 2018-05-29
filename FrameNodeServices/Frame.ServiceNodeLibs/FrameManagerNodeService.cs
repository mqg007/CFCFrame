using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;

using Frame.ServiceNodeInterfaces;
using WebServiceManager;
using Entitys.ComonEnti;
using Common;
using FrameCommon;

namespace Frame.ServiceNodeLibs
{
    public class FrameManagerNodeService : BaseService, IFrameManagerNode
    {
        public FrameManagerNodeService()
        {
            //这里具体服务语言包 
            DataTable servlangtmp = (DataTable)currCache.Get("i18nFrameNodei18nLang");
            if (servlangtmp != null)
            {
                if (currlang == envirObj.I18nCurrLang)
                {
                    i18nModuleCurrLang = servlangtmp;
                }
                else
                {
                    string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), envirObj.I18nCurrLang);
                    i18nModuleCurrLang = this.GetI18nLang(FrameNodei18nLang);
                }
            }
            else
            {
                string FrameNodei18nLang = string.Format(APPConfig.GetAPPConfig().GetConfigValue("FrameNodei18nLang", ""), envirObj.I18nCurrLang);
                i18nModuleCurrLang = this.GetI18nLang(FrameNodei18nLang);
            }
        }

        #region 升级接口实现

        /// <summary>
        /// 向中心节点报告同源单点数据操作失败任务
        /// 节点中心后续会继续进行补充完成数据操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns>0 成功  其他失败</returns>
        public string ReportSameDataActionTaskN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                List<SSY_DATA_ACTION_TASK> model = this.json.Deserialize<List<SSY_DATA_ACTION_TASK>>(reqdata.reqdata);

                string nodeDataAction = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_DATA_ACTION_TASK.xml";
                string attiName = "id|data_real_conn|action_sql|action_sql_params|action_status|execute_cnt|max_execute_cnt|remarks|timestampss";
                List<string> attiValue = new List<string>();
                string attiValueOne = @"{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}";

                for (int i = 0; i < model.Count; i++)
                {
                    string tempattiValueOne = string.Format(attiValueOne, model[i].ID, model[i].Data_real_conn, model[i].Action_sql, model[i].Action_status,
                        model[i].Execute_cnt, model[i].Max_execute_cnt, model[i].Remarks, model[i].Timestampss);
                    attiValue.Add(tempattiValueOne);
                }

                bool tempFlag = Utility.AddNodeForXml("root", "item", nodeDataAction, attiName, attiValue, "|");

                if (tempFlag)
                {
                    resdata = this.MakeResponseData("1", this.GetI18nLangItem("ReportSameDataActionTaskSuccess", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("ReportSameDataActionTaskFailed", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("ReportSameDataActionTaskErr", this.i18nModuleCurrLang) + ex.Message,
                    string.Empty, string.Empty);
            }    

            return json.Serialize(resdata);    
        }

        /// <summary>
        /// 获取节点中心同源单点数据操作任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetSameDataActionTaskStatusN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_DATA_ACTION_TASK model = this.json.Deserialize<SSY_DATA_ACTION_TASK>(reqdata.reqdata);

                string cols = "id|data_real_conn|action_sql|action_sql_params|action_status|execute_cnt|max_execute_cnt|remarks|timestampss";
                string colTypes = "String|String|String|String|String|String|String|String|String";
                string nodeDataAction = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_DATA_ACTION_TASK.xml";
                System.Data.DataTable dtdataActionTask = Common.Utility.GetTableFromXml(cols, colTypes, nodeDataAction);

                List<SSY_DATA_ACTION_TASK> dataActionTask = new List<SSY_DATA_ACTION_TASK>();
                if (Utility.DtHasData(dtdataActionTask))
                {
                    dataActionTask = UtilitysForT<SSY_DATA_ACTION_TASK>.GetListsObj(dtdataActionTask);
                }

                if (dataActionTask.Count > 0)
                {
                    resdata = this.MakeResponseData("0", string.Empty, json.Serialize(dataActionTask), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindSameDataActionTaskStatu", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("getSameDataActionTaskStatuErr", this.i18nModuleCurrLang) + ex.Message, 
                    string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 向节点中心报告业务节点、数据节点异常情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns>0 成功  其他失败</returns>
        public string RecordNodeErrorLogN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                List<SSY_NODE_ERRORS> model = this.json.Deserialize<List<SSY_NODE_ERRORS>>(reqdata.reqdata);

                string nodeErrorLog = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_NODE_ERRORS.xml";
                string attiName = "id|url_addr|node_typs|error_desc|remarks|timestampss";
                List<string> attiValue = new List<string>();
                string attiValueOne = @"{0}|{1}|{2}|{3}|{4}|{5}";

                for (int i = 0; i < model.Count; i++)
                {
                    string tempattiValueOne = string.Format(attiValueOne, model[i].ID, model[i].Url_addr, model[i].Node_typs, model[i].Error_desc,
                        model[i].Remarks, model[i].Timestampss);
                    attiValue.Add(tempattiValueOne);
                }

                bool tempFlag = Utility.AddNodeForXml("root", "item", nodeErrorLog, attiName, attiValue, "|");

                if (tempFlag)
                {
                    #region 同时修正节点中心、业务节点、业务数据库运行状态           
                    for (int i = 0; i < model.Count; i++)
                    {
                        ServiceType servType = ServiceType.BizDueWith;
                        string node_addr = model[i].Url_addr;
                        string errStr = model[i].Error_desc;
                        if (model[i].Node_typs == "DataBaseErr")
                        {
                            servType = ServiceType.DataBaseErr;
                        }
                        else if (model[i].Node_typs == "BizDueWith")
                        {
                            servType = ServiceType.BizDueWith;
                        }
                        else if (model[i].Node_typs == "NodeCenter")
                        {
                            servType = ServiceType.NodeCenter;
                        }
                        this.UpdateNodeRunStatus(servType, node_addr, errStr);
                    }
                    #endregion

                    resdata = this.MakeResponseData("1", this.GetI18nLangItem("RecordNodeErrorLogSuccess", this.i18nModuleCurrLang), "0", string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("RecordNodeErrorLogFailed", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("RecordNodeErrorLogErr", this.i18nModuleCurrLang) + ex.Message,
                    string.Empty, string.Empty);
            }            

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 查询节点异常情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetNodeErrorLogsN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                SSY_NODE_ERRORS model = this.json.Deserialize<SSY_NODE_ERRORS>(reqdata.reqdata);

                string cols = "id|url_addr|node_typs|error_desc|remarks|timestampss";
                string colTypes = "String|String|String|String|String|String";
                string useNodeErrorLog = APPConfig.GetAPPConfig().GetConfigValue("xmldataPath", "") + "\\SSY_NODE_ERRORS.xml";
                System.Data.DataTable dtXmlNodeErrors = Common.Utility.GetTableFromXml(cols, colTypes, useNodeErrorLog);

                List<SSY_NODE_ERRORS> nodeErrors = new List<SSY_NODE_ERRORS>();
                if (Utility.DtHasData(dtXmlNodeErrors))
                {
                    nodeErrors = UtilitysForT<SSY_NODE_ERRORS>.GetListsObj(dtXmlNodeErrors);
                }

                if (nodeErrors.Count > 0)
                {
                    resdata = this.MakeResponseData("0", string.Empty, json.Serialize(nodeErrors), string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindNodeErrorLog", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("getNodeErrorLogErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        /// <summary>
        /// 加载节点运行状态，包括全部节点（可用或不可用）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetNodeRunStatuN(string req)
        {
            try
            {
                reqdata = this.AnaRequestData(req);
                string usestate = reqdata.reqdata;

                UseNodeCollection usenodes = new UseNodeCollection();
                usenodes = this.GetUseNodeCollections("");

                if (usenodes.BizNodeList.Count > 0 && usenodes.DataNodeList.Count > 0)
                {
                    resdata = this.MakeResponseData("1", string.Empty, json.Serialize(usenodes), string.Empty);
                }
                else if (usenodes.BizNodeList.Count <= 0 || usenodes.DataNodeList.Count <= 0)
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindDataNodesOrBizNodes", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
                else
                {
                    resdata = this.MakeResponseData("0", this.GetI18nLangItem("noFindDataNodesAndBizNodes", this.i18nModuleCurrLang), string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                resdata = this.MakeResponseData("0", this.GetI18nLangItem("getDataNodeAndBizNodeErr", this.i18nModuleCurrLang) + ex.Message, string.Empty, string.Empty);
            }

            return json.Serialize(resdata);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Caching;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization;

using Common;

namespace FrameCommon
{
    public class BaseServiceUtility
    {
        /// <summary>
        /// 加载服务配置文件
        /// </summary>
        /// <param name="serviceconfigpath"></param>
        /// <returns></returns>
        public static DataTable GetServiceConfig(string serviceconfigpath)
        {
            DataTable dttmp = new DataTable();
            string cols = "id|url_addr|moudname|moudiden|servname|serviden|servcodename|use_status|remarks|version|servclass|servtype";
            string colTypes = "String|String|String|String|String|String|String|String|String|String|String|String";
            dttmp = Common.Utility.GetTableFromXml(cols, colTypes, serviceconfigpath);

            return dttmp;
        }

        /// <summary>
        /// 加载业务节点配置文件
        /// </summary>
        /// <param name="biznodeconfigpath"></param>
        /// <returns></returns>
        public static DataTable GetBizNodesConfig(string biznodeconfigpath)
        {
            DataTable dttmp = new DataTable();
            string cols = "id|url_addr|use_status|moudiden|remarks|timestampss";
            string colTypes = "String|String|String|String|String|String";
            dttmp = Common.Utility.GetTableFromXml(cols, colTypes, biznodeconfigpath);

            return dttmp;
        }

        /// <summary>
        /// 获取当前应用服务配置
        /// </summary>
        /// <param name="serviden"></param>
        /// <param name="version"></param>
        /// <param name="servclass"></param>
        /// <param name="servtype"></param>
        /// <param name="dtServConfig"></param>
        /// <returns></returns>
        public static DataRow GetServiceConfigOne(string serviden, string version, string servclass, string servtype, DataTable dtServConfig)
        {
            for (int i = 0; i < dtServConfig.Rows.Count; i++)
            {
                if (dtServConfig.Rows[i]["serviden"].ToString().ToUpper() == serviden.ToUpper() &&
                    dtServConfig.Rows[i]["version"].ToString().ToUpper() == version.ToUpper() &&
                    dtServConfig.Rows[i]["servclass"].ToString().ToUpper() == servclass.ToUpper() &&
                    dtServConfig.Rows[i]["servtype"].ToString().ToUpper() == servtype.ToUpper())
                {
                    return dtServConfig.Rows[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 生成html返回信息
        /// </summary>
        /// <param name="respflag"></param>
        /// <param name="resptoolstr"></param>
        /// <param name="respdata"></param>
        /// <param name="pagertottlecounts"></param>
        /// <returns></returns>
        public static RespData MakeResponseData(string respflag, string resptoolstr, string respdata, string pagertottlecounts)
        {
            RespData resdata = new RespData();
            resdata.respflag = respflag;
            resdata.resptoolstr = resptoolstr;
            resdata.respdata = respdata;
            resdata.pagertottlecounts = pagertottlecounts;
            return resdata;
        }

        /// <summary>
        /// 发送http请求获取返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <param name="reqType">post get</param>
        /// <param name="reqMethord"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static RespData RequestHttp(string url, string req, string reqType, string reqMethord, string token)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            string resstr = @"";
            Req reqTemp = new Req();
            ReqData resqq = new ReqData();
            resqq.reqdata = req;
            resqq.reqtype = reqType;

            reqTemp.req = json.Serialize(resqq);

            #region 原生态方式

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = reqMethord.ToUpper();
            request.Timeout = 60000;
            request.ReadWriteTimeout = request.Timeout;
            request.Headers.Add("token", token);

            byte[] sendData = Encoding.UTF8.GetBytes(json.Serialize(reqTemp));
            if (sendData != null && sendData.Length > 0)
            {
                using (var streamRequst = request.GetRequestStream())
                {
                    streamRequst.Write(sendData, 0, sendData.Length);
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            try
            {
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    resstr = reader.ReadToEnd().TrimStart("\u005C\u0022".ToCharArray()).TrimEnd("\u005C\u0022".ToCharArray()).Replace("\\", "");
                    reader.Close();
                }
            }
            catch (Exception exs)
            {
                throw exs;
            }

            #endregion

            #region webclient方式

            //using (System.Net.WebClient wc = new System.Net.WebClient())
            //{
            //    wc.Headers.Add("Content-Type", "application/json");
            //    wc.Headers.Add("token", token);
            //    if (reqMethord.ToUpper() == "POST")
            //    {
            //        Req reqTemp = new Req();
            //        ReqData resqq = new ReqData();
            //        resqq.reqdata = req;
            //        resqq.reqtype = reqType;
            //        reqTemp.req = json.Serialize(resqq);
            //        byte[] sendData = Encoding.UTF8.GetBytes(json.Serialize(reqTemp));
            //        wc.Headers.Add("ContentLength", sendData.Length.ToString());
            //        byte[] recData = wc.UploadData(url, reqMethord.ToUpper(), sendData);
            //        resstr = Encoding.UTF8.GetString(recData).TrimStart("\u005C\u0022".ToCharArray()).TrimEnd("\u005C\u0022".ToCharArray()).Replace("\\", "");;
            //    }
            //    else
            //    {
            //        resstr = wc.DownloadString(url);
            //    }                
            //}

            #endregion

            return json.Deserialize<RespData>(resstr);
        }


        /// <summary>
        /// 获取语言包
        /// </summary>
        /// <param name="langpath"></param>
        /// <returns></returns>
        public static DataTable GetI18nLang(string langpath)
        {
            DataTable dttmp = new DataTable();
            string cols = "id|texts";
            string colTypes = "String|String";
            dttmp = Common.Utility.GetTableFromXml(cols, colTypes, langpath);

            return dttmp;
        }

        /// <summary>
        /// 获取具体语言项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtlangs"></param>
        /// <returns></returns>
        public static string GetI18nLangItem(string id, DataTable dtlangs)
        {
            for (int i = 0; i < dtlangs.Rows.Count; i++)
            {
                if (dtlangs.Rows[i]["id"].ToString() == id)
                {
                    return dtlangs.Rows[i]["texts"].ToString();
                }
            }
            return "";
        }
    }

    #region 公共实体类

    /// <summary>
    /// 接口用户
    /// </summary>
    [DataContract]
    public class MemoInterfaceUser
    {
        [DataMember]
        public string uid { set; get; }

        [DataMember]
        public string upwd { set; get; }

        [DataMember]
        public string token { set; get; }
    }

    /// <summary>
    /// html get请求实体
    /// </summary>
    [DataContract]
    public class ReqData
    {
        /// <summary>
        /// 请求识别,预留，用以实现以后支持重载实现
        /// </summary>
        [DataMember]
        public string reqtype { set; get; }

        /// <summary>
        /// 请求动作(例如增add 删del 改mod 等等含义)
        /// </summary>
        [DataMember]
        public string reqaction { set; get; }

        /// <summary>
        /// 请求参数(放置请求参数的json串)
        /// </summary>
        [DataMember]
        public string reqdata { set; get; }
    }

    /// <summary>
    /// html post请求实体
    /// </summary>
    [DataContract]
    public class Req
    {
        /// <summary>
        /// 请求识别
        /// </summary>
        [DataMember]
        public string req { set; get; }
    }

    /// <summary>
    /// html响应实体
    /// </summary>
    [DataContract]
    public class RespData
    {
        /// <summary>
        /// 响应结果识别
        /// </summary>
        [DataMember]
        public string respflag { set; get; }

        /// <summary>
        /// 响应结果提示信息
        /// </summary>
        [DataMember]
        public string resptoolstr { set; get; }

        /// <summary>
        /// 响应结果返回数据内容
        /// </summary>
        [DataMember]
        public string respdata { set; get; }

        /// <summary>
        /// 分页时，返回的记录总数
        /// </summary>
        [DataMember]
        public string pagertottlecounts { set; get; }


    }

    #endregion

}

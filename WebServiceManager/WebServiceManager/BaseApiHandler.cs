using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;


namespace WebServiceManager
{
    public class BaseApiHandler
    {
        public static JavaScriptSerializer json = new JavaScriptSerializer();

        public BaseApiHandler()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 生成响应实体
        /// </summary>
        /// <param name="status"></param>
        /// <param name="datetime"></param>
        /// <param name="respdatas"></param>
        /// <param name="tottlerecords"></param>
        /// <param name="resperrordatas"></param>
        /// <returns></returns>
        public static response MakeResponseData(string status, string datetime, string respdatas, string tottlerecords, string resperrordatas)
        {
            response resdata = new response();
            resdata.status = status;
            resdata.datetime = datetime;
            resdata.respdatas = respdatas;
            resdata.tottlerecords = tottlerecords;
            resdata.resperrordatas = resperrordatas;

            return resdata;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 发送http请求获取返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqMethord"></param>
        /// <param name="sn"></param>
        /// <param name="output"></param>
        /// <param name="reqiden"></param>
        /// <param name="reqversion"></param>
        /// <param name="reqdataiden"></param>
        /// <param name="reqdatas"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static response RequestHttp(string url, string reqMethord, string sn, string output, string reqiden, string reqversion, string reqdataiden, string reqdatas, string token)
        {
            string resstr = @"";
            reqobj resqq = new reqobj();
            resqq.sn = sn;
            resqq.output = output;
            resqq.reqiden = reqiden;
            resqq.reqversion = reqversion;
            resqq.reqdataiden = reqdataiden;
            resqq.reqdatas = reqdatas;

            //test start

            //手动拼接json测试
            //string sstest = "{\"reqparams\": {\"output\": \"json\",\"reqiden\": \"UpdateHospitalMaterialCategory\", \"reqdataiden\": \"DIH00101010\",\"reqdatas\": {\"reqdata\": {\"value\": { \"HospitalMaterialCategory\": [{\"code\": \"0010201\", \"name\": \"球囊\", \"state\": \"1\"}, {\"code\": \"0010203\", \"name\": \"支架\", \"state\": \"1\"}] } } } } } ";
            //string sstest = "{\"output\": \"json\",\"reqiden\": \"UpdateHospitalMaterialCategory\", \"reqdataiden\": \"DIH00101010\",\"reqdatas\": {\"reqdata\": {\"value\": { \"HospitalMaterialCategory\": [{\"code\": \"0010201\", \"name\": \"球囊\", \"state\": \"1\"}, {\"code\": \"0010203\", \"name\": \"支架\", \"state\": \"1\"}] } } } } ";

            //test end

            #region 原生态方式

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = reqMethord.ToUpper();
            request.Timeout = 60000;
            request.ReadWriteTimeout = request.Timeout;
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("token", token);
            }            

            if (reqMethord.ToUpper() == "POST")
            {
                byte[] sendData = Encoding.UTF8.GetBytes("'" + json.Serialize(resqq) + "'");
                //byte[] sendData = Encoding.UTF8.GetBytes("'" + sstest + "'");
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
            }
            else if(reqMethord.ToUpper() == "GET")
            {
                request.ContentLength = 0;
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

            #region HttpClient方式

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var content = new StringContent("'" + BaseApiHandler.json.Serialize(resqq) + "'", Encoding.UTF8);
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var tmpResult = client.PostAsync(url, content).Result;
            //    tmpResult.EnsureSuccessStatusCode();
            //    resstr = tmpResult.Content.ReadAsStringAsync().Result;
            //}

            #endregion

            //resstr = resstr.Replace("[","'[").Replace("]", "]'");
            return json.Deserialize<response>(BaseApiHandler.FormatReqDatas(BaseApiHandler.FormatRespDatas(resstr)));
        }

        /// <summary>
        /// 发送http请求获取返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqMethord"></param>
        /// <param name="sn"></param>
        /// <param name="output"></param>
        /// <param name="reqiden"></param>
        /// <param name="reqversion"></param>
        /// <param name="reqdataiden"></param>
        /// <param name="reqdatas"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string RequestHttpCustom(string url, string reqMethord, string sn, string output, string reqiden, string reqversion, string reqdataiden, string reqdatas, string token)
        {
            string resstr = @"";
            reqobj resqq = new reqobj();
            resqq.sn = sn;
            resqq.output = output;
            resqq.reqiden = reqiden;
            resqq.reqversion = reqversion;
            resqq.reqdataiden = reqdataiden;
            resqq.reqdatas = reqdatas;
            
            #region 原生态方式

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = reqMethord.ToUpper();
            request.Timeout = 60000;
            request.ReadWriteTimeout = request.Timeout;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("token", token);
            }

            if (reqMethord.ToUpper() == "POST")
            {
                byte[] sendData = Encoding.UTF8.GetBytes(json.Serialize(resqq.reqdatas));
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
            }
            else if (reqMethord.ToUpper() == "GET")
            {
                request.ContentLength = 0;
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

            #region HttpClient方式

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var content = new StringContent("'" + BaseApiHandler.json.Serialize(resqq) + "'", Encoding.UTF8);
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var tmpResult = client.PostAsync(url, content).Result;
            //    tmpResult.EnsureSuccessStatusCode();
            //    resstr = tmpResult.Content.ReadAsStringAsync().Result;
            //}

            #endregion

            return resstr;
        }

        /// <summary>
        /// 格式化序列化结果
        /// </summary>
        /// <param name="oldstr"></param>
        /// <returns></returns>
        public static string FormatReqDatas(string oldstr)
        {
            return oldstr.Replace("\"'", "'").Replace("'\"", "'");
        }

        /// <summary>
        /// 格式化序列化结果
        /// </summary>
        /// <param name="oldstr"></param>
        /// <returns></returns>
        public static string FormatRespDatas(string oldstr)
        {
            return oldstr.Replace("u0027", "'"); 
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        public static void RecordLog(string logContent, string logPath)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string filepath = logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            //StreamWriter sw = File.AppendText(filepath);
            //sw.WriteLine(logContent);
            //sw.Close();

            using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.WriteLine(logContent);
                sw.Close();
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        /// <param name="fileType">文件分类名</param>
        public static void RecordLog(string logContent, string logPath, string fileType)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string filepath = logPath + "\\" + fileType + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            //StreamWriter sw = File.AppendText(filepath);
            //sw.WriteLine(logContent);
            //sw.Close();

            using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.WriteLine(logContent);
                sw.Close();
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        /// <param name="fileType">文件分类名</param>
        /// <param name="isRecordLog"></param>
        public static void RecordLog(string logContent, string logPath, string fileType, string isRecordLog)
        {
            if(isRecordLog.ToUpper() == "Y")
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                string filepath = logPath + "\\" + fileType + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                //StreamWriter sw = File.AppendText(filepath);
                //sw.WriteLine(logContent);
                //sw.Close();

                using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
                {
                    sw.WriteLine(logContent);
                    sw.Close();
                }
            }            
        }

        /// <summary>
        /// 将字符串转换为字符串数组,按位转换,|分割
        /// </summary>
        /// <param name="oldstr"></param>
        /// <param name="spliteChar"></param>
        /// <returns></returns>
        public static string FormatStringToCharacterArray(string oldstr)
        {
            string reqdataTmp = "";
            char[] values = oldstr.ToCharArray();
            foreach (char letter in values)
            {
                reqdataTmp += letter.ToString() + "|";
            }

            return reqdataTmp.TrimEnd('|');
        }


        #region 字符串和Byte之间的转化
        /// <summary>
        /// 数字和字节之间互转
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int IntToBitConverter(int num)
        {
            int temp = 0;
            byte[] bytes = BitConverter.GetBytes(num);//将int32转换为字节数组
            temp = BitConverter.ToInt32(bytes, 0);//将字节数组内容再转成int32类型
            return temp;
        }

        /// <summary>
        /// 将字符串转为16进制字符，允许中文
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16) + " ";
            }
            return result;
        }
        /// <summary>
        /// 将16进制字符串转为字符串
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }
        /// <summary>
        /// byte[]转为16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 十进制字符串转为16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string IntStrToHexStr(string bytes)
        {
            return int.Parse(bytes).ToString("X2");
        }

        /// <summary>
        /// 将16进制的字符串转为byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        /// <summary>
        /// 单个ASCII转字符串
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string Ascii2Str(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }

        /// <summary>
        /// 单个字符的字符串转ASCII
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int Str2Ascii(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }
        }

        /// <summary>
        /// 单个字符的字符串转ASCII字符串
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Str2AsciiStr(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return Convert.ToString(intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }
        }


        /// <summary>
        /// 将多个字符的字符串转换为ascii码字符串
        /// </summary>
        /// <param name="tmpstr"></param>
        /// <returns></returns>
        public static string Strs2AsciiStr(string tmpstr)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(tmpstr);  //数组array为对应的ASCII数组   

            string ASCIIstr2 = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                int asciicode = (int)(array[i]);
                ASCIIstr2 += Convert.ToString(asciicode);//字符串ASCIIstr2 为对应的ASCII字符串
            }

            return ASCIIstr2;
        }

        /// <summary>
        /// 将多个字符的ascii码字符串转换为字符串
        /// </summary>
        /// <param name="asciistr"></param>
        /// <returns></returns>
        public static string AsciiStr2Strs(string asciistr)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(asciistr);  //数组array为对应的ASCII数组   
            string resStr = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                char asciiChar = (char)(array[i]);
                resStr += Convert.ToString(asciiChar);//字符串ASCIIstr2 为对应的ASCII字符串
            }
            return resStr;
        }

        /// <summary>
        /// 将多个字符的ascii码字符串转换为byte[]
        /// </summary>
        /// <param name="asciistr"></param>
        /// <returns></returns>
        public static byte[] AsciiStr2ByteArray(string asciistr)
        {
            return System.Text.Encoding.ASCII.GetBytes(asciistr);  //数组array为对应的ASCII数组   
        }

        /// <summary>
        /// 16进制的ASCII串转字符串
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        public static string Ascii2StrsFor16(string asciiCode)
        {
            byte[] buff = new byte[asciiCode.Length / 2];
            int index = 0;
            for (int i = 0; i < asciiCode.Length; i += 2)
            {
                buff[index] = Convert.ToByte(asciiCode.Substring(i, 2), 16);
                ++index;
            }

            return Encoding.Default.GetString(buff);
        }

        /// <summary>
        /// 字符串转16进制的ASCII串
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Str2AsciisFor16(string character)
        {
            StringBuilder resStr = new StringBuilder();
            byte[] str16 = System.Text.ASCIIEncoding.Default.GetBytes(character);
            foreach (byte str in str16)
            {
                resStr.Append(str.ToString("x"));
            }

            return resStr.ToString();
        }

        #endregion

    }

    #region 公共类    

    /// <summary>
    /// 请求实体
    /// </summary>
    [DataContract]
    public class reqobj
    {
        /// <summary>
        /// 请求接口权限码
        /// </summary>
        [DataMember]
        public string sn { set; get; }

        /// <summary>
        /// 请求格式(默认json, 同时支持json、xml<暂未支持>)
        /// </summary>
        [DataMember]
        public string output { set; get; }

        /// <summary>
        /// 接口识别(描述接口识别码，为后续负载均衡进行识别接口，可为空)
        /// </summary>
        [DataMember]
        public string reqiden { set; get; }       

        /// <summary>
        /// 数据主体识别
        /// </summary>
        [DataMember]
        public string reqdataiden { set; get; }

        /// <summary>
        /// 请求业务数据内容
        /// </summary>
        [DataMember]
        public string reqdatas { set; get; }

        /// <summary>
        /// 请求接口版本号
        /// </summary>
        [DataMember]
        public string reqversion { set; get; }

        /// <summary>
        /// 当前语言
        /// </summary>
        [DataMember]
        public string reqi18n { set; get; }




    }

    /// <summary>
    /// 请求内容实体项
    /// </summary>
    [DataContract]
    public class reqdata
    {
        /// <summary>
        /// 业务内容健名
        /// </summary>
        [DataMember]
        public string key { set; get; }

        /// <summary>
        /// 业务内容健值
        /// </summary>
        [DataMember]
        public string value { set; get; }
    }

    /// <summary>
    /// 响应实体
    /// </summary>
    [DataContract]
    public class response
    {
        /// <summary>
        /// 返回结果状态码,如果成功返回0，其他参照具体业务状态码
        /// </summary>
        [DataMember]
        public string status { set; get; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public string datetime { set; get; }      

        /// <summary>
        /// 返回信息(取值具体业务类集合的json序列化串，若没有具体数据返回时，可为空)
        /// </summary>
        [DataMember]
        public string respdatas { set; get; }

        /// <summary>
        /// 查询时指定返回结果总记录数，不用时可为空, 该值为接口返回，调用方赋值空即可
        /// </summary>
        [DataMember]
        public string tottlerecords { set; get; }

        /// <summary>
        /// 返回错误信息(取值resperrordata集合或数组的json序列化串)
        /// </summary>
        [DataMember]
        public string resperrordatas { set; get; }
    }

    /// <summary>
    /// 发生错误结果
    /// </summary>
    [DataContract]
    public class resperrordata
    {
        [DataMember]
        public string code
        {
            get;
            set;
        }

        [DataMember]
        public string code_info
        {
            get;
            set;
        }
    }    

    /// <summary>
    /// 分页参数
    /// </summary>
    [DataContract]
    public class querypager
    {
        [DataMember]
        public int totalrecords
        {
            get;
            set;
        }

        [DataMember]
        public int pageindex
        {
            get;
            set;
        }

        [DataMember]
        public int pagesize
        {
            get;
            set;
        }
    }

    #region PCL操作相关

    /// <summary>
    /// PLC操作指令请求，用于接收业务操作发来的指令
    /// </summary>
    [DataContract]
    public class SendPLCParamsJson
    {
        /// <summary>
        /// 事物识别码
        /// </summary>
        [DataMember]
        public string TransIdentifier { set; get; }

        /// <summary>
        /// 功能识别
        /// </summary>
        [DataMember]
        public string FunctionIdentifier { set; get; }

        /// <summary>
        /// 起始地址
        /// </summary>
        [DataMember]
        public string StartAddress { set; get; }

        /// <summary>
        /// 访问长度
        /// </summary>
        [DataMember]
        public string AccessLength { set; get; }

        /// <summary>
        /// 请求数据
        /// </summary>
        [DataMember]
        public string RequestData { set; get; }

    }

    /// <summary>
    /// PLC响应
    /// </summary>
    [DataContract]
    public class RespPLCParams
    {
        /// <summary>
        /// 事物识别码
        /// </summary>
        [DataMember]
        public string TransIdentifier { set; get; }

        /// <summary>
        /// 功能识别
        /// </summary>
        [DataMember]
        public string FunctionIdentifier { set; get; }

        /// <summary>
        /// 起始地址
        /// </summary>
        [DataMember]
        public string StartAddress { set; get; }

        /// <summary>
        /// 访问长度
        /// </summary>
        [DataMember]
        public string AccessLength { set; get; }

        /// <summary>
        /// 数据内容
        /// </summary>
        [DataMember]
        public string ResponseDatas { set; get; }
    }

    #endregion

    #endregion
}

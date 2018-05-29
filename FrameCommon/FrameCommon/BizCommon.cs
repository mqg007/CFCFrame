using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace FrameCommon
{
    /// <summary>
    /// 自定义生成ID委托
    /// </summary>
    /// <returns></returns>
    public delegate string MakeCustemTypeID();

    /// <summary>
    /// 生成ID规则类型
    /// </summary>
    public enum MakeIDType
    {
        /// <summary>
        /// 获取系统时间
        /// </summary>
        YMDHMM,
        /// <summary>
        /// GUID产生，若选择GUID，可以跟formmat参数格式
        /// </summary>
        GUID,
        /// <summary>
        /// 获取系统时间，同时增加一位随机数
        /// </summary>
        YMDHMS_1,
        /// <summary>
        /// 获取系统时间，同时增加2位随机数
        /// </summary>
        YMDHMS_2,
        /// <summary>
        /// 获取系统时间，同时增加3位随机数
        /// </summary>
        YMDHMS_3,
        /// <summary>
        /// 获取系统时间，同时增加4位随机数
        /// </summary>
        YMDHMS_4,
        /// <summary>
        /// 获取系统时间，同时增加5位随机数
        /// </summary>
        YMDHMS_5,
        /// <summary>
        /// 获取系统时间，同时增加10位随机数
        /// </summary>
        YMDHMS_10,
        /// <summary>
        /// 自己定义的ID规则,该规则需要传入生成的委托函数event，且能够生成字符串ID
        /// </summary>
        CUSTEMTYPE
    }

    /// <summary>
    /// 服务类型
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 节点中心服务
        /// </summary>
        NodeCenter = 1,

        /// <summary>
        /// 业务服务
        /// </summary>
        BizDueWith = 2,

        /// <summary>
        /// 数据库错误
        /// </summary>
        DataBaseErr = 3
    }

    /// <summary>
    /// 字典类型
    /// </summary>
    public enum DictType
    {
        /// <summary>
        /// 公共字典
        /// </summary>
        FrameDict = 1,

        /// <summary>
        /// 业务字典
        /// </summary>
        BizDict = 2
    }

    public class BizCommon
    {
        /// <summary>
        /// 获取当前生成ID类型
        /// </summary>
        /// <param name="makeIDTypeStr"></param>
        /// <returns></returns>
        public static MakeIDType GetCurrMakeIDType(string makeIDTypeStr)
        {
            MakeIDType currType = MakeIDType.GUID; 

            switch (makeIDTypeStr)
            {
                case "YMDHMM":
                    currType = MakeIDType.YMDHMM;
                    break;
                case "YMDHMS_1":
                    currType = MakeIDType.YMDHMS_1;
                    break;
                case "YMDHMS_10":
                    currType = MakeIDType.YMDHMS_10;
                    break;
                case "YMDHMS_2":
                    currType = MakeIDType.YMDHMS_2;
                    break;
                case "YMDHMS_3":
                    currType = MakeIDType.YMDHMS_3;
                    break;
                case "YMDHMS_4":
                    currType = MakeIDType.YMDHMS_4;
                    break;
                case "YMDHMS_5":
                    currType = MakeIDType.YMDHMS_5;
                    break;
                case "CUSTEMTYPE":
                    currType = MakeIDType.CUSTEMTYPE;
                    break;
                case "GUID":
                    currType = MakeIDType.GUID;
                    break;
                default:
                    break;
            }           

            return currType;
        }

        /// <summary>
        /// 获取当前生成ID的委托
        /// </summary>
        /// <param name="makeCustemTypeIDStr"></param>
        /// <returns></returns>
        public static MakeCustemTypeID GetMakeCustemTypeID(string makeCustemTypeIDStr)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// 获取当前字典类型
        /// </summary>
        /// <param name="dictTypeStr"></param>
        /// <returns></returns>
        public static DictType GetDictType(string dictTypeStr)
        {
            DictType currType = DictType.BizDict;

            switch (dictTypeStr)
            {
                case "FrameDict":
                    currType = DictType.FrameDict;
                    break;
                case "BizDict":
                    currType = DictType.BizDict;
                    break;
                default:
                    break;
            }

            return currType;
        }

        #region 公共部分

        /// <summary>
        /// 查找字符串从字符串数组里
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oriArrStr"></param>
        /// <returns></returns>
        public static bool FindStrFromStrArry(string target, string[] oriArrStr)
        {
            for (int i = 0; i < oriArrStr.Length; i++)
            {
                if (oriArrStr[i] == target)
                {
                    return true;
                }
            }

            return false;
        }
          
        /// <summary>
        /// 转换米
        /// </summary>
        /// <param name="inPut"></param>
        /// <param name="inUnit"></param>
        /// <returns></returns>
        public static double ToMeter(double inPut, string inUnit)
        {
            switch (inUnit)
            {
                case "M":
                    return inPut;
                case "KM":
                    return inPut * 1000;
                case "NM":
                    return inPut * 1852;
                case "FT":
                    return inPut * 0.3048;
                case "FL":
                    return inPut * 30.48;
                case "SM": //十米
                    return inPut * 10;
                default:
                    throw new Exception("未知的距离单位" + inUnit);
            }
        }

        public static double ToFeet(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 3.2808399;
        }

        public static double ToFL(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.032808399;
        }

        public static double ToNM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.00054;
        }

        public static double ToKM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.001;
        }

        public static double ToSTDMeter(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit);
        }

        #endregion      

        #region 导航数据业务

        /// <summary>
        /// 获取对比配置文件集合
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="configXmlPath"></param>
        /// <returns></returns>
        public static DataSet GetCompConfig(string objData, string configXmlPath)
        {
            DataTable dtTemp = Common.Utility.GetTableFromXml("id|tablename|colname|iscomp|colnamedesc|isIdentifier",
            "String|String|String|String|String|String",
            configXmlPath);

            //过滤掉非当前表的配置
            DataTable dtOk = dtTemp.Clone();
            dtOk.Rows.Clear();

            DataTable dtAirwayDetail = dtOk.Clone();
            dtAirwayDetail.Clear();

            DataRow dr = null;

            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                if (objData == "PA" && dtTemp.Rows[j]["tablename"].ToString() == "AD_HP")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "D " && dtTemp.Rows[j]["tablename"].ToString() == "VOR")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "DBPN" && dtTemp.Rows[j]["tablename"].ToString() == "NDB")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "EAPC" && dtTemp.Rows[j]["tablename"].ToString() == "WAYPOINT")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "ER")
                {
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY")
                    {
                        dr = dtOk.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtOk.Rows.Add(dr);
                    }
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY_DETAIL")
                    {
                        dr = dtAirwayDetail.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtAirwayDetail.Rows.Add(dr);
                    }
                }

                //该分支只为航路明细操作时，提取明细变化情况，比较不使用
                if (objData == "ERDetail")
                {
                    if (dtTemp.Rows[j]["tablename"].ToString() == "AIRWAY_DETAIL")
                    {
                        dr = dtAirwayDetail.NewRow();
                        dr.ItemArray = dtTemp.Rows[j].ItemArray;
                        dtAirwayDetail.Rows.Add(dr);
                    }
                }

                if (objData == "PG" && dtTemp.Rows[j]["tablename"].ToString() == "RUNWAY")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "TAKEOFF" && dtTemp.Rows[j]["tablename"].ToString() == "TAKEOFF")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "LANDSTANDARD" && dtTemp.Rows[j]["tablename"].ToString() == "LANDSTANDARD")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }

                if (objData == "ILS_CAT" && dtTemp.Rows[j]["tablename"].ToString() == "ILS_CAT")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }
                if (objData == "ENROUTE" && dtTemp.Rows[j]["tablename"].ToString() == "COMPANY_ENROUTE")
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }
            }

            DataSet dsTemp = new DataSet();
            dtOk.TableName = "basedata";
            dsTemp.Tables.Add(dtOk);

            dtAirwayDetail.TableName = "airwayDetailData";
            dsTemp.Tables.Add(dtAirwayDetail);

            return dsTemp;
        }

        #endregion


    }

   
}

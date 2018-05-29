using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Text;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Remoting;
using System.Reflection;
using System.Net;
using System.ComponentModel;


namespace Common
{
    #region 表结构对象
    /// <summary>
    /// 表结构对象
    /// </summary>
    public class TableType
    {
        string _col;
        public string Cols
        {
            get
            {
                return this._col;
            }
            set
            {
                this._col = value;
            }
        }

        string _type;
        public string Types
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
    #endregion

    #region 表的列名对象（中文和字符）
    /// <summary>
    /// 表的列名对象（中文和字符）
    /// </summary>
    public class TableColName
    {
        string _col1;
        public string ColChinese
        {
            get
            {
                return this._col1;
            }
            set
            {
                this._col1 = value;
            }
        }

        string _col2;
        public string ColCharacter
        {
            get
            {
                return this._col2;
            }
            set
            {
                this._col2 = value;
            }
        }
    }
    #endregion

    #region 工具类
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utility
    {
        static public string FilterNull(string str)
        {
            return str == null ? string.Empty : str;
        }

        #region 获取年份字符串
        /// <summary>
        /// 获取年份字符串
        /// </summary>
        /// <returns>返回要填充的年份字符串,以逗号分割</returns>
        static public string GetYear(int intervalYear)
        {
            DateTime currentTime = DateTime.Now;
            string backStr = "";

            for (int i = currentTime.Year - intervalYear; i <= currentTime.Year + intervalYear; i++)
            {
                backStr = backStr + i.ToString() + ",";
            }

            backStr = backStr.TrimEnd(',');

            return backStr;
        }
        #endregion

        #region 获取月份字符串
        /// <summary>
        /// 获取月份字符串
        /// </summary>
        /// <returns>返回月份字符串，以逗号分割</returns>
        static public string GetMonth()
        {
            string backStr = "";
            for (int i = 1; i <= 12; i++)
            {
                if (i > 9)
                {
                    backStr = backStr + i.ToString() + ",";
                }
                else
                {
                    backStr = backStr + "0" + i.ToString() + ",";
                }
            }

            backStr = backStr.TrimEnd(',');

            return backStr;
        }
        #endregion

        #region 获取日字符串
        /// <summary>
        /// 获取日字符串
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns>返回日的字符串，以逗号分割</returns>
        static public string GetDay(int iYear, int iMonth)
        {
            string backStr = "";
            int iDayCount = 30;
            switch (iMonth)
            {
                case 1:
                    iDayCount = 31;
                    break;
                case 3:
                    iDayCount = 31;
                    break;
                case 5:
                    iDayCount = 31;
                    break;
                case 7:
                    iDayCount = 31;
                    break;
                case 8:
                    iDayCount = 31;
                    break;
                case 10:
                    iDayCount = 31;
                    break;
                case 12:
                    iDayCount = 31;
                    break;
                case 4:
                    iDayCount = 30;
                    break;
                case 6:
                    iDayCount = 30;
                    break;
                case 9:
                    iDayCount = 30;
                    break;
                case 11:
                    iDayCount = 30;
                    break;
                case 2:
                    {
                        if ((SqlInt64.Mod(iYear, 4) == 0) && (SqlInt64.Mod(iYear, 100) != 0))
                        {
                            iDayCount = 29;
                        }
                        else
                        {
                            iDayCount = 28;
                        }
                    }
                    break;
            }

            for (int i = 1; i <= iDayCount; i++)
            {
                if (i > 9)
                {
                    backStr = backStr + i.ToString() + ",";
                }
                else
                {
                    backStr = backStr + "0" + i.ToString() + ",";
                }
            }

            backStr = backStr.TrimEnd(',');

            return backStr;
        }
        #endregion        

        #region 获取小时字符串
        /// <summary>
        /// 获取小时字符串
        /// </summary>
        /// <returns>返回小时字符串,以逗号分割</returns>
        static public string GetHour()
        {
            string backStr = "";

            for (int i = 0; i < 24; i++)
            {
                if (i > 9)
                {
                    backStr = backStr + i.ToString() + ",";
                }
                else
                {
                    backStr = backStr + "0" + i.ToString() + ",";
                }
            }
            backStr = backStr.TrimEnd(',');

            return backStr;

        }
        #endregion

        #region 获取分钟字符串
        /// <summary>
        /// 获取分钟字符串
        /// </summary>
        /// <returns>返回分钟字符串,以逗号分割</returns>
        static public string GetMinute()
        {
            string backStr = "";

            for (int i = 0; i < 60; i++)
            {
                if (i > 9)
                {
                    backStr = backStr + i.ToString() + ",";
                }
                else
                {
                    backStr = backStr + "0" + i.ToString() + ",";
                }
            }
            backStr = backStr.TrimEnd(',');

            return backStr;
        }
        #endregion

        #region 英文星期转换中文显示
        /// <summary>
        /// 英文星期转换中文显示
        /// </summary>
        /// <param name="dfw"></param>
        /// <returns>返回星期中文显示</returns>
        static public string GetChineseWeekName(DayOfWeek dfw)
        {
            if (dfw.Equals(DayOfWeek.Monday))
            {
                return "星期一";
            }
            else if (dfw.Equals(DayOfWeek.Tuesday))
            {
                return "星期二";
            }
            else if (dfw.Equals(DayOfWeek.Wednesday))
            {
                return "星期三";
            }
            else if (dfw.Equals(DayOfWeek.Thursday))
            {
                return "星期四";
            }
            else if (dfw.Equals(DayOfWeek.Friday))
            {
                return "星期五";
            }
            else if (dfw.Equals(DayOfWeek.Saturday))
            {
                return "星期六";
            }
            else if (dfw.Equals(DayOfWeek.Sunday))
            {
                return "星期日";
            }
            return string.Empty;
        }
        #endregion

        #region 某一字符个数
        /// <summary>
        /// 某一字符个数
        /// </summary>
        /// <param name="iNum">字符个数</param>      
        /// <param name="symbol">字符</param>
        /// <returns>字符组成字符串</returns>
        public static string GetNumSymbol(int iNum, string symbol)
        {
            string backStr = String.Empty;
            for (int i = 1; i <= iNum; i++)
            {
                backStr = backStr + symbol;
            }
            return backStr;
        }

        /// <summary>
        /// 左对齐在右侧补字符
        /// </summary>
        /// <param name="info"></param>
        /// <param name="length">指定的长度</param>
        /// <param name="c">要补得字符</param>
        /// <returns></returns>
        public static string GetPadRightString(string info, int length, char c)
        {
            return info.Trim().PadRight(length, c).Substring(0, length);
        }

        /// <summary>
        /// 右对齐对齐在左侧补字符
        /// </summary>
        /// <param name="info"></param>
        /// <param name="length">指定的长度</param>
        /// <param name="c">要补得字符</param>
        /// <returns></returns>
        public static string GetPadLeftString(string info, int length, char c)
        {
            return info.Trim().PadLeft(length, c).Substring(0, length);
        }

        #endregion

        /// <summary>
        /// 判断字符串的长度，包括字母、数字、汉子
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool ChangeByte(string str, int i)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            int m = b.Length;
            if (m < i)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取随机的指定长度随机数
        /// </summary>
        /// <param name="len">随机数长度</param>
        /// <returns></returns>
        public static int GetRandNum(int len)
        {
            Random rnd = new Random();
            return rnd.Next(0, int.Parse(Utility.GetNumSymbol(len, "9")));
        }

        /// <summary>
        /// 获取指定上下限长度的随机数
        /// </summary>
        /// <param name="min">下限(含)</param>
        /// <param name="max">上限(含)</param>
        /// <returns></returns>
        public static int GetRandNum(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        #region 增加序号
        /// <summary>
        /// 增加序号
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="sequenceName">序号列名</param>
        /// <param name="hasFirstRow">是否要第一行</param>
        /// <returns></returns>
        static public DataView GetSequance(DataView dv, string sequenceName, bool hasFirstRow)
        {
            if (dv.Table.Rows.Count > 0)
            {
                dv.Table.Columns.Add(new DataColumn(sequenceName, typeof(String)));

                if (hasFirstRow)
                {
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        dv.Table.Rows[i].BeginEdit();
                        dv.Table.Rows[i][sequenceName] = i + 1;                       
                        dv.Table.Rows[i].EndEdit();
                    }
                }
                else
                {
                    for (int i = 1; i < dv.Table.Rows.Count; i++)
                    {
                        dv.Table.Rows[i].BeginEdit();
                        dv.Table.Rows[i][sequenceName] = i;
                        dv.Table.Rows[i].EndEdit();
                    }
                }      

                return dv;
            }
            else
            {
                return null;
            }
        }        

        /// <summary>
        /// 增加序号分页
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="sequenceName">序号列名</param>
        /// <param name="hasFirstRow">是否要第一行</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        static public DataView GetSequance(DataView dv, string sequenceName, bool hasFirstRow, int currentPageIndex, int pageSize)
        {
            if (dv.Table.Rows.Count > 0)
            {
                dv.Table.Columns.Add(new DataColumn(sequenceName, typeof(String)));

                if (hasFirstRow)
                {
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        dv.Table.Rows[i].BeginEdit();
                        dv.Table.Rows[i][sequenceName] = pageSize  * (currentPageIndex - 1) +  i + 1;
                        dv.Table.Rows[i].EndEdit();
                    }
                }
                else
                {
                    for (int i = 1; i < dv.Table.Rows.Count; i++)
                    {
                        dv.Table.Rows[i].BeginEdit();
                        dv.Table.Rows[i][sequenceName] = pageSize * (currentPageIndex - 1) + i;
                        dv.Table.Rows[i].EndEdit();
                    }
                }

                return dv;
            }
            else
            {
                return null;
            }
        }
        #endregion     

        #region 根据datatable的列返回其对应的实体
        /// <summary>
        /// 根据datatable的列返回其对应的实体
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static object GetListObj(object s, DataRow dr)
        {
            object obj = null;
            obj = Activator.CreateInstance(s.GetType(), null);//创建指定类型实例
            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性
            foreach (DataColumn dc in dr.Table.Columns)
            {
                foreach (PropertyInfo t in fields)
                {
                    if (dc.ColumnName.ToUpper() == t.Name.ToUpper())
                    {
                        t.SetValue(obj, dr[dc.ColumnName], null);
                        break;
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 根据datatable的列返回其对应的实体集合List
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<object> GetListsObj(object s, DataTable dt)
        {
            List<object> lists = new List<object>();
            object udtemp = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                udtemp = Common.Utility.GetListObj(s, dt.Rows[i]);
                lists.Add(udtemp);
            }

            return lists;
        } 
        
        /// <summary>
        /// 字符串集合按字符串形式返回，中间分隔符分隔
        /// </summary>
        /// <param name="lsg"></param>
        /// <returns></returns>
        public static string GetList2String(List<string> lsts, string splitChar)
        {
            string resStr = string.Empty;
            for (int i = 0; i < lsts.Count; i++)
            {
                resStr += lsts[i] + splitChar;
            }
            return resStr;
        }      


        #endregion

        //***********************************************************************************************

        #region 构造表
        /// <summary>
        /// 构造表
        /// </summary>
        /// <param name="cols">列名和类型对象集合</param>
        /// <returns>表</returns>
        public static DataTable MakeTable(List<TableType> cols)
        {
            DataTable dt = new DataTable();

            DataColumn col;

            foreach (TableType var in cols)
            {
                col = new DataColumn(var.Cols, System.Type.GetType("System." + var.Types));
                dt.Columns.Add(col);
            }

            return dt;
        }

        /// <summary>
        /// 构造表
        /// </summary>
        /// <param name="colsName">列名字符串,列名以'|'分割</param>
        /// <param name="colsTypeName">列类型字符串,列类型以'|'分割</param>
        /// <returns>表</returns>
        public static DataTable MakeTable(string colsName, string colsTypeName)
        {
            List<TableType> DepElys = new List<TableType>();
            string[] colss = colsName.Split('|');
            string[] colsTypes = colsTypeName.Split('|');

            for (int i = 0; i < colss.Length; i++)
            {
                TableType DepEly = new TableType();
                DepEly.Cols = colss[i];
                DepEly.Types = colsTypes[i];
                DepElys.Add(DepEly);
            }
            return Utility.MakeTable(DepElys);            
        }
        #endregion

        #region xml某节点返回DataTable
        /// <summary>
        /// xml返回DataTable
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="xmlFilePath"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXmlNode(List<TableType> cols, string xmlFilePath, string ID)
        {
            ////构造表
            DataTable dt = Utility.MakeTable(cols);

            //初始化表
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            //递归节点
            Utility.Due(dt, ele, cols, ID);

            return dt;

        }
        #endregion

        #region 递归xml某节点
        /// <summary>
        /// 递归xml某节点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ele"></param>
        /// <param name="cols"></param>
        /// <param name="ID"></param>
        public static void Due(DataTable dt, XmlElement ele, List<TableType> cols, string ID)
        {
            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                if (ele.ChildNodes[i].Attributes[cols[0].Cols].Value.ToString() == ID)
                {
                    DataRow subDr = dt.NewRow();

                    foreach (TableType var in cols)
                    {
                        subDr[var.Cols] = ele.ChildNodes[i].Attributes[var.Cols].Value.ToString();
                    }
                    dt.Rows.Add(subDr);

                    //递归开始
                    if (ele.ChildNodes[i].ChildNodes.Count > 0)
                    {
                        Due(dt, (XmlElement)ele.ChildNodes[i], cols, ID);
                    }
                    break;
                }
            }
        }
        #endregion

        #region xml返回DataTable
        /// <summary>
        /// xml返回DataTable
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXml(List<TableType> cols, string xmlFilePath)
        {
            ////构造表
            DataTable dt = Utility.MakeTable(cols);

            //初始化表
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            DataRow dr = dt.NewRow();

            foreach (TableType var in cols)
            {
                dr[var.Cols] = ele.Attributes[var.Cols].Value.ToString();
            }
            dt.Rows.Add(dr);

            //递归节点
            Utility.Due(dt, ele, cols);

            return dt;
        }

        /// <summary>
        /// xml返回DataTable
        /// </summary>
        /// <param name="colNames">|分割</param>
        /// <param name="colTypes">|分割</param>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXml(string colNames, string colTypes, string xmlFilePath)
        {
            //构造表
            List<TableType> DepElys = new List<TableType>();
            string[] colss = colNames.Split('|');
            string[] colsTypes = colTypes.Split('|');
            for (int i = 0; i < colss.Length; i++)
            {
                TableType DepEly = new TableType();
                DepEly.Cols = colss[i];
                DepEly.Types = colsTypes[i];
                DepElys.Add(DepEly);
            }

            DataTable dt = Utility.MakeTable(DepElys);

            //初始化表
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            DataRow dr = dt.NewRow();

            foreach (TableType var in DepElys)
            {
                dr[var.Cols] = ele.Attributes[var.Cols].Value.ToString();
            }
            dt.Rows.Add(dr);

            //递归节点
            Utility.Due(dt, ele, DepElys);

            return dt;
        }
        #endregion

        #region 递归xml节点
        /// <summary>
        /// 递归xml节点
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ele"></param>
        /// <param name="cols"></param>
        public static void Due(DataTable dt, XmlElement ele, List<TableType> cols)
        {
            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                DataRow subDr = dt.NewRow();

                foreach (TableType var in cols)
                {
                    subDr[var.Cols] = ele.ChildNodes[i].Attributes[var.Cols].Value.ToString();
                }
                dt.Rows.Add(subDr);

                //递归开始
                if (ele.ChildNodes[i].ChildNodes.Count > 0)
                {
                    Due(dt, (XmlElement)ele.ChildNodes[i], cols);
                }
            }
        }
        #endregion

        #region xml某节点操作
        /// <summary>
        /// 获取某节点值
        /// </summary>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <param name="idNodeAttrName">id节点属性名</param>
        /// <param name="idNodeAttrValue">id节点属性值</param>       
        /// <param name="curNodeAttrName">操作节点属性名</param>
        public static string GetAttributeFromXml(string xmlFilePath, string idNodeAttrName, string idNodeAttrValue, string curNodeAttrName)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                if (ele.ChildNodes[i].Attributes[idNodeAttrName].Value.ToString() == idNodeAttrValue)
                {
                    return ele.ChildNodes[i].Attributes[curNodeAttrName].Value.ToString();                    
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 更新某节点值
        /// </summary>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <param name="idNodeAttrName">id节点属性名</param>
        /// <param name="idNodeAttrValue">id节点属性值</param>
        /// <param name="curNodeAttrName">操作节点属性名</param>
        /// <param name="curNodeAttrValue">操作节点属性值</param>
        public static bool UpdateAttributeFromXml(string xmlFilePath, string idNodeAttrName, string idNodeAttrValue, string curNodeAttrName,
            string curNodeAttrValue)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                if (ele.ChildNodes[i].Attributes[idNodeAttrName].Value.ToString() == idNodeAttrValue)
                {
                    ele.ChildNodes[i].Attributes[curNodeAttrName].Value = curNodeAttrValue;
                    xd.Save(xmlFilePath);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新某节点值，每组节点名和节点对应属性值必须对应
        /// </summary>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <param name="idNodeAttrNames">id节点属性名,多个用，号分割</param>
        /// <param name="idNodeAttrValues">id节点属性值,多个用，号分割</param>
        /// <param name="curNodeAttrNames">操作节点属性名,多个用，号分割</param>
        /// <param name="curNodeAttrValues">操作节点属性值,多个用，号分割</param>
        /// <param name="splitChar">分割符号</param>
        public static bool UpdateAttributeFromXmls(string xmlFilePath, string idNodeAttrNames, string idNodeAttrValues, string curNodeAttrNames,
            string curNodeAttrValues, char splitChar)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            for (int i = 0; i < ele.ChildNodes.Count; i++)
            {
                if (Utility.GetCurrNodeByIdFormXml(ele.ChildNodes[i], idNodeAttrNames, idNodeAttrValues))
                {
                    string[] curNodeAttrName = curNodeAttrNames.Split(splitChar);
                    string[] curNodeAttrValue = curNodeAttrValues.Split(splitChar);

                    for (int j = 0; j < curNodeAttrName.Length; j++)
                    {
                        ele.ChildNodes[i].Attributes[curNodeAttrName[j]].Value = curNodeAttrValue[j];
                    }
                    
                    xd.Save(xmlFilePath);
                    return true;
                }               
            }
            return false;
        }

        /// <summary>
        /// 查找当前要操作的xml节点，根据ID
        /// </summary>
        /// <param name="eleNode">当前节点</param>
        /// <param name="idNodeAttrNames"></param>
        /// <param name="idNodeAttrValues"></param>
        /// <returns></returns>
        private static bool GetCurrNodeByIdFormXml(XmlNode eleNode, string idNodeAttrNames, string idNodeAttrValues)
        {
            bool hasCurrId = true;

            string[] idNodeAttrName = idNodeAttrNames.Split(',');
            string[] idNodeAttrValue = idNodeAttrValues.Split(',');

            for (int j = 0; j < idNodeAttrName.Length; j++)
            {
                if (eleNode.Attributes[idNodeAttrName[j]].Value.ToString() == idNodeAttrValue[j])
                {
                    hasCurrId = hasCurrId && true;
                }
                else
                {
                    hasCurrId = hasCurrId && false;
                }
            }

            return hasCurrId;           
        }

        /// <summary>
        /// xml增加节点
        /// </summary>
        /// <param name="rootName"></param>
        /// <param name="subName"></param>
        /// <param name="xmlFilePath"></param>
        /// <param name="nodeAttrNames"></param>
        /// <param name="nodeAttrValues"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static bool AddNodeForXml(string rootName, string subName, string xmlFilePath, string nodeAttrNames, List<string> nodeAttrValues, string splitChar)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(xmlFilePath);

                XmlNode root = xd.SelectSingleNode(rootName);
                XmlElement xe = xd.CreateElement(subName);
                string[] attiNames = nodeAttrNames.Split(splitChar.ToCharArray());
                for (int j = 0; j < nodeAttrValues.Count; j++)
                {
                    string[] attiValues = nodeAttrValues[j].Split(splitChar.ToCharArray());
                    for (int i = 0; i < attiNames.Length; i++)
                    {
                        xe.SetAttribute(attiNames[i], attiValues[i]);
                    }
                }                
                root.AppendChild(xe);
                xd.Save(xmlFilePath);                
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region DataTable生成xml
        /// <summary>
        /// DataTable生成xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootText"></param>
        /// <param name="startText"></param>
        /// <param name="tcn">列名和中文描述对象集合</param>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static string MakeXmlFromDataTable(DataTable dt, string rootText, string startText, List<TableColName> tcn, string xmlFilePath)
        {
            try
            {
                DataView dv = dt.DefaultView;

                //string xmlFilePath = this.Page.Server.MapPath("xml\\function\\makeXml.xml");
                string xmlHead = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

                string xmlRootStart = "<" + rootText + ">";
                string xmlRootEnd = "</" + rootText + ">";

                string xmlStart = "<" + startText + ">";
                string xmlEnd = "</" + startText + ">";

                string xmlFile = "";
                xmlFile += xmlHead + xmlRootStart;

                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    xmlFile += xmlStart;
                    foreach (TableColName var in tcn)
                    {
                        xmlFile += "<" + var.ColChinese + ">" + dv[i][var.ColCharacter].ToString() + "</" + var.ColChinese + ">";
                    }

                    xmlFile += xmlEnd;
                }

                xmlFile += xmlRootEnd;
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlFile);
                xd.Save(xmlFilePath);

                return "已成功生成xml文件！";
            }
            catch (Exception ex)
            {
                return "生成xml文件失败！" + ex.ToString();
            }

        }
        #endregion

        #region 表格内容处理
        /// <summary>
        /// 依据某列值提取内容
        /// </summary>
        /// <param name="dtTemp">原始表</param>
        /// <param name="objName">参照列名</param>
        /// <param name="objValue">参照列值</param>
        public static DataTable GetTableByColumnOne(DataTable dtTemp, string objName, string objValue)
        {
            DataTable dtOk = null;
            dtOk = dtTemp.Clone();
            DataRow dr = null;
            dtOk.Rows.Clear();
            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                if (dtTemp.Rows[j][objName].ToString() == objValue)
                {
                    dr = dtOk.NewRow();
                    dr.ItemArray = dtTemp.Rows[j].ItemArray;
                    dtOk.Rows.Add(dr);
                }
            }

            return dtOk;
        }

        /// <summary>
        /// 给表格进行排序
        /// </summary>
        /// <param name="sortColumns">排序字段,多个字段用,分割</param>
        /// <param name="dtSoc">排序表格</param>
        /// <returns></returns>
        public static DataTable SortTable(string sortColumns, DataTable dtSoc)
        {
            DataTable sortT = null;
            if (Common.Utility.DtHasData(dtSoc))
            {
                sortT = dtSoc.Clone();
                sortT.Rows.Clear();
                DataRow[] drs = new DataRow[dtSoc.Rows.Count];
                drs = dtSoc.Select("1=1", sortColumns);
                for (int i = 0; i < drs.Length; i++)
                {
                    sortT.Rows.Add(drs[i].ItemArray);
                }
            }

            return sortT;
        }

        #endregion

        #region xml和DataSet
        /// <summary>
        /// 将xml对象内容字符串转换为DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// 将xml文件转换为DataSet
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        public static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);

                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                //xmlDS.ReadXml(xmlFile);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// 将DataSet转换为xml对象字符串
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>
        public static string ConvertDataSetToXML(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                UnicodeEncoding utf = new UnicodeEncoding();
                return utf.GetString(arr).Trim();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <param name="xmlFile"></param>
        public static void ConvertDataSetToXMLFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //返回Unicode编码的文本
                UnicodeEncoding utf = new UnicodeEncoding();
                StreamWriter sw = new StreamWriter(xmlFile);
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sw.WriteLine(utf.GetString(arr).Trim());
                sw.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        #endregion

        #region 字节数组和Image
        /// <summary>
        /// 将字节数组转换成Image
        /// </summary>
        /// <param name="byteList"></param>
        /// <returns></returns>
        public static Image ByteToImage(byte[] byteList)
        {
            MemoryStream stream1 = new MemoryStream(byteList);
            Image image1 = Image.FromStream(stream1, true);
            stream1.Close();
            return image1;
        }

        /// <summary>
        /// 将Image类型转换成byte[]类型
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image image)
        {
            MemoryStream stream1 = new MemoryStream();
            image.Save(stream1, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] buffer1 = new byte[(int)stream1.Length];
            stream1.Position = 0;
            stream1.Read(buffer1, 0, buffer1.Length);
            stream1.Close();
            return buffer1;
        }
        #endregion

        #region "判断是否有数据"
        public static bool DtHasData(DataTable dt)
        {
            if (dt == null)
            {
                return false;
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool DsHasData(DataSet ds)
        {
            if (ds == null)
            {
                return false;
            }
            else
            {
                if (ds.Tables.Count > 0)
                {
                    return DtHasData(ds.Tables[0]);
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool DvHasData(DataView dv)
        {
            if (dv == null)
            {
                return false;
            }
            else
            {
                return DtHasData(dv.Table);
            }
        }

        public static bool ObjHasData(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(obj.ToString().Trim()))
                {
                    return false;
                }
            }

            return true;
        }
        
        #endregion

        //***********************************************************************************************
        #region 10以下数补零返字符串
        /// <summary>
        /// 10以下数补零返字符串
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public static string addZero(int old)
        {
            if (old < 10)
            {
                return "0" + old;
            }
            else
            {
                return old.ToString();
            }
        }

        /// <summary>
        /// 数字前补零
        /// </summary>
        /// <param name="old"></param>
        /// <param name="strLength">总字符长度</param>
        /// <returns></returns>
        public static string addZero(int old, int strLength)
        {
            string newStr = Convert.ToString(Math.Abs(old));

            for (int i = 0; i < (strLength - old.ToString().Length); i++)
            {
                newStr = "0" + newStr;
            }

            if (old < 0)
                newStr = "-" + newStr;

            return newStr;
        }
        #endregion

        #region 员工年假计算

        /// <summary>
        /// 计算年度年假天数以小时返回一天按8小时计算
        /// </summary>
        /// <param name="takePartInWorkDate">参加工作日期(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">来公司日期</param>        
        /// <param name="yearRule">年假计算规则(1|2|3)默认选择1即法定标准</param>
        /// <param name="hasOneYear">是否要求满一年才能享受年假(是|否)默认否</param>
        /// <returns></returns>
        public static int CaluYearVacationHasDays(string takePartInWorkDate, string fromCompDate, string yearRule, string hasOneYear)
        {
            int baseCnt = Utility.CaluYearVacationBaseCount(takePartInWorkDate, fromCompDate, yearRule); //年假基数     
       
            double fromCompYear = 0; //来公司天数    
            TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(fromCompDate);
            fromCompYear = (double)ts.Days / 365;

            double hasDays = 0; //计算所得年休假天数

            if (hasOneYear.Equals("是"))
            {
                if (fromCompYear >= 1)
                {
                    //可以休年假                
                    if (fromCompYear < 2)
                    {
                        //休当年剩余天数的应得年假
                        TimeSpan ts1 = Convert.ToDateTime(Convert.ToDateTime(fromCompDate).Year.ToString() + "-12-31") - Convert.ToDateTime(fromCompDate);
                        hasDays = Math.Round((double)ts1.Days / 365 * baseCnt);
                    }
                    else
                    {
                        //休满年假
                        hasDays = baseCnt;
                    }                   
                }
                else
                {
                    hasDays = 0;  //不可以休年假
                }
            }
            else
            {
                //可以休年假                
                if (fromCompYear < 1)
                {
                    //休当年剩余天数的应得年假
                    TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(fromCompDate).Year.ToString() + "-12-31") - Convert.ToDateTime(fromCompDate);
                    hasDays = Math.Round((double)ts2.Days / 365 * baseCnt);
                }
                else
                {
                    //休满年假
                    hasDays = baseCnt;
                }
            }

            return (int)hasDays * 8;
        }

        /// <summary>
        /// 计算年假基数
        /// </summary>
        /// <param name="takePartInWorkDate">参加工作日期(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">来公司日期</param> 
        /// <param name="yearRule">年假规则</param>
        /// <returns></returns>
        public static int CaluYearVacationBaseCount(string takePartInWorkDate, string fromCompDate, string yearRule)
        {
            if (yearRule.Equals("1"))
            {
                return Utility.CaluYearVacationBaseCountOne(takePartInWorkDate);
            }
            else if(yearRule.Equals("2"))
            {
                return Utility.CaluYearVacationBaseCountTwo(takePartInWorkDate, fromCompDate);
            }
            else if(yearRule.Equals("3"))
            {
                return Utility.CaluYearVacationBaseCountThree(takePartInWorkDate);
            }
            else
            {
                return 0;
            }            
        }

        
        /// <summary>
        /// 计算年假基数按规则1即法定年休假
        /// 工龄一年以上(含一年)十年以下(含十年)，年休假5天
        /// 工龄十年以上二十年以下(含二十年)，年休假10天
        /// 工龄二十年以上，年休假15天
        /// </summary>
        /// <param name="takePartInWorkDate">参加工作日期(yyyy-MM-dd)</param>
        /// <returns></returns>
        public static int CaluYearVacationBaseCountOne(string takePartInWorkDate)
        {
            int baseCnt = 0;
            TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(takePartInWorkDate);
            baseCnt = (int)(ts.Days / 365);

            if (baseCnt >= 1 && baseCnt <= 10)
            {
                return 5;
            }

            if (baseCnt > 10 && baseCnt <= 20)
            {
                return 10;
            }

            if (baseCnt > 20)
            {
                return 15;
            }
            return 0;
        }

        /// <summary>
        /// 计算年假基数按规则2(规则1即法定年休假 + 公司福利假)
        /// 福利假规则
        /// 来公司满三年以上的员工，从满三年之日起开始享有1天的公司福利假
        /// 每增加一年增加1天，最高不超过5天
        /// </summary>
        /// <param name="takePartInWorkDate">参加工作日期(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">来公司日期</param> 
        /// <returns></returns>
        public static int CaluYearVacationBaseCountTwo(string takePartInWorkDate, string fromCompDate)
        {
            int baseCnt = Utility.CaluYearVacationBaseCountOne(takePartInWorkDate);
            int fromCompYear = 0;
            TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(fromCompDate);
            fromCompYear = (int)(ts.Days / 365);

            if (fromCompYear == 3)
            {
                baseCnt = baseCnt + 1;
            }
            else if (fromCompYear == 4)
            {
                baseCnt = baseCnt + 2;
            }
            else if (fromCompYear == 5)
            {
                baseCnt = baseCnt + 3;
            }
            else if (fromCompYear == 6)
            {
                baseCnt = baseCnt + 4;
            }
            else if (fromCompYear == 7)
            {
                baseCnt = baseCnt + 5;
            }
            else if (fromCompYear > 7)
            {
                baseCnt = baseCnt + 5;
            }

            return baseCnt;
        }

        /// <summary>
        /// 计算年假基数按规则3
        /// 五年以下(含五年)，年休假5天
        /// 满五年，每年递增一天，最多不超过15天
        /// </summary>
        /// <param name="takePartInWorkDate">参加工作日期(yyyy-MM-dd)</param>        
        /// <returns></returns>
        public static int CaluYearVacationBaseCountThree(string takePartInWorkDate)
        {
            int baseCnt = 0;
            TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(takePartInWorkDate);
            int takePartInWorkCnt = (int)(ts.Days / 365);

            if (takePartInWorkCnt <= 5)
            {
                baseCnt = 5;
            }
            else if (takePartInWorkCnt > 5)
            {
                if (takePartInWorkCnt < 15)
                {
                    baseCnt = takePartInWorkCnt;
                }
                else
                {
                    baseCnt = 15;
                }
            }     

            return baseCnt;
        }

        #endregion     
       
        #region 集合类处理
        /// <summary>
        /// 获取List字符串集合
        /// </summary>
        /// <param name="values">多个字符串用|分割</param>
        /// <returns></returns>
        public static List<string> GetList(string values)
        {
            List<string> socList = new List<string>();
            string[] strs = values.Split('|');
            for (int i = 0; i < strs.Length; i++)
            {
                socList.Add(strs[i].ToString());                
            }
            
            return socList;    
        }

        /// <summary>
        /// 将字符串List都转换成写的List
        /// </summary>
        /// <param name="soc"></param>
        /// <returns></returns>
        public static List<string> GetList2Upper(List<string> soc)
        {
            List<string> tar = new List<string>();

            foreach (var item in soc)
            {
                tar.Add(item.ToUpper());
            }

            return tar;
        }

        #endregion

        #region 实体类和datatable的datarow相互赋值

        /// <summary>
        /// 从datable的row获取对象(要求表的列名至少包含目标对象的全部属性，同时要求列名和属性名一致)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="oriobj"></param>
        /// <returns></returns>
        public static object GetObjFormDataTableRow(DataTable dt, int rowIndex, object oriobj)
        {
            object obj = Activator.CreateInstance(oriobj.GetType());
            Type t = obj.GetType();
            System.Reflection.PropertyInfo[] pps = t.GetProperties();

            for (int i = 0; i < pps.Length; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (pps[i].Name.ToUpper() == dt.Columns[j].ColumnName.ToUpper())
                    {
                        pps[i].SetValue(obj, Convert.ChangeType(dt.Rows[rowIndex][dt.Columns[j].ColumnName], pps[i].PropertyType), null);
                        break;
                    }
                }
            }

            return obj;
        }


        /// <summary>
        /// 从实体类获取datatable对象的datarow(要求表的列名至少包含目标对象的全部属性，同时要求列名和属性名一致)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowIndex"></param>
        /// <param name="oriobj"></param>
        /// <returns></returns>
        public static DataRow GetDataTableRowFormObj(DataTable dt, int rowIndex, object oriobj)
        {
            DataRow drTemp = dt.NewRow();

            object obj = Activator.CreateInstance(oriobj.GetType());
            Type t = obj.GetType();
            System.Reflection.PropertyInfo[] pps = t.GetProperties();

            for (int i = 0; i < pps.Length; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (pps[i].Name.ToUpper() == dt.Columns[j].ColumnName.ToUpper())
                    {
                        drTemp[dt.Columns[j].ColumnName] = pps[i].GetValue(obj, null) == null ? "" : pps[i].GetValue(obj, null).ToString();
                        break;
                    }
                }
            }

            return drTemp;
        }

        #endregion

        #region JSON相关处理

        /// <summary>
        /// datatable转换为json数组
        /// </summary>
        /// <param name="dttmp"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> Datatable2Json(DataTable dttmp)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dttmp.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dttmp.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }

            return parentRow;
        }

        #endregion

        #region 全半角转换

        /// <summary>
        /// 转半角
        /// </summary>
        public static string ToDBC(string input)
        {
            if (input == null) return null;

            //全角转半角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }

            return new string(c);
        }

        /// <summary>
        /// 转全角
        /// </summary>
        public static string ToSBC(string input)
        {
            if (input == null) return null;

            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }

            return new string(c);
        }

        #endregion

        #region 记录日志

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

            StreamWriter sw = File.AppendText(logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            sw.WriteLine(logContent);
            sw.Close();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        /// <param name="isExecute">Y 记录日志  N 不进行记录</param>
        public static void RecordLog(string logContent, string logPath, string isExecute)
        {
            if(isExecute.ToUpper() == "Y")
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                StreamWriter sw = File.AppendText(logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                sw.WriteLine(logContent);
                sw.Close();

            }            
        }

        #endregion

        #region ResolveUrl(解析相对Url)
        /// <summary>
        /// 解析相对Url
        /// </summary>
        /// <param name="relativeUrl">相对Url</param>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return string.Empty;
            relativeUrl = relativeUrl.Replace("\\", "/");
            if (relativeUrl.StartsWith("/"))
                return relativeUrl;
            if (relativeUrl.Contains("://"))
                return relativeUrl;
            return VirtualPathUtility.ToAbsolute(relativeUrl);
        }

        #endregion

        #region HtmlEncode(对html字符串进行编码)
        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = int.MinValue;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region Session操作
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                return;
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public static string GetSession(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            return HttpContext.Current.Session[key] as string;
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            HttpContext.Current.Session.Contents.Remove(key);
        }

        #endregion

        #region Cookie操作
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }
        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="CookiesName">Cookie对象名称</param>
        public static void RemoveCookie(string CookiesName)
        {
            HttpCookie objCookie = new HttpCookie(CookiesName.Trim());
            objCookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }
        #endregion

        #region GetFileControls(获取客户端文件控件集合)

        /// <summary>
        /// 获取有效客户端文件控件集合,文件控件必须上传了内容，为空将被忽略,
        /// 注意:Form标记必须加入属性 enctype="multipart/form-data",服务器端才能获取客户端file控件.
        /// </summary>
        public static List<HttpPostedFile> GetFileControls()
        {
            var result = new List<HttpPostedFile>();
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
                return result;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.ContentLength == 0)
                    continue;
                result.Add(files[i]);
            }
            return result;
        }

        #endregion

        #region GetFileControl(获取第一个有效客户端文件控件)

        /// <summary>
        /// 获取第一个有效客户端文件控件,文件控件必须上传了内容，为空将被忽略,
        /// 注意:Form标记必须加入属性 enctype="multipart/form-data",服务器端才能获取客户端file控件.
        /// </summary>
        public static HttpPostedFile GetFileControl()
        {
            var files = GetFileControls();
            if (files == null || files.Count == 0)
                return null;
            return files[0];
        }

        #endregion

        #region HttpWebRequest(请求网络资源)

        /// <summary>
        /// 请求类型
        /// </summary>
        public enum WebRequestType
        {
            /// <summary>
            /// Http请求
            /// </summary>
            Http = 0,

            /// <summary>
            /// wcf服务请求
            /// </summary>
            Wcf = 1,

            /// <summary>
            /// webServies请求
            /// </summary>
            WebService = 2
        }

        /// <summary>
        /// 检验url是否可用
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestType"></param>
        /// <param name="errStr"></param>
        /// <returns></returns>
        public static bool VerifyAvailable(string requestUri, WebRequestType requestType, ref string errStr)
        {
            bool success = false;

            if (requestType == WebRequestType.Http)
            {
                var request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
                try
                {
                    request.AllowAutoRedirect = false;
                    request.AllowWriteStreamBuffering = false;
                    request.Headers.Add("Cache-Control", "no-store");
                    request.Headers.Add("Pragma", "no-cache");
                    request.Method = WebRequestMethods.Http.Head;
                    var response = request.GetResponse() as HttpWebResponse;
                    if (response == null)
                    {
                        errStr = "响应为null";
                    }
                    response.Close();
                    success = response.StatusCode == HttpStatusCode.OK;
                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        response.Close();
                        success = true;
                    }
                    else
                    {
                        errStr = "发生异常，原因：" + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    errStr = "发生异常，原因：" + ex.Message;
                }
            }
            else if (requestType == WebRequestType.Wcf)
            {
                //预留处理
            }
            else if (requestType == WebRequestType.WebService)
            {
                //预留处理
            }

            return success;
        }    

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源地址</param>
        public static string HttpWebRequestOp(string url)
        {
            return HttpWebRequestOp(url, string.Empty, Encoding.GetEncoding("utf-8"));
        }

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源Url地址</param>
        /// <param name="parameters">提交的参数,格式：参数1=参数值1&amp;参数2=参数值2</param>
        public static string HttpWebRequestOp(string url, string parameters)
        {
            return HttpWebRequestOp(url, parameters, Encoding.GetEncoding("utf-8"), true);
        }

        /// <summary>
        /// 请求网络资源,返回响应的文本
        /// </summary>
        /// <param name="url">网络资源地址</param>
        /// <param name="parameters">提交的参数,格式：参数1=参数值1&amp;参数2=参数值2</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isPost">是否Post提交</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="cookie">Cookie容器</param>
        /// <param name="timeout">超时时间</param>
        public static string HttpWebRequestOp(string url, string parameters, Encoding encoding, bool isPost = false,
             string contentType = "application/x-www-form-urlencoded", CookieContainer cookie = null, int timeout = 120000)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.CookieContainer = cookie;
            if (isPost)
            {
                byte[] postData = encoding.GetBytes(parameters);
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = postData.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (Stream stream = response.GetResponseStream())
            {
                if (stream == null)
                    return string.Empty;
                using (var reader = new StreamReader(stream, encoding))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        #endregion



        //bootom
    }


    #endregion

    #region 深度克隆

    /// <summary>
    /// 扩展克隆
    /// </summary>
    public static class DeepCloneExtends
    {
        public static T DeepCloneObject<T>(this T t) where T : class
        {
            T model = System.Activator.CreateInstance<T>();                     //实例化一个T类型对象
            PropertyInfo[] propertyInfos = model.GetType().GetProperties();     //获取T对象的所有公共属性
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //判断值是否为空，如果空赋值为null见else
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                    NullableConverter nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    //将convertsionType转换为nullable对的基础基元类型
                    propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(t), nullableConverter.UnderlyingType), null);
                }
                else
                {
                    propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(t), propertyInfo.PropertyType), null);
                }
            }
            return model;
        }
        public static IList<T> DeepCloneList<T>(this IList<T> tList) where T : class
        {
            IList<T> listNew = new List<T>();
            foreach (var item in tList)
            {
                T model = System.Activator.CreateInstance<T>();                     //实例化一个T类型对象
                PropertyInfo[] propertyInfos = model.GetType().GetProperties();     //获取T对象的所有公共属性
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    //判断值是否为空，如果空赋值为null见else
                    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //如果convertsionType为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                        NullableConverter nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                        //将convertsionType转换为nullable对的基础基元类型
                        propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(item), nullableConverter.UnderlyingType), null);
                    }
                    else
                    {
                        propertyInfo.SetValue(model, Convert.ChangeType(propertyInfo.GetValue(item), propertyInfo.PropertyType), null);
                    }
                }
                listNew.Add(model);
            }
            return listNew;
        }
    }

    #endregion
}

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
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Common
{
    #region ��ṹ����
    /// <summary>
    /// ��ṹ����
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

    #region ��������������ĺ��ַ���
    /// <summary>
    /// ��������������ĺ��ַ���
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

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    public class Utility
    {
        static public string FilterNull(string str)
        {
            return str == null ? string.Empty : str;
        }


        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region ��ȡ����ַ���
        /// <summary>
        /// ��ȡ����ַ���
        /// </summary>
        /// <returns>����Ҫ��������ַ���,�Զ��ŷָ�</returns>
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

        #region ��ȡ�·��ַ���
        /// <summary>
        /// ��ȡ�·��ַ���
        /// </summary>
        /// <returns>�����·��ַ������Զ��ŷָ�</returns>
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

        #region ��ȡ���ַ���
        /// <summary>
        /// ��ȡ���ַ���
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns>�����յ��ַ������Զ��ŷָ�</returns>
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

        #region ��ȡСʱ�ַ���
        /// <summary>
        /// ��ȡСʱ�ַ���
        /// </summary>
        /// <returns>����Сʱ�ַ���,�Զ��ŷָ�</returns>
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

        #region ��ȡ�����ַ���
        /// <summary>
        /// ��ȡ�����ַ���
        /// </summary>
        /// <returns>���ط����ַ���,�Զ��ŷָ�</returns>
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

        #region Ӣ������ת��������ʾ
        /// <summary>
        /// Ӣ������ת��������ʾ
        /// </summary>
        /// <param name="dfw"></param>
        /// <returns>��������������ʾ</returns>
        static public string GetChineseWeekName(DayOfWeek dfw)
        {
            if (dfw.Equals(DayOfWeek.Monday))
            {
                return "����һ";
            }
            else if (dfw.Equals(DayOfWeek.Tuesday))
            {
                return "���ڶ�";
            }
            else if (dfw.Equals(DayOfWeek.Wednesday))
            {
                return "������";
            }
            else if (dfw.Equals(DayOfWeek.Thursday))
            {
                return "������";
            }
            else if (dfw.Equals(DayOfWeek.Friday))
            {
                return "������";
            }
            else if (dfw.Equals(DayOfWeek.Saturday))
            {
                return "������";
            }
            else if (dfw.Equals(DayOfWeek.Sunday))
            {
                return "������";
            }
            return string.Empty;
        }
        #endregion

        #region ĳһ�ַ�����
        /// <summary>
        /// ĳһ�ַ�����
        /// </summary>
        /// <param name="iNum">�ַ�����</param>      
        /// <param name="symbol">�ַ�</param>
        /// <returns>�ַ�����ַ���</returns>
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
        /// ��������Ҳಹ�ַ�
        /// </summary>
        /// <param name="info"></param>
        /// <param name="length">ָ���ĳ���</param>
        /// <param name="c">Ҫ�����ַ�</param>
        /// <returns></returns>
        public static string GetPadRightString(string info, int length, char c)
        {
            return info.Trim().PadRight(length, c).Substring(0, length);
        }

        /// <summary>
        /// �Ҷ����������ಹ�ַ�
        /// </summary>
        /// <param name="info"></param>
        /// <param name="length">ָ���ĳ���</param>
        /// <param name="c">Ҫ�����ַ�</param>
        /// <returns></returns>
        public static string GetPadLeftString(string info, int length, char c)
        {
            return info.Trim().PadLeft(length, c).Substring(0, length);
        }

        #endregion

        /// <summary>
        /// �ж��ַ����ĳ��ȣ�������ĸ�����֡�����
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
        /// ��ȡ�����ָ�����������
        /// </summary>
        /// <param name="len">���������</param>
        /// <returns></returns>
        public static int GetRandNum(int len)
        {
            Random rnd = new Random();
            return rnd.Next(0, int.Parse(Utility.GetNumSymbol(len, "9")));
        }

        /// <summary>
        /// ��ȡָ�������޳��ȵ������
        /// </summary>
        /// <param name="min">����(��)</param>
        /// <param name="max">����(��)</param>
        /// <returns></returns>
        public static int GetRandNum(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        #region �������
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="sequenceName">�������</param>
        /// <param name="hasFirstRow">�Ƿ�Ҫ��һ��</param>
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
        /// ������ŷ�ҳ
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="sequenceName">�������</param>
        /// <param name="hasFirstRow">�Ƿ�Ҫ��һ��</param>
        /// <param name="currentPageIndex">��ǰҳ</param>
        /// <param name="pageSize">ҳ��С</param>
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

        #region ����datatable���з������Ӧ��ʵ��
        /// <summary>
        /// ����datatable���з������Ӧ��ʵ��
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static object GetListObj(object s, DataRow dr)
        {
            object obj = null;
            obj = Activator.CreateInstance(s.GetType(), null);//����ָ������ʵ��
            PropertyInfo[] fields = obj.GetType().GetProperties();//��ȡָ����������й�������
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
        /// ����datatable���з������Ӧ��ʵ�弯��List
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
        /// �ַ������ϰ��ַ�����ʽ���أ��м�ָ����ָ�
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

        #region �����
        /// <summary>
        /// �����
        /// </summary>
        /// <param name="cols">���������Ͷ��󼯺�</param>
        /// <returns>��</returns>
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
        /// �����
        /// </summary>
        /// <param name="colsName">�����ַ���,������'|'�ָ�</param>
        /// <param name="colsTypeName">�������ַ���,��������'|'�ָ�</param>
        /// <returns>��</returns>
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

        #region xmlĳ�ڵ㷵��DataTable
        /// <summary>
        /// xml����DataTable
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="xmlFilePath"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXmlNode(List<TableType> cols, string xmlFilePath, string ID)
        {
            ////�����
            DataTable dt = Utility.MakeTable(cols);

            //��ʼ����
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            //�ݹ�ڵ�
            Utility.Due(dt, ele, cols, ID);

            return dt;

        }
        #endregion

        #region �ݹ�xmlĳ�ڵ�
        /// <summary>
        /// �ݹ�xmlĳ�ڵ�
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

                    //�ݹ鿪ʼ
                    if (ele.ChildNodes[i].ChildNodes.Count > 0)
                    {
                        Due(dt, (XmlElement)ele.ChildNodes[i], cols, ID);
                    }
                    break;
                }
            }
        }
        #endregion

        #region xml����DataTable
        /// <summary>
        /// xml����DataTable
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXml(List<TableType> cols, string xmlFilePath)
        {
            ////�����
            DataTable dt = Utility.MakeTable(cols);

            //��ʼ����
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            DataRow dr = dt.NewRow();

            foreach (TableType var in cols)
            {
                dr[var.Cols] = ele.Attributes[var.Cols].Value.ToString();
            }
            dt.Rows.Add(dr);

            //�ݹ�ڵ�
            Utility.Due(dt, ele, cols);

            return dt;
        }

        /// <summary>
        /// xml����DataTable
        /// </summary>
        /// <param name="colNames">|�ָ�</param>
        /// <param name="colTypes">|�ָ�</param>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static DataTable GetTableFromXml(string colNames, string colTypes, string xmlFilePath)
        {
            //�����
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

            //��ʼ����
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFilePath);
            XmlElement ele = xd.DocumentElement;

            DataRow dr = dt.NewRow();

            foreach (TableType var in DepElys)
            {
                dr[var.Cols] = ele.Attributes[var.Cols].Value.ToString();
            }
            dt.Rows.Add(dr);

            //�ݹ�ڵ�
            Utility.Due(dt, ele, DepElys);

            return dt;
        }
        #endregion

        #region �ݹ�xml�ڵ�
        /// <summary>
        /// �ݹ�xml�ڵ�
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

                //�ݹ鿪ʼ
                if (ele.ChildNodes[i].ChildNodes.Count > 0)
                {
                    Due(dt, (XmlElement)ele.ChildNodes[i], cols);
                }
            }
        }
        #endregion

        #region xmlĳ�ڵ����
        /// <summary>
        /// ��ȡĳ�ڵ�ֵ
        /// </summary>
        /// <param name="xmlFilePath">xml�ļ�·��</param>
        /// <param name="idNodeAttrName">id�ڵ�������</param>
        /// <param name="idNodeAttrValue">id�ڵ�����ֵ</param>       
        /// <param name="curNodeAttrName">�����ڵ�������</param>
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
        /// ����ĳ�ڵ�ֵ
        /// </summary>
        /// <param name="xmlFilePath">xml�ļ�·��</param>
        /// <param name="idNodeAttrName">id�ڵ�������</param>
        /// <param name="idNodeAttrValue">id�ڵ�����ֵ</param>
        /// <param name="curNodeAttrName">�����ڵ�������</param>
        /// <param name="curNodeAttrValue">�����ڵ�����ֵ</param>
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
        /// ����ĳ�ڵ�ֵ��ÿ��ڵ����ͽڵ��Ӧ����ֵ�����Ӧ
        /// </summary>
        /// <param name="xmlFilePath">xml�ļ�·��</param>
        /// <param name="idNodeAttrNames">id�ڵ�������,����ã��ŷָ�</param>
        /// <param name="idNodeAttrValues">id�ڵ�����ֵ,����ã��ŷָ�</param>
        /// <param name="curNodeAttrNames">�����ڵ�������,����ã��ŷָ�</param>
        /// <param name="curNodeAttrValues">�����ڵ�����ֵ,����ã��ŷָ�</param>
        /// <param name="splitChar">�ָ����</param>
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
        /// ���ҵ�ǰҪ������xml�ڵ㣬����ID
        /// </summary>
        /// <param name="eleNode">��ǰ�ڵ�</param>
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
        /// xml���ӽڵ�
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

        #region DataTable����xml
        /// <summary>
        /// DataTable����xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootText"></param>
        /// <param name="startText"></param>
        /// <param name="tcn">�����������������󼯺�</param>
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

                return "�ѳɹ�����xml�ļ���";
            }
            catch (Exception ex)
            {
                return "����xml�ļ�ʧ�ܣ�" + ex.ToString();
            }

        }
        #endregion

        #region ������ݴ���
        /// <summary>
        /// ����ĳ��ֵ��ȡ����
        /// </summary>
        /// <param name="dtTemp">ԭʼ��</param>
        /// <param name="objName">��������</param>
        /// <param name="objValue">������ֵ</param>
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
        /// ������������
        /// </summary>
        /// <param name="sortColumns">�����ֶ�,����ֶ���,�ָ�</param>
        /// <param name="dtSoc">������</param>
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

        #region xml��DataSet
        /// <summary>
        /// ��xml���������ַ���ת��ΪDataSet
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
                //��streamװ�ص�XmlTextReader
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
        /// ��xml�ļ�ת��ΪDataSet
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
                //��streamװ�ص�XmlTextReader
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
        /// ��DataSetת��Ϊxml�����ַ���
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
                //��streamװ�ص�XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //��WriteXml����д���ļ�.
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
        /// ��DataSetת��Ϊxml�ļ�
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
                //��streamװ�ص�XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //��WriteXml����д���ļ�.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //����Unicode������ı�
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

        #region �ֽ������Image
        /// <summary>
        /// ���ֽ�����ת����Image
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
        /// ��Image����ת����byte[]����
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

        #region "�ж��Ƿ�������"
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
        #region 10���������㷵�ַ���
        /// <summary>
        /// 10���������㷵�ַ���
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
        /// ����ǰ����
        /// </summary>
        /// <param name="old"></param>
        /// <param name="strLength">���ַ�����</param>
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

        #region Ա����ټ���

        /// <summary>
        /// ����������������Сʱ����һ�찴8Сʱ����
        /// </summary>
        /// <param name="takePartInWorkDate">�μӹ�������(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">����˾����</param>        
        /// <param name="yearRule">��ټ������(1|2|3)Ĭ��ѡ��1��������׼</param>
        /// <param name="hasOneYear">�Ƿ�Ҫ����һ������������(��|��)Ĭ�Ϸ�</param>
        /// <returns></returns>
        public static int CaluYearVacationHasDays(string takePartInWorkDate, string fromCompDate, string yearRule, string hasOneYear)
        {
            int baseCnt = Utility.CaluYearVacationBaseCount(takePartInWorkDate, fromCompDate, yearRule); //��ٻ���     
       
            double fromCompYear = 0; //����˾����    
            TimeSpan ts = DateTime.Now.Date - Convert.ToDateTime(fromCompDate);
            fromCompYear = (double)ts.Days / 365;

            double hasDays = 0; //�����������ݼ�����

            if (hasOneYear.Equals("��"))
            {
                if (fromCompYear >= 1)
                {
                    //���������                
                    if (fromCompYear < 2)
                    {
                        //�ݵ���ʣ��������Ӧ�����
                        TimeSpan ts1 = Convert.ToDateTime(Convert.ToDateTime(fromCompDate).Year.ToString() + "-12-31") - Convert.ToDateTime(fromCompDate);
                        hasDays = Math.Round((double)ts1.Days / 365 * baseCnt);
                    }
                    else
                    {
                        //�������
                        hasDays = baseCnt;
                    }                   
                }
                else
                {
                    hasDays = 0;  //�����������
                }
            }
            else
            {
                //���������                
                if (fromCompYear < 1)
                {
                    //�ݵ���ʣ��������Ӧ�����
                    TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(fromCompDate).Year.ToString() + "-12-31") - Convert.ToDateTime(fromCompDate);
                    hasDays = Math.Round((double)ts2.Days / 365 * baseCnt);
                }
                else
                {
                    //�������
                    hasDays = baseCnt;
                }
            }

            return (int)hasDays * 8;
        }

        /// <summary>
        /// ������ٻ���
        /// </summary>
        /// <param name="takePartInWorkDate">�μӹ�������(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">����˾����</param> 
        /// <param name="yearRule">��ٹ���</param>
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
        /// ������ٻ���������1���������ݼ�
        /// ����һ������(��һ��)ʮ������(��ʮ��)�����ݼ�5��
        /// ����ʮ�����϶�ʮ������(����ʮ��)�����ݼ�10��
        /// �����ʮ�����ϣ����ݼ�15��
        /// </summary>
        /// <param name="takePartInWorkDate">�μӹ�������(yyyy-MM-dd)</param>
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
        /// ������ٻ���������2(����1���������ݼ� + ��˾������)
        /// �����ٹ���
        /// ����˾���������ϵ�Ա������������֮����ʼ����1��Ĺ�˾������
        /// ÿ����һ������1�죬��߲�����5��
        /// </summary>
        /// <param name="takePartInWorkDate">�μӹ�������(yyyy-MM-dd)</param>
        /// <param name="fromCompDate">����˾����</param> 
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
        /// ������ٻ���������3
        /// ��������(������)�����ݼ�5��
        /// �����꣬ÿ�����һ�죬��಻����15��
        /// </summary>
        /// <param name="takePartInWorkDate">�μӹ�������(yyyy-MM-dd)</param>        
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
       
        #region �����ദ��
        /// <summary>
        /// ��ȡList�ַ�������
        /// </summary>
        /// <param name="values">����ַ�����|�ָ�</param>
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
        /// ���ַ���List��ת����д��List
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

        #region ʵ�����datatable��datarow�໥��ֵ

        /// <summary>
        /// ��datable��row��ȡ����(Ҫ�����������ٰ���Ŀ������ȫ�����ԣ�ͬʱҪ��������������һ��)
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
        /// ��ʵ�����ȡdatatable�����datarow(Ҫ�����������ٰ���Ŀ������ȫ�����ԣ�ͬʱҪ��������������һ��)
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

        #region JSON��ش���

        /// <summary>
        /// datatableת��Ϊjson����
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

        #region ȫ���ת��

        /// <summary>
        /// ת���
        /// </summary>
        public static string ToDBC(string input)
        {
            if (input == null) return null;

            //ȫ��ת��ǣ�
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
        /// תȫ��
        /// </summary>
        public static string ToSBC(string input)
        {
            if (input == null) return null;

            //���תȫ�ǣ�
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

        #region ��¼��־

        /// <summary>
        /// ��¼��־
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

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
                {
                    sw.WriteLine(logContent);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //�����쳣������ȱ�����һ����Ƶ��¼��־
            }
        }

        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        /// <param name="isExecute">Y ��¼��־  N �����м�¼</param>
        public static void RecordLog(string logContent, string logPath, string isExecute)
        {
            if(isExecute.ToUpper() == "Y")
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                try
                {
                    string filepath = logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                    using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
                    {
                        sw.WriteLine(logContent);
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    //�����쳣������ȱ�����һ����Ƶ��¼��־
                }
            }            
        }

        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="logPath"></param>
        /// <param name="isExecute">Y ��¼��־  N �����м�¼</param>
        /// <param name="fileName">�ļ�����ֻ֧��Ӣ��</param>
        public static void RecordLog(string logContent, string logPath, string isExecute, string fileName)
        {
            if (isExecute.ToUpper() == "Y")
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                try
                {
                    string filepath = logPath + "\\" + fileName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                    using (System.IO.StreamWriter sw = new StreamWriter(filepath, true))
                    {
                        sw.WriteLine(logContent);
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    //�����쳣������ȱ�����һ����Ƶ��¼��־
                }
            }
        }

        #endregion

        #region ResolveUrl(�������Url)
        /// <summary>
        /// �������Url
        /// </summary>
        /// <param name="relativeUrl">���Url</param>
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

        #region HtmlEncode(��html�ַ������б���)
        /// <summary>
        /// ��html�ַ������б���
        /// </summary>
        /// <param name="html">html�ַ���</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }
        /// <summary>
        /// ��html�ַ������н���
        /// </summary>
        /// <param name="html">html�ַ���</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(��Url���б���)

        /// <summary>
        /// ��Url���б���
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">�����ַ��Ƿ�ת�ɴ�д,����,"http://"ת��"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// ��Url���б���
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">�ַ�����</param>
        /// <param name="isUpper">�����ַ��Ƿ�ת�ɴ�д,����,"http://"ת��"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// ��ȡ��д�����ַ���
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

        #region UrlDecode(��Url���н���)

        /// <summary>
        /// ��Url���н���,����javascript��encodeURIComponent�����������,Ӧʹ��utf-8�ַ�����������
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// ��Url���н���,����javascript��encodeURIComponent�����������,Ӧʹ��utf-8�ַ�����������
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">�ַ�����,����javascript��encodeURIComponent�����������,Ӧʹ��utf-8�ַ�����������</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #region Session����
        /// <summary>
        /// дSession
        /// </summary>
        /// <typeparam name="T">Session��ֵ������</typeparam>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                return;
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// дSession
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        /// <param name="value">Session�ļ�ֵ</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// ��ȡSession��ֵ
        /// </summary>
        /// <param name="key">Session�ļ���</param>        
        public static string GetSession(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            return HttpContext.Current.Session[key] as string;
        }
        /// <summary>
        /// ɾ��ָ��Session
        /// </summary>
        /// <param name="key">Session�ļ���</param>
        public static void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            HttpContext.Current.Session.Contents.Remove(key);
        }

        #endregion

        #region Cookie����
        /// <summary>
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
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
        /// дcookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <param name="strValue">ֵ</param>
        /// <param name="strValue">����ʱ��(����)</param>
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
        /// ��cookieֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>cookieֵ</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }
        /// <summary>
        /// ɾ��Cookie����
        /// </summary>
        /// <param name="CookiesName">Cookie��������</param>
        public static void RemoveCookie(string CookiesName)
        {
            HttpCookie objCookie = new HttpCookie(CookiesName.Trim());
            objCookie.Expires = DateTime.Now.AddYears(-5);
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }
        #endregion

        #region GetFileControls(��ȡ�ͻ����ļ��ؼ�����)

        /// <summary>
        /// ��ȡ��Ч�ͻ����ļ��ؼ�����,�ļ��ؼ������ϴ������ݣ�Ϊ�ս�������,
        /// ע��:Form��Ǳ���������� enctype="multipart/form-data",�������˲��ܻ�ȡ�ͻ���file�ؼ�.
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

        #region GetFileControl(��ȡ��һ����Ч�ͻ����ļ��ؼ�)

        /// <summary>
        /// ��ȡ��һ����Ч�ͻ����ļ��ؼ�,�ļ��ؼ������ϴ������ݣ�Ϊ�ս�������,
        /// ע��:Form��Ǳ���������� enctype="multipart/form-data",�������˲��ܻ�ȡ�ͻ���file�ؼ�.
        /// </summary>
        public static HttpPostedFile GetFileControl()
        {
            var files = GetFileControls();
            if (files == null || files.Count == 0)
                return null;
            return files[0];
        }

        #endregion

        #region HttpWebRequest(����������Դ)

        /// <summary>
        /// ��������
        /// </summary>
        public enum WebRequestType
        {
            /// <summary>
            /// Http����
            /// </summary>
            Http = 0,

            /// <summary>
            /// wcf��������
            /// </summary>
            Wcf = 1,

            /// <summary>
            /// webServies����
            /// </summary>
            WebService = 2
        }

        /// <summary>
        /// ����url�Ƿ����
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
                        errStr = "��ӦΪnull";
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
                        errStr = "�����쳣��ԭ��" + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    errStr = "�����쳣��ԭ��" + ex.Message;
                }
            }
            else if (requestType == WebRequestType.Wcf)
            {
                //Ԥ������
            }
            else if (requestType == WebRequestType.WebService)
            {
                //Ԥ������
            }

            return success;
        }    

        /// <summary>
        /// ����������Դ,������Ӧ���ı�
        /// </summary>
        /// <param name="url">������Դ��ַ</param>
        public static string HttpWebRequestOp(string url)
        {
            return HttpWebRequestOp(url, string.Empty, Encoding.GetEncoding("utf-8"));
        }

        /// <summary>
        /// ����������Դ,������Ӧ���ı�
        /// </summary>
        /// <param name="url">������ԴUrl��ַ</param>
        /// <param name="parameters">�ύ�Ĳ���,��ʽ������1=����ֵ1&amp;����2=����ֵ2</param>
        public static string HttpWebRequestOp(string url, string parameters)
        {
            return HttpWebRequestOp(url, parameters, Encoding.GetEncoding("utf-8"), true);
        }

        /// <summary>
        /// ����������Դ,������Ӧ���ı�
        /// </summary>
        /// <param name="url">������Դ��ַ</param>
        /// <param name="parameters">�ύ�Ĳ���,��ʽ������1=����ֵ1&amp;����2=����ֵ2</param>
        /// <param name="encoding">�ַ�����</param>
        /// <param name="isPost">�Ƿ�Post�ύ</param>
        /// <param name="contentType">��������</param>
        /// <param name="cookie">Cookie����</param>
        /// <param name="timeout">��ʱʱ��</param>
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


        /// <summary>
        /// ����http�����ȡ���ؽ��
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqMethord"></param>
        /// <param name="contentType"></param>
        /// <param name="reqdatas"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string RequestHttp(string url, string reqMethord, string contentType, string reqdatas, string token)
        {
            string resstr = @"";

            #region ԭ��̬��ʽ

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.ContentType = contentType;
            request.Method = reqMethord.ToUpper();
            request.Timeout = 60000;
            request.ReadWriteTimeout = request.Timeout;
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("token", token);
            }

            if (reqMethord.ToUpper() == "POST")
            {
                byte[] sendData = Encoding.UTF8.GetBytes(reqdatas);
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

            #region webclient��ʽ

            //using (System.Net.WebClient wc = new System.Net.WebClient())
            //{
            //    wc.Headers.Add("Content-Type", contentType);
            //    if (!string.IsNullOrEmpty(token))
            //    {
            //        wc.Headers.Add("token", token);
            //    }                
            //    if (reqMethord.ToUpper() == "POST")
            //    {
            //        byte[] sendData = Encoding.UTF8.GetBytes(reqdatas);
            //        wc.Headers.Add("ContentLength", sendData.Length.ToString());
            //        byte[] recData = wc.UploadData(url, reqMethord.ToUpper(), sendData);
            //        resstr = Encoding.UTF8.GetString(recData).TrimStart("\u005C\u0022".ToCharArray()).TrimEnd("\u005C\u0022".ToCharArray()).Replace("\\", ""); ;
            //    }
            //    else
            //    {
            //        resstr = wc.DownloadString(url);
            //    }
            //}

            #endregion

            #region HttpClient��ʽ

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            //    var content = new StringContent(reqdatas, Encoding.UTF8);
            //    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            //    var tmpResult = client.PostAsync(url, content).Result;
            //    tmpResult.EnsureSuccessStatusCode();
            //    resstr = tmpResult.Content.ReadAsStringAsync().Result;
            //}

            #endregion

            return Utility.FormatReqDatas(Utility.FormatRespDatas(resstr));
        }

        #endregion


        #region �ַ������ת������

        /// <summary>
        /// ��ʽ�����л����
        /// </summary>
        /// <param name="oldstr"></param>
        /// <returns></returns>
        public static string FormatReqDatas(string oldstr)
        {
            return oldstr.Replace("\"'", "'").Replace("'\"", "'");
        }

        /// <summary>
        /// ��ʽ�����л����
        /// </summary>
        /// <param name="oldstr"></param>
        /// <returns></returns>
        public static string FormatRespDatas(string oldstr)
        {
            return oldstr.Replace("u0027", "'");
        }

        /// <summary>
        /// ���ַ���ת��Ϊ�ַ�������,��λת��,|�ָ�
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

        /// <summary>
        /// ���ֺ��ֽ�֮�以ת
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int IntToBitConverter(int num)
        {
            int temp = 0;
            byte[] bytes = BitConverter.GetBytes(num);//��int32ת��Ϊ�ֽ�����
            temp = BitConverter.ToInt32(bytes, 0);//���ֽ�����������ת��int32����
            return temp;
        }

        /// <summary>
        /// ���ַ���תΪ16�����ַ�����������
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//����ָ�����뽫string����ֽ�����
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//���ֽڱ�Ϊ16�����ַ�
            {
                result += Convert.ToString(b[i], 16) + " ";
            }
            return result;
        }
        /// <summary>
        /// ��16�����ַ���תΪ�ַ���
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
            //����ָ�����뽫�ֽ������Ϊ�ַ���
            return encode.GetString(b);
        }
        /// <summary>
        /// byte[]תΪ16�����ַ���
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
        /// ʮ�����ַ���תΪ16�����ַ���
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string IntStrToHexStr(string bytes)
        {
            return int.Parse(bytes).ToString("X2");
        }

        /// <summary>
        /// ��16���Ƶ��ַ���תΪbyte[]
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
        /// ����ASCIIת�ַ���
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
        /// �����ַ����ַ���תASCII
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
        /// �����ַ����ַ���תASCII�ַ���
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
        /// ������ַ����ַ���ת��Ϊascii���ַ���
        /// </summary>
        /// <param name="tmpstr"></param>
        /// <returns></returns>
        public static string Strs2AsciiStr(string tmpstr)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(tmpstr);  //����arrayΪ��Ӧ��ASCII����   

            string ASCIIstr2 = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                int asciicode = (int)(array[i]);
                ASCIIstr2 += Convert.ToString(asciicode);//�ַ���ASCIIstr2 Ϊ��Ӧ��ASCII�ַ���
            }

            return ASCIIstr2;
        }

        /// <summary>
        /// ������ַ���ascii���ַ���ת��Ϊ�ַ���
        /// </summary>
        /// <param name="asciistr"></param>
        /// <returns></returns>
        public static string AsciiStr2Strs(string asciistr)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(asciistr);  //����arrayΪ��Ӧ��ASCII����   
            string resStr = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                char asciiChar = (char)(array[i]);
                resStr += Convert.ToString(asciiChar);//�ַ���ASCIIstr2 Ϊ��Ӧ��ASCII�ַ���
            }
            return resStr;
        }

        /// <summary>
        /// ������ַ���ascii���ַ���ת��Ϊbyte[]
        /// </summary>
        /// <param name="asciistr"></param>
        /// <returns></returns>
        public static byte[] AsciiStr2ByteArray(string asciistr)
        {
            return System.Text.Encoding.ASCII.GetBytes(asciistr);  //����arrayΪ��Ӧ��ASCII����   
        }

        /// <summary>
        /// 16���Ƶ�ASCII��ת�ַ���
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
        /// �ַ���ת16���Ƶ�ASCII��
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


        //bootom
    }


    #endregion

    #region ��ȿ�¡

    /// <summary>
    /// ��չ��¡
    /// </summary>
    public static class DeepCloneExtends
    {
        public static T DeepCloneObject<T>(this T t) where T : class
        {
            T model = System.Activator.CreateInstance<T>();                     //ʵ����һ��T���Ͷ���
            PropertyInfo[] propertyInfos = model.GetType().GetProperties();     //��ȡT��������й�������
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //�ж�ֵ�Ƿ�Ϊ�գ�����ո�ֵΪnull��else
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    //���convertsionTypeΪnullable�࣬����һ��NullableConverter�࣬�����ṩ��Nullable�ൽ������Ԫ���͵�ת��
                    NullableConverter nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                    //��convertsionTypeת��Ϊnullable�ԵĻ�����Ԫ����
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
                T model = System.Activator.CreateInstance<T>();                     //ʵ����һ��T���Ͷ���
                PropertyInfo[] propertyInfos = model.GetType().GetProperties();     //��ȡT��������й�������
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    //�ж�ֵ�Ƿ�Ϊ�գ�����ո�ֵΪnull��else
                    if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        //���convertsionTypeΪnullable�࣬����һ��NullableConverter�࣬�����ṩ��Nullable�ൽ������Ԫ���͵�ת��
                        NullableConverter nullableConverter = new NullableConverter(propertyInfo.PropertyType);
                        //��convertsionTypeת��Ϊnullable�ԵĻ�����Ԫ����
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Common
{
    
    /// <summary>
    /// 公共泛型类，处理一些公共的泛型转换及处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class UtilitysForT<T>
    {
        #region 数据表和List集合相互转换处理
        /// <summary>
        /// 根据datatable的列返回其对应的实体集合List(泛型版本)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> GetListsObj(DataTable dt)
        {
            List<T> lists = new List<T>();
            T udtemp;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                udtemp = Common.UtilitysForT<T>.GetListObj(dt.Rows[i], i);   
                lists.Add(udtemp);
            }

            return lists;
        }

        /// <summary>
        /// 根据datatable的列返回其对应的实体(泛型版本)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="indexRow"></param>
        /// <returns></returns>
        public static T GetListObj(DataRow dr, int indexRow)
        {
            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例

            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性
            foreach (DataColumn dc in dr.Table.Columns)
            {
                foreach (PropertyInfo t in fields)
                {
                    if (dc.ColumnName.ToUpper() == t.Name.ToUpper())
                    {
                        t.SetValue(obj, dr[dc.ColumnName].ToString(), null);
                        break;
                    }
                }
            }

            //处理序号
            foreach (PropertyInfo t in fields)
            {
                if (t.Name.ToUpper() == "SequenceXXX".ToUpper())
                {
                    t.SetValue(obj, (indexRow + 1).ToString(), null);
                    break;
                }
            }
                
            return (T)obj;
        }

        /// <summary>
        /// 根据泛型T获取DataRow
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataRow GetDataRowFromT(T t, DataTable dt)
        {
            DataRow dr = dt.NewRow();

            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例
            obj = t;

            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性
            
            foreach (PropertyInfo ts in fields)
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    if (dc.ColumnName.ToUpper() == ts.Name.ToUpper())
                    {
                        dr[dc.ColumnName] = ts.GetValue(obj, null) == null ? "" : ts.GetValue(obj, null).ToString();
                        break;
                    }
                }               
            }
            
            return dr;
        }

        /// <summary>
        /// 根据list装载的类型返回DataTable结构(泛型版本)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataTableEmptyT()
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例

            PropertyInfo[] fields = obj.GetType().GetProperties();//获取指定对象的所有公共属性
            
            //反射泛型类型创建表
            foreach (PropertyInfo t in fields)
            {
                dc = new DataColumn(t.Name);
                dt.Columns.Add(dc);
            }

            return dt;
        }

        /// <summary>
        /// 根据list装载的类型集合返回DataTable数据(泛型版本)
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromListT(List<T> lists)
        {
            DataTable dt = UtilitysForT<T>.GetDataTableEmptyT();
            DataRow dr = null;

            foreach (T t in lists)
            {
                dr = UtilitysForT<T>.GetDataRowFromT(t, dt);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #region 泛型List相关处理

        /// <summary>
        /// 获取实体全部操作列
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<string> GetAllColumns(T t)
        {
            List<string> allcolumns = new List<string>();
            object obj = UtilitysForT<T>.GetObj(t);   
            PropertyInfo[] fields = obj.GetType().GetProperties();

            foreach (PropertyInfo ts in fields)
            {
                allcolumns.Add(ts.Name.ToUpper());
            }

            return allcolumns;
        }

        /// <summary>
        /// 对比两个泛型对象属性是否都相等
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="tar"></param>
        /// <returns></returns>
        public static bool CompareObjectValue(object soc, object tar)
        {
            bool matchSuccess = true;
            PropertyInfo[] fields = soc.GetType().GetProperties();

            foreach (PropertyInfo t in fields)
            {
                foreach (PropertyInfo t2 in fields)
                {
                    if (t.Name.ToUpper() == t2.Name.ToUpper())
                    {
                        matchSuccess = matchSuccess & ((t.GetValue(soc, null) == null ? "" : t.GetValue(soc, null).ToString()) == 
                            (t2.GetValue(tar, null) == null ? "" : t2.GetValue(tar, null).ToString()));
                        break;
                    }
                }                 
            }

            return matchSuccess;
        }        

        /// <summary>
        /// 对比两个泛型，处理操作标志位 增I 改M 删D
        /// </summary>
        /// <param name="soc">当前目标</param>
        /// <param name="oldSoc">历史目标</param>
        /// <param name="mainProperty">主键属性名</param>
        /// <returns></returns>
        public static List<T> DueWithListT(List<T> soc, List<T> oldSoc, List<string> mainProperty)
        {
            List<T> reslist = new List<T>();
            object obj1 = null;
            object obj2 = null;
            PropertyInfo[] fields = UtilitysForT<T>.GetObjPropertyInfo(soc[0]);//获取指定对象的所有公共属性
            string mainValue = ""; //主键值
            string mainValueOld = ""; //历史目标主键值
            bool matchSuccess = false; //匹配成功

            //正向比较，处理I M
            foreach (var item1 in soc)
            {
                matchSuccess = false;
                obj1 = UtilitysForT<T>.GetObj(item1);
                mainValue = UtilitysForT<T>.GetMainPropertyValue(item1, mainProperty);               

                //查找历史目录主键
                foreach (var item2 in oldSoc)
                {
                    obj2 = UtilitysForT<T>.GetObj(item2);
                    mainValueOld = UtilitysForT<T>.GetMainPropertyValue(item2, mainProperty);

                    if(mainValue == mainValueOld)
                    {                        
                        matchSuccess = true;
                        break;
                    }   
                }

                if (matchSuccess)
                {
                    //匹配上, 再比较是否有变化，有是修改，没有忽略
                    if(!UtilitysForT<T>.CompareObjectValue(obj1, obj2))
                    {                    
                        UtilitysForT<T>.SetObjOpFlag(item1, "M");
                        reslist.Add(item1);
                    }
                }
                else
                {
                    //没有匹配上，是新增
                    UtilitysForT<T>.SetObjOpFlag(item1, "I");
                    reslist.Add(item1);
                }
            }

            //反向比较，处理D
            foreach (var item1 in oldSoc)
            {
                matchSuccess = false;
                obj1 = UtilitysForT<T>.GetObj(item1);
                mainValue = UtilitysForT<T>.GetMainPropertyValue(item1, mainProperty);

                //查找历史目录主键
                foreach (var item2 in soc)
                {
                    obj2 = UtilitysForT<T>.GetObj(item2);
                    mainValueOld = UtilitysForT<T>.GetMainPropertyValue(item2, mainProperty);

                    if (mainValue == mainValueOld)
                    {
                        matchSuccess = true;
                        break;
                    }
                }

                if (matchSuccess)
                {
                    //匹配上, 上次已经处理，这次忽略                   
                }
                else
                {
                    //没有匹配上，是删除
                    UtilitysForT<T>.SetObjOpFlag(item1, "D");
                    reslist.Add(item1);
                }
            }

            return reslist;
        }

        /// <summary>
        /// 获取泛型对象实例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object GetObj(T t)
        {
            object obj;
            Type type = typeof(T);
            obj = Activator.CreateInstance(type, null);//创建指定泛型类型实例
            obj = t;

            return obj;
        }

        /// <summary>
        /// 设置实体操作位标识
        /// </summary>
        /// <param name="t"></param>
        /// <param name="opflag">增I 改M 删除D</param>
        public static void SetObjOpFlag(T t, string opflag)
        {
            PropertyInfo[] fields = UtilitysForT<T>.GetObjPropertyInfo(t);//获取指定对象的所有公共属性
            object obj = UtilitysForT<T>.GetObj(t);

            foreach (PropertyInfo ts in fields)
            {
                if (ts.Name.ToUpper() == "OpFlag".ToUpper())
                {
                    ts.SetValue(obj, opflag.ToUpper(), null);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取泛型集合主键对应的值(多个主键时，直接拼接在一起)
        /// </summary>
        /// <param name="soc"></param>
        /// <param name="mainProperty"></param>
        /// <returns></returns>
        public static string GetMainPropertyValue(T soc, List<string> mainProperty)
        {
            PropertyInfo[] fields = UtilitysForT<T>.GetObjPropertyInfo(soc);//获取指定对象的所有公共属性
            string mainValue = "";
            object obj = UtilitysForT<T>.GetObj(soc);

            //获取主键值
            foreach (var item in mainProperty)
            {
                foreach (PropertyInfo t in fields)
                {
                    if (t.Name.ToUpper() == item.ToUpper())
                    {
                        mainValue = mainValue + (t.GetValue(obj, null) == null ? "" : t.GetValue(obj, null).ToString());
                        break;
                    }
                }
            }

            return mainValue;
        }

        /// <summary>
        /// 获取对象属性数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetObjPropertyInfo(T t)
        {
            object obj = UtilitysForT<T>.GetObj(t);
            return obj.GetType().GetProperties();//获取指定对象的所有公共属性
        }

        /// <summary>
        /// 通过序列化方式克隆一个新对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> CloneForSerialize(List<T> t)
        {
            //序列化
            string opLists = string.Empty;
            bool flags = Common.JsonSerializer.Serialize(t, out opLists);

            //反序列化
            List<T> temps = new List<T>();
            bool tempFlag = Common.JsonSerializer.Deserialize<List<T>>(opLists, out temps);

            return temps;
        }

        

        ///// <summary>
        ///// 泛型查找List内容 TODO 有空再处理
        ///// </summary>
        ///// <param name="soc"></param>
        ///// <returns></returns>
        //public static List<T> Find(List<T> soc, string )
        //{
        //    List<T> tar = new List<T>();
        //    tar = soc.FindAll(delegate (T t)
        //    {
        //        PropertyInfo[] fields = UtilitysForT<T>.GetObjPropertyInfo(t);//获取指定对象的所有公共属性
        //        string mainValue = "";
        //        object obj = UtilitysForT<T>.GetObj(soc);

        //        //获取主键值
        //        foreach (var item in mainProperty)
        //        {
        //            foreach (PropertyInfo t in fields)
        //            {
        //                if (t.Name.ToUpper() == item.ToUpper())
        //                {
        //                    mainValue = mainValue + t.GetValue(obj, null).ToString();
        //                    break;
        //                }
        //            }
        //        }

        //        return t.DetailType == "BaseScele";
        //    });

        //    //lstBaseScele = srdmp.FindAll(delegate (SaleRuleDetailModel srdms) { return srdms.DetailType == "BaseScele"; });

        //    return null;
        //}



        #endregion

        #region "判断List是否有数据"

        /// <summary>
        /// 判断list是否有数据
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static bool ListHasData(List<T> lists)
        {
            if (lists == null)
            {
                return false;
            }
            else
            {
                if(lists.Count <= 0)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

    }
}

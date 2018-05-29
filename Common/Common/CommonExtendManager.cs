using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 公共类扩展管理
    /// </summary>
    public static  class CommonExtendManager
    {
        #region 字典相关操作扩展

        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false)
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddOrPeplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="values"></param>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }

        #endregion

        #region 进制转换相关扩展

        /// <summary>
        /// 二进制转十六进制
        /// </summary>
        public static string ToHex(this byte[] bytes)
        {
            var result = "";
            for (var index = 0; index < bytes.Length; index++)
                result += bytes[index].ToString("X2");
            return result;
        }
        /// <summary>
        /// 十六进制转二进制
        /// </summary>
        public static byte[] ToByte(this string hexs)
        {
            var result = new List<byte>();
            for (var index = 0; index < hexs.Length; index += 2)
                result.Add(System.Convert.ToByte(hexs.Substring(index, 2), 16));
            return result.ToArray();
        }

        #endregion        

        #region 枚举相关操作

        /// <summary>  
        /// 获取枚举变量值的 Description 属性  
        /// </summary>  
        /// <param name="obj">枚举变量</param>  
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>  
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>  
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            try
            {
                Type _enumType = obj.GetType();
                System.ComponentModel.DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(System.ComponentModel.DescriptionAttribute));
                }
                else
                {
                    System.Reflection.FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(System.ComponentModel.DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
                return string.Empty;
            }
            return obj.ToString();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class RegExpress
    {
        /// <summary>
        /// 字母和数字
        /// </summary>
        public const string charactersAndNumber = "^[0-9a-zA-Z]";

        /// <summary>
        /// 货币格式
        /// </summary>
        public const string currencys = "^-?\\d+(.\\d{2})?$";
        /// <summary>
        /// 数字
        /// </summary>
        public const string numbers = "^[0-9]";

        /// <summary>
        /// 字母
        /// </summary>
        public const string characters = "^[a-zA-Z]";

        /// <summary>
        /// email格式
        /// </summary>
        public const string emailReg = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        /// <summary>
        /// 网页地址格式
        /// </summary>
        public const string homePage = "http://([\\w-]+\\.)+[\\w-]+(/[\\w-./?%&=]*)?";

        /// <summary>
        /// 任意字符
        /// </summary>
        public const string allCharacter = "^[0-9a-zA-Z\u4e00-\u9fa5]";

        /// <summary>
        /// 日期(yyyy_MM_dd)
        /// </summary>
        public const string yyyy_MM_dd = "^\\d{4}-\\d{2}-\\d{2}$";

        /// <summary>
        /// 正数
        /// </summary>
        public const string zNum = "(^\\+?|^\\d?)\\d*\\.?\\d+$";
 
        /// <summary>
        /// 负数
        /// </summary>
        public const string negative = "^-\\d*\\.?\\d+$";


        /// <summary>
        /// 整数
        /// </summary>
        public const string integer = "(^-?|^\\+?|\\d)\\d+$";
        
        /// <summary>
        /// 浮点数
        /// </summary>
        public const string floats = "(^-?|^\\+?|^\\d?)\\d*\\.\\d+$";
        
    }
}

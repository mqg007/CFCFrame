using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

using Entitys.ComonEnti;

namespace FrameCommon
{
    public class SysEnvironment
    {
        /// <summary>
        /// 业务节点地址
        /// </summary>
        public static string  BizNodeAddr { get; set; }

        /// <summary>
        /// 系统登录用户
        /// </summary>
        public static SSY_USER_DICT SysUserDict { get; set; }

        /// <summary>
        /// 分布式管理参数
        /// </summary>
        public static DistributeDataNodeManagerParams distManagerParam { get; set; }        

        /// <summary>
        /// 访问IP
        /// </summary>
        public static string Ips { get; set; }

        /// <summary>
        /// 令牌加密算法
        /// </summary>
        public static string TokenEncrpType { get; set; }

        /// <summary>
        /// 令牌加密公钥
        /// </summary>
        public static string TokenEncrpPublicKey { get; set; }

        /// <summary>
        /// 令牌密文
        /// </summary>
        public static string TokenEncrpValue { get; set; }               

        /// <summary>
        /// 环境序列化结果
        /// </summary>
        public static string EnvirmentStr { get; set; }

        /// <summary>
        /// 当前语言
        /// </summary>
        public static string I18nCurrLang { get; set; }
    }

    /// <summary>
    /// 用于序列化到服务
    /// </summary>
    [DataContract]
    public class SysEnvironmentSerialize
    {
        /// <summary>
        /// 业务节点地址
        /// </summary>
        [DataMember]
        public string BizNodeAddr { get; set; }

        /// <summary>
        /// 系统登录用户
        /// </summary>
        [DataMember]
        public SSY_USER_DICT SysUserDict { get; set; }

        /// <summary>
        /// 分布式管理参数
        /// </summary>
        [DataMember]
        public DistributeDataNodeManagerParams distManagerParam { get; set; }        

        /// <summary>
        /// 访问IP
        /// </summary>
        [DataMember]
        public string Ips { get; set; }

        /// <summary>
        /// 令牌加密算法
        /// </summary>
        [DataMember]
        public string TokenEncrpType { get; set; }

        /// <summary>
        /// 令牌加密公钥
        /// </summary>
        [DataMember]
        public string TokenEncrpPublicKey { get; set; }

        /// <summary>
        /// 令牌密文
        /// </summary>
        [DataMember]
        public string TokenEncrpValue { get; set; }

        /// <summary>
        /// 当前语言
        /// </summary>
        [DataMember]
        public string I18nCurrLang { get; set; }
    }

    /// <summary>
    /// 管理系统环境变量
    /// </summary>
    public class ManagerSysEnvironment
    {
        /// <summary>
        /// 初始化空的环境变量序列实例化
        /// </summary>
        /// <returns></returns>
        public static SysEnvironmentSerialize GetSysEnvironmentSerializeEmpty()
        {
            SysEnvironmentSerialize senvir = new SysEnvironmentSerialize();
            senvir.BizNodeAddr = string.Empty;
            senvir.Ips = string.Empty;
            senvir.SysUserDict = new SSY_USER_DICT();
            senvir.TokenEncrpPublicKey = string.Empty;
            senvir.TokenEncrpType = string.Empty;
            senvir.TokenEncrpValue = string.Empty;
            senvir.distManagerParam = new DistributeDataNodeManagerParams();
            senvir.I18nCurrLang = string.Empty;

            return senvir;
        }

        /// <summary>
        /// 环境变量序列实例化
        /// </summary>
        /// <returns></returns>
        public static SysEnvironmentSerialize GetSysEnvironmentSerialize()
        {
            SysEnvironmentSerialize senvir = new SysEnvironmentSerialize();
            senvir.BizNodeAddr = SysEnvironment.BizNodeAddr;
            senvir.Ips = SysEnvironment.Ips;
            senvir.SysUserDict = SysEnvironment.SysUserDict;
            senvir.TokenEncrpPublicKey = SysEnvironment.TokenEncrpPublicKey;
            senvir.TokenEncrpType = SysEnvironment.TokenEncrpType;
            senvir.TokenEncrpValue = SysEnvironment.TokenEncrpValue;
            senvir.distManagerParam = SysEnvironment.distManagerParam;
            senvir.I18nCurrLang = SysEnvironment.I18nCurrLang;

            return senvir;
        }

        /// <summary>
        /// 将环境变量序列化实例赋值给环境变量实例
        /// </summary>
        /// <returns></returns>
        public static void GetSysEnvironmentSerialize2SysEnvironment(SysEnvironmentSerialize senvir)
        {
            SysEnvironment.BizNodeAddr = senvir.BizNodeAddr;
            SysEnvironment.Ips = senvir.Ips;
            SysEnvironment.SysUserDict = senvir.SysUserDict;
            SysEnvironment.TokenEncrpPublicKey = senvir.TokenEncrpPublicKey;
            SysEnvironment.TokenEncrpType = senvir.TokenEncrpType;
            SysEnvironment.TokenEncrpValue = senvir.TokenEncrpValue;
            SysEnvironment.distManagerParam = senvir.distManagerParam;
            SysEnvironment.I18nCurrLang = senvir.I18nCurrLang;
                        
        }

     
    }

}

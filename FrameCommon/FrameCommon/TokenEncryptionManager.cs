using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Entitys.ComonEnti;

namespace FrameCommon
{
    /// <summary>
    /// 令牌管理 
    /// </summary>
    public class TokenEncryptionManager
    {
        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <returns></returns>
        public static byte[] EncryToken()
        {
            string encryType = SysEnvironment.TokenEncrpType; //加密算法类型     
            SysEnvironmentSerialize envserialize = new SysEnvironmentSerialize();
            //TODO 将静态类转换为实例

            //预留按算法加密处理,使用公钥加密
            switch (encryType)
            {
                case "one":
                    //TODO 令牌处理
                    break;
                default:
                    break;
            }

            envserialize.TokenEncrpValue = ""; //加密后的密文

            byte[] toks = null;
            //bool temptoks = XmlSerializer.Serialize(envserialize, out toks);
            return toks;
        }

        /// <summary>
        /// 解密令牌
        /// </summary>
        /// <param name="encryToken"></param>
        /// <param name="encryType"></param>
        /// <returns></returns>
        public static SysEnvironmentSerialize DeEncryToken(byte[] encryToken)
        {
            //反序列华
            SysEnvironmentSerialize envserialize = new SysEnvironmentSerialize();

            //TODO
            //bool tt = XmlSerializer.Deserialize<SysEnvironmentSerialize>(encryToken, out envserialize);

            //string encryType = envserialize.TokenEncrpType; //加密算法类型
            //string deStr = ""; //解密结果
            ////预留按算法解密
            //switch (encryType)
            //{
            //    case "one":
            //        //TODO 令牌处理
            //        break;
            //    default:
            //        break;
            //}

            //envserialize.TokenEncrpValue = deStr;

            return envserialize; 
        }
    }
}

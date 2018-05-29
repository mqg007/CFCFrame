using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace Common
{
    public class Security
    {                
        #region 加密信息   
        
        /// <summary>
        /// 加密信息
        /// </summary>
        /// <param name="input">原始信息</param>      
        /// <param name="encryStr">加密使用的密钥</param>      
        /// <param name="index">密钥长度</param>
        /// <returns>加密后的信息</returns>
        public static string EncryptInfo(string input, string encryStr, int index)
        {            
            StringBuilder tmpstr = new StringBuilder();
            int iRandNum = 0;
            Random rnd = new Random();
            for (int i = 0; i < input.Length; i++)
            {
                tmpstr.Append(input.Substring(i, 1));
                for (int j = 0; j < index * 2; j++)
                {
                    iRandNum = rnd.Next(encryStr.Length - 1);
                    tmpstr.Append(encryStr.Substring(iRandNum, 1));
                }
            }

            return tmpstr.ToString();
        }      


        /// <summary>
        /// 解密信息
        /// </summary>
        /// <param name="encryInput">加密信息</param>      
        /// <param name="encryStr">加密使用的密钥</param>
        /// <param name="index">密钥长度</param>
        /// <param name="IsEncry">是否加密</param>
        /// <returns>加密后的信息</returns>
        public static string DeEncryptInfo(string encryInput, string encryStr, int index, string IsEncry)
        {
            StringBuilder tmpstr = new StringBuilder();
            if (IsEncry.Equals("Y"))
            {
                for (int i = 0; i < encryInput.Length; i++)
                {
                    tmpstr.Append(encryInput.Substring(i, 1));
                    i = i + index * 2;
                }
            }
            else
            {
                tmpstr.Append(encryInput);
            }

            return tmpstr.ToString();
        }

        /// <summary>
        /// 加密信息
        /// </summary>
        /// <param name="input">原始信息</param>
        /// <param name="flag">true加强加密；false普通加密</param>
        /// <returns>加密后的信息</returns>
        public static byte[] EncryptInfo(string input, bool flag)
        {
            byte[] sha1Pwd;
            SHA1 sha1 = SHA1.Create();
            string highCode = "128167213105241091992541172169014025413312010521667211123";
            if (flag)
            {
                sha1Pwd = sha1.ComputeHash(Encoding.Unicode.GetBytes(input + highCode));
            }
            else
            {
                sha1Pwd = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
            }
            sha1.Clear();

            return sha1Pwd;
        }

        /// <summary>
        /// 加密后哈希值转化为字符串
        /// </summary>
        /// <param name="hsv">加密后的哈希值</param>
        /// <returns>加密后的哈希值的字符串</returns>
        public static string EncryptInfoToString(byte[] hsv)
        {
            string hsvStr = string.Empty;
            for (int i = 0; i < hsv.Length; i++)
            {
                hsvStr = hsvStr + hsv[i].ToString();
                
            }
            return hsvStr;
        }    
        
        /// <summary>
        /// 随机密码生成
        /// </summary>
        /// <param name="pwdchars">生成的随机密码串可以使用哪些字符</param>
        /// <param name="pwdlen">生成的随机密码串的长度</param>
        /// <returns>随机明文密码</returns>
        public static string MakeLightPassword(string pwdchars, int pwdlen)
        {
            StringBuilder tmpstr = new StringBuilder();
            int iRandNum;
            Random rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                iRandNum = rnd.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[iRandNum]);
            }
            return tmpstr.ToString();
        }

        #endregion

        #region 比较两个字节数组
        /// <summary>
        /// 比较两个字节数组
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>是否相等</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
        #endregion

        #region 使用正则表达式检查用户输入
        /// <summary>
        /// 使用正则表达式检查用户输入
        /// </summary>
        /// <param name="reg">使用的正则表达式</param>
        /// <param name="input">用户输入</param>
        /// <returns>是否合法</returns>
        public static bool CheckInput(string reg, string input)
        {
            //如果长度大于零，则表示有需要验证的内容，否则就表示没有需要验证的内容
            if (input != null && input.Length != 0)
            {
                System.Text.RegularExpressions.Regex ex = new System.Text.RegularExpressions.Regex(reg);
                return ex.IsMatch(input);
            }
            return true;
        }
        #endregion

        #region 使用正则表达式删除用户输入中的脚本内容
        /// <summary>
        /// 使用正则表达式删除用户输入中的脚本内容
        /// </summary>
        /// <param name="text">用户输入</param>
        /// <returns>清理后的文本</returns>
        public static string ClearScript(string text)
        {
            string pattern;

            if (text.Length == 0)
                return text;

            pattern = @"(?i)<script([^>])*>(\w|\W)*</script([^>])*>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            pattern = @"<script([^>])*>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            pattern = @"</script>";
            text = System.Text.RegularExpressions.Regex.Replace(text, pattern, String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return text;
        }
        #endregion

        #region AES加密解密

        /// <summary>
        /// Rijndael加密
        /// </summary>
        /// <param name="data">需要加密的字符数据</param>
        /// <param name="key">密匙，长度可以为：64位(byte[8])，128位(byte[16])，192位(byte[24])，256位(byte[32])</param>
        /// <param name="iv">iv向量，长度为128（byte[16]）</param>
        /// <returns>加密后的字符</returns>
        public static string EnRijndael(string data, byte[] key, byte[] iv)
        {
            Rijndael rijndael = Rijndael.Create();
            byte[] tmp = null;
            ICryptoTransform encryptor = rijndael.CreateEncryptor(key, iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(data);
                    writer.Flush();
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);
        }

        /// <summary>
        /// Rijndael解密
        /// </summary>
        /// <param name="data">需要加密的字符数据</param>
        /// <param name="key">密匙，长度可以为：64位(byte[8])，128位(byte[16])，192位(byte[24])，256位(byte[32])</param>
        /// <param name="iv">iv向量，长度为128（byte[16]）</param>
        /// <returns>解密后的字符</returns>
        public static string DeRijndael(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            Rijndael rijndael = Rijndael.Create();
            ICryptoTransform decryptor = rijndael.CreateDecryptor(key, iv);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    StreamReader reader = new StreamReader(cs);
                    result = reader.ReadLine();
                    reader.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">待加密的字符数据</param>
        /// <param name="key">密匙，长度可以为：128位(byte[16])，192位(byte[24])，256位(byte[32])</param>
        /// <param name="iv">iv向量，长度必须为128位（byte[16]）</param>
        /// <returns>加密后的字符</returns>
        public static string EnAES(string data, byte[] key, byte[] iv)
        {
            Aes aes = Aes.Create();
            byte[] tmp = null;

            ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                }
                tmp = ms.ToArray();
            }
            return Convert.ToBase64String(tmp);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="data">待加密的字符数据</param>
        /// <param name="key">密匙，长度可以为：128位(byte[16])，192位(byte[24])，256位(byte[32])</param>
        /// <param name="iv">iv向量，长度必须为128位（byte[16]）</param>
        /// <returns>加密后的字符</returns>
        public static string DeAES(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            Aes aes = Aes.Create();

            ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    StreamReader reader = new StreamReader(cs);
                    result = reader.ReadLine();
                    reader.Close();
                }
            }
            aes.Clear();
            return result;
        }

        /// <summary>
        /// 生成长度字符串1-256
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CreateKey(int num)
        {
            //备选字符
            string securitycode = "Drn1eAgPMvdUiD63aV9R3dmF8Tb9C1mWpA9L5O34S43wf8V9o13sImOms3iYPdWzq7dj17FTZm5agaC4tdsm0mdeBLp6hMtdD2v49xl43kDkyBuheIFMdFeeDX0rukNX1kv664c8Gd0PuugMk8ds104wxitn91ZNi1am9MHTRzH4Ss54rP8m5T4I9ngoQ2P5NWmru4ImP012t0LxtfP1zR44n4evs4nN4a2dmJnmcbk7c60j241z1WWt8m9uOGieM967bb1a";
            char[] scodes = securitycode.ToCharArray();

            Random rand = new Random();
            StringBuilder strb = new StringBuilder();

            for (int i = 0; i < num; i++)
            {
                int tempcharindex = rand.Next(1, scodes.Length);
                strb.Append(scodes[tempcharindex].ToString());
            }
            return strb.ToString();
        }

        /// <summary>
        /// 生成字符串的byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] CreateKeyByte(string str)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(str);
        }
        #endregion

        public static byte[] MakeVerifyCode()
        {
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            string chkCode = string.Empty;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //写入Session、验证码加密 TODO 增加md5加密整理
            //Utility.WriteSession("hqd007_session_verifycode", Md5.md5(chkCode.ToLower(), 16));
            Utility.WriteSession("hqd007sessionverifycode", chkCode.ToLower());
            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// 格式化文本（防止SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Formatstr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex10 = new System.Text.RegularExpressions.Regex(@"select", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex11 = new System.Text.RegularExpressions.Regex(@"update", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex12 = new System.Text.RegularExpressions.Regex(@"delete", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex10.Replace(html, "s_elect");
            html = regex11.Replace(html, "u_pudate");
            html = regex12.Replace(html, "d_elete");
            html = html.Replace("'", "’");
            html = html.Replace("&nbsp;", " ");
            return html;
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string ReplaceHtml(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&hellip;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&mdash;", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }

    }


    /// <summary>
    /// 加密类，主要用于加密用户密码。
    /// </summary>
    public class EncryptSingleton
    {
        private static EncryptSingleton encryptSingleton = null;
        private bool isUseEncrypt = true;  //是否使用加密

        SymmetricAlgorithm sAlgorithm;
        const string IV = "vmhcLw99CmQ=";
        const string KEY = "8b+rsILdpPY=";

        /// <summary>
        /// 构造函数。
        /// </summary>
        private EncryptSingleton()
        {
            sAlgorithm = new DESCryptoServiceProvider();

        }
        /// <summary>
        /// 是否加密。
        /// </summary>
        public bool UseEncrypt
        {
            set
            {
                isUseEncrypt = value;
            }
        }
        /// <summary>
        /// 得到唯一实例。
        /// </summary>
        /// <returns>EncryptSingleton实例。</returns>
        public static EncryptSingleton GetInstance()
        {

            if (encryptSingleton == null)
                encryptSingleton = new EncryptSingleton();

            return encryptSingleton;
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="txt">要加密的字符串</param>
        /// <returns>加密后的字符串。</returns>
        public string GetEncryptedString(string txt)
        {
            string str;
            if (isUseEncrypt)
                str = HashTextMD5(txt);
            else
                str = txt;

            if (str.Length > 8)
                return str.Substring(0, 8);
            else
                return str;

        }
        /// <summary>
        /// 使用md5加密。
        /// </summary>
        /// <param name="TextToHash">要加密的字符串。</param>
        /// <returns>加密结果。</returns>
        private string HashTextMD5(string textToHash)
        {

            MD5CryptoServiceProvider md5;
            Byte[] bytValue;
            Byte[] bytHash;

            //创建新的加密服务提供程序对象
            md5 = new MD5CryptoServiceProvider();

            //将原始字符串转换成字节数组
            bytValue = System.Text.Encoding.UTF8.GetBytes(textToHash);

            // 计算散列，并返回一个字节数组
            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            //返回散列值的 Base64 编码字符串
            return Convert.ToBase64String(bytHash);
        }

        /// <summary>
        /// 加密密码。
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>加密后的密码</returns>
        public string EncryptPWD(string pwd)
        {
            return EncryptSingleton.GetInstance().GetEncryptedString(pwd);
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="Value">要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public string EncryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;


            //生成临时加密对
            //mCSP.GenerateKey();
            //mCSP.GenerateIV();
            //Console.WriteLine(Convert.ToBase64String(  mCSP.IV));
            //Console.WriteLine(Convert.ToBase64String(mCSP.Key));

            sAlgorithm.IV = Convert.FromBase64String(IV);
            sAlgorithm.Key = Convert.FromBase64String(KEY);

            ct = sAlgorithm.CreateEncryptor(sAlgorithm.Key, sAlgorithm.IV);

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="Value">要解密的字符串。</param>
        /// <returns>源字符串。</returns>
        public string DecryptString(string Value)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            sAlgorithm.IV = Convert.FromBase64String(IV);
            sAlgorithm.Key = Convert.FromBase64String(KEY);

            ct = sAlgorithm.CreateDecryptor(sAlgorithm.Key, sAlgorithm.IV);

            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

    }
}

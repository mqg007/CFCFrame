using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;

namespace Common
{
    /// <summary>
    /// 系统配置文件读取配置参数的类。
    /// </summary>
    public class APPConfig
    {
        private static APPConfig appConfig = null;
        Configuration config = null;

        /// <summary>
        /// 私有构造函数。
        /// </summary>
        private APPConfig()
        {
            SetConfig();
        }

        void SetConfig()
        {
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch
            {
                config = WebConfigurationManager.OpenWebConfiguration("~");
            }
        }
        public object GetSectionValue(string SectionName)
        {

            return ConfigurationManager.GetSection(SectionName);
        }
        /// <summary>
        /// 得到一个应用配置。
        /// </summary>
        /// <returns>应用配置实例</returns>
        public static APPConfig GetAPPConfig()
        {
            if (appConfig == null)
            {
                appConfig = new APPConfig();
            }

            return appConfig;
        }

        /// <summary>
        /// 根据配置的键名，得到对应的值。
        /// </summary>
        /// <returns>值</returns>
        /// <param name="KeyString">键名</param>
        public string GetConfigValue(string KeyString)
        {
            string strVal = null;

            try
            {                
                strVal = config.AppSettings.Settings[KeyString].Value;
            }

            catch (Exception e)
            {
                throw new Exception("从配置文件中读取" + KeyString + "出错！", e);
            }
            return strVal;
        }

        /// <summary>
        /// 根据配置的键名，得到对应的值。
        /// </summary>
        /// <returns>值</returns>
        /// <param name="KeyString">键名</param>
        /// <param name="DefaultValue">如果没有找到键值，设置此缺省值。</param>
        public string GetConfigValue(string KeyString, string DefaultValue)
        {
            string strVal = null;
            if (config.AppSettings.Settings[KeyString] == null)
                return DefaultValue;

            try
            {
                strVal = config.AppSettings.Settings[KeyString].Value;
                if (strVal == null)
                    return DefaultValue;
            }
            catch
            {
                strVal = DefaultValue;
            }
            return strVal;
        }


        public void Save()
        {
            //config.Save();
        }

        /// <summary>
        /// 保存某Section到配置文件中
        /// </summary>
        /// <param name="xmlNodes"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public bool SetConfigSection(List<XmlElement> xmlNodes, string sectionName)
        {

            //为配置文件定义一个FileInfo对象
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //将配置文件读入XML DOM
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

            xmlDocument.Load(FileInfo.FullName);

            //找到正确的节点并将它的值改为新的
            try
            {
                xmlDocument.SelectSingleNode("//" + sectionName).RemoveAll(); //删除所有结点

                foreach (XmlElement node in xmlNodes)
                {
                    XmlElement element = xmlDocument.CreateElement(sectionName);

                    foreach (XmlAttribute att in node.Attributes)
                    {
                        element.SetAttribute(att.Name, att.Value);

                    }

                    xmlDocument.SelectSingleNode("//" + sectionName).AppendChild(element);
                }
            }
            catch (Exception err)
            {
                throw new Exception("保存配置" + sectionName + "出错!", err);
            }

            //保存修改过的配置文件
            xmlDocument.Save(FileInfo.FullName);

            SetConfig();

            return true;
        }
        /// <summary>
        /// 保存设置。
        /// </summary>
        /// <param name="KeyString">健名</param>
        /// <param name="KeyValue">值</param>
        /// <returns></returns>
        public bool SetConfigValue(string KeyString, object KeyValue)
        {
            //为配置文件定义一个FileInfo对象
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //将配置文件读入XML DOM
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

            xmlDocument.Load(FileInfo.FullName);

            //找到正确的节点并将它的值改为新的
            try
            {
                bool isModified = false;
                foreach (System.Collections.IEnumerable ie in xmlDocument.GetElementsByTagName("appSettings"))
                {
                    System.Xml.XmlNode node = ie as System.Xml.XmlNode;

                    foreach (System.Xml.XmlNode Node in node.ChildNodes)
                    {
                        if (Node.Name == "add")
                        {
                            if (Node.Attributes.GetNamedItem("key").Value == KeyString)
                            {
                                Node.Attributes.GetNamedItem("value").Value = KeyValue.ToString();
                                isModified = true;
                                break;
                            }
                        }
                    }
                    if (!isModified)
                    {
                        //添加一个结点
                        System.Xml.XmlElement xmlE = xmlDocument.CreateElement("add");
                        xmlE.SetAttribute("key", KeyString);
                        xmlE.SetAttribute("value", KeyValue.ToString());
                        xmlDocument.SelectSingleNode("//appSettings").AppendChild(xmlE);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("保存配置" + KeyString + "出错!", err);
            }

            //保存修改过的配置文件
            xmlDocument.Save(FileInfo.FullName);
            SetConfig();

            return true;
        }

    }

}

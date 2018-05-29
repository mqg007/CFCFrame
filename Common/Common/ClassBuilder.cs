using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Common
{
    public class ClassBuilder
    {
        /// <summary>
        /// 根据类名创建对象。
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static object CreateObject(string className)
        {
            object instance = null ;

            string[] s = className.Split(',');
            if (s.Length >= 2)
            {
                string strAssemblyName = s[1];
                if (strAssemblyName.IndexOf(':') < 0)   //不包括路径
                {
                    string path = "";
                    if (System.Environment.CurrentDirectory + "\\" == AppDomain.CurrentDomain.BaseDirectory)//Windows应用程序则相等
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory;
                    }                    
                    else
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + "bin\\"; //支持web访问
                    }
                    strAssemblyName = path  + s[1];
                }

                if (File.Exists(strAssemblyName))
                {
                    Assembly assembly = GetAssemblyFromAppDomain(strAssemblyName);
                    if (assembly == null)
                        assembly = Assembly.LoadFrom(strAssemblyName);

                    instance = assembly.CreateInstance(s[0]);
                }
                else
                {
                    throw new InvalidDataException("配置文件不正确，找不到动态库文件:" + strAssemblyName + " ！");
                }
            }
            else
            {
                instance = Assembly.GetExecutingAssembly().CreateInstance(className);
                if (instance == null)
                {
                    instance = Assembly.GetCallingAssembly().CreateInstance(className);
                }
                if (instance == null)
                {
                    IEnumerator ie = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
                    while (ie.MoveNext())
                    {
                        Assembly ab = ie.Current as Assembly;
                        if (ab != null)
                        {
                            instance = ab.CreateInstance(className);
                            if (instance != null)
                                break;
                        }
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 从动态库中创建一个类的实例。
        /// </summary>
        /// <param name="strClassName">类名</param>
        /// <param name="strAssemblyName">动态库名</param>
        /// <returns></returns>
        public static object CreateObject(string strClassName, string strAssemblyName)
        {

            if (strAssemblyName.IndexOf(":") < 1)  //不包含盘符,则添加addin
            {
                strAssemblyName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                     +  strAssemblyName;
            }

            object newInstance = null;
            //此处应该判断否存在
            if (File.Exists(strAssemblyName))
            {
                Assembly assembly = GetAssemblyFromAppDomain(strAssemblyName);
                if (assembly == null)
                    assembly = Assembly.LoadFrom(strAssemblyName);
            
                newInstance = assembly.CreateInstance(strClassName);
            }
            if (newInstance == null)
            {
                throw new CreateObjectException(strClassName);               
            }
            return newInstance;
        }

        /// <summary>
        /// 判断某构件是否已经加载，如果加载，从当前程序域中返回该构件。
        /// </summary>
        /// <param name="assemblyName">要判断的构件全名（包括路径和文件名）</param>
        /// <returns></returns>
        private static Assembly GetAssemblyFromAppDomain(string assemblyName)
        {
            IEnumerator ie = AppDomain.CurrentDomain.GetAssemblies().GetEnumerator();
            while (ie.MoveNext())
            {
                Assembly ab = ie.Current as Assembly;
                try
                {
                    if (ab != null && ab.Location.ToUpper() == assemblyName.ToUpper()) return ab;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 对象已经存在时抛出的例外。
    /// </summary>
    public class ObjectExistException : Exception
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ObjectExistException()
            : base("对象已经创建！")
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
    }

    /// <summary>
    /// 文件不存在时抛出的例外。
    /// </summary>
    public class FileNotExistException : Exception
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="FileName">不存在的文件名</param>
        public FileNotExistException(string FileName)
            : base("文件" + FileName + "不存在！")
        {
        }
    }
    /// <summary>
    /// 根据类名创建对象失败时抛出此例外
    /// </summary>
    public class CreateObjectException : Exception
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ObjectClassName">创建失败的类名</param>
        public CreateObjectException(string ObjectClassName)
            : base("不能创建类" + ObjectClassName + "的实例！")
        {
        }
    }
}

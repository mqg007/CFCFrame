using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys.ComonEnti;

namespace FrameCommon
{
    public enum LogTypeDomain
    {
        /// <summary>
        /// 界面
        /// </summary>
        UI,
        /// <summary>
        /// 业务
        /// </summary>
        Biz,
        /// <summary>
        /// 数据
        /// </summary>
        Data,
        /// <summary>
        /// 系统
        /// </summary>
        Sys,
        /// <summary>
        /// 异常
        /// </summary>
        ExceptionErr
    }

    public enum LogLevelOption
    {
        /// <summary>
        /// 严重错误
        /// </summary>
        HighErr,
        /// <summary>
        /// 普通错误
        /// </summary>
        NormalErr,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 异常
        /// </summary>
        ExecptionErr
    }

    public enum LogAction
    {
        /// <summary>
        /// 查询
        /// </summary>
        Query,
        /// <summary>
        /// 增加
        /// </summary>
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        Modify,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 登录
        /// </summary>
        Login,
        /// <summary>
        /// 退出
        /// </summary>
        Quit,
        /// <summary>
        /// 异常
        /// </summary>
        ExecptionErr
    }

    /// <summary>
    /// 系统日志通用管理
    /// </summary>
    public class LogCommon
    {
        /// <summary>
        /// 创建日志实体
        /// </summary>
        /// <param name="DOMAINNAME"></param>
        /// <param name="OPTIONNAME"></param>
        /// <param name="RECORDTIME"></param>
        /// <param name="CLASSNAME"></param>
        /// <param name="METHORDNAME"></param>
        /// <param name="logaction"></param>
        /// <param name="TABLENAME"></param>
        /// <param name="RECORDIDENCOLS"></param>
        /// <param name="RECORDIDENCOLSVALUES"></param>
        /// <param name="USERNAMES"></param>
        /// <param name="IPS"></param>
        /// <param name="FUNCTIONNAME"></param>
        /// <param name="DESCRIPTIONS"></param>
        /// <param name="SYSTEMNAME"></param>
        /// <param name="SFLAG"></param>
        /// <returns></returns>
        public static SSY_LOGENTITY CreateLogDataEnt(LogTypeDomain DOMAINNAME, LogLevelOption OPTIONNAME, string RECORDTIME, string CLASSNAME, string METHORDNAME,
            LogAction logaction, string TABLENAME, string RECORDIDENCOLS, string RECORDIDENCOLSVALUES, string USERNAMES, string IPS, string FUNCTIONNAME,
            string DESCRIPTIONS, string SYSTEMNAME, string SFLAG)
        {
            SSY_LOGENTITY ssylog = new SSY_LOGENTITY();
            ssylog.DOMAINNAME = DOMAINNAME.ToString();
            ssylog.OPTIONNAME = OPTIONNAME.ToString();
            ssylog.RECORDTIME = RECORDTIME;
            ssylog.CLASSNAME = CLASSNAME;
            ssylog.METHORDNAME = METHORDNAME;
            ssylog.OPERATERIDEN = logaction.ToString();
            ssylog.TABLENAME = TABLENAME;
            ssylog.RECORDIDENCOLS = RECORDIDENCOLS;
            ssylog.RECORDIDENCOLSVALUES = RECORDIDENCOLSVALUES;
            ssylog.USERNAMES = USERNAMES;
            ssylog.IPS = IPS;
            ssylog.FUNCTIONNAME = FUNCTIONNAME;
            if (Common.Utility.ChangeByte(DESCRIPTIONS, 4000))
            {
                ssylog.DESCRIPTIONS = DESCRIPTIONS;
            }
            else
            {
                ssylog.DESCRIPTIONS = DESCRIPTIONS.Substring(0, 3900); //预防汉子多占位,这里只取3900
            }            
            ssylog.SYSTEMNAME = SYSTEMNAME;
            ssylog.SFLAG = SFLAG;

            ssylog.OPFlag = "I"; //日志全部是新增

            return ssylog;
        } 
    }
}

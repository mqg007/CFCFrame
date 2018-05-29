using System;
using System.Runtime.Serialization;

namespace Entitys.ComonEnti
{
    [DataContract]
    public class ResultCollection<T>
    {
        public T Result { get; set; }
    }

    /// <summary>
    /// 通用泛型返回值类型
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    [DataContract]
    public class SSY_ResponseResult<T>
    {
        /// <summary>
        /// 指示操作是否完成
        /// </summary>
        [DataMember(Name = "Completed", Order = 0)]
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 返回数据的值
        /// </summary>
        [DataMember(Name = "Result", Order = 1)]
        public T Result { get; set; }

        /// <summary>
        /// 与请求过程产生的异常关联的错误代码，无异常返回 0，无关联的错误代码返回 -1
        /// </summary>
        [DataMember(Name = "ErrorCode", Order = 2)]
        public int ErrorCode { get; private set; }


        [DataMember(Name = "IdentifyCode", Order = 2)]
        public string IdentifyCode { get; private set; }

        /// <summary>
        /// 请求过程产生的异常信息，无异常返回 null 值
        /// </summary>
        [DataMember(Name = "Exception", Order = 3)]
        public string Exception { get; set; }

        /// <summary>
        /// 正常返回提示信息编码
        /// </summary>
        [DataMember(Name = "InfoCode", Order = 4)]
        public string InfoCode { get; set; }

        /// <summary>
        /// 正常返回提示信息内容
        /// </summary>
        [DataMember(Name = "InfoCodeContent", Order = 5)]
        public string InfoCodeContent { get; set; }

        public SSY_ResponseResult(T result, string infoCode, string infoCodeContent, bool isCompleted = true)
        {
            this.Result = result;
            this.IsCompleted = isCompleted;
            this.InfoCode = infoCode;
            this.InfoCodeContent = InfoCodeContent;
        }

        public SSY_ResponseResult(T result, bool isCompleted = true)
        {
            this.Result = result;
            this.IsCompleted = isCompleted;
            this.ErrorCode = 0;
        }

        public SSY_ResponseResult(int errorCode) : this(errorCode, string.Empty) { }

        public SSY_ResponseResult(string exception) : this(-1, exception) { }

        public SSY_ResponseResult(int errorCode, string exception)
        {
            this.IsCompleted = false;
            this.ErrorCode = errorCode;
            this.Exception = exception;
        }

        public SSY_ResponseResult(string identifyCode, string exception)
        {
            this.IsCompleted = false;
            this.Exception = exception;
            this.IdentifyCode = identifyCode;

            if (string.IsNullOrWhiteSpace(identifyCode)) this.ErrorCode = -1; else this.ErrorCode = int.Parse(identifyCode.Substring(0, 4));
        }

        public SSY_ResponseResult(int errorCode, string identifyCode, string exception)
        {
            this.IsCompleted = false;
            this.ErrorCode = errorCode;
            this.Exception = exception;
            this.IdentifyCode = identifyCode;
        }

        public SSY_ResponseResult(int errorCode, Exception exception)
        {
            this.ErrorCode = errorCode;

            if (exception == null)
                return;

            Exception ex;
            if (exception.InnerException == null)
            {
                ex = exception;
            }
            else
            {
                ex = exception.InnerException;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
            }
            this.Exception = ex.Message;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.ServiceModel.Web;
using System.ServiceModel.Description;


namespace Common
{
    /// <summary>
    /// 动态调用服务
    /// </summary>
    public class DynamicInvokeWCF
    {
        /// <summary>
        /// WCF地址常量Key
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static TService Create<TService>(string url, int TimeOut = 60)
        {            
            if (url.StartsWith("http://")) return HttpCreate<TService>(url, TimeOut);
            if (url.StartsWith("https://")) return HttpsCreate<TService>(url, TimeOut);
            throw new Exception("服务地址错误：" + url);
        }

        private static TService HttpCreate<TService>(string uri, int TimeOut)
        {
            WSHttpBinding binding = new System.ServiceModel.WSHttpBinding();

            binding.SendTimeout = TimeSpan.FromSeconds(TimeOut);
            binding.ReceiveTimeout = TimeSpan.FromSeconds(TimeOut);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;

            #region  设置一些配置信息
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            #endregion

            #region 服务令牌处理 

            string token = string.Empty;
            AddressHeader[] addressHeaders = new AddressHeader[] { };

            if (string.IsNullOrEmpty(token) && OperationContext.Current != null)
            {
                int index = OperationContext.Current.IncomingMessageHeaders.FindHeader("token", "");
                if (index >= 0) token = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index);
            }
            if (string.IsNullOrEmpty(token) && WebOperationContext.Current != null)
            {
                if(WebOperationContext.Current.IncomingRequest.Headers["token"] != null)
                {
                    token = WebOperationContext.Current.IncomingRequest.Headers["token"].ToString();
                }                
            }  
            if (!string.IsNullOrEmpty(token))
            {
                //若存在令牌，则继续添加令牌，不存在，则不管令牌
                AddressHeader addressHeader1 = AddressHeader.CreateAddressHeader("token", "", token);
                addressHeaders = new AddressHeader[] { addressHeader1 };
            }

            #endregion

            EndpointAddress endpoint = new EndpointAddress(new Uri(uri), addressHeaders);

            ChannelFactory<TService> channelfactory = new ChannelFactory<TService>(binding, endpoint);
            //返回工厂创建MaxItemsInObjectGraph 
            foreach (OperationDescription op in channelfactory.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                {
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }
            TService tservice = channelfactory.CreateChannel();
            return tservice;
        }

        private static TService HttpsCreate<TService>(string url, int TimeOut)
        {
            Uri uri = new Uri(url);

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            WSHttpBinding binding = new System.ServiceModel.WSHttpBinding();
            binding.SendTimeout = new TimeSpan(0, TimeOut / 60, TimeOut % 60);
            binding.ReceiveTimeout = new TimeSpan(0, TimeOut / 60, TimeOut % 60);

            #region  设置一些配置信息
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            #endregion
          
            System.ServiceModel.Web.WebOperationContext ctx = System.ServiceModel.Web.WebOperationContext.Current;
            var token = ctx.IncomingRequest.Headers["token"].ToString();

            AddressHeaderCollection addressHeaderColl;
            if (!string.IsNullOrEmpty(token))
            {
                AddressHeader addressHeader1 = AddressHeader.CreateAddressHeader("token", "", token); 
                AddressHeader[] addressHeaders = new AddressHeader[] { addressHeader1 };
                addressHeaderColl = new AddressHeaderCollection(addressHeaders);
            }
            else
            {
                addressHeaderColl = new AddressHeaderCollection();
            }
            //
            //string[] cers = ZH.Security.Client.CertUtil.GetHostCertificate(uri);
            //TODO 证书处理
            string[] cers = "".Split('.');
            var IdentityDns = EndpointIdentity.CreateDnsIdentity(cers[0]);
            //
            System.ServiceModel.EndpointAddress endpoint = new EndpointAddress(uri, IdentityDns, addressHeaderColl);

            ChannelFactory<TService> channelfactory = new ChannelFactory<TService>(binding, endpoint);
            foreach (OperationDescription op in channelfactory.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                {
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }
            //
            channelfactory.Credentials.ServiceCertificate.DefaultCertificate = new X509Certificate2(Convert.FromBase64String(cers[1])); // new X509Certificate2(x509certificate);            \

            //
            //返回工厂创建
            TService tservice = channelfactory.CreateChannel();
            //
            return tservice;

        }

        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //string[] cers = ZH.Security.Client.CertUtil.GetHostCertificate((sender as System.Net.HttpWebRequest).Address);
            //TODO  证书处理
            string[] cers = "".Split('.');
            for (int i = 0; i < cers.Length; i++)
            {
                X509Certificate cer = new X509Certificate(Convert.FromBase64String(cers[i * 2 + 1]));
                if (certificate.Equals(cer)) return true;
            }
            return false;
        }
    }
}




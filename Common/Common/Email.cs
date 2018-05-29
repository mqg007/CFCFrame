using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;


namespace Common
{
    public class Email
    {
        public static bool SendMail(string mailto, string title, string body, string mailHost, string mailUserName, string mailPassword,
            string mailDisplayName, string strMailAddress, string strSubject, ref string errStr)
        {       
            bool sendBL = false;
            strSubject = title;                  

            try
            {
                SmtpClient client = new SmtpClient(mailHost);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.EnableSsl = false;
                client.Credentials = new System.Net.NetworkCredential(mailUserName, mailPassword);
                client.Port = 25;
                MailAddress from = new MailAddress(strMailAddress, mailDisplayName, System.Text.Encoding.Default);
                MailAddress to = new MailAddress(mailto);
                MailMessage message = new MailMessage(from, to);
                message.Subject = strSubject.ToString();
                message.Body = body.ToString();
                message.SubjectEncoding = System.Text.Encoding.Default;
                message.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
                message.IsBodyHtml = true;

                client.Send(message);
                sendBL = true;
            }
            catch (System.Net.Mail.SmtpException ehttp)
            {
                sendBL = false;
                errStr = "\n发送邮件出现异常！\nSource=" + ehttp.Source + "\nMessage=" + ehttp.Message;
            }

            return sendBL;
        }
       
    }
}

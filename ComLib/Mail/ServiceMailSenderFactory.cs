using System;
using System.Net.Mail;
using System.Reflection;
using ComLib.Exceptions;

namespace ComLib.Mail
{
    public interface IServiceMailSender
    {
        MailAddressCollection Receivers { get; set; } 
        void SetData(params string[] data);
        // object[] Data { get; set; }
        void Send(SmtpClient client);
        void SendAsync(SmtpClient client);
    }

    public static class ServiceMailSenderFactory
    {
        public static IServiceMailSender CreateServiceMailSender(string mailType)
        {
            var currentNameSpace = typeof (IServiceMailSender).Namespace;

            var type = Type.GetType(currentNameSpace + "." + mailType + "ServiceMailSender");
            if (type == null)
            {
                throw new MailSenderCreationException();
            }
            var sender = (IServiceMailSender) type.InvokeMember(null,
                                                                BindingFlags.DeclaredOnly
                                                                | BindingFlags.NonPublic
                                                                | BindingFlags.Instance
                                                                | BindingFlags.CreateInstance,
                                                                null, null, null);
            return sender;
        }

        // 假工厂
        public static IServiceMailSender CreateServiceMailSender<T>() where T : IServiceMailSender
        {
            var type = typeof (T);
            var sender = (IServiceMailSender) type.InvokeMember(null,
                                                                BindingFlags.DeclaredOnly
                                                                | BindingFlags.NonPublic
                                                                | BindingFlags.Instance
                                                                | BindingFlags.CreateInstance,
                                                                null, null, null);
            return sender;
        }
    }
}

using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;

namespace Samsonite.Library.Utility
{
   public  class MailHelper : IDisposable
    {
        private string UserName { get; set; }
        private string Password { get; set; }
        private SmtpClient Client { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string FromName { get; set; } = "";
        public string FromEmail { get; set; }
        public MailHelper(string server, int port,bool useSsl, string userName, string pwd, string fromEmail=null)
        {
            UseSsl = useSsl;
            Server = server;
            Port = port;
            UserName = userName;
            Password = pwd;
            FromEmail = fromEmail?? userName;
            Client = new SmtpClient();
        }
        public void Dispose()
        {
            Client.Disconnect(true);
            Client.Dispose();
        }
        public void Send(string sub, string msg, string to, string cc = null, string bcc = null)
        {
            if (!Client.IsConnected)
            {
                Client.Connect(Server, Port, UseSsl);
                Client.Authenticate(UserName, Password);
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(FromName, FromEmail));
            var toAddresses = to.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(item => new MailboxAddress("", item));
            message.To.AddRange(toAddresses);
            if (!string.IsNullOrEmpty(cc))
            {
                var ccAddresses = cc.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(item => new MailboxAddress("", item));
                message.Cc.AddRange(ccAddresses);
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                var bccAddresses = bcc.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(item => new MailboxAddress("", item));
                message.Bcc.AddRange(bccAddresses);
            }

            message.Subject = sub;
            message.Body = new TextPart("html")
            {
                Text = msg
            };
            Client.Send(message);
        }
    }
}

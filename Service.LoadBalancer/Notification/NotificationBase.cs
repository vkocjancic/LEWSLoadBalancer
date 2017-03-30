using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.LoadBalancer.Notification
{
    public abstract class NotificationBase : INotify
    {

        #region Properties

        public string From { get; set; }
        public string Recipients { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; } = 25;
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }

        #endregion

        #region Constructors

        public NotificationBase()
        {
            From = Properties.Settings.Default.FromAddress;
            Recipients = Properties.Settings.Default.AdminMail;
            SmtpServer = Properties.Settings.Default.SmtpServer;
            SmtpPort = Properties.Settings.Default.SmtpPort;
            SmtpUsername = Properties.Settings.Default.SmtpUsername;
            SmtpPassword = Properties.Settings.Default.SmtpPassword;
        }

        #endregion

        #region INotify implementation

        public void Send()
        {
            using (var mail = new MailMessage(From, Recipients))
            using (var client = new SmtpClient(SmtpServer, SmtpPort))
            {
                mail.Subject = GetSubject();
                mail.Body = GetBody();
                if (!string.IsNullOrEmpty(SmtpUsername))
                {
                    client.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
                }
                client.Send(mail);
            }
        }

        #endregion

        #region Abstract methods

        protected abstract string GetSubject();

        protected abstract string GetBody();

        #endregion
    }
}

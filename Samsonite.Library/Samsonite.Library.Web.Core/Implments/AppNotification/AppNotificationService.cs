using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Core
{
    public class AppNotificationService : AppNotificationCore, IAppNotificationService
    {
        private IMailService _mailService;
        private appEntities _appDB;
        public AppNotificationService(IMailService mailService, appEntities appEntities)
        {
            _mailService = mailService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 发送服务警告邮件
        /// </summary>
        /// <param name="workflowID"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void SendServiceModuleNotification(string workflowID, AppNotificationLevel level, string message)
        {
            ServiceModuleNotificationTemplate objTemplate = new ServiceModuleNotificationTemplate(_mailService, _appDB)
            {
                WorkflowID = workflowID,
                Level = level,
                Message = message
            };
            objTemplate.Send();
        }

        /// <summary>
        /// 发送登录验证码邮件
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="recvMail"></param>
        public void SendLoginVerificationCodeNotification(string verificationCode, string recvMail)
        {
            LoginVerficationNotificationTemplate objTemplate = new LoginVerficationNotificationTemplate(_mailService, _appDB)
            {
                VerificationCode = verificationCode,
                RecvMail = recvMail
            };
            objTemplate.Send();
        }
    }


}

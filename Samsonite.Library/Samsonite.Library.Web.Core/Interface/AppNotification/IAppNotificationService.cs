using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Core
{
    public interface IAppNotificationService
    {
        /// <summary>
        /// 发送服务警告邮件
        /// </summary>
        /// <param name="workflowID"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        void SendServiceModuleNotification(string workflowID, AppNotificationLevel level, string message);

        /// <summary>
        /// 发送登录验证码邮件
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <param name="recvMail"></param>
        void SendLoginVerificationCodeNotification(string verificationCode, string recvMail);
    }
}

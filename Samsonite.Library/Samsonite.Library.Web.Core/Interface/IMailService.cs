using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;

namespace Samsonite.Library.Web.Core
{
    public interface IMailService
    {
        /// <summary>
        /// 返回默认邮件发送对象
        /// </summary>
        /// <returns></returns>
        MailHelper MailDefaultConfig();

        /// <summary>
        /// 添加邮件到待发送邮件表
        /// </summary>
        /// <param name="objSMMailSend"></param>
        void SendEmail(SMMailSend objSMMailSend);
    }
}

using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using System;

namespace Samsonite.Library.Web.Core
{
    public class MailService : AppConfigCore, IMailService
    {
        private appEntities _appDB;
        public MailService(appEntities appEntities) : base(appEntities)
        {
            _appDB = appEntities;
        }

        /// <summary>
        /// 返回默认邮件发送对象
        /// </summary>
        /// <returns></returns>
        public MailHelper MailDefaultConfig()
        {
            //配置邮件服务器
            var emailConfig = this.GetSysConfig.EmailConfig;
            return new MailHelper(emailConfig.ServerHost, emailConfig.Port, false, emailConfig.MailUsername, emailConfig.MailPassword);
        }

        /// <summary>
        /// 添加邮件到待发送邮件表
        /// </summary>
        /// <param name="objSMMailSend"></param>
        public void SendEmail(SMMailSend objSMMailSend)
        {
            try
            {
                _appDB.SMMailSend.Add(objSMMailSend);
                _appDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

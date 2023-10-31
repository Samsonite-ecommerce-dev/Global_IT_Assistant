using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using System;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class LoginVerficationNotificationTemplate : AppNotificationCore
    {
        private IMailService _mailService;
        private appEntities _appDB;
        public LoginVerficationNotificationTemplate(IMailService mailService, appEntities appEntities)
        {
            _mailService = mailService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string RecvMail { get; set; }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            try
            {
                SysTemplate objSysTemplate = _appDB.SysTemplate.Where(p => p.TemplateIndentify == LOGIN_VERIFICATION_CODE_TEMPLATE_INDNTIFY).SingleOrDefault();
                if (objSysTemplate != null)
                {
                    string _template = objSysTemplate.TemplateContent;
                    _template = _template.Replace("{{SystemName}}", SYSTEM_NAME);
                    _template = _template.Replace("{{VerificationCode}}", this.VerificationCode);
                    //立即发送邮件
                    //默认发件服务器
                    MailHelper mailHelper = _mailService.MailDefaultConfig();
                    mailHelper.Send(objSysTemplate.TemplateTitle, _template, this.RecvMail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

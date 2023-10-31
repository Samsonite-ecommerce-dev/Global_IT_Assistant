using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class ServiceModuleNotificationTemplate : AppNotificationCore
    {
        private IMailService _mailService;
        private appEntities _appDB;
        public ServiceModuleNotificationTemplate(IMailService mailService, appEntities appEntities)
        {
            _mailService = mailService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 工作流ID
        /// </summary>
        public string WorkflowID { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public AppNotificationLevel Level { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            try
            {
                SysTemplate objSysTemplate = _appDB.SysTemplate.Where(p => p.TemplateIndentify == SERVICE_TEMPLATE_INDNTIFY).SingleOrDefault();
                if (objSysTemplate != null)
                {
                    string _template = objSysTemplate.TemplateContent;
                    _template = _template.Replace("{{SystemName}}", SYSTEM_NAME);
                    _template = _template.Replace("{{WorkflowID}}", this.WorkflowID);
                    _template = _template.Replace("{{Level}}", this.GetLevelDisplay(Level));
                    _template = _template.Replace("{{Message}}", this.Message);
                    //读取邮件组
                    SendMailGroup objSendMailGroup = _appDB.SendMailGroup.Where(p => p.ID == SERVICE_TEMPLATE_GROUPID).SingleOrDefault();
                    if (objSendMailGroup != null)
                    {
                        _mailService.SendEmail(new SMMailSend()
                        {
                            RecvMail = objSendMailGroup.MailAddresses.Replace(",", ";"),
                            MailTitle = objSysTemplate.TemplateTitle,
                            MailContent = _template,
                            SendUserid = 0,
                            SendUserIP = string.Empty,
                            SendCount = 0,
                            SendMessage = string.Empty,
                            SendState = 0,
                            OutboxID = 0,
                            CreateTime = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

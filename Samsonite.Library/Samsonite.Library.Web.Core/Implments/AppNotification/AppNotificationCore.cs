using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Core
{
    public class AppNotificationCore
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public const string SYSTEM_NAME = "Global IT Management System";

        /// <summary>
        /// 系统警告邮件模板标识
        /// </summary>
        public const string SERVICE_TEMPLATE_INDNTIFY = "E001";

        /// <summary>
        /// 系统警告邮件模板发送组ID
        /// </summary>
        public const int SERVICE_TEMPLATE_GROUPID = 1;

        /// <summary>
        /// 登录验证码邮件模板标识
        /// </summary>
        public const string LOGIN_VERIFICATION_CODE_TEMPLATE_INDNTIFY = "E002";

        #region 函数
        /// <summary>
        /// 获取等级显示
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected string GetLevelDisplay(AppNotificationLevel level)
        {
            string _result = string.Empty;
            switch (level)
            {
                case AppNotificationLevel.Info:
                    _result = $"<span class=\"color_primary\">{AppNotificationLevel.Info.ToString()}</span>";
                    break;
                case AppNotificationLevel.Warning:
                    _result = $"<span class=\"color_warning\">{AppNotificationLevel.Warning.ToString()}</span>";
                    break;
                case AppNotificationLevel.Error:
                    _result = $"<span class=\"color_danger\">{AppNotificationLevel.Error.ToString()}</span>";
                    break;
                case AppNotificationLevel.Debug:
                    _result = $"<span class=\"color_fail\">{AppNotificationLevel.Debug.ToString()}</span>";
                    break;
                default:
                    break;
            }
            return _result;
        }
        #endregion
    }
}

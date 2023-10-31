using Microsoft.Extensions.Configuration;
using Samsonite.Library.Web.Core.Models;
using System.IO;

namespace Samsonite.Library.Web.Core
{
    public class AppGlobalConst
    {
        private static AppGlobalConst instance = null;

        public static AppGlobalConst Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppGlobalConst();
                }
                return instance;
            }
        }

        #region 全局静态常量
        /// <summary>
        /// 配置信息缓存名
        /// </summary>
        public const string CONFIG_CACHE_NAME = "SYSCONFIG";

        /// <summary>
        /// 配置信息缓存时间
        /// </summary>
        public const int CONFIG_CACHE_TIME = 365;

        /// <summary>
        /// 语言包缓存名
        /// </summary>
        public const string LANGUAGE_PACK_CACHE_NAME = "LANGUAGE_PACK";

        /// <summary>
        /// 语言包缓存名时间
        /// </summary>
        public const int LANGUAGE_PACK_CACHE_TIME = 365;

        /// <summary>
        /// 用户当前选中语言包Cookie名
        /// </summary>
        public const string USER_CURRENT_LANGUAGE_PACK_COOKIE_NAME = "LOGIN_LANGUAGE_PACK";

        /// <summary>
        /// 用户当前选中语言包Cookie时间
        /// </summary>
        public const int USER_CURRENT_LANGUAGE_PACK_COOKIE_TIME = 30;

        /// <summary>
        /// 用户登录Session名
        /// </summary>
        public const string USER_LOGIN_SESSION_NAME = "LOGIN_INFO";

        /// <summary>
        /// 用户登录用户Cookie名
        /// </summary>
        public const string USER_LOGIN_ACCOUNT_COOKIE_NAME = "LOGIN_INFO_UNAME";

        /// <summary>
        /// 用户登录密码Cookie密码
        /// </summary>
        public const string USER_LOGIN_PWD_COOKIE_NAME = "LOGIN_INFO_UPWD";

        /// <summary>
        /// 用户邮箱验证Session名
        /// </summary>
        public const string USER_LOGIN_VERIFICATION_SESSION_NAME = "LOGIN_VERIFICATION_CODE";

        /// <summary>
        /// DES密钥
        /// </summary>
        public const string DES_KEY = "sam86lb!23";
        #endregion

        #region 全局静态配置参数
        /// <summary>
        /// 全局静态配置
        /// </summary>
        public static GlobalConfigModel GetGlobalStaticConfig
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                //创建配置根对象
                var configurationRoot = builder.Build();
                //获取配置根目录
                var rootSection = configurationRoot.GetSection("SiteConfig");

                return new GlobalConfigModel()
                {
                    HttpURL = rootSection.GetSection("httpURL").Value,
                    SessionKey = $"{rootSection.GetSection("basePrefix").Value}_SESSION_SAMSONITE_OMS",
                    CookieKey = $"{rootSection.GetSection("basePrefix").Value}_COOKIE_SAMSONITE_OMS",
                    CacheKey = $"{rootSection.GetSection("basePrefix").Value}_CacheKey_SAMSONITE_OMS",
                    UploadFilePath = "/UploadFile/Temporary",
                    UploadCachePath = "/UploadFile/CacheFile",
                    ImagePath = $"{rootSection.GetSection("imagePath").Value}",
                    SitePhysicalPath = rootSection.GetSection("sitePhysicalPATH").Value
                };
            }
        }

        /// <summary>
        /// 语言包配置
        /// </summary>
        public const string LANGUAGE_PACK_KEY = "LANGUAGE_PACK";

        /// <summary>
        /// 邮件服务器配置
        /// </summary>
        public const string EMAIL_CONFIG_KEY = "EMAIL_CONFIG";

        /// <summary>
        /// 短信配置
        /// </summary>
        public const string SMS_CONFIG_KEY = "SMS_CONFIG";

        /// <summary>
        /// 登录配置
        /// </summary>
        public const string LOGIN_CONFIG_KEY = "LOGIN_CONFIG";

        /// <summary>
        /// 密码规则配置
        /// </summary>
        public const string PASSWORD_CONFIG_KEY = "PASSWORD_CONFIG";

        /// <summary>
        /// 皮肤
        /// </summary>
        public const string SKIN_STYLE_KEY = "SKIN_STYLE";
        #endregion
    }
}

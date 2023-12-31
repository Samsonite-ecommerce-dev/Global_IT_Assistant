﻿using System.Collections.Generic;

namespace Samsonite.Library.Web.Core.Models
{
    public class AppConfigModel : SysConfigModel
    {
        public GlobalConfigModel GlobalConfig { get; set; }
    }

    public class GlobalConfigModel
    {
        /// <summary>
        /// 域名地址
        /// </summary>
        public string HttpURL { get; set; }

        /// <summary>
        /// Session前缀
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// Cookie前缀
        /// </summary>
        public string CookieKey { get; set; }

        /// <summary>
        /// Cache前缀
        /// </summary>
        public string CacheKey { get; set; }

        /// <summary>
        /// 文件上传目录
        /// </summary>
        public string UploadFilePath { get; set; }

        /// <summary>
        /// 文件缓存目录
        /// </summary>
        public string UploadCachePath { get; set; }

        /// <summary>
        /// 图片目录
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 站点物理地址
        /// </summary>
        public string SitePhysicalPath { get; set; }
    }

    public class SysConfigModel
    {
        /// <summary>
        /// 语言版本
        /// </summary>
        public List<int> LanguageTypes { get; set; }

        /// <summary>
        /// 登录配置
        /// </summary>
        public LoginConfigModel LoginConfig { get; set; }

        /// <summary>
        /// 密码规则配置
        /// </summary>
        public PasswordConfigModel PasswordConfig { get; set; }

        /// <summary>
        /// 邮件配置
        /// </summary>
        public EmailModel EmailConfig { get; set; }

        /// <summary>
        /// 短信配置
        /// </summary>
        public SMSModel SMSConfig { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string SkinStyle { get; set; }
    }

    public class LoginConfigModel
    {
        /// <summary>
        /// 登录信息保存日期(天)
        /// </summary>
        public int CookieSaveTime { get; set; }
    }

    public class PasswordConfigModel
    {
        /// <summary>
        /// 最近N次修改密码不能重复
        /// </summary>
        public int PastNum { get; set; }

        /// <summary>
        /// 密码过期时间
        /// </summary>
        public int ValidityTime { get; set; }

        /// <summary>
        /// 连续密码错误次数锁定
        /// </summary>
        public int ErrorLockNum { get; set; }
    }
}

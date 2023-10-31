namespace Samsonite.Library.Web.Core.Models
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 内部人员
        /// </summary>
        InternalStaff = 1,
        /// <summary>
        /// 外部人员
        /// </summary>
        Customer = 2
    }

    public enum UserStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 被锁定
        /// </summary>
        Locked = 1,
        /// <summary>
        /// 密码过期
        /// </summary>
        ExpiredPwd = 2
    }

    public enum UserLoginType
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        Account = 0,
        /// <summary>
        /// MFA登录
        /// </summary>
        MFA = 1,
        /// <summary>
        /// 邮件验证
        /// </summary>
        VerificationCode = 2
    }
}

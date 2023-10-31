
namespace Samsonite.Library.Web.Core.Models
{
    public class LoginResponse
    {
        /// <summary>
        /// 登录结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 登录类型
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// 登录邮箱
        /// </summary>
        public string LoginEmail { get; set; }

        /// <summary>
        /// 登录阶段,值1/2
        /// 1.默认账号密码验证阶段
        /// 2.获取验证码阶段,当UserLoginType是邮件验证时,如果账号和密码正确情况下会返回2,表示需要进去第二步验证码操作
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
    }
}

using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Core
{
    public interface ILoginService
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="verificationCode"></param>
        /// <param name="isRemember">默认保存1天，记住密码保存30天</param>
        /// <returns></returns>
        LoginResponse UserLogin(string userName, string password, string verificationCode, bool isRemember);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        PostResponse SendVerificationCode(string userEmail);

        /// <summary>
        /// 用户退出
        /// </summary>
        void UserLoginOut();

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PostResponse ForgetPassword(ForgetPasswordRequest request);
    }
}

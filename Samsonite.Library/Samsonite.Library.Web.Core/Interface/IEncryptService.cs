namespace Samsonite.Library.Web.Core
{
    public interface IEncryptService
    {
        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        string CreateRandomPassword();

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        string CreatePrivateKey(int length);

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string EncryptPassword(string password, string key);
    }
}

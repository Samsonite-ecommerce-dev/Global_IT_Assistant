using Microsoft.AspNetCore.Http;

namespace Samsonite.Library.Web.Core
{
    public interface IAppCookie
    {
        /// <summary>
        /// 添加Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectTime"></param>
        void Set(string key, string value, int effectTime);

        /// <summary>
        /// 添加Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cookieOptions"></param>
        void Set(string key, string value, CookieOptions cookieOptions);

        /// <summary>
        /// 去读Cookie值
        /// </summary>
        /// <param name="key"></param>
        string Get(string key);

        /// <summary>
        /// 清空Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void Remove(string key);
    }
}
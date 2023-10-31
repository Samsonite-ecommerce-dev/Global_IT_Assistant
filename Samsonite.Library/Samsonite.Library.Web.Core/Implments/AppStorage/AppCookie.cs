using Microsoft.AspNetCore.Http;
using Samsonite.Library.Utility;
using System;

namespace Samsonite.Library.Web.Core
{
    public class AppCookie : IAppCookie
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IResponseCookies _cookie_res => _httpContextAccessor.HttpContext.Response.Cookies;
        private IRequestCookieCollection _cookie_req => _httpContextAccessor.HttpContext.Request.Cookies;
        private string _cookieKey = string.Empty;
        public AppCookie(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cookieKey = AppGlobalConst.GetGlobalStaticConfig.CookieKey;
        }

        /// <summary>
        /// 添加Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectTime"></param>
        public void Set(string key, string value, int effectTime)
        {
            try
            {
                //cookie值加密
                if (!string.IsNullOrEmpty(value))
                    value = EncryptHelper.DESEncrypt(value, AppGlobalConst.DES_KEY);
                _cookie_res.Append($"{_cookieKey}_{key}", value, new CookieOptions() { Expires = DateTime.Now.AddDays(effectTime) });
            }
            catch { }
        }

        /// <summary>
        /// 添加Cookie值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cookieOptions"></param>
        public void Set(string key, string value, CookieOptions cookieOptions)
        {
            try
            {
                //cookie值加密
                if (!string.IsNullOrEmpty(value))
                    value = EncryptHelper.DESEncrypt(value, AppGlobalConst.DES_KEY);
                _cookie_res.Append($"{_cookieKey}_{key}", value, cookieOptions);
            }
            catch { }
        }

        /// <summary>
        /// 去读Cookie值
        /// </summary>
        /// <param name="key"></param>
        public string Get(string key)
        {
            try
            {
                string _value = string.Empty;
                _cookie_req.TryGetValue($"{_cookieKey}_{key}", out _value);
                //cookie值解密
                if (!string.IsNullOrEmpty(_value))
                    _value = EncryptHelper.DESDecrypt(_value, AppGlobalConst.DES_KEY);
                return _value;

            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 清空Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Remove(string key)
        {
            _cookie_res.Delete($"{_cookieKey}_{key}");
        }
    }
}

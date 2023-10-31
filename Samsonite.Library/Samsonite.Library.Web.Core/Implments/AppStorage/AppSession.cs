using Microsoft.AspNetCore.Http;

namespace Samsonite.Library.Web.Core
{
    public class AppSession : IAppSession
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private string _sessionKey = string.Empty;
        public AppSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionKey = AppGlobalConst.GetGlobalStaticConfig.SessionKey;
        }

        /// <summary>
        /// 存储Session值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value)
        {
            _session.SetString($"{_sessionKey}_{key}", value);
        }

        /// <summary>
        /// 读取Session值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return _session.GetString($"{_sessionKey}_{key}");
        }

        /// <summary>
        /// 清空Session
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _session.Remove($"{_sessionKey}_{key}");
        }

        /// <summary>
        /// 清空全部Session
        /// </summary>
        public void Clear()
        {
            _session.Clear();
        }
    }
}

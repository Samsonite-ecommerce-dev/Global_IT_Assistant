namespace Samsonite.Library.Web.Core
{
    public interface IAppSession
    {
        /// <summary>
        /// 存储Session值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        /// <summary>
        /// 读取Session值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 清空Session
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 清空全部Session
        /// </summary>
        void Clear();
    }
}
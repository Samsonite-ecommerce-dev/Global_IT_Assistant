namespace Samsonite.Library.Web.Core
{
    public interface IAppCache
    {
        /// <summary>
        /// 存储Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectTime"></param>
        void Set(string key, object value, int effectTime);

        /// <summary>
        /// 读取Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 清空Cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
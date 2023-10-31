using System;

namespace Samsonite.Library.Web.Business.Custom.Models
{
    public enum CatelogType
    {
        /// <summary>
        /// 站点
        /// </summary>
        WebSite = 1,
        /// <summary>
        /// API
        /// </summary>
        API = 2,
        /// <summary>
        /// 服务器
        /// </summary>
        Server = 3,
        /// <summary>
        /// 数据库
        /// </summary>
        Database = 4
    }

    public enum EnvironmentType
    {
        /// <summary>
        /// 正式
        /// </summary>
        Production = 1,
        /// <summary>
        /// 测试
        /// </summary>
        Testing = 2
    }
}

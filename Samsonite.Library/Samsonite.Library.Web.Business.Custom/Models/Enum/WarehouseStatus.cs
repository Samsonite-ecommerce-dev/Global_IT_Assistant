using System;

namespace Samsonite.Library.Web.Business.Custom.Models
{
    public enum WarehouseStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        UnDeal = 0,
        /// <summary>
        /// 处理中
        /// </summary>
        Dealing = 3,
        /// <summary>
        /// 处理成功
        /// </summary>
        DealSuccessful = 1,
        /// <summary>
        /// 处理失败
        /// </summary>
        DealFail = 2
    }
}

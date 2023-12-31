﻿using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Business.Basic
{
    public interface IApiLogService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryResponse<WebApiAccessLog> GetQuery(ApiLogSearchRequest request);
    }
}

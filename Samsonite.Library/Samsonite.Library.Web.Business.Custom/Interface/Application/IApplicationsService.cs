using Samsonite.Library.Data.Entity.Models;
using System.Collections.Generic;

namespace Samsonite.Library.Web.Business.Custom
{
    public interface IApplicationsService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<ApplicationInfo> GetQuery();

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        List<View_Application> GetDetail(int applicationID);
    }
}

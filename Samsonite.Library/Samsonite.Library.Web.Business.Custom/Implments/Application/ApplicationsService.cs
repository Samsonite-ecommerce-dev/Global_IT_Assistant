using Microsoft.EntityFrameworkCore;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core;
using System.Collections.Generic;
using System.Linq;

namespace Samsonite.Library.Web.Business.Custom
{
    public class ApplicationsService : IApplicationsService
    {
        private IBaseService _baseService;
        private appEntities _appDB;
        public ApplicationsService(IBaseService baseService, appEntities appEntities)
        {
            _baseService = baseService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<ApplicationInfo> GetQuery()
        {
            List<ApplicationInfo> _result = new List<ApplicationInfo>();
            var _list = _appDB.ApplicationInfo.AsQueryable();

            //返回数据
            _result = _list.AsNoTracking().ToList();
            return _result;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        public List<View_Application> GetDetail(int applicationID)
        {
            List<View_Application> _result = new List<View_Application>();
            var _list = _appDB.View_Application.AsQueryable();

            _list = _list.Where(p => p.ApplicationID == applicationID);

            //排序
            _list = _list.OrderBy(p => p.SortID);

            //返回数据
            _result = _list.AsNoTracking().ToList();
            return _result;
        }
    }
}

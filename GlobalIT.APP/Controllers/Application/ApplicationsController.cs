using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Custom;
using Samsonite.Library.Web.Business.Custom.Models;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GlobalIT.APP.Controllers
{
    public class ApplicationsController : BaseController
    {
        private IBaseService _baseService;
        private IApplicationsService _applicationsService;
        private appEntities _appBD;
        public ApplicationsController(IBaseService baseService, IApplicationsService applicationsService, appEntities appEntities) : base(baseService)
        {
            _baseService = baseService;
            _applicationsService = applicationsService;
            _appBD = appEntities;
        }

        #region 初始化
        [HttpGet]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult Initialize_Info(string type, int id)
        {
            if (type == "index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(),
                    //功能权限
                    userAuthorization = this.FunctionPowers()
                });
            }
            else if (type == "detail")
            {
                int _appID = VariableHelper.SaferequestInt(id);
                ApplicationInfo objApplicationInfo = _appBD.ApplicationInfo.Where(p => p.ID == id).SingleOrDefault();
                if (objApplicationInfo != null)
                {
                    //读取详情
                    var details = _applicationsService.GetDetail(objApplicationInfo.SortID);

                    //返回数据
                    return Json(new
                    {
                        //菜单栏
                        navMenu = this.MenuBar(16, new List<string>() { objApplicationInfo.ApplicationName }),
                        //功能权限
                        userAuthorization = this.FunctionPowers(),
                        //详细信息
                        appName = objApplicationInfo.ApplicationName,
                        appDetails = from c in details.GroupBy(p => new { p.CatelogID, p.CatelogName }).Select(o => o.Key).OrderBy(t => t.CatelogID)
                                     select new
                                     {
                                         catelogId = c.CatelogID,
                                         catelogName = c.CatelogName,
                                         environments = from e in details.Where(p => p.CatelogID == c.CatelogID).OrderByDescending(p => p.EnvironmentID).ToList()
                                                        select new
                                                        {
                                                            environmentId = e.EnvironmentID,
                                                            environmentName = e.EnvironmentName,
                                                            environmentContent = e.EnvironmentContent,
                                                            link = (c.CatelogID == (int)CatelogType.WebSite || c.CatelogID == (int)CatelogType.API) ? e.EnvironmentContent : "javascript:void(0)",
                                                            color = (e.EnvironmentID == 1) ? "primary" : "info"
                                                        }


                                     }
                    });
                }
                else
                {
                    return Json(new { });
                }
            }
            else
            {
                return Json(new { });
            }
        }
        #endregion

        #region 查询
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public JsonResult Index_Message()
        {
            //查询
            var _list = _applicationsService.GetQuery();
            var _result = new
            {
                rows = from dy in _list.GroupBy(p => new { p.GroupID, p.GroupName }).Select(o => o.Key)
                       select new
                       {
                           groupID = dy.GroupID,
                           groupName = dy.GroupName,
                           groups = from item in _list.Where(p => p.GroupID == dy.GroupID)
                                    select new
                                    {
                                        appID = item.GroupID,
                                        appName = item.ApplicationName,
                                        appFlag = $"/UploadFile/Flag/{item.GroupIcon}",
                                        appUrl = $"{Url.Action("Detail", "Applications")}?id={item.ID}"
                                    }
                       }
            };
            return Json(_result);
        }
        #endregion

        #region 详情
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public ActionResult Detail(int id)
        {
            //过滤参数
            int _appID = VariableHelper.SaferequestInt(id);
            ApplicationInfo objApplicationInfo = _appBD.ApplicationInfo.Where(p => p.ID == _appID).SingleOrDefault();
            if (objApplicationInfo != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Type = (int)ErrorType.NoMessage });
            }
        }
        #endregion
    }
}
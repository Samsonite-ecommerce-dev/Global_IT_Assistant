using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Custom;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GlobalIT.APP.Controllers
{
    public class OMSAPIToolsController : BaseController
    {
        private IBaseService _baseService;
        private IOMSAPIToolsService _oMSAPIToolsService;
        public OMSAPIToolsController(IBaseService baseService, IOMSAPIToolsService oMSAPIToolsService) : base(baseService)
        {
            _baseService = baseService;
            _oMSAPIToolsService = oMSAPIToolsService;
        }

        #region 初始化
        [HttpGet]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult Initialize_Info(string type)
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
            else if (type == "kr_wms_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Korea WMS API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetKrWMSParams()
                });
            }
            else if (type == "th_wms_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Thailand WMS API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetThWMSParams()
                });
            }
            else if (type == "th_pf_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Thailand Platform API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetThPlatformParams()
                });
            }
            else if (type == "hk_tumi_wms_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Hongkong Tumi WMS API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetHkTumiWMSParams()
                });
            }
            else if (type == "my_wms_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Malaysia WMS API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetMyWMSParams()
                });
            }
            else if (type == "my_pf_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Malaysia Platform API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetMyPlatformParams()
                });
            }
            else if (type == "jpn_wms_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Japan WMS API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetJpnWMSParams()
                });
            }
            else if (type == "jpn_pf_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Japan Platform API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetJpnPlatformParams()
                });
            }
            else if (type == "tw_pf_index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(17, new List<string>() { "Taiwan Platform API" }),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    //默认参数
                    parameterInfo = _oMSAPIToolsService.GetTwPlatformParams()
                });
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
            var _list = _oMSAPIToolsService.GetQuery();
            var _result = new
            {
                rows = from dy in _list.GroupBy(p => new { p.GroupID, p.GroupName }).Select(o => o.Key)
                       select new
                       {
                           groupID = dy.GroupID,
                           groupName = dy.GroupName,
                           groups = from pn in _list.Where(p => p.GroupID == dy.GroupID).GroupBy(p => new { p.ApplicationID, p.ApplicationName }).Select(o => o.Key)
                                    select new
                                    {
                                        projectName = pn.ApplicationName,
                                        environments = from d in _list.Where(p => p.ApplicationID == pn.ApplicationID).OrderBy(p => p.SortID).ToList()
                                                       select new
                                                       {
                                                           name = d.APIName,
                                                           link = d.InterfaceAddress,
                                                           url = d.Url,
                                                           color = "primary"
                                                       }
                                    }
                       }
            };
            return Json(_result);
        }
        #endregion

        #region Korea Api
        #region WMS
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult KR_WMS()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult KR_WMS_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //查询
            var _res = _oMSAPIToolsService.GetKrApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion

        #region Thailand Api
        #region WMS
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult TH_WMS()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult TH_WMS_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 2;

            //查询
            var _res = _oMSAPIToolsService.GetThApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion

        #region Platform
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult TH_PF()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult TH_PF_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 4;

            //查询
            var _res = _oMSAPIToolsService.GetThApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion

        #region Hongkong Tumi Api
        #region WMS
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult HK_TUMI_WMS()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult HK_TUMI_WMS_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 10;

            //查询
            var _res = _oMSAPIToolsService.GetHkTumiApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion

        #region Malaysia Api
        #region WMS
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult MY_WMS()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult MY_WMS_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 13;

            //查询
            var _res = _oMSAPIToolsService.GetMyApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion

        #region Platform
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult MY_PF()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult MY_PF_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 15;

            //查询
            var _res = _oMSAPIToolsService.GetMyApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion

        #region Japan Api
        #region WMS
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult JPN_WMS()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult JPN_WMS_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 16;

            //查询
            var _res = _oMSAPIToolsService.GetJpnApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion

        #region Platform
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult JPN_PF()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult JPN_PF_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 17;

            //查询
            var _res = _oMSAPIToolsService.GetJpnApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion

        #region Taiwan Api
        #region Platform
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public ActionResult TW_PF()
        {
            return View();
        }

        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult TW_PF_Message(OMSApiToolRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //设置API的ID
            request.ApiID = 18;

            //查询
            var _res = _oMSAPIToolsService.GetTwApiResult(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message,
                requestInfo = _res.RequestInfo,
                responseInfo = _res.ResponseInfo
            };
            return Json(_result);
        }
        #endregion
        #endregion
    }
}
using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Basic;
using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Linq;

namespace GlobalIT.APP.Controllers
{
    public class TemplateConfigController : BaseController
    {
        private ITemplateConfigService _templateService;
        private appEntities _appDB;
        public TemplateConfigController(IBaseService baseService, ITemplateConfigService TemplateService, appEntities appEntities) : base(baseService)
        {
            _templateService = TemplateService;
            _appDB = appEntities;
        }

        #region 初始化
        [HttpGet]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(Action = "index")]
        public JsonResult Initialize_Info(string type, int id)
        {
            var _templateTypeList = _templateService.TemplateTypeObject().Select(p => new
            {
                label = p.Value,
                value = p.Key
            });

            if (type == "index")
            {
                //返回数据
                return Json(new
                {
                    //菜单栏
                    navMenu = this.MenuBar(),
                    //功能权限
                    userAuthorization = this.FunctionPowers(),
                    templateTypeList = _templateTypeList
                });
            }
            else if (type == "add")
            {
                //返回数据
                return Json(new
                {
                    templateTypeList = _templateTypeList
                });
            }
            else if (type == "edit")
            {
                //过滤参数
                int _uploadID = VariableHelper.SaferequestInt(id);

                SysTemplate objSysTemplate = _appDB.SysTemplate.Where(p => p.ID == _uploadID).SingleOrDefault();
                if (objSysTemplate != null)
                {
                    //返回数据
                    return Json(new
                    {
                        templateTypeList = _templateTypeList,
                        model = new
                        {
                            id = objSysTemplate.ID,
                            name = objSysTemplate.TemplateName,
                            indentify = objSysTemplate.TemplateIndentify,
                            type = objSysTemplate.TemplateType,
                            title = objSysTemplate.TemplateTitle,
                            content = objSysTemplate.TemplateContent,
                            sender = objSysTemplate.TemplateSender,
                            remark = objSysTemplate.Remark
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

        [HttpPost]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public JsonResult Index_Message(TemplateConfigSearchRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            //查询
            var _list = _templateService.GetQuery(request);
            var _result = new
            {
                total = _list.TotalRecord,
                rows = from dy in _list.Items
                       select new
                       {
                           ck = dy.ID,
                           s1 = dy.TemplateName,
                           s2 = dy.TemplateIndentify,
                           s3 = (dy.TemplateType == (int)TemplateType.SMS) ? "短信" : "邮件",
                           s4 = dy.Remark,
                           s5 = dy.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                       }
            };
            return Json(_result);
        }
        #endregion

        #region 添加
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(IsAntiforgeryToken = true)]
        public JsonResult Add_Message(TemplateConfigAddRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            var _res = _templateService.Add(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message
            };
            return Json(_result);
        }
        #endregion

        #region 编辑
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public ActionResult Edit(int id)
        {
            //过滤参数
            int _uploadID = VariableHelper.SaferequestInt(id);

            SysTemplate objSysTemplate = _appDB.SysTemplate.Where(p => p.ID == _uploadID).SingleOrDefault();
            if (objSysTemplate != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error", new { Type = (int)ErrorType.NoMessage });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(IsAntiforgeryToken = true)]
        public JsonResult Edit_Message(TemplateConfigEditRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            var _res = _templateService.Edit(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message
            };
            return Json(_result);
        }
        #endregion
    }
}
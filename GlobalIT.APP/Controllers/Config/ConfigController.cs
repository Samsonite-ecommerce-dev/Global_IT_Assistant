using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Basic;
using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Linq;

namespace GlobalIT.APP.Controllers
{
    public class ConfigController : BaseController
    {
        private IBaseService _baseService;
        private IConfigService _configService;
        public ConfigController(IBaseService baseService, IConfigService configService) : base(baseService)
        {
            _baseService = baseService;
            _configService = configService;
        }

        #region 配置
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public JsonResult Index_Info()
        {
            //读取配置信息
            var objAppConfig = _configService.GetConfigInfo();

            //返回数据
            return Json(new
            {
                //菜单栏
                navMenu = this.MenuBar(),
                //功能权限
                userAuthorization = this.FunctionPowers(),
                //语言包集合
                languageList = _baseService.LanguageTypeOption().Select(p => new
                {
                    label = p.LanguageName,
                    value = p.LanguageCode
                }).ToList(),
                model = new
                {
                    languagePack = _baseService.LanguageTypeConfigOption().Select(o => o.LanguageCode).ToList(),
                    cookieSaveTime = objAppConfig.LoginConfig.CookieSaveTime,
                    pastNum = objAppConfig.PasswordConfig.PastNum,
                    validityTime = objAppConfig.PasswordConfig.ValidityTime,
                    errorLockNum = objAppConfig.PasswordConfig.ErrorLockNum,
                    mailStmp = objAppConfig.EmailConfig.ServerHost,
                    mailPort = objAppConfig.EmailConfig.Port,
                    mailUserName = objAppConfig.EmailConfig.MailUsername,
                    mailPassword = objAppConfig.EmailConfig.MailPassword,
                    smsHost = objAppConfig.SMSConfig.ServerHost,
                    smsAccount = objAppConfig.SMSConfig.AccountSid,
                    smsAuthToken = objAppConfig.SMSConfig.AuthToken,
                    smsSender = objAppConfig.SMSConfig.Sender,
                    skinStyle = objAppConfig.SkinStyle.ToString()
                }
            }); ;
        }

        [HttpPost]
        [ServiceFilter(typeof(UserPowerAuthorize))]
        public JsonResult Index_Message(ConfigUpdateRequest request)
        {
            //过滤参数
            ValidateHelper.Validate(request);

            var _res = _configService.Update(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message
            };
            return Json(_result);
        }
        #endregion

        #region 重置系统配置缓存
        [ServiceFilter(typeof(UserPowerAuthorize))]
        [AuthorizePropertyAttribute(IsAntiforgeryToken = true)]
        public JsonResult Reset_Message()
        {
            try
            {
                _baseService.ResetConfigCache();
                //返回信息
                return Json(new
                {
                    result = true,
                    msg = "系统配置更新成功"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = false,
                    msg = ex.Message
                });
            }
        }
        #endregion
    }
}
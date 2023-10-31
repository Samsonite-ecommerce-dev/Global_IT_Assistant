using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;

namespace GlobalIT.APP.Controllers
{
    public class LoginController : BaseController
    {
        public IBaseService _baseService;
        private ILoginService _loginService;
        public LoginController(IBaseService baseService, ILoginService loginService) : base(baseService)
        {
            _baseService = baseService;
            _loginService = loginService;
        }

        #region 账号登录
        public ActionResult Index()
        {
            //加载语言包
            ViewBag.LanguagePack = this.GetLanguagePack;

            return View();
        }

        [HttpPost]
        public ActionResult Index_Message(LoginRequest request)
        {
            //过滤参数
            ValidateHelper.Validate<LoginRequest>(request);

            var _res = _loginService.UserLogin(request.UserName, request.Password, request.VerificationCode, request.IsRemember);
            var _result = new
            {
                result = _res.Result,
                loginType = _res.LoginType,
                userEmail = _res.LoginEmail,
                step = _res.Step,
                msg = _res.Message
            };
            return Json(_result);
        }

        [HttpPost]
        public JsonResult Send_VerificationCode(string request)
        {
            var _res = _loginService.SendVerificationCode(request);
            var _result = new
            {
                result = _res.Result,
                msg = _res.Message
            };
            return Json(_result);
        }
        #endregion

        #region  账号退出
        public RedirectResult LoginOut()
        {
            //清空信息
            _loginService.UserLoginOut();
            return Redirect("~/Login/Index");
        }
        #endregion
    }
}
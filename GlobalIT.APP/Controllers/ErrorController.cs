using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Web;

namespace GlobalIT.APP.Controllers
{
    public class ErrorController : Controller
    {
        private IBaseService _baseService;
        public ErrorController(IBaseService baseService)
        {
            _baseService = baseService;
        }

        [HttpGet]
        public JsonResult Initialize_Info(ErrorRequest request)
        {
            //过滤参数
            ValidateHelper.Validate<ErrorRequest>(request);

            int _statusCode = 0;
            string _message = string.Empty;
            switch (request.Type)
            {
                case (int)ErrorType.NoExsit:
                    _statusCode = 400;
                    _message = "对不起,您访问的页面不存在";
                    break;
                case (int)ErrorType.NoMessage:
                    _statusCode = 400;
                    _message = "对不起,该信息不存在";
                    break;
                case (int)ErrorType.NoPower:
                    _statusCode = 400;
                    _message = "对不起,您没有操作权限";
                    break;
                case (int)ErrorType.LoginTimeOut:
                    _statusCode = 400;
                    _message = "Login timeout.please login again.<a href=\"javascript:appVue.goLogin();\" class=\"href-blue-line\">登录</a>";
                    break;
                case (int)ErrorType.Other:
                    _statusCode = (request.StatusCode != 0) ? request.StatusCode : 400;
                    _message = HttpUtility.UrlDecode(request.Message);
                    break;
                default:
                    break;
            }

            //返回数据
            return Json(new
            {
                statusCode = _statusCode,
                errMessage = _message
            });
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
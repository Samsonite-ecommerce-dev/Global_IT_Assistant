using Microsoft.AspNetCore.Mvc.Filters;
using Samsonite.Library.Web.Core;

namespace GlobalIT.APP
{
    public class GlobalExceptions : IExceptionFilter
    {
        private IBaseService _baseService;
        public GlobalExceptions(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public void OnException(ExceptionContext context)
        {
            _baseService.SystemLog(context.Exception.ToString());
        }
    }
}

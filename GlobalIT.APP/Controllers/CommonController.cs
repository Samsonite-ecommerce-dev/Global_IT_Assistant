using GlobalIT.APP.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GlobalIT.APP.Controllers
{
    public class CommonController : BaseController
    {
        private IWebHostEnvironment _webHostEnvironment;
        private QuickTimeHelper _quickTimeHelper;
        public CommonController(IBaseService baseService, IWebHostEnvironment webHostEnvironment) : base(baseService)
        {
            _webHostEnvironment = webHostEnvironment;
            _quickTimeHelper = new QuickTimeHelper(baseService);
        }

        #region 获取icon图标
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(UserLoginAuthorize))]
        public ActionResult SelectIcon()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilter(typeof(UserLoginAuthorize))]
        public JsonResult SelectIcon_Info()
        {
            List<IconModel> _list = new List<IconModel>();
            //读取Icon文件
            XmlDocument _xml = new XmlDocument();
            _xml.Load($"{_webHostEnvironment.WebRootPath}/Content/fonts/awesome/xml/icon.xml");
            XmlNode _root = _xml.SelectSingleNode("root");
            XmlNodeList _nodelist = _root.ChildNodes;
            //存放到字典中
            foreach (XmlNode _n in _nodelist)
            {
                _list.Add(new IconModel
                {
                    Type = _n.Attributes["type"].Value,
                    Value = _n.Attributes["value"].Value,
                    Css = _n.Attributes["class"].Value
                });
            }

            return Json(new
            {
                iconList = from item in _list.GroupBy(p => p.Type).Select(o => o.Key).ToList()
                           select new
                           {
                               type = item,
                               icons = from icon in _list.Where(p => p.Type == item).ToList()
                                       select new
                                       {
                                           css = icon.Css,
                                           value = icon.Value
                                       }
                           }
            });
        }
        #endregion

        #region 快捷时间值
        /// <summary>
        /// 获取快捷时间值
        /// </summary>
        /// <returns></returns>
        [ServiceFilter(typeof(UserLoginAuthorize))]
        public JsonResult QuickTime_Message(int type, string format)
        {
            type = VariableHelper.SaferequestInt(type);
            format = VariableHelper.SaferequestStr(format);
            string[] _times = _quickTimeHelper.GetQuickTime(type);
            var _result = new
            {
                t1 = (!string.IsNullOrEmpty(format)) ? VariableHelper.SaferequestTime(_times[0]).ToString(format) : _times[0],
                t2 = (!string.IsNullOrEmpty(format)) ? VariableHelper.SaferequestTime(_times[1]).ToString(format) : _times[1],
            };
            return Json(_result);
        }
        #endregion

    }
}
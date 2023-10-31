using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GlobalIT.APP.Helper
{
    public class UserHelper
    {
        private IBaseService _baseService;
        public UserHelper(IBaseService baseService)
        {
            _baseService = baseService;
        }

        #region 账号类型
        /// <summary>
        /// 账号类型集合
        /// </summary>
        /// <returns></returns>
        private List<DefineEnum> UserTypeReflect()
        {
            //加载语言包
            var _languagePack = _baseService.CurrentLanguagePack;

            List<DefineEnum> _result = new List<DefineEnum>()
            {
                new DefineEnum() { ID = (int)UserType.InternalStaff, Display = _languagePack["users_index_type_1"], Css = "text-primary" },
                new DefineEnum() { ID = (int)UserType.Customer, Display = _languagePack["users_index_type_2"], Css = "text-warning" }
            };
            return _result;
        }

        /// <summary>
        /// 全部账号类型列表(包换客户)
        /// </summary>
        /// <returns></returns>
        public List<DefineSelectOption> UserTypeObject()
        {
            List<DefineSelectOption> _result = new List<DefineSelectOption>();
            foreach (var _o in UserTypeReflect())
            {
                _result.Add(new DefineSelectOption() { Label = _o.Display, Value = _o.ID });
            }
            return _result;
        }

        /// <summary>
        /// 账号类型显示值
        /// </summary>
        /// <param name="status"></param>
        /// <param name="css"></param>
        /// <returns></returns>
        public string GetUserTypeDisplay(int status, bool css = false)
        {
            string _result = string.Empty;
            DefineEnum _O = UserTypeReflect().Where(p => p.ID == status).SingleOrDefault();
            if (_O != null)
            {
                if (css)
                {
                    _result = string.Format("<label class=\"{0}\">{1}</label>", _O.Css, _O.Display);
                }
                else
                {
                    _result = _O.Display;
                }
            }
            return _result;
        }
        #endregion

        #region 登录方式
        /// <summary>
        /// 登录方式集合
        /// </summary>
        /// <returns></returns>
        private List<DefineEnum> UserLoginTypeReflect()
        {
            //加载语言包
            var _languagePack = _baseService.CurrentLanguagePack;

            List<DefineEnum> _result = new List<DefineEnum>();
            _result.Add(new DefineEnum() { ID = (int)UserLoginType.Account, Display = _languagePack["users_index_login_type_1"], Css = "color_primary" });
            //_result.Add(new DefineEnum() { ID = (int)UserLoginType.MFA, Display = _languagePack["users_index_login_type_2"], Css = "color_success" });
            _result.Add(new DefineEnum() { ID = (int)UserLoginType.VerificationCode, Display = _languagePack["users_index_login_type_3"], Css = "color_warning" });
            return _result;
        }

        /// <summary>
        /// 登录方式列表
        /// </summary>
        /// <returns></returns>
        public List<DefineSelectOption> UserLoginTypeObject()
        {
            List<DefineSelectOption> _result = new List<DefineSelectOption>();
            foreach (var _o in UserLoginTypeReflect())
            {
                _result.Add(new DefineSelectOption() { Label = _o.Display, Value = _o.ID });
            }
            return _result;
        }

        /// <summary>
        /// 登录方式显示值
        /// </summary>
        /// <param name="status"></param>
        /// <param name="css"></param>
        /// <returns></returns>
        public string GetUserLoginTypeDisplay(int status, bool css = false)
        {
            string _result = string.Empty;
            DefineEnum _O = UserLoginTypeReflect().Where(p => p.ID == status).SingleOrDefault();
            if (_O != null)
            {
                if (css)
                {
                    _result = string.Format("<label class=\"{0}\">{1}</label>", _O.Css, _O.Display);
                }
                else
                {
                    _result = _O.Display;
                }
            }
            return _result;
        }
        #endregion
    }
}
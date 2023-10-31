using Microsoft.AspNetCore.Http;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Samsonite.Library.Web.Core
{
    public class AppUserCore : AppStorageCore
    {
        private IAppSession _appSession;
        private IAppCookie _appCookie;
        private IHttpContextAccessor _httpContextAccessor;
        private appEntities _appDB;
        public AppUserCore(IAppCache appCache, IAppSession appSession, IAppCookie appCookie, IHttpContextAccessor httpContextAccessor, appEntities appEntities) : base(appCache, appCookie, appEntities)
        {
            _appSession = appSession;
            _appCookie = appCookie;
            _httpContextAccessor = httpContextAccessor;
            _appDB = appEntities;
        }

        #region 当前用户登录信息
        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        /// <returns></returns>
        public UserSessionModel GetCurrentLoginUser()
        {
            UserSessionModel userSessionModel = null;
            try
            {
                string _sessionKey = _appSession.Get(AppGlobalConst.USER_LOGIN_SESSION_NAME);
                if (!string.IsNullOrEmpty(_sessionKey))
                {
                    userSessionModel = JsonSerializer.Deserialize<UserSessionModel>(_sessionKey);
                }
                else
                {
                    //如果不存在,则去读cookie
                    string _userName = _appCookie.Get(AppGlobalConst.USER_LOGIN_ACCOUNT_COOKIE_NAME);
                    string _userPass = _appCookie.Get(AppGlobalConst.USER_LOGIN_PWD_COOKIE_NAME);
                    if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_userPass))
                    {
                        userSessionModel = GetUserMessage(_userName, _userPass);
                    }
                }
            }
            catch
            {
                userSessionModel = null;
            }
            return userSessionModel;
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentUserID()
        {
            var userSessionModel = this.GetCurrentLoginUser();
            return (userSessionModel != null) ? userSessionModel.Userid : 0;
        }

        /// <summary>
        /// 当前语言包ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentLanguageType()
        {
            string _lgType = _appCookie.Get(AppGlobalConst.USER_CURRENT_LANGUAGE_PACK_COOKIE_NAME);
            if (!string.IsNullOrEmpty(_lgType))
            {
                return VariableHelper.SaferequestInt(_lgType);
            }
            else
            {
                //如果不存在cookie,则读取用户的默认语言
                UserSessionModel objUserSessionModel = this.GetCurrentLoginUser();
                if (objUserSessionModel != null)
                {
                    return objUserSessionModel.DefaultLanguage;
                }
                else
                {
                    //如果没有设置语言,则获取默认语言包
                    return this.LanguageTypes().Where(p => p.IsDefault).SingleOrDefault().ID;
                }
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public string GetCurrentRequestIP()
        {
            return HttpHelper.GetRequestIP(_httpContextAccessor.HttpContext);
        }

        /// <summary>
        /// 根据账户和密码(md5加密后)获取信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPass"></param>
        /// <returns></returns>
        public UserSessionModel GetUserMessage(string userName, string userPass)
        {
            UserSessionModel userSessionModel = new UserSessionModel();
            UserInfo objUserInfo = _appDB.UserInfo.SingleOrDefault<UserInfo>(p => p.UserName == userName && p.Pwd == userPass.ToLower());
            if (objUserInfo != null)
            {
                userSessionModel = this.GetUserModel(objUserInfo);
                //写入Session
                SetUserSession(userSessionModel);
            }
            else
            {
                userSessionModel = null;
            }
            return userSessionModel;
        }

        /// <summary>
        /// 根据获取当前页面的FuncID
        /// </summary>
        /// <param name="areas">如果是Area目录在的,此处需要增加Area目录名</param>
        /// <returns></returns>
        public int GetCurrentFunctionID(string areas = "")
        {
            string _currentUrl = HttpHelper.GetAbsolutePath(_httpContextAccessor.HttpContext);
            int t = _currentUrl.IndexOf("?");
            if (t > -1) _currentUrl = _currentUrl.Substring(0, t);
            int _FunID = 0;
            if (!string.IsNullOrEmpty(_currentUrl))
            {
                //取功能标识
                if (_currentUrl.IndexOf("/") > -1)
                {
                    string[] _urlArray = _currentUrl.Split('/');
                    string _func_sign = string.Empty;
                    //因为有时候会省略index,所以需要加以分别判断
                    if (!string.IsNullOrEmpty(areas))
                    {
                        if (areas.ToUpper() == _urlArray[1].ToUpper())
                        {
                            _func_sign = _urlArray[2];
                        }
                        else
                        {
                            _func_sign = _urlArray[1];
                        }
                    }
                    else
                    {
                        _func_sign = _urlArray[1];
                    }

                    SysFunction objSysFunction = _appDB.SysFunction.Where(p => p.FuncSign.ToLower() == _func_sign.ToLower()).SingleOrDefault();
                    if (objSysFunction != null)
                    {
                        _FunID = objSysFunction.Funcid;
                    }
                }
            }
            return _FunID;
        }

        /// <summary>
        /// 获取功能权限
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public List<string> GetFunctionPowers(int functionID)
        {
            List<string> _result = new List<string>();
            UserSessionModel objUserSessionModel = this.GetCurrentLoginUser();
            UserSessionModel.UserPower objUserPower = objUserSessionModel.UserPowers.Where(p => p.FunctionID == functionID).FirstOrDefault();
            if (objUserPower != null)
            {
                _result = objUserPower.FunctionPower;
            }
            return _result;
        }

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="languagePack"></param>
        /// <param name="query"></param>
        /// <param name="additions"></param>
        /// <returns></returns>
        public string GetMenuBar(int functionID, Dictionary<string, string> languagePack, string query = "", List<string> additions = null)
        {
            string _result = string.Empty;
            SysFunction objSysFunction = _appDB.SysFunction.Where(p => p.Funcid == functionID).SingleOrDefault();
            if (objSysFunction != null)
            {
                string _url = $"/{objSysFunction.FuncUrl}";
                if (!string.IsNullOrEmpty(query))
                {
                    _url += "?" + query;
                }
                SysFunctionGroup objSysFunctionGroup = _appDB.SysFunctionGroup.Where(p => p.Groupid == objSysFunction.Groupid).SingleOrDefault();
                if (objSysFunctionGroup != null)
                {
                    _result = string.Format("<div class=\"main-top-category\">{0}</div>", languagePack[$"menu_function_{objSysFunction.Funcid}"]);
                    _result += string.Format("<ol><li>{0}</li><li class=\"split\">/</li><li><a href=\"{1}\">{2}</a></li>", languagePack[$"menu_group_{objSysFunctionGroup.Groupid}"], _url, languagePack[$"menu_function_{objSysFunction.Funcid}"]);
                    if (additions != null)
                    {
                        foreach (var addition in additions)
                        {
                            _result += string.Format("<li class=\"split\">/</li><li>{0}</li>", addition);
                        }
                    }
                    _result += "</ol>";
                }
            }
            return _result;
        }

        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="languagePack"></param>
        /// <returns></returns>
        public string GetMenuName(int functionID, Dictionary<string, string> languagePack)
        {
            string _result = string.Empty;
            SysFunction objSysFunction = _appDB.SysFunction.Where(p => p.Funcid == functionID).SingleOrDefault();
            if (objSysFunction != null)
            {
                _result = languagePack[$"menu_function_{objSysFunction.Funcid}"];
            }
            return _result;
        }

        /// <summary>
        /// 获取订单查询页面菜单Tab
        /// </summary>
        /// <param name="languagePack"></param>
        /// <returns></returns>
        public string GetSearchOrderTab(Dictionary<string, string> languagePack)
        {
            string _result = string.Empty;
            SysFunction objSysFunction = _appDB.SysFunction.Where(p => p.Funcid == 1).SingleOrDefault();
            if (objSysFunction != null)
            {
                _result = languagePack[$"menu_function_{objSysFunction.Funcid}"];
            }
            return _result;
        }
        #endregion

        #region 账号登录
        /// <summary>
        /// 获取User模型
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public UserSessionModel GetUserModel(UserInfo userInfo)
        {
            //读取权重集合
            var userWeights = (from ur in _appDB.UserRoles.Where(p => p.Userid == userInfo.Userid)
                               join sr in _appDB.SysRole on ur.Roleid equals sr.Roleid
                               select sr.RoleWeight).ToList();

            return new UserSessionModel()
            {
                Userid = userInfo.Userid,
                UserName = userInfo.UserName,
                Passwd = userInfo.Pwd,
                UserType = userInfo.Type,
                RoleWeight = userWeights.Any() ? userWeights.Min() : 99,
                UserStatus = userInfo.Status,
                UserPowers = this.GetUserFunctions(userInfo.Userid),
                DefaultLanguage = userInfo.DefaultLanguage
            };
        }

        /// <summary>
        /// 保存用户登录Session
        /// </summary>
        /// <param name="userSessionModel"></param>
        public void SetUserSession(UserSessionModel userSessionModel)
        {
            _appSession.Set(AppGlobalConst.USER_LOGIN_SESSION_NAME, JsonSerializer.Serialize<UserSessionModel>(userSessionModel));
        }

        /// <summary>
        /// 保存用户登录Cookie
        /// </summary>
        /// <param name="userSessionModel"></param>
        /// <param name="time"></param>
        public void SetUserCookie(UserSessionModel userSessionModel, int time)
        {
            //插入cookie
            _appCookie.Set(AppGlobalConst.USER_LOGIN_ACCOUNT_COOKIE_NAME, userSessionModel.UserName, time);
            _appCookie.Set(AppGlobalConst.USER_LOGIN_PWD_COOKIE_NAME, userSessionModel.Passwd, time);
        }

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="userid"></param>
        private List<UserSessionModel.UserPower> GetUserFunctions(int userid)
        {
            List<UserSessionModel.UserPower> _result = new List<UserSessionModel.UserPower>();
            //获取用户权限组
            List<int> objUserRoless = _appDB.UserRoles.Where(p => p.Userid == userid).Select(p => p.Roleid).ToList();
            if (objUserRoless.Count > 0)
            {
                //获取具体权限
                var objSysRoleFunctions = _appDB.SysRoleFunction.Where(p => objUserRoless.Contains(p.Roleid));
                foreach (var _o in objSysRoleFunctions)
                {
                    List<string> _OperPowers = new List<string>();
                    //操作权限
                    foreach (string _str in _o.Powers.Split(','))
                    {
                        _OperPowers.Add(_str.ToLower());
                    }
                    var _p = _result.Where(p => p.FunctionID == _o.Funid).FirstOrDefault();
                    if (_p != null)
                    {
                        _p.FunctionPower = this.MergePowers(_p.FunctionPower, _OperPowers);
                    }
                    else
                    {
                        _result.Add(new UserSessionModel.UserPower() { FunctionID = _o.Funid, FunctionPower = _OperPowers });
                    }
                }
            }
            return _result;
        }

        /// <summary>
        /// 合并多个操作权限集合
        /// </summary>
        /// <param name="orgPower"></param>
        /// <param name="newPower"></param>
        /// <returns></returns>
        private List<string> MergePowers(List<string> orgPower, List<string> newPower)
        {
            foreach (string _str in newPower)
            {
                if (!orgPower.Contains(_str))
                {
                    orgPower.Add(_str.ToLower());
                }
            }
            return orgPower;
        }
        #endregion
    }
}
using Microsoft.AspNetCore.Http;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class BaseService : AppLogCore, IBaseService
    {
        public BaseService(IAppCache appCache, IAppSession appSession, IAppCookie appCookie, IHttpContextAccessor httpContextAccessor, appEntities appEntities, logEntities logEntities) : base(appCache, appSession, appCookie, httpContextAccessor, appEntities, logEntities)
        {

        }

        #region 系统配置信息
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <returns></returns>
        public void LoadConfigCache()
        {
            this.LoadAppConfigCache();
        }

        /// <summary>
        /// 重置系统配置缓存
        /// </summary>
        public void ResetConfigCache()
        {
            this.ResetAppConfigCache();
        }

        /// <summary>
        /// 从缓存中获取应用配置信息
        /// </summary>
        public AppConfigModel CurrentApplicationConfig
        {
            get
            {
                return this.GetAppConfigCache();
            }
        }
        #endregion

        #region 系统语言信息
        /// <summary>
        /// 语言分类集合
        /// </summary>
        /// <returns></returns>
        public List<LanguageType> LanguageTypeOption()
        {
            //默认语言为英文
            //js文件名称默认值有中文简体,中文繁体和英文
            return this.LanguageTypes();
        }

        /// <summary>
        /// 根据站点配置获取当前语言分类列表
        /// </summary>
        /// <returns></returns>
        public List<LanguageType> LanguageTypeConfigOption()
        {
            return this.LanguageTypes().Where(p => this.GetSysConfig.LanguageTypes.Contains(p.ID)).ToList();
        }

        /// <summary>
        /// 初始化语言缓存
        /// </summary>
        public void LoadLanguagePackCache()
        {
            this.LoadAppLanguagePackCache();
        }

        /// <summary>
        /// 重置语言包缓存
        /// </summary>
        public void ResetLanguagePackCache()
        {
            this.ResetAppLanguagePackCache();
        }
        #endregion

        #region 当前用户登录信息
        /// <summary>
        /// 当前语言包ID
        /// </summary>
        /// <returns></returns>
        public int CurrentLanguageType
        {
            get
            {
                return this.GetCurrentLanguageType();
            }
        }

        /// <summary>
        /// 设置当前用户语言包
        /// </summary>
        /// <param name="type"></param>
        public void SaveCurrentLanguagePack(int type)
        {
            this.SaveUserLanguagePack(type);
        }

        /// <summary>
        /// 获取当前加载语言文件后缀
        /// </summary>
        /// <returns></returns>
        public string CurrentLanguageFileExt
        {
            get
            {
                return this.LanguageTypes().Where(p => p.ID == this.CurrentLanguageType).SingleOrDefault().Lang;
            }
        }

        /// <summary>
        /// 获取语言包
        /// </summary>
        public Dictionary<string, string> CurrentLanguagePack
        {
            get
            {
                return this.GetLanguagePack(this.CurrentLanguageType);
            }
        }

        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        public UserSessionModel CurrentLoginUser
        {
            get
            {
                return this.GetCurrentLoginUser();
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        public int CurrentUserID
        {
            get
            {
                return this.GetCurrentUserID();
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public string CurrentRequestIP
        {
            get
            {
                return this.GetCurrentRequestIP();
            }
        }

        /// <summary>
        /// 获取User模型
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public UserSessionModel UserModel(UserInfo userInfo)
        {
            return this.GetUserModel(userInfo);
        }

        /// <summary>
        /// 当前页面ID
        /// </summary>
        public int CurrentFunctionID
        {
            get
            {
                return this.GetCurrentFunctionID();
            }
        }

        /// <summary>
        /// 获取当前功能栏权限
        /// </summary>
        /// <returns></returns>
        public List<string> FunctionPowers()
        {
            return this.GetFunctionPowers(this.GetCurrentFunctionID());
        }

        /// <summary>
        /// 获取当前功能栏权限
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public List<string> FunctionPowers(int functionID)
        {
            return this.GetFunctionPowers(functionID);
        }

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string MenuBar(string query = "")
        {
            return this.GetMenuBar(this.GetCurrentFunctionID(), this.CurrentLanguagePack, query);
        }

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public string MenuBar(int functionID, string query = "")
        {
            return this.GetMenuBar(functionID, this.CurrentLanguagePack, query);
        }

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="additions"></param>
        /// <returns></returns>
        public string MenuBar(int functionID, List<string> additions)
        {
            return this.GetMenuBar(functionID, this.CurrentLanguagePack, "", additions);
        }

        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public string MenuName(int functionID)
        {
            return this.GetMenuName(functionID, this.CurrentLanguagePack);
        }

        /// <summary>
        /// 获取订单查询页面菜单Tab
        /// </summary>
        /// <returns></returns>
        public string GetSearchOrderTab()
        {
            return this.GetSearchOrderTab(this.CurrentLanguagePack);
        }
        #endregion

        #region 系统日志
        #region 系统操作日志
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void InsertLog<T>(T data, string record)
        {
            this.InsertAppLog(data, record);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void InsertLog<T>(List<T> data, string record)
        {
            this.InsertAppLog(data, record);
        }

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void UpdateLog<T>(T data, string record)
        {
            this.UpdateAppLog(data, record);
        }

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void UpdateLog<T>(List<T> data, string record)
        {
            this.UpdateAppLog(data, record);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        public void DeleteLog(string tableName, string record, string message = "")
        {
            this.DeleteAppLog(tableName, record, message);
        }

        /// <summary>
        /// 还原日志
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        public void RestoreLog(string tableName, string record, string message = "")
        {
            this.RestoreAppLog(tableName, record, message);
        }

        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        public void SystemLog(string message, string logLevel = "")
        {
            this.SystemAppLog(message, logLevel);
        }
        #endregion

        #region 用户登入日志
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="webAppLoginLog"></param>
        public void LoginLog(WebAppLoginLog webAppLoginLog)
        {
            this.LoginAppLog(webAppLoginLog);
        }

        /// <summary>
        /// 密码修改日志
        /// </summary>
        /// <param name="objWebAppPasswordLog"></param>
        public void PasswordLog(WebAppPasswordLog objWebAppPasswordLog)
        {
            this.PasswordAppLog(objWebAppPasswordLog);
        }
        #endregion
        #endregion
    }
}
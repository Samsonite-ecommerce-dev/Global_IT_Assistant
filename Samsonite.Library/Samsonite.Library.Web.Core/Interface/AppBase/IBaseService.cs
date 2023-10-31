using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;

namespace Samsonite.Library.Web.Core
{
    public interface IBaseService
    {
        #region 系统配置信息
        /// <summary>
        /// 加载系统配置缓存
        /// </summary>
        void LoadConfigCache();

        /// <summary>
        /// 重置系统配置缓存
        /// </summary>
        void ResetConfigCache();

        /// <summary>
        /// 从缓存中获取系统配置信息
        /// </summary>
        AppConfigModel CurrentApplicationConfig { get; }
        #endregion

        #region 系统语言信息
        /// <summary>
        /// 语言分类集合
        /// </summary>
        /// <returns></returns>
        List<LanguageType> LanguageTypeOption();

        /// <summary>
        /// 根据站点配置获取当前语言分类列表
        /// </summary>
        /// <returns></returns>
        List<LanguageType> LanguageTypeConfigOption();

        /// <summary>
        /// 初始化语言缓存
        /// </summary>
        /// <returns></returns>
        void LoadLanguagePackCache();

        /// <summary>
        /// 重置语言包缓存
        /// </summary>
        void ResetLanguagePackCache();
        #endregion

        #region 当前用户登录信息
        /// <summary>
        /// 当前语言包ID
        /// </summary>
        /// <returns></returns>
        int CurrentLanguageType { get; }

        /// <summary>
        /// 设置当前用户语言包
        /// </summary>
        /// <param name="type"></param>
        void SaveCurrentLanguagePack(int type);

        /// <summary>
        /// 获取当前加载语言文件后缀
        /// </summary>
        /// <returns></returns>
        string CurrentLanguageFileExt { get; }

        /// <summary>
        /// 获取语言包
        /// </summary>
        Dictionary<string, string> CurrentLanguagePack { get; }

        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        UserSessionModel CurrentLoginUser { get; }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        string CurrentRequestIP { get; }

        /// <summary>
        /// 获取User模型
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        UserSessionModel UserModel(UserInfo userInfo);

        /// <summary>
        /// 当前页面ID
        /// </summary>
        int CurrentFunctionID { get; }

        /// <summary>
        /// 获取当前功能栏权限
        /// </summary>
        /// <returns></returns>
        List<string> FunctionPowers();

        /// <summary>
        /// 获取当前功能栏权限
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        List<string> FunctionPowers(int functionID);

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string MenuBar(string query = "");

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        string MenuBar(int functionID, string query = "");

        /// <summary>
        /// 获取当前导航栏
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="additions"></param>
        /// <returns></returns>
        string MenuBar(int functionID, List<string> additions);

        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        string MenuName(int functionID);

        /// <summary>
        /// 获取订单查询页面菜单Tab
        /// </summary>
        /// <returns></returns>
        string GetSearchOrderTab();
        #endregion

        #region 系统日志
        #region 系统操作日志
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        void InsertLog<T>(T data, string record);

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void InsertLog<T>(List<T> data, string record);

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        public void UpdateLog<T>(T data, string record);

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="record"></param>
        void UpdateLog<T>(List<T> data, string record);

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        void DeleteLog(string tableName, string record, string message = "");

        /// <summary>
        /// 还原日志
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="record"></param>
        /// <param name="message"></param>
        void RestoreLog(string tableName, string record, string message = "");

        /// <summary>
        /// 系统日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        void SystemLog(string message, string logLevel = "");
        #endregion

        #region 用户登入日志
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="webAppLoginLog"></param>
        void LoginLog(WebAppLoginLog webAppLoginLog);

        /// <summary>
        /// 密码修改日志
        /// </summary>
        /// <param name="objWebAppPasswordLog"></param>
        void PasswordLog(WebAppPasswordLog objWebAppPasswordLog);
        #endregion
        #endregion
    }
}

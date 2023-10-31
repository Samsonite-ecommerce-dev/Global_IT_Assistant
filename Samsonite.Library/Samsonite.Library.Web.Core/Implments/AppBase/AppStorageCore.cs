using Microsoft.EntityFrameworkCore;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class AppStorageCore : AppLanguageCore
    {
        private IAppCache _appCache;
        private IAppCookie _appCookie;
        private appEntities _appDB;
        public AppStorageCore(IAppCache appCache, IAppCookie appCookie, appEntities appEntities) : base(appEntities)
        {
            _appCache = appCache;
            _appCookie = appCookie;
            _appDB = appEntities;
        }

        #region 配置信息缓存
        /// <summary>
        /// 初始化配置信息
        /// </summary>
        /// <returns></returns>
        public void LoadAppConfigCache()
        {
            var _cacheObject = _appCache.Get(AppGlobalConst.CONFIG_CACHE_NAME);
            if (_cacheObject == null)
            {
                //写入缓存
                _appCache.Set(AppGlobalConst.CONFIG_CACHE_NAME, this.GetAppConfig, AppGlobalConst.CONFIG_CACHE_TIME);
            }
        }

        /// <summary>
        /// 重置系统配置缓存
        /// </summary>
        public void ResetAppConfigCache()
        {
            var _cacheObject = _appCache.Get(AppGlobalConst.CONFIG_CACHE_NAME);
            if (_cacheObject != null)
            {
                _appCache.Remove(AppGlobalConst.CONFIG_CACHE_NAME);
            }
            //重新插入缓存
            _appCache.Set(AppGlobalConst.CONFIG_CACHE_NAME, this.GetAppConfig, AppGlobalConst.CONFIG_CACHE_TIME);
        }

        /// <summary>
        /// 从缓存中读取全局应用配置信息(包含全局静态配置和全局动态参数配置)
        /// </summary>
        public AppConfigModel GetAppConfigCache()
        {
            AppConfigModel _result = new AppConfigModel();
            var _cacheObject = _appCache.Get(AppGlobalConst.CONFIG_CACHE_NAME);
            if (_cacheObject != null)
            {
                _result = (AppConfigModel)_cacheObject;
            }
            else
            {
                _result = this.GetAppConfig;
                //写入缓存
                _appCache.Set(AppGlobalConst.CONFIG_CACHE_NAME, _result, AppGlobalConst.CONFIG_CACHE_TIME);
            }
            return _result;
        }
        #endregion

        #region 语言包缓存
        /// <summary>
        /// 初始化语言缓存
        /// </summary>
        public void LoadAppLanguagePackCache()
        {
            //从数据库读取语言包
            var _loadLanguageTypes = this.GetSysConfig.LanguageTypes;
            List<View_LanguagePack> objView_LanguagePack_List = _appDB.View_LanguagePack.Where(p => !p.IsDelete).AsNoTracking().ToList();
            foreach (var _O in this.LanguageTypes())
            {
                if (_loadLanguageTypes.Contains(_O.ID))
                {
                    //单个语言包缓存格式为_cacheName+'_'+语言包ID
                    var _cacheObject = _appCache.Get($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{_O.ID}");
                    if (_cacheObject == null)
                    {
                        this.InsertLanguagePackCache(objView_LanguagePack_List, _O.ID);
                    }
                }
            }
        }

        /// <summary>
        /// 重置语言包缓存
        /// </summary>
        public void ResetAppLanguagePackCache()
        {
            //从数据库读取语言包
            var _loadLanguageTypes = this.GetSysConfig.LanguageTypes;
            List<View_LanguagePack> objView_LanguagePack_List = _appDB.View_LanguagePack.Where(p => !p.IsDelete).AsNoTracking().ToList();
            foreach (var _O in this.LanguageTypes())
            {
                if (_loadLanguageTypes.Contains(_O.ID))
                {
                    //单个语言包缓存格式为_CacheName+'_'+语言包ID
                    var _cacheObject = _appCache.Get($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{_O.ID}");
                    if (_cacheObject != null)
                    {
                        _appCache.Remove($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{_O.ID}");
                    }
                    //重新插入缓存
                    this.InsertLanguagePackCache(objView_LanguagePack_List, _O.ID);
                }
            }
        }

        /// <summary>
        /// 写入语言包缓存
        /// </summary>
        /// <param name="packs"></param>
        /// <param name="typeID"></param>
        private void InsertLanguagePackCache(List<View_LanguagePack> packs, int typeID)
        {
            var objLanguageType = this.LanguageTypes().Where(p => p.ID == typeID).SingleOrDefault();
            if (objLanguageType != null)
            {
                var _languagePacks = packs.Where(p => p.LanguageTypeID == typeID).ToList();
                try
                {
                    Dictionary<string, string> _pack = new Dictionary<string, string>();
                    foreach (var item in _languagePacks)
                    {
                        _pack.Add(item.PackKey, item.PackValue);
                    }
                    _appCache.Set($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{objLanguageType.ID}", _pack, AppGlobalConst.LANGUAGE_PACK_CACHE_TIME);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 根据类型获取语言包
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetLanguagePack(int type)
        {
            Dictionary<string, string> _result = new Dictionary<string, string>();
            try
            {
                object _cacheObject = _appCache.Get($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{type}");
                if (_cacheObject != null)
                {
                    _result = (Dictionary<string, string>)_cacheObject;
                }
                else
                {
                    //从数据库读取语言包
                    List<View_LanguagePack> _languagePacks = _appDB.View_LanguagePack.Where(p => !p.IsDelete && p.LanguageTypeID == type).AsNoTracking().ToList();
                    foreach (var item in _languagePacks)
                    {
                        _result.Add(item.PackKey, item.PackValue);
                    }
                    //写入缓存
                    _appCache.Set($"{AppGlobalConst.LANGUAGE_PACK_CACHE_NAME}_{type}", _result, AppGlobalConst.LANGUAGE_PACK_CACHE_TIME);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

        /// <summary>
        /// 设置当前用户语言包
        /// </summary>
        /// <param name="type"></param>
        public void SaveUserLanguagePack(int type)
        {
            _appCookie.Set(AppGlobalConst.USER_CURRENT_LANGUAGE_PACK_COOKIE_NAME, type.ToString(), AppGlobalConst.USER_CURRENT_LANGUAGE_PACK_COOKIE_TIME);
        }
        #endregion 
    }
}
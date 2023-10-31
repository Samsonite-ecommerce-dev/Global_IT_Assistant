using Microsoft.EntityFrameworkCore;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Samsonite.Library.Web.Core
{
    public class AppConfigCore
    {
        private appEntities _appDB;
        public AppConfigCore(appEntities appEntities)
        {
            _appDB = appEntities;
        }

        #region 全局动态参数配置
        /// <summary>
        /// 读取系统配置信息
        /// </summary>
        /// <returns></returns>
        private SysConfigModel GetSysBasicConfig()
        {
            //读取所有配置信息
            List<SysConfig> objSysConfigs = _appDB.SysConfig.AsNoTracking().ToList();
            return new SysConfigModel()
            {
                LanguageTypes = GetLanguagePackConfig(objSysConfigs),
                LoginConfig = GetLoginConfig(objSysConfigs),
                PasswordConfig = GetPasswordConfig(objSysConfigs),
                EmailConfig = GetEmailConfig(objSysConfigs),
                SMSConfig = GetSMSConfig(objSysConfigs),
                SkinStyle = GetSkinStyleConfig(objSysConfigs)
            };
        }

        /// <summary>
        /// 获取语言包配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        /// <returns></returns>
        private List<int> GetLanguagePackConfig(List<SysConfig> sysConfigList)
        {
            List<int> _result = new List<int>();
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.LANGUAGE_PACK_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                //如果值为空,那么Split之后仍然会生成一个数组
                if (!string.IsNullOrEmpty(objSysConfig.ConfigValue))
                {
                    //语言集合
                    var objLanguagePacks = _appDB.LanguageType.ToList();

                    string[] _value = objSysConfig.ConfigValue.Split('|');
                    foreach (string _str in _value)
                    {
                        var _o = objLanguagePacks.Where(p => p.LanguageCode == _str).SingleOrDefault();
                        if (_o != null)
                        {
                            _result.Add(_o.ID);
                        }
                    }
                }
            }
            return _result;
        }

        /// <summary>
        /// 登录配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        private LoginConfigModel GetLoginConfig(List<SysConfig> sysConfigList)
        {
            var _result = new LoginConfigModel();
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.LOGIN_CONFIG_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                _result = JsonSerializer.Deserialize<LoginConfigModel>(objSysConfig.ConfigValue);
            }
            return _result;
        }

        /// <summary>
        /// 密码规则配置配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        private PasswordConfigModel GetPasswordConfig(List<SysConfig> sysConfigList)
        {
            var _result = new PasswordConfigModel();
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.PASSWORD_CONFIG_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                _result = JsonSerializer.Deserialize<PasswordConfigModel>(objSysConfig.ConfigValue);
            }
            return _result;
        }

        /// <summary>
        /// 邮件配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        private EmailModel GetEmailConfig(List<SysConfig> sysConfigList)
        {
            var _result = new EmailModel();
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.EMAIL_CONFIG_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                _result = JsonSerializer.Deserialize<EmailModel>(objSysConfig.ConfigValue);
            }
            return _result;
        }

        /// <summary>
        /// 短信配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        private SMSModel GetSMSConfig(List<SysConfig> sysConfigList)
        {
            var _result = new SMSModel();
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.SMS_CONFIG_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                _result = JsonSerializer.Deserialize<SMSModel>(objSysConfig.ConfigValue);
            }
            return _result;
        }

        /// <summary>
        /// 皮肤配置
        /// </summary>
        /// <param name="sysConfigList"></param>
        private string GetSkinStyleConfig(List<SysConfig> sysConfigList)
        {
            string _result = string.Empty;
            var objSysConfig = sysConfigList.Where(p => p.ConfigKey.ToUpper() == AppGlobalConst.SKIN_STYLE_KEY).SingleOrDefault();
            if (objSysConfig != null)
            {
                _result = objSysConfig.ConfigValue;
            }
            return _result;
        }
        #endregion

        /// <summary>
        /// 全局静态配置
        /// </summary>
        /// <returns></returns>
        public GlobalConfigModel GetGlobalConfig
        {
            get
            {
                return AppGlobalConst.GetGlobalStaticConfig;
            }
        }

        /// <summary>
        /// 全局动态参数配置
        /// </summary>
        /// <returns></returns>
        public SysConfigModel GetSysConfig
        {
            get
            {
                return this.GetSysBasicConfig();
            }
        }

        /// <summary>
        /// 全局应用配置信息(包含全局静态配置和全局动态参数配置)
        /// </summary>
        /// <returns></returns>
        public AppConfigModel GetAppConfig
        {
            get
            {
                var sysConfigModel = this.GetSysBasicConfig();
                return new AppConfigModel()
                {
                    GlobalConfig = AppGlobalConst.GetGlobalStaticConfig,
                    LanguageTypes = sysConfigModel.LanguageTypes,
                    LoginConfig = sysConfigModel.LoginConfig,
                    PasswordConfig = sysConfigModel.PasswordConfig,
                    EmailConfig = sysConfigModel.EmailConfig,
                    SMSConfig = sysConfigModel.SMSConfig,
                    SkinStyle = sysConfigModel.SkinStyle
                };
            }
        }
    }
}
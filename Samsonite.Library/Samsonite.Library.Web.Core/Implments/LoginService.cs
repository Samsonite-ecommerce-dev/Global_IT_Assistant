using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Linq;
using System.Text.Json;

namespace Samsonite.Library.Web.Core
{
    public class LoginService : ILoginService
    {
        private IBaseService _baseService;
        private IAppNotificationService _appNotificationService;
        private IEncryptService _encryptService;
        private IAppSession _appSession;
        private IAppCookie _appCookie;
        private appEntities _appDB;
        public LoginService(IBaseService baseService, IAppNotificationService appNotificationService, IEncryptService encryptService, IAppSession appSession, IAppCookie appCookie, appEntities appEntities)
        {
            _baseService = baseService;
            _appNotificationService = appNotificationService;
            _encryptService = encryptService;
            _appSession = appSession;
            _appCookie = appCookie;
            _appDB = appEntities;
        }

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="verificationCode"></param>
        /// <param name="isRemember">默认保存1天，记住密码保存30天</param>
        /// <returns></returns>
        public LoginResponse UserLogin(string userName, string password, string verificationCode, bool isRemember)
        {
            LoginResponse _result = new LoginResponse();
            string _privateKey = string.Empty;
            string _ip = _baseService.CurrentRequestIP;
            //加载语言包
            var _languagePack = _baseService.CurrentLanguagePack;
            try
            {
                if (string.IsNullOrEmpty(userName)) throw new UserLoginException(null, _languagePack["login_index_message_no_username"]);
                if (string.IsNullOrEmpty(password)) throw new UserLoginException(null, _languagePack["login_index_message_no_password"]);

                UserInfo objUserInfo = _appDB.UserInfo.SingleOrDefault(p => p.UserName == userName);
                if (objUserInfo != null)
                {
                    if (objUserInfo.Status == (int)UserStatus.Locked)
                    {
                        throw new UserLoginException(objUserInfo, _languagePack["login_index_message_username_lock"]);
                    }
                    else
                    {
                        _privateKey = objUserInfo.PrivateKey;
                        if (objUserInfo.Pwd.ToLower() == _encryptService.EncryptPassword(password, _privateKey).ToLower())
                        {
                            //如果是需要验证码的校验
                            if (objUserInfo.LoginType == (int)UserLoginType.VerificationCode)
                            {
                                //密码验证通过,挑战到验证码界面
                                _result.Step = 2;

                                if (string.IsNullOrEmpty(verificationCode))
                                {
                                    throw new UserLoginException(objUserInfo, _languagePack["login_index_message_no_verify_code"]);
                                }
                                else
                                {
                                    var verificationCodeSession = this.GetEmailVerificationCode();
                                    if (string.IsNullOrEmpty(verificationCodeSession))
                                    {
                                        throw new UserLoginException(objUserInfo, _languagePack["login_index_message_err_wrong_verify_code"]);
                                    }
                                    else
                                    {
                                        if (verificationCode != verificationCodeSession)
                                        {
                                            throw new UserLoginException(objUserInfo, _languagePack["login_index_message_err_wrong_verify_code"]);
                                        }
                                    }
                                }
                            }

                            //查看密码是否已经过期
                            bool IsExpired = false;
                            if (objUserInfo.LastPwdEditTime == null)
                            {
                                IsExpired = true;
                            }
                            else
                            {
                                if (objUserInfo.LastPwdEditTime.Value.AddDays(_baseService.CurrentApplicationConfig.PasswordConfig.ValidityTime) < DateTime.Now)
                                {
                                    IsExpired = true;
                                }
                            }
                            //如果密码已经过期
                            if (IsExpired)
                            {
                                objUserInfo.Status = (int)UserStatus.ExpiredPwd;
                            }
                            //重置连续密码错误次数
                            objUserInfo.PwdErrorNum = 0;
                            //更新最后登录时间
                            objUserInfo.LastLoginTime = DateTime.Now;
                            //写入登录信息
                            UserSessionModel userSessionModel = _baseService.UserModel(objUserInfo);
                            //上机日志表
                            _baseService.LoginLog(new WebAppLoginLog()
                            {
                                LoginStatus = true,
                                LoginType = objUserInfo.LoginType,
                                Account = objUserInfo.UserName,
                                Password = _encryptService.EncryptPassword(password, _privateKey),
                                UserID = objUserInfo.Userid,
                                IP = _ip,
                                Remark = string.Empty,
                                AddTime = DateTime.Now
                            }); ;
                            //写入Session
                            _appSession.Set(AppGlobalConst.USER_LOGIN_SESSION_NAME, JsonSerializer.Serialize<UserSessionModel>(userSessionModel));
                            //写入Cookie
                            if (isRemember)
                            {
                                //插入cookie
                                int _time = _baseService.CurrentApplicationConfig.LoginConfig.CookieSaveTime;
                                _appCookie.Set(AppGlobalConst.USER_LOGIN_ACCOUNT_COOKIE_NAME, userSessionModel.UserName, _time);
                                _appCookie.Set(AppGlobalConst.USER_LOGIN_PWD_COOKIE_NAME, userSessionModel.Passwd, _time);
                            }
                            //设置默认语言
                            _baseService.SaveCurrentLanguagePack(objUserInfo.DefaultLanguage);
                            //返回信息
                            _result.Result = true;
                            _result.LoginType = objUserInfo.LoginType;
                            _result.LoginEmail = objUserInfo.Email;
                            _result.Message = string.Empty;
                        }
                        else
                        {
                            //累计连续密码错误次数
                            objUserInfo.PwdErrorNum += 1;

                            //保存日志
                            _baseService.LoginLog(new WebAppLoginLog()
                            {
                                LoginStatus = false,
                                LoginType = objUserInfo.LoginType,
                                Account = userName,
                                Password = _encryptService.EncryptPassword(password, _privateKey),
                                UserID = 0,
                                IP = _ip,
                                Remark = _languagePack["login_index_message_err"],
                                AddTime = DateTime.Now
                            });

                            //如果超过限制错误次数
                            int _maxErrorLockNum = _baseService.CurrentApplicationConfig.PasswordConfig.ErrorLockNum;
                            if (objUserInfo.PwdErrorNum >= _maxErrorLockNum)
                            {
                                objUserInfo.Status = (int)UserStatus.Locked;

                                throw new UserLoginException(objUserInfo, string.Format(_languagePack["login_index_message_err_lock"], _maxErrorLockNum));
                            }
                            else
                            {
                                throw new UserLoginException(objUserInfo, _languagePack["login_index_message_err"]);
                            }
                        }
                        _appDB.SaveChanges();
                    }
                }
                else
                {
                    throw new UserLoginException(objUserInfo, _languagePack["login_index_message_username_not_exist"]);
                }
            }
            catch (UserLoginException ex)
            {
                //保存日志
                _baseService.LoginLog(new WebAppLoginLog()
                {
                    LoginStatus = false,
                    LoginType = (ex.UserInfo != null) ? ex.UserInfo.LoginType : 0,
                    Account = userName,
                    Password = _encryptService.EncryptPassword(password, _privateKey),
                    UserID = (ex.UserInfo != null) ? ex.UserInfo.Userid : 0,
                    IP = _ip,
                    Remark = ex.ErrorMessage,
                    AddTime = DateTime.Now
                });

                //返回信息
                _result.Result = false;
                _result.LoginType = (ex.UserInfo != null) ? ex.UserInfo.LoginType : 0;
                _result.LoginEmail = (ex.UserInfo != null) ? ex.UserInfo.Email : "";
                _result.Message = ex.ErrorMessage;
            }
            return _result;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public PostResponse SendVerificationCode(string userEmail)
        {
            PostResponse _result = new PostResponse();

            //加载语言包
            var _languagePack = _baseService.CurrentLanguagePack;
            try
            {
                if (string.IsNullOrEmpty(userEmail)) throw new Exception(_languagePack["login_index_message_no_email"]);

                UserInfo objUserInfo = _appDB.UserInfo.Where(p => p.Email == userEmail).SingleOrDefault();
                if (objUserInfo != null)
                {
                    string _code = this.CreateVerificationCode(6);
                    //创建Session
                    this.SaveEmailVerificationCodeSession(_code);
                    //发送验证码
                    _appNotificationService.SendLoginVerificationCodeNotification(_code, objUserInfo.Email);

                    //返回信息
                    _result.Result = true;
                    _result.Message = _languagePack["login_index_message_send_verify_code"];
                }
                else
                {
                    throw new Exception(_languagePack["login_index_message_no_email"]);
                }
            }
            catch (Exception ex)
            {
                //返回信息
                _result.Result = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public void UserLoginOut()
        {
            //清空session
            _appSession.Remove(AppGlobalConst.USER_LOGIN_SESSION_NAME);
            _appSession.Clear();
            //清空cookie
            _appCookie.Remove(AppGlobalConst.USER_LOGIN_ACCOUNT_COOKIE_NAME);
            _appCookie.Remove(AppGlobalConst.USER_LOGIN_PWD_COOKIE_NAME);
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PostResponse ForgetPassword(ForgetPasswordRequest request)
        {
            //加载语言包
            var _languagePack = _baseService.CurrentLanguagePack;

            PostResponse _result = new PostResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserName)) throw new Exception(_languagePack["login_forget_message_no_username"]);
                if (string.IsNullOrEmpty(request.Email)) throw new Exception(_languagePack["login_forget_message_no_email"]);

                UserInfo objUserInfo = _appDB.UserInfo.SingleOrDefault<UserInfo>(p => p.UserName == request.UserName);
                if (objUserInfo != null)
                {
                    if (objUserInfo.Email.ToLower() == request.Email.ToLower())
                    {
                        //创建新密码
                        string _newPassword = _encryptService.CreateRandomPassword();
                        //发送邮件

                        //更新密码
                        objUserInfo.Pwd = _encryptService.EncryptPassword(_newPassword, objUserInfo.PrivateKey);
                        objUserInfo.LastPwdEditTime = DateTime.Now;
                        _appDB.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("您输入的账号和邮箱不匹配");
                    }
                }
                else
                {
                    throw new Exception("用户不存在");
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
            }
            catch (Exception ex)
            {
                //返回信息
                _result.Result = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        #region 函数
        /// <summary>
        /// 创建随机验证码
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string CreateVerificationCode(int num)
        {
            string _result = string.Empty;
            string[] _array = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + i);
                _result += _array[rnd.Next(0, 9)];
            }
            return _result;
        }

        /// <summary>
        /// 保存邮箱验证码Session
        /// </summary>
        /// <param name="verificationCode"></param>
        private void SaveEmailVerificationCodeSession(string verificationCode)
        {
            _appSession.Set(AppGlobalConst.USER_LOGIN_VERIFICATION_SESSION_NAME, verificationCode);
        }

        /// <summary>
        /// 读取邮箱验证码Session
        /// </summary>
        /// <param name="verificationCode"></param>
        private string GetEmailVerificationCode()
        {
            return _appSession.Get(AppGlobalConst.USER_LOGIN_VERIFICATION_SESSION_NAME);
        }

        private class UserLoginException : Exception
        {
            public UserLoginException(UserInfo userInfo, string errorMessage)
            {
                this.UserInfo = userInfo;
                this.ErrorMessage = errorMessage;
            }

            public UserInfo UserInfo { get; set; }

            public string ErrorMessage { get; set; }
        }
        #endregion
    }
}
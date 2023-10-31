using Samsonite.Library.Utility;
using System;

namespace Samsonite.Library.Web.Core
{
    public class EncryptService: IEncryptService
    {
        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <returns></returns>
        public string CreateRandomPassword()
        {
            string _result = string.Empty;
            string[] _array = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] _spArray = new string[] { "+", "_" };
            Random rnd = new Random();
            for (int i = 0; i < 7; i++)
            {
                rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + i);
                _result += _array[rnd.Next(0, 35)];
            }
            _result = $"{_result.Substring(0, 4)}{_spArray[rnd.Next(0, 2)]}{_result.Substring(4)}";
            return _result;
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreatePrivateKey(int length)
        {
            string _result = string.Empty;
            string[] _array = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + i);
                _result += _array[rnd.Next(0, 61)];
            }
            return _result;
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string EncryptPassword(string password, string key)
        {
            string _result = string.Empty;
            //先加盐
            _result = password + key;
            //再HMAC_SHA256加密
            _result = EncryptHelper.HMAC_SHA256(_result, key);
            return _result;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Samsonite.Library.Data.Entity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Samsonite.Library.Web.Core
{
    public class AppLanguageCore : AppConfigCore
    {

        private appEntities _appDB;
        public AppLanguageCore(appEntities appEntities) : base(appEntities)
        {
            _appDB = appEntities;
        }

        /// <summary>
        /// 语言分类集合
        /// </summary>
        /// <returns></returns>
        public List<LanguageType> LanguageTypes()
        {
            //默认语言为英文
            //js文件名称默认值有中文简体,中文繁体和英文
            return _appDB.LanguageType.AsNoTracking().ToList();
        }
    }
}

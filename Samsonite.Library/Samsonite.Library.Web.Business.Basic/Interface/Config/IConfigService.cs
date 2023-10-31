using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Business.Basic
{
    public interface IConfigService
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        SysConfigModel GetConfigInfo();

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PostResponse Update(ConfigUpdateRequest request);
    }
}

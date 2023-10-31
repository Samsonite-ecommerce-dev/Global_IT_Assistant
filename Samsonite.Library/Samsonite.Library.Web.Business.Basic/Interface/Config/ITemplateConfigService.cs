using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;

namespace Samsonite.Library.Web.Business.Basic
{
    public interface ITemplateConfigService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryResponse<SysTemplate> GetQuery(TemplateConfigSearchRequest request);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PostResponse Add(TemplateConfigAddRequest request);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PostResponse Edit(TemplateConfigEditRequest request);

        /// <summary>
        /// 模板类型列表
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> TemplateTypeObject();

        /// <summary>
        /// 根据模板标识获取模板
        /// </summary>
        /// <param name="indentify"></param>
        /// <returns></returns>
        SysTemplate GetTemplate(string indentify);
    }
}

using Samsonite.Library.Data.Entity.Models;
using System.Collections.Generic;

namespace Samsonite.Library.Web.Business.Custom
{
    public interface IOMSAPIToolsService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<View_ApplicationApi> GetQuery();

        /// <summary>
        /// 韩国仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetKrWMSParams();

        /// <summary>
        /// 韩国API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetKrApiResult(OMSApiToolRequest request);

        /// <summary>
        /// 泰国仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetThWMSParams();

        /// <summary>
        /// 泰国平台API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetThPlatformParams();

        /// <summary>
        /// 泰国API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetThApiResult(OMSApiToolRequest request);

        /// <summary>
        /// 香港Tumi仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetHkTumiWMSParams();

        /// <summary>
        /// 香港TumiAPI请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetHkTumiApiResult(OMSApiToolRequest request);

        /// <summary>
        /// 马来西亚仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetMyWMSParams();

        /// <summary>
        /// 马来西亚平台库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetMyPlatformParams();

        /// <summary>
        /// 马来西亚API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetMyApiResult(OMSApiToolRequest request);

        /// <summary>
        /// 日本仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetJpnWMSParams();

        /// <summary>
        /// 日本平台库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetJpnPlatformParams();

        /// <summary>
        /// 日本API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetJpnApiResult(OMSApiToolRequest request);

        /// <summary>
        /// 台湾仓库API参数
        /// </summary>
        /// <returns></returns>
        OMSApiToolInitializeResponse GetTwPlatformParams();

        /// <summary>
        /// 台湾API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        OMSApiResultResponse GetTwApiResult(OMSApiToolRequest request);
    }
}

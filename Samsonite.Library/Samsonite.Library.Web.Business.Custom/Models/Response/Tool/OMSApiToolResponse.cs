using Samsonite.Library.Web.Core.Models;
using System.Collections.Generic;

namespace Samsonite.Library.Web.Business.Custom
{
    public class OMSApiToolInitializeResponse
    {
        /// <summary>
        /// 加密方式(md5)
        /// </summary>
        public const string SIGN_METHOD_MD5 = "md5";

        /// <summary>
        /// 加密方式(sha256)
        /// </summary>
        public const string SIGN_METHOD_SHA256 = "sha";

        public OMSApiToolInitializeResponse()
        {
            this.UserID = "";
            this.Version = "1.0";
            this.Format = "json";
            this.Method = "md5";
        }

        public string UserID { get; set; }

        /// <summary>
        /// 1.0
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// json
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 加密方式
        /// md5:HMAC_MD5
        /// sha:HMAC_SHA256
        /// </summary>
        public string Method { get; set; }

        public string AccessToken { get; set; }

        public List<OMSApiToolInitializeInterface> APIInterfaces { get; set; }
    }

    public class OMSApiToolInitializeInterface
    {
        public int APIPathID { get; set; }

        public string APIPath { get; set; }

        public string RequestURL { get; set; }

        public string HttpMethod { get; set; }

        /// <summary>
        /// GET方法参数集合
        /// </summary>
        public List<CommomParamRequest> Parameters { get; set; }

        /// <summary>
        /// POST方法参数集合
        /// </summary>
        public List<object> PostDatas { get; set; }
    }

    public class OMSApiRequestInfo
    {
        public string RequestUrl { get; set; }

        public string RequestData { get; set; }

        public string ResponseInfo { get; set; }
    }

    public class OMSApiResultResponse : PostResponse
    {
        public string RequestInfo { get; set; }

        public string ResponseInfo { get; set; }
    }
}

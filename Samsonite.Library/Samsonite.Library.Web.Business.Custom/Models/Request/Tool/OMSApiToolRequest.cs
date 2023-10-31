using System.Text.Json.Serialization;

namespace Samsonite.Library.Web.Business.Custom
{
    public class OMSApiToolRequest
    {
        /// <summary>
        /// API在数据库中的记录ID，用于输别API的基本信息
        /// </summary>
        public int ApiID { get; set; }
        public string UserID { get; set; }

        public string Version { get; set; }

        public string Format { get; set; }

        public string Method { get; set; }

        public string AccessToken { get; set; }

        public int apiPath { get; set; }

        public string requestURL { get; set; }

        public string httpMethod { get; set; }

        public string paramArray { get; set; }

        public string postData { get; set; }
    }

    public class CommomParamRequest
    {
        [JsonPropertyName("param")]
        public string Param { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonPropertyName("placeholder")]
        public string Placeholder { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Samsonite.Library.Web.Core.Models
{
    public class QueryResponse<T>
    {
        /// <summary>
        /// 返回数据记录总数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> Items { get; set; }
    }

    public class PostResponse
    {
        /// <summary>
        /// 登录结果
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        [JsonPropertyName("is_error")]
        public bool IsError { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }
    }
}

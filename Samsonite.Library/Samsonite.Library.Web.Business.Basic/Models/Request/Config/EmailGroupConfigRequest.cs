﻿using System.Text.Json.Serialization;
using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Business.Basic.Models
{
    public class EmailGroupConfigSearchRequest : PageRequest
    {
        public string Keyword { get; set; }
    }

    public class EmailGroupConfigAddRequest
    {
        public string GroupName { get; set; }

        public string MailAddresses { get; set; }

        public string Remark { get; set; }
    }

    public class EmailGroupConfigEditRequest
    {
        public int ID { get; set; }

        public string GroupName { get; set; }

        public string MailAddresses { get; set; }

        public string Remark { get; set; }
    }

    public class MailAddressesAttr
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}

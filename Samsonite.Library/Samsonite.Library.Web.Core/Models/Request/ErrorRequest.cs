﻿namespace Samsonite.Library.Web.Core.Models
{
    public class ErrorRequest
    {
        public int Type { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}

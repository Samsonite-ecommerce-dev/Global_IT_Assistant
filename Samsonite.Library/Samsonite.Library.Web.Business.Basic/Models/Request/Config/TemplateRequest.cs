using Samsonite.Library.Web.Core.Models;

namespace Samsonite.Library.Web.Business.Basic.Models
{
    public class TemplateConfigSearchRequest : PageRequest
    {
        public string Keyword { get; set; }

        public int Type { get; set; }
    }

    public class TemplateConfigAddRequest
    {
        public string Name { get; set; }

        public string Indentify { get; set; }

        public int Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Sender { get; set; }

        public string Remark { get; set; }
    }

    public class TemplateConfigEditRequest
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Indentify { get; set; }

        public int Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Sender { get; set; }

        public string Remark { get; set; }
    }
}

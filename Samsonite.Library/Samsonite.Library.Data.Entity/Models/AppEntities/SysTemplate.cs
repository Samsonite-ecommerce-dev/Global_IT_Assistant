using System;
using System.Collections.Generic;

namespace Samsonite.Library.Data.Entity.Models
{
    public partial class SysTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 模板标识
        /// </summary>
        public string TemplateIndentify { get; set; }

        /// <summary>
        /// 模板类型:1.邮件,2:短信
        /// </summary>
        public int TemplateType { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string TemplateTitle { get; set; }

        /// <summary>
        /// 模板内容
        /// </summary>
        public string TemplateContent { get; set; }

        /// <summary>
        /// 短信专用sender
        /// </summary>
        public string TemplateSender { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortID { get; set; }

        /// <summary>
        /// 模板备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }

    }
}

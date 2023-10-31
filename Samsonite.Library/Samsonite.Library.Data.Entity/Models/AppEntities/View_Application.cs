using System;
using System.Collections.Generic;

namespace Samsonite.Library.Data.Entity.Models
{
    public partial class View_Application
    {
        /// <summary>
        /// 
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CatelogID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CatelogName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EnvironmentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EnvironmentContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}

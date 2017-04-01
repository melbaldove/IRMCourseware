using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRMCourseware.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public int GroupID { get; set; }

        public virtual Group Group { get; set; }
    }
}
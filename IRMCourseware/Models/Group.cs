using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRMCourseware.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string GroupNo { get; set; }
        public string Title { get; set; }
        public string GroupMembers { get; set; }

        
        public virtual ICollection<Document> Documents { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stackoverflow.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Questions = new HashSet<QuestionTag>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<QuestionTag> Questions { get; set; }
    }
}
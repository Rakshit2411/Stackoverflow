using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stackoverflow.Models
{
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<QuestionTag>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime Timeposted { get; set; }
        public int Votes { get; set; }

        [ForeignKey ("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<QuestionTag> Tags { get; set; }
    }
}
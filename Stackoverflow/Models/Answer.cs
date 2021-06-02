using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Stackoverflow.Models
{
    public class Answer
    {
        public Answer()
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int Votes { get; set; }
        public bool? Isaccepted { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
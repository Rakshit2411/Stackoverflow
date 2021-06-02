using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stackoverflow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
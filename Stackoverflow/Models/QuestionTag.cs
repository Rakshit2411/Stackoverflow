using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stackoverflow.Models
{
    public class QuestionTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int QuestionId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Question Question { get; set; }
    }
}
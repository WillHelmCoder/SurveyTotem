using Intelemark.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intelemark.Entities
{
    public class AnsweredForm : Entity
    {
        public string Answer { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [ForeignKey("Record")]
        public int RecordId { get; set; }
        public virtual Record Record { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
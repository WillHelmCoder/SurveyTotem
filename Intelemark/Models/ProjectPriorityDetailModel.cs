namespace Intelemark.Models
{
    public class ProjectPriorityDetailModel : Model
    {
        public int ProjectPriorityId { get; set; }
        public virtual ProjectPriorityModel ProjectPriority { get; set; }
        public string Type { get; set; }
        public string FieldValue { get; set; }
        public int FieldPriorityValue { get; set; }
    }
}
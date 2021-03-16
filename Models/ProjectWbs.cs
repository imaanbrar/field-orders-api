using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class ProjectWbs
    {
        public ProjectWbs()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string TaskCode { get; set; }
        public string TaskDescription { get; set; }
        public decimal Budget { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Project Project { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}

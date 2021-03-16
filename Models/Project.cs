using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class Project
    {
        public Project()
        {
            Order = new HashSet<Order>();
            ProjectWbs = new HashSet<ProjectWbs>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Company Company { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<ProjectWbs> ProjectWbs { get; set; }
    }
}

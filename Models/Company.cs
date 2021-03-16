using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class Company
    {
        public Company()
        {
            FieldVendor = new HashSet<FieldVendor>();
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<FieldVendor> FieldVendor { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}

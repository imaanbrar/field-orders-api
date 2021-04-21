using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class RecentOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Order Order { get; set; }
        public User User { get; set; }
    }
}

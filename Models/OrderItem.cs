using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemNumber { get; set; }
        public decimal? Quantity { get; set; }
        public string Uom { get; set; }
        public string Description { get; set; }
        public int? WbsId { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Order Order { get; set; }
        public ProjectWbs Wbs { get; set; }
    }
}

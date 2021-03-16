using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class OrderComment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Order Order { get; set; }
    }
}

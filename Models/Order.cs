using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            FieldVendor = new HashSet<FieldVendor>();
            OrderComment = new HashSet<OrderComment>();
            OrderItem = new HashSet<OrderItem>();
            RecentOrder = new HashSet<RecentOrder>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string OrderType { get; set; }
        public DateTime? InitiatedDate { get; set; }
        public DateTime? RasDate { get; set; }
        public DateTime? GoodsReceived { get; set; }
        public string DeliveryPoint { get; set; }
        public int? OriginatorId { get; set; }
        public bool ReadyForPurchase { get; set; }
        public bool IssuedToVendor { get; set; }
        public int? ShippingMethodId { get; set; }
        public DateTime? CloseOutDate { get; set; }
        public decimal? Gst { get; set; }
        public decimal? Pst { get; set; }
        public decimal? Hst { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int StatusId { get; set; }

        public Project Project { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<FieldVendor> FieldVendor { get; set; }
        public ICollection<OrderComment> OrderComment { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
        public ICollection<RecentOrder> RecentOrder { get; set; }
    }
}

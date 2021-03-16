using System;
using System.Collections.Generic;

namespace FieldOrdersAPI.Models
{
    public partial class FieldVendor
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string CompanyName { get; set; }
        public int? CompanyId { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactCell { get; set; }
        public string ContactFax { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationPhone { get; set; }
        public string LocationFax { get; set; }
        public string LocationEmail { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Company Company { get; set; }
        public Order Order { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.DTO_s
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int ProductcategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductcCategory { get; set; }
        public decimal ProductCost { get; set; }
        public double NormalDayDiscount { get; set; }
        public double AddWeekendsDiscount { get; set; }
        public double DeliveryCharges { get; set; }
    }
}

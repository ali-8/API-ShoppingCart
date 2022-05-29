using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.DTO_s
{
    public class ShoppingCartDTO
    {
        public int CartID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal? NormalDayDiscount { get; set; }
        public decimal? AddWeekendsDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TotalCost { get; set; }
    }
}

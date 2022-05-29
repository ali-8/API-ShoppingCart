using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblCart
    {
        public int CartId { get; set; }
        public int FkProductId { get; set; }
        public int ProductQuantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal? NormalDayDiscount { get; set; }
        public decimal? AddWeekendsDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal TotalCost { get; set; }

        public virtual TblProduct FkProduct { get; set; }
    }
}

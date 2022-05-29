using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblDiscount
    {
        public int DiscountId { get; set; }
        public int FkProductId { get; set; }
        public double NormalDayDiscount { get; set; }
        public double AddWeekendsDiscount { get; set; }

        public virtual TblProduct FkProduct { get; set; }
    }
}

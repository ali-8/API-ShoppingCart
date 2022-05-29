using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblDeliveryCharge
    {
        public int DeliveryChargesId { get; set; }
        public int FkProductcategoryId { get; set; }
        public double DeliveryCharges { get; set; }

        public virtual TblProductCategory FkProductcategory { get; set; }
    }
}

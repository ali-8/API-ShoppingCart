using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblProductCategory
    {
        public TblProductCategory()
        {
            TblDeliveryCharges = new HashSet<TblDeliveryCharge>();
            TblProducts = new HashSet<TblProduct>();
        }

        public int ProductcategoryId { get; set; }
        public string ProductCategoryName { get; set; }

        public virtual ICollection<TblDeliveryCharge> TblDeliveryCharges { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}

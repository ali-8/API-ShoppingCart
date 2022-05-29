using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblCarts = new HashSet<TblCart>();
            TblDiscounts = new HashSet<TblDiscount>();
            TblProductCostTables = new HashSet<TblProductCostTable>();
        }

        public int ProductId { get; set; }
        public int FkProductcategoryId { get; set; }
        public string ProductName { get; set; }

        public virtual TblProductCategory FkProductcategory { get; set; }
        public virtual ICollection<TblCart> TblCarts { get; set; }
        public virtual ICollection<TblDiscount> TblDiscounts { get; set; }
        public virtual ICollection<TblProductCostTable> TblProductCostTables { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class TblProductCostTable
    {
        public int ProductCostId { get; set; }
        public int FkProductId { get; set; }
        public int ProductCost { get; set; }

        public virtual TblProduct FkProduct { get; set; }
    }
}

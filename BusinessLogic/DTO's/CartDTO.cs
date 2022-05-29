using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.DTO_s
{
    public class CartDTO
    {
        public List<ShoppingCartDTO> ShoppingCartDTO { get; set; }
        public decimal CartTotal { get; set; }
        public bool FinalDiscount { get; set; }
    }
}

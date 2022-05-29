using BusinessLogic.DTO_s;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPILogic.Controllers
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _repo;

        public ProductController(IProductService Repository)
        {

            _repo = Repository;
        }


        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetProductList()
        {
            try
            {

                return Ok(await _repo.GetProductList());
            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddCart")]
        public async Task<IActionResult> AddCart([FromBody] ShoppingCartDTO shoppingCartDTO)
        {
            try
            {
                await _repo.AddProductToCart(shoppingCartDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("cart/list")]
        public async Task<IActionResult> GetProductCartList()
        {
            try
            {

                return Ok(await _repo.GetCartList());
            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPatch]
        [Route("UpdateCart")]
        public async Task<IActionResult> UpdateProductToCart([FromBody] ShoppingCartDTO shoppingCartDTO)
        {
            try
            {
                await _repo.UpdateProductToCart(shoppingCartDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{cartID}/delete")]
        public async Task<IActionResult> UpdateProductToCart(int cartID)
        {
            try
            {
                await _repo.DeleteProductFromCart(cartID);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("bill")]
        public IActionResult Print(List<int> cartids)
        {

            try
            {
                var t = _repo.BillGenerate(cartids);
                if (t == null) return NotFound();
                return t;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

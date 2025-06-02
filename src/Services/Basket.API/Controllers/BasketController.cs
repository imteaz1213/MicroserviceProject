using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;
using Basket.API.Repositories;
using System.Net;
using Basket.API.Models;
using Basket.API.GrpcServices;
namespace Basket.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : BaseController
    {
        IBasketRepository _basketRepository;
        DiscountGrpcService _discountGrpcService;   
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetBasket(string userName)
        {
            try
            {
                var basket = await _basketRepository.GetBasket(userName);

                return CustomResult("Basket data loaded successfully",basket);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message,HttpStatusCode.NotFound);

            }
        }



        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            try
            {
                //todo : communicate discount.grpc
                //calculate latest price 
                //create discount grpc service 

                foreach(var item in basket.Items)
                {
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                    item.Price -= coupon.Amount;
                }

                var updatedBasket = await _basketRepository.UpdateBasket(basket);
                return CustomResult("Basket updated successfully", updatedBasket);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            try
            {
                await _basketRepository.DeleteBasket(userName);
                return CustomResult("Basket deleted successfully", HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return CustomResult(e.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}


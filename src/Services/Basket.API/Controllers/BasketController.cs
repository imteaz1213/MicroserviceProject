using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;
using Basket.API.Repositories;
using System.Net;
using Basket.API.Models;
using Basket.API.GrpcServices;
using MassTransit;
using AutoMapper;
using EventBus.Messages.Events;
namespace Basket.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : BaseController
    {
        IBasketRepository _basketRepository;
        DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService,IPublishEndpoint publishEndpoint,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
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

        [HttpPost]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Checkout([FromBody]BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);

            if(basket == null)
            {
                return CustomResult("Basket is empty",HttpStatusCode.NoContent);
            }
            //send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            //remove basket
            await _basketRepository.DeleteBasket(basket.UserName);
            return CustomResult("order has been placed");
        }
    }
}



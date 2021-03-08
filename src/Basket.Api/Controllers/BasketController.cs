using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IStockRepository _stockRepository;

        public BasketController(IBasketRepository basketRepository, IStockRepository stockRepository)
        {
            _basketRepository = basketRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] BasketCart basket)
        {
            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(BasketCartItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ControlProductStock([FromBody] BasketCartItem basketCartItem)
        {
            return Ok(await _stockRepository.ControlProductStock(basketCartItem));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            return Ok(await _basketRepository.DeleteBasket(userName));
        }

    }
}

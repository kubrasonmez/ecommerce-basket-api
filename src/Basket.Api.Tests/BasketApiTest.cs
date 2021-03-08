using Basket.Api.Controllers;
using Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Basket.Api.Tests
{
    public class BasketApiTest
    {
        private readonly Mock<IBasketRepository> _mockBasketRepo;
        private readonly Mock<IStockRepository> _mockStockRepo;
        private readonly BasketController _controller;

        public BasketApiTest()
        {
            _mockBasketRepo = new Mock<IBasketRepository>();
            _mockStockRepo = new Mock<IStockRepository>();
            _controller = new BasketController(_mockBasketRepo.Object, _mockStockRepo.Object);
        }

        [Fact]
        public async Task GetBasket_ActionExecutes_ReturnOkWithBasketCart()
        {
            _mockBasketRepo.Setup(repo => repo.GetBasket(It.IsAny<string>())).ReturnsAsync(GetTestCart());

            var result = await _controller.GetBasket(It.IsAny<string>());

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnCart = Assert.IsType<Entities.BasketCart>(okResult.Value);

            Assert.Equal<int>(2, returnCart.Items.Count);
        }

        [Fact]
        public async Task UpdateBasket_ActionExecutes_ReturnOkWithBasketCart()
        {
            _mockBasketRepo.Setup(repo => repo.UpdateBasket(It.IsAny<Entities.BasketCart>())).ReturnsAsync(GetTestCart());

            var result = await _controller.UpdateBasket(It.IsAny<Entities.BasketCart>());

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnCart = Assert.IsType<Entities.BasketCart>(okResult.Value);

            Assert.Equal<int>(2, returnCart.Items.Count);
        }

        [Theory]
        [InlineData(1960)]
        public async Task BasketTotalPrice_Should_Equal_ExpectedData(decimal expectedTotalPrice)
        {
            _mockBasketRepo.Setup(repo => repo.UpdateBasket(It.IsAny<Entities.BasketCart>())).ReturnsAsync(GetTestCart());

            var result = await _controller.UpdateBasket(It.IsAny<Entities.BasketCart>());

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnCart = Assert.IsType<Entities.BasketCart>(okResult.Value);

            Assert.Equal<decimal>(expectedTotalPrice, returnCart.TotalPrice);
        }

        [Theory]
        [InlineData(true, 3)]
        [InlineData(false, 2)]
        public async Task AddNewItem_ControlProductStock_ReturnItemsCount(bool isSufficientStock, int cartItemsCount)
        {
            _mockStockRepo.Setup(repo => repo.ControlProductStock(It.IsAny<Entities.BasketCartItem>())).ReturnsAsync(isSufficientStock);
            
            var result = await _controller.ControlProductStock(It.IsAny<Entities.BasketCartItem>());

            var okResult = Assert.IsType<OkObjectResult>(result);

            var value = Assert.IsType<bool>(okResult.Value);

            //Get basket cart
            _mockBasketRepo.Setup(repo => repo.GetBasket(It.IsAny<string>())).ReturnsAsync(GetTestCart());
            var resultGetBasket = await _controller.GetBasket(It.IsAny<string>());
            var okResultGetBasket = Assert.IsType<OkObjectResult>(resultGetBasket);
            var returnCartGetBasket = Assert.IsType<Entities.BasketCart>(okResultGetBasket.Value);

            if (value)
            {
                //Add new item to basket cart because product stock is sufficient.
                returnCartGetBasket.Items.Add(GetTestCartItem());

                //Update basket cart
                _mockBasketRepo.Setup(repo => repo.UpdateBasket(It.IsAny<Entities.BasketCart>())).ReturnsAsync(returnCartGetBasket);
                var resultUpdateBasket = await _controller.UpdateBasket(It.IsAny<Entities.BasketCart>());
                var okResultUpdateBasket = Assert.IsType<OkObjectResult>(resultUpdateBasket);
                var returnCartUpdateBasket = Assert.IsType<Entities.BasketCart>(okResultUpdateBasket.Value);
                
                //Control cart item count 
                Assert.Equal<int>(cartItemsCount, returnCartUpdateBasket.Items.Count);
            }
            else
            {
                // Product stock isn't sufficient so couldn't update basket cart.
                Assert.Equal<int>(cartItemsCount, returnCartGetBasket.Items.Count);
            }
        }

        [Fact]
        public async Task DeleteBasket_ActionExecutes_ReturnOkWithBasketCart()
        {
            _mockBasketRepo.Setup(repo => repo.DeleteBasket(It.IsAny<string>()));

            var result = await _controller.DeleteBasket(It.IsAny<string>());

            Assert.IsType<OkObjectResult>(result);
        }

        private Entities.BasketCart GetTestCart()
        {
            return new Entities.BasketCart()
            {
                UserName = "kubra",
                Items = new List<Entities.BasketCartItem>()
                {
                    new Entities.BasketCartItem(){ Price = 120, ProductId = "11", ProductName = "Tshirt", Quantity = 3 },
                    new Entities.BasketCartItem(){ Price = 320, ProductId = "41", ProductName = "Sweatshirt", Quantity = 5 }
                }
            }; 
        }

        private Entities.BasketCartItem GetTestCartItem()
        {
            return new Entities.BasketCartItem() { Price = 520, ProductId = "21", ProductName = "Bag", Quantity = 5 };
        }
    }
}

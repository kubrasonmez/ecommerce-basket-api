using Basket.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class StockRepository : IStockRepository
    {
        public Task<bool> ControlProductStock(Entities.BasketCartItem cart) => Task.FromResult(true);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<bool> ControlProductStock(Entities.BasketCartItem cart);
    }
}

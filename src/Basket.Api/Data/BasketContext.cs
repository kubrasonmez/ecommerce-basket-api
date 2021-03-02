using Basket.Api.Data.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Data
{
    public class BasketContext : IBasketContext
    {
        public IDatabase Redis { get; }

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            Redis = redisConnection.GetDatabase();
        }

    }
}

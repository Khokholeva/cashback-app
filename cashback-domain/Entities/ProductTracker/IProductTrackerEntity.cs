using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.Shop;

namespace cashback_domain.Entities.ProductTracker
{
    public interface IProductTrackerEntity : IEntity<int>
    {
        public Dictionary<IShopEntity, (int Price, string URL)> PricesAtShops { get; init; }
        public DateTime PriceGotAt { get; init; }
    }
}

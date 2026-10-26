using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.Shop;
using cashback_domain.Enums;

namespace cashback_domain.Entities.Coupon_or_Promocde
{
    internal interface IPromotionEntity : IEntity<int>
    {
        public double DiscountValue { get; init; }
        public DiscountMeasurementUnit DiscountMeasurementUnit { get; init; }
        public IShopEntity Shop { get; init; }
        public DateTime ValidUntil { get; init; }
    }
}

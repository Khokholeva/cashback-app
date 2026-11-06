using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.DB;

namespace cashback_domain.Entities.Coupon_or_Promocde
{
    public interface IPromotionRepository : ISqLiteDbRepository<IPromotionEntity>
    {
    }
}

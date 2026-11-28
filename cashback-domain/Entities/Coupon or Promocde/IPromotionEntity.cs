using cashback_domain.Enums;

namespace cashback_domain.Entities
{
    public interface IPromotionEntity : IEntity<int?>
    {
        public double DiscountValue { get; }
        public DiscountMeasurementUnit DiscountMeasurementUnit { get; }
        public int? ShopID { get; }
        public DateTime? ValidUntil { get; }
    }
}

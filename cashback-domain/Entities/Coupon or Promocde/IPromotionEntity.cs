using cashback_domain.Enums;

namespace cashback_domain.Entities
{
    public interface IPromotionEntity : IEntity<int?>
    {
        public double DiscountValue { get; init; }
        public DiscountMeasurementUnit DiscountMeasurementUnit { get; init; }
        public int? ShopID { get; init; }
        public DateTime? ValidUntil { get; init; }
    }
}

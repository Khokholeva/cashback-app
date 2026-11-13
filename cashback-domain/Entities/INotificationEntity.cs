namespace cashback_domain.Entities
{
    public interface INotificationEntity<T> : IEntity<T>
    {
        public DateTime NextNotification { get; set; }
    }
}

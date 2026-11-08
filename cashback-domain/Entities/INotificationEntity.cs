namespace cashback_domain.Entities
{
    public interface INotificationEntity<T> : IEntity<T>
        where T : notnull
    {
        public DateTime NextNotification { get; set; }
    }
}

namespace cashback_domain.Entities
{
    public interface IBankEntity : IEntity<int?>
    {
        public string Name { get; }
    }
}

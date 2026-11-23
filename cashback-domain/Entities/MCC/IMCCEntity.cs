namespace cashback_domain.Entities
{
    public interface IMCCEntity : IEntity<int?>
    {
        public string Name { get; init; }

        public string Description { get; init; }
    }
}

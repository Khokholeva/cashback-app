namespace cashback_domain.Entities
{
    public interface IMCCEntity : IEntity<int>
    {
        public int Code { get; init; }

        public string Name { get; init; }

        public string Desctiption { get; init; }
    }
}

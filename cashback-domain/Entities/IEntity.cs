namespace cashback_domain.Entities
{
    public interface IEntity<TID>
    {
        public TID? Id { get; set; }
    }

    public static class EntityExtensions
    {
        public static bool Equals<TEntity,TID>(this TEntity entity, TEntity other)
            where TEntity : IEntity<TID>
        {
            if (other is null || other.Id is null) return false;
            return other.Id.Equals(entity.Id) && other.GetType().IsInstanceOfType(entity);
        }

        public static int GetHashCode<TEntity, TID>(this TEntity entity)
            where TEntity : IEntity<TID>
        {
            return entity.Id?.GetHashCode() ?? 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities.DB;

namespace cashback_domain.Entities
{
    public interface IEntity<TID>
        where TID : notnull
    {
        public TID Id { get; set; }

        public virtual bool Equals(IEntity<TID> other)
        {
            if (other is null) return false;
            return other.Id.Equals(Id) && other.GetType().IsInstanceOfType(this);
        }

        public virtual int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void SaveToDB(IDBRepository<IEntity<TID> ,TID> dBRepository);

        
    }
}

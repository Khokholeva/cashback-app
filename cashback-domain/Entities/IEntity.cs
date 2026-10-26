using System;
using System.Collections.Generic;
using System.Text;

namespace cashback_domain.Entities
{
    internal interface IEntity<TID>
        where TID : notnull
    {
        public TID Id { get; set; }

        public virtual bool Equals(IEntity<TID> other)
        {
            if (other is null) return false;
            return other.Id.Equals(Id);
        }

        public virtual int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

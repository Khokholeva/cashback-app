using System;
using System.Collections.Generic;
using System.Text;

namespace cashback_domain.Entities
{
    public interface IBankEntity : IEntity<int>
    {
        public string Name { get; init; }
        public string? Description { get; init; }
    }
}

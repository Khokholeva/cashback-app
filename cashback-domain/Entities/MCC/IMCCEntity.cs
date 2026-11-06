using System;
using System.Collections.Generic;
using System.Text;

namespace cashback_domain.Entities.MCC
{
    public interface IMCCEntity : IEntity<int>
    {
        public int Code { get; init; }

        public string Name { get; init; }

        public string Desctiption { get; init; }
    }
}

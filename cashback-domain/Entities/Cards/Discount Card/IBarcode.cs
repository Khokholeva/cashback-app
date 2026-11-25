using System;
using System.Collections.Generic;
using System.Text;

namespace cashback_domain.Entities.Cards.Discount_Card
{
    public interface IBarcode
    {
        string Format { get; init; }
        string Value { get; init; }
    }
}

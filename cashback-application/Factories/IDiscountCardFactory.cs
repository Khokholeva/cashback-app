using System;
using System.Collections.Generic;
using System.Text;
using cashback_domain.Entities;
using ZXing.Net.Maui;

namespace cashback_application.Factories
{
    public interface IDiscountCardFactory
    {
        DiscountCardEntity CreateDiscountCard(BarcodeResult barcode, int shopId);
    }
}

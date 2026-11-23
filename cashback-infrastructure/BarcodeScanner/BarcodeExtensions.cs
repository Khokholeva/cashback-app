using System;
using System.Collections.Generic;
using System.Text;
using ZXing.Net.Maui;
using System.Reflection;

namespace cashback_infrastructure
{
    public static class BarcodeExtensions
    {
        public static BarcodeFormat StringToBarcodeFormat(this string stringFormat)
        {
            return (BarcodeFormat)typeof(BarcodeFormat)
                .GetFields()
                .Where(field => field.IsLiteral)
                .First(field => field.Name == stringFormat)?
                .GetRawConstantValue();

        }
    }
}

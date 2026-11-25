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
            var barcodeType = typeof(BarcodeFormat)
                .GetFields()
                .FirstOrDefault(field => field.IsLiteral && field.Name == stringFormat)
                ?? throw new ArgumentException($"{stringFormat} не является существующим типом штрихкода!");
                

            return (BarcodeFormat)barcodeType.GetRawConstantValue();
        }
    }
}

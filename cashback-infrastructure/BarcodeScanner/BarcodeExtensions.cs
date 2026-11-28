using System;
using System.Collections.Generic;
using System.Text;
using ZXing.Net.Maui;
using System.Reflection;
using cashback_domain.Entities.Cards.Discount_Card;
using cashback_infrastructure.DB;

namespace cashback_infrastructure
{
    public static class BarcodeExtensions
    {
        private static Dictionary<string, BarcodeFormat> stringToBarcodeType;

        public static BarcodeFormat ToBarcodeFormat(this string stringFormat)
        {
            stringToBarcodeType = typeof(BarcodeFormat)
            .GetFields()
            .Where(field => field.IsLiteral)
            .ToDictionary(
                field => field.Name.ToLower(),
                field => (BarcodeFormat)field.GetRawConstantValue());


            return stringToBarcodeType.TryGetValue(stringFormat.ToLower(), out var fmt)
                    ? fmt
                    : throw new KeyNotFoundException($"Тип штрихкода {stringFormat} не существует!");
        }

        public static IBarcode ToIBarcode(this BarcodeResult barcode)
        {
            return new BarcodeFromDB(barcode.Format.ToString(), barcode.Value);
        }

        public static BarcodeResult ToBarcodeResult(this IBarcode barcode)
        {
            return new BarcodeResult()
            {
                Format = barcode.Format.ToBarcodeFormat(),
                Value = barcode.Value
            };
        }
    }
}

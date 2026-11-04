using ZXing.Net.Maui;

namespace cashback_infrastructure
{
    public static class BarcodeScanner
    {
        //Ищет код на фото по пути к файлу
        //возвращает тип + значение либо null если не нашел
        public static async Task<BarcodeResult?> DecodeBarcodeFromImageAsync(string fileName)
        {

            var results = await BarcodeReader.DecodeFromFileAsync(
            fileName,
            new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                TryHarder = true,
            });

            if (results != null && results.Length > 0)
            {
                return results[0];
            }
            //решить, как и где обрабатывать фото без кода
            return null;
        }
    }
}

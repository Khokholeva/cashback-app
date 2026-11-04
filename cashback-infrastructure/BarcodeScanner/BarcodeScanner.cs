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

        
        //размер, формат, цвета кодов можно выбрать общие для нашего приложения и задать здесь
        //если нам нужны разные форматы (например, qr) - сделать разные методы или добавить параматр?
        static BarcodeGeneratorOptions ean13GeneratorOptions = new BarcodeGeneratorOptions
        {
            Format = BarcodeFormat.Ean13,
            Width = 512,
            Height = 256,
            Margin = 2,
            ForegroundColor = Colors.Black,
            BackgroundColor = Colors.White

        };

        //создает баркод для заданного значения и сохраняет его по заданному имени файла
        public static async void GenerateBarcodeToFile(string value, string filename) {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, filename);
            await BarcodeGenerator.WriteToFileAsync(
              value,
              filePath,
              ean13GeneratorOptions
             );
        }

        //создает баркод для значения и пишет его в заданный поток
        public static async void GenerateBarcodeToSream(string value, Stream stream)
        {
            await BarcodeGenerator.WriteToStreamAsync(
              value,
              stream,
              ean13GeneratorOptions);
        }
    }
}

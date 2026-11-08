using ZXing.Net.Maui;

namespace cashback_infrastructure
{
    public class BarcodeGenerators
    {
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
        public static async void GenerateBarcodeToFile(string value, string filename)
        {
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

        //генерирует код заданного формата и размера в поток
        public static async void GenerateFormatToStream(BarcodeResult barcodeData, Stream stream, int width, int height) {
            await BarcodeGenerator.WriteToStreamAsync(
              barcodeData.Value,
              stream,
              new BarcodeGeneratorOptions
              {
                  Format = barcodeData.Format,
                  Width = width,
                  Height = height,
                  Margin = 2,
                  ForegroundColor = Colors.Black,
                  BackgroundColor = Colors.White
                }
              );
        }
    }
}

using cashback_infrastructure;

namespace cashback_maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            //сканер баркодов - добавить нужные опции
            cameraBarcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions();
        }

        //при нахождение баркода - добавить нужные действия (переход на другую страницу, сохранение, тп = метод в application)
        //пока что просто уведомление
        protected void BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();
            if (first is null)
            {
                return;
            }

            Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlertAsync("Barcode Detected", first.Value, "OK");
            });
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        //обработчик кнопки "Выбрать файл"
        //открывает проводник, направляет выбранное фото в BarcodeScanner, показывает результат в уведомлении
        private async void OnPickFileClicked(object? sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.image" } },
                { DevicePlatform.Android, new[] { "image/*" } },
                { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png" } }
            });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение с штрих-кодом",
                FileTypes = customFileType
            });
            if (result is null)
            {
                Dispatcher.DispatchAsync(async () =>
                {
                    await DisplayAlertAsync("Ошибка", "Не получено фото", "OK");
                });
                return;
            }

            var barcode = await BarcodeScanner.DecodeBarcodeFromImageAsync(result.FullPath);

            if (barcode is null)
                Dispatcher.DispatchAsync(async () =>
                {
                    await DisplayAlertAsync("Ошибка", "Штрих-код не найден", "ОК");
                });
            else
                Dispatcher.DispatchAsync(async () =>
                {
                    await DisplayAlertAsync("Найден штрихкод", barcode.Value, "OK");
                });
        }
    }
}

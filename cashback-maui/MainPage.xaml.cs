using ZXing.Net.Maui.Controls;

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
    }
}

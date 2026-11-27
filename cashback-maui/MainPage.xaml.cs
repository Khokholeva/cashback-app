namespace cashback_maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Загружаем главный экран приложения
        ContentHost.Content = new Views.CashbacksView();
    }
}
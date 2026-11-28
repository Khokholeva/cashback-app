namespace cashback_maui.Navigation;

public interface IContentManager
{
    //Текущее отображаемое представление
    ContentView? Current { get; }

    //Текущая основная вкладка
    ContentView? CurrentRoot { get; }

    //Можно ли вернуться назад
    bool CanGoBack { get; }

    //Находимся ли мы на основной вкладке
    bool IsAtRoot { get; }

    //Уведомление об изменении навигации
    event EventHandler? NavigationChanged;

    //Открыть подвкладку
    void NavigateTo(ContentView view);

    //Переключить основную вкладку
    void SetRoot(ContentView view);

    //Вернуться назад
    bool GoBack();
}
using Microsoft.Maui.Controls;

namespace cashback_maui.Navigation;

public class ContentManager : IContentManager
{
    private readonly ContentView _host;

    private readonly Stack<ContentView> _rootHistory = new();
    private readonly Stack<ContentView> _subviewHistory = new();

    public ContentView? Current =>
        _subviewHistory.Count > 0
            ? _subviewHistory.Peek()
            : CurrentRoot;

    public ContentView? CurrentRoot =>
        _rootHistory.Count > 0
            ? _rootHistory.Peek()
            : null;

    public bool IsAtRoot => _subviewHistory.Count == 0;

    public bool CanGoBack =>
        _subviewHistory.Count > 0 ||
        _rootHistory.Count > 1;

    public event EventHandler? NavigationChanged;

    public ContentManager(ContentView host)
    {
        _host = host;
    }

    //Открытие подвкладки
    public void NavigateTo(ContentView view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (CurrentRoot == null)
            throw new InvalidOperationException(
                "Cannot navigate without a root view.");

        if (ReferenceEquals(Current, view))
            return;

        (Current as IAppearable)?.OnDisappear();

        _subviewHistory.Push(view);

        ShowCurrent();
    }

    //Переключение основной вкладки
    public void SetRoot(ContentView view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (ReferenceEquals(CurrentRoot, view) && IsAtRoot)
            return;

        (Current as IAppearable)?.OnDisappear();

        //Закрываем все подвкладки
        _subviewHistory.Clear();

        //Запоминаем новую основную вкладку
        if (!ReferenceEquals(CurrentRoot, view))
            _rootHistory.Push(view);

        ShowCurrent();
    }

    //Возвращение назад
    public bool GoBack()
    {
        if (!CanGoBack)
            return false;

        (Current as IAppearable)?.OnDisappear();

        if (_subviewHistory.Count > 0)
        {
            _subviewHistory.Pop();
        }
        else
        {
            _rootHistory.Pop();
        }

        ShowCurrent();

        return true;
    }

    //Обновление отображаемого представления
    private void ShowCurrent()
    {
        var view = Current;

        if (view == null)
            return;

        _host.Content = view;

        (view as IAppearable)?.OnAppear();

        NavigationChanged?.Invoke(this, EventArgs.Empty);
    }
}
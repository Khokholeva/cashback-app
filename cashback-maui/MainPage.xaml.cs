using cashback_maui.Navigation;
using cashback_maui.Views;

namespace cashback_maui;

public partial class MainPage : ContentPage
{
    private readonly IContentManager _contentManager;

    //Основные представления создаются один раз
    private readonly CashbacksView _cashbacksView;
    private readonly DiscountsView _discountsView;
    private readonly MenuView _menuView;

    public MainPage()
    {
        InitializeComponent();

        _cashbacksView = new CashbacksView();
        _discountsView = new DiscountsView();
        _menuView = new MenuView();

        _contentManager = new ContentManager(ContentHost);

        _contentManager.NavigationChanged += OnNavigationChanged;

        //Начальная вкладка
        _contentManager.SetRoot(_cashbacksView);
    }

    //Переключение вкладок
    private void OnCashbacksClicked(object sender, EventArgs e)
    {
        _contentManager.SetRoot(_cashbacksView);
    }

    private void OnDiscountsClicked(object sender, EventArgs e)
    {
        _contentManager.SetRoot(_discountsView);
    }

    private void OnMenuClicked(object sender, EventArgs e)
    {
        _contentManager.SetRoot(_menuView);
    }

    //Обновление нижней навигации
    private void OnNavigationChanged(object? sender, EventArgs e)
    {
        bool isRoot = _contentManager.IsAtRoot;

        //На подвкладках нижнее меню скрыто
        BottomNavigation.IsVisible = isRoot;

        //Выделение активной вкладки
        CashbackIndicator.IsVisible =
            ReferenceEquals(_contentManager.CurrentRoot, _cashbacksView);

        DiscountIndicator.IsVisible =
            ReferenceEquals(_contentManager.CurrentRoot, _discountsView);

        MenuIndicator.IsVisible =
            ReferenceEquals(_contentManager.CurrentRoot, _menuView);

        //Цвет иконок
        CashbackIcon.Source = CashbackIndicator.IsVisible
            ? "cashback_cards_light.svg"
            : "cashback_cards_dark.svg";

        DiscountIcon.Source = DiscountIndicator.IsVisible
            ? "discount_cards_light.svg"
            : "discount_cards_dark.svg";

        MenuIcon.Source = MenuIndicator.IsVisible
            ? "menu_light.svg"
            : "menu_dark.svg";
    }

    //Системная кнопка Android Back
    protected override bool OnBackButtonPressed()
    {
        if (_contentManager.GoBack())
            return true;

        return base.OnBackButtonPressed();
    }
}
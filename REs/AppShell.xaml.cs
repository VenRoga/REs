using REs.Resources.Pages;

namespace REs
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();             
            #region подписки на страницы
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(NewTaskPage), typeof(NewTaskPage));
            Routing.RegisterRoute(nameof(CompletedPage), typeof(CompletedPage));
            Routing.RegisterRoute(nameof(InProccessPage), typeof(InProccessPage));
            #endregion
        }
    }
}

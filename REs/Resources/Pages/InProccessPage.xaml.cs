using Microsoft.Maui.Controls;
using REs.Resources.ViewModels;

namespace REs.Resources.Pages
{
    public partial class InProccessPage : ContentPage
    {
        private readonly InProccessVM _vm;

        public InProccessPage(InProccessVM vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.LoadTasksCommand.ExecuteAsync(null);
        }
    }
}
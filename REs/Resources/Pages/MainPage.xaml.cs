using Microsoft.Maui.Controls;
using REs.Resources.ViewModels;

namespace REs.Resources.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
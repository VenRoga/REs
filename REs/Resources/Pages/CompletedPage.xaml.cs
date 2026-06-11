using Microsoft.Maui.Controls;
using REs.Resources.ViewModels;

namespace REs.Resources.Pages
{
    public partial class CompletedPage : ContentPage
    {
        public CompletedPage(CompletedVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
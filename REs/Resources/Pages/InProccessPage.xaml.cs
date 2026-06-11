using Microsoft.Maui.Controls;
using REs.Resources.ViewModels;

namespace REs.Resources.Pages
{
    public partial class InProccessPage : ContentPage
    {
        public InProccessPage(InProccessVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
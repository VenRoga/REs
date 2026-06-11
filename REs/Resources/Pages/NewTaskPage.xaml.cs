using Microsoft.Maui.Controls;
using REs.Resources.ViewModels;

namespace REs.Resources.Pages
{
    public partial class NewTaskPage : ContentPage
    {
        public NewTaskPage(NewTaskVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using REs.Resources.Pages;

namespace REs.Resources.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        #region переход на страницы
        [RelayCommand]
        private async Task GoToInProccess()
        {
            await Shell.Current.GoToAsync(nameof(InProccessPage));
        }

        [RelayCommand]
        private async Task GoToCompleted()
        {
            await Shell.Current.GoToAsync(nameof(CompletedPage));
        }
        [RelayCommand]
        private async Task GoToNewTask()
        {
            await Shell.Current.GoToAsync(nameof(NewTaskPage));
        }
        #endregion
    }
}

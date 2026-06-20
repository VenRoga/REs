using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModelsLib;
using REs.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace REs.Resources.ViewModels
{
    public partial class CompletedVM : ObservableObject
    {
        #region поля
        private readonly APIServices _apiServices;
        [ObservableProperty]
        private ObservableCollection<TaskListItemVM> _tasks = new();
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage;
        #endregion
        public CompletedVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }
        #region команды
        [RelayCommand]
        private async Task LoadTasks()
        {
            if (IsBusy) return;
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var tasks = await _apiServices.GetReadyTasksAsync();
                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(new TaskListItemVM(task));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading completed tasks: {ex.Message}";
                Debug.WriteLine($"{ErrorMessage}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task DeleteTask(TaskListItemVM taskVm)
        {
            if (taskVm == null) return;
            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Task",
                $"Are you sure you want to delete '{taskVm.Name}'?",
                "Yes", "No");

            if (!confirm) return;
            IsBusy = true;
            try
            {
                var success = await _apiServices.DeleteTaskAsync(taskVm.Id);
                if (success)
                {
                    Tasks.Remove(taskVm);
                    await Shell.Current.DisplayAlert("Success", "Task deleted", "OK");
                }
                else
                {
                    ErrorMessage = "Failed to delete task";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task ReopenTask(TaskListItemVM taskVm)
        {
            if (taskVm == null) return;

            IsBusy = true;
            try
            {
                Debug.WriteLine($"Reopen: Id={taskVm.Id}, Name={taskVm.Name}, Deadline={taskVm.Deadline}");

                var task = new TaskModel
                {
                    Id = taskVm.Id,
                    Name = taskVm.Name,
                    Ready = false,
                    Deadline = taskVm.Deadline
                };

                var success = await _apiServices.UpdateTaskAsync(task);
                Debug.WriteLine($"UpdateTaskAsync вернул: {success}");

                if (success)
                {
                    Tasks.Remove(taskVm);
                    await Shell.Current.DisplayAlert("Success", "Task reopened!", "OK");
                }
                else
                {
                    ErrorMessage = "Failed to reopen task";
                    Debug.WriteLine("Ошибка: сервер вернул false");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Debug.WriteLine($"Исключение: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}
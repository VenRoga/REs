using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModelsLib;
using REs.Resources.Pages;
using REs.Services;

namespace REs.Resources.ViewModels
{
    public partial class NewTaskVM : ObservableObject
    {
        private readonly APIServices _apiServices;

        #region поля
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _taskName;
        [ObservableProperty]
        private DateTime _endTime = DateTime.Now.AddDays(1);
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage;
        #endregion
        public NewTaskVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }
        #region команды
        [RelayCommand]
        private async Task CreateTask()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                ErrorMessage = "Please enter task name"; return;
            }
            if (EndTime < DateTime.Now)
            {
                ErrorMessage = "End time cannot be in the past"; return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var newTask = new TaskModel
                {
                    Name = TaskName,
                    Ready = false,
                    Created = DateTime.Now,
                    Ended = EndTime
                };

                var createdTask = await _apiServices.CreateTaskAsync(newTask);

                if (createdTask != null && createdTask.Id > 0)
                {
                    Id = createdTask.Id;
                    await Shell.Current.DisplayAlert("Success", "Task created successfully!", "OK");
                    TaskName = string.Empty;
                    EndTime = DateTime.Now.AddDays(1);
                    await GoToInProccess();
                }
                else
                {
                    ErrorMessage = "Failed to create task on server.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Network error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToInProccess()
        {
            await Shell.Current.GoToAsync(nameof(InProccessPage));
        }
        #endregion
    }
}
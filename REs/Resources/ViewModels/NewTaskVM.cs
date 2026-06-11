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

        [ObservableProperty]
        private string taskName;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

       
        public NewTaskVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }

        partial void OnTaskNameChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                ErrorMessage = string.Empty;
        }

        [RelayCommand]
        private async Task CreateTask()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                ErrorMessage = "Please enter task name";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var newTask = new TaskModel
                {
                    Name = TaskName,
                    Ready = false,
                    Created = DateTime.Now
                };

                var success = await _apiServices.CreateTaskAsync(newTask);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Task created successfully!", "OK");
                    TaskName = string.Empty;
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
        private async Task GoToInProccess() =>
            await Shell.Current.GoToAsync(nameof(InProccessPage));

        [RelayCommand]
        private async Task GoToCompleted() =>
            await Shell.Current.GoToAsync(nameof(CompletedPage));
    }
}
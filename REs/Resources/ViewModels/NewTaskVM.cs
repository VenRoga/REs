using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModelsLib;
using REs.Resources.Pages;
using REs.Services;
using System.Diagnostics;
using System.Globalization;

namespace REs.Resources.ViewModels
{
    public partial class NewTaskVM : ObservableObject
    {
        #region поля
        private readonly APIServices _apiServices;
        [ObservableProperty]
        private string _endTimeString = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _taskName;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage;
        public bool IsFormValid => !string.IsNullOrWhiteSpace(TaskName) && !IsBusy;
        #endregion

        public NewTaskVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }
        #region команды
        [RelayCommand(CanExecute = nameof(IsFormValid))]
        //создание задачи
        private async Task CreateTask()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                ErrorMessage = "Please enter task name";
                return;
            }
            if (string.IsNullOrWhiteSpace(EndTimeString))
            {
                ErrorMessage = "Please enter a date";
                return;
            }
            if (!DateTime.TryParseExact(EndTimeString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndTime))
            {
                ErrorMessage = "Please enter a valid date format (dd.mm.yyyy)";
                return;
            }
            if (parsedEndTime.Date < DateTime.Today)
            {
                ErrorMessage = "Deadline cannot be in the past";
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
                    Created = DateTime.UtcNow,
                    Ended = null,
                    Deadline = parsedEndTime.ToUniversalTime()
                };
                var json = System.Text.Json.JsonSerializer.Serialize(newTask);
                Debug.WriteLine($"Sending: {json}");
                var createdTask = await _apiServices.CreateTaskAsync(newTask);
                if (createdTask != null && createdTask.Id > 0)
                {
                    await Shell.Current.DisplayAlert("Success", "Task created successfully!", "OK");
                    TaskName = string.Empty;
                    EndTimeString = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");
                }
                else
                {
                    ErrorMessage = "Failed to create task on server.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                    ErrorMessage += $"\nInner: {ex.InnerException.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
        //переход между страницами
        [RelayCommand]
        private async Task GoToInProccessPage()
        {
            await Shell.Current.GoToAsync(nameof(InProccessPage));
        }
        [RelayCommand]
        private static async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}
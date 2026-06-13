using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModelsLib;
using REs.Resources.Pages;
using REs.Services;
using System.Globalization;

namespace REs.Resources.ViewModels
{
    public partial class NewTaskVM : ObservableObject
    {
        private readonly APIServices _apiServices;

        #region поля
        public bool IsFormValid => !string.IsNullOrWhiteSpace(TaskName) && !string.IsNullOrWhiteSpace(EndTmeString) && !IsBusy;
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))] 
        private string _taskName;
        [ObservableProperty]
        private DateTime _endTime;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))] 
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateTaskCommand))]
        [NotifyPropertyChangedFor(nameof(IsFormValid))]
        private string _endTmeString = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");

        #endregion
        public NewTaskVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }
        #region команды
        [RelayCommand(CanExecute = nameof(IsFormValid))]
        private async Task CreateTask()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                ErrorMessage = "Please enter task name";
                return;
            }
            if (!DateTime.TryParseExact(EndTmeString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndTime))
            {
                ErrorMessage = "Please enter a valid date format (dd.mm.yyyy)";
                return;
            }
            if (parsedEndTime.Date < DateTime.Today)
            {
                ErrorMessage = "End time cannot be in the past";
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
                    Created = DateTime.Now,
                    Ended = parsedEndTime
                };

                var createdTask = await _apiServices.CreateTaskAsync(newTask);
                if (createdTask != null && createdTask.Id > 0)
                {
                    Id = createdTask.Id;
                    await Shell.Current.DisplayAlert("Success", "Task created successfully!", "OK");
                    TaskName = string.Empty;
                    EndTmeString = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy"); 
                    await GoToInProccessPage();
                }
                else ErrorMessage = "Failed to create task on server.";
            }
            catch (Exception ex) { ErrorMessage = $"Network error: {ex.Message}"; }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task GoToInProccessPage()
        {
            await Shell.Current.GoToAsync(nameof(InProccessPage));
        }
        [RelayCommand]
        static private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync("..");
        }       
        #endregion
    }
}
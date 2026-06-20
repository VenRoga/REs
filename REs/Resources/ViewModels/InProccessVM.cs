using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using ModelsLib;
using REs.Services;

namespace REs.Resources.ViewModels
{
    public partial class InProccessVM : ObservableObject
    {
        private readonly APIServices _apiServices;

        [ObservableProperty]
        private ObservableCollection<TaskListItemVM> _tasks = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public InProccessVM(APIServices apiServices)
        {
            _apiServices = apiServices;
        }

        [RelayCommand]
        public void SortByDeadline()
        {
            var sortedTasks = Tasks
              .OrderBy(task => task.Deadline?.ToLocalTime().Date ?? DateTime.MaxValue)
        .ThenBy(task => task.Name)
        .ToList();

            Tasks = new ObservableCollection<TaskListItemVM>(sortedTasks);
        }

        [RelayCommand]
        public async Task LoadTasks()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var tasks = await _apiServices.GetTasksAsync();

                var inProcessTasks = tasks
                    .Where(task => !task.Ready)
                    .OrderBy(task => task.Name)
                    .Select(task => new TaskListItemVM(task));

                Tasks = new ObservableCollection<TaskListItemVM>(inProcessTasks);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load tasks: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public partial class TaskListItemVM : ObservableObject
    {
        public TaskModel Task { get; }

        public int Id => Task.Id;
        public string Name => Task.Name;
        public DateTime? Deadline => Task.Deadline;
        public bool IsReady => Task.Ready;

        public string DeadlineText => Deadline.HasValue
            ? Deadline.Value.ToLocalTime().ToString("dd.MM.yyyy")
            : "No deadline";

        public Color DeadlineColor
        {
            get
            {
                if (Task.Ready)
                    return Color.FromArgb("#800080");

                if (!Deadline.HasValue)
                    return Colors.Gray;

                var daysLeft = (Deadline.Value.ToLocalTime().Date - DateTime.Today).Days;

                if (daysLeft <= 1)
                    return Colors.Red;

                if (daysLeft == 2)
                    return Colors.Goldenrod;

                return Colors.Green;
            }
        }

        public TaskListItemVM(TaskModel task)
        {
            Task = task;
        }
    }
}
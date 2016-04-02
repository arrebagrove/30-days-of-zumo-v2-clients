using Microsoft.WindowsAzure.MobileServices;
using Shellmonger.TaskList.Services;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Shellmonger.TaskList.Pages
{
    public sealed partial class TasksPage : Page
    {
        private IMobileServiceTable<TodoItem> dataTable = App.CloudProvider.GetTable<TodoItem>();
        private MobileServiceCollection<TodoItem, TodoItem> tasks;

        public TasksPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When the page is brought up, refresh the table.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshTasks();
        }

        /// <summary>
        /// Event Handler: Log Out
        /// </summary>
        private async void HangUp_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!LogoutIcon.IsTapEnabled) return;
            App.CloudProvider.Trace("TasksPage", "HangUp_Click");
            // Change the color of the button to gray and disable it
            var color = LogoutIcon.Foreground;
            LogoutIcon.Foreground = new SolidColorBrush(Colors.Gray);
            LogoutIcon.IsTapEnabled = false;

            try
            {
                await App.CloudProvider.LogoutAsync();
                Frame.Navigate(typeof(EntryPage));
            }
            catch (CloudAuthenticationException exception)
            {
                System.Diagnostics.Debug.WriteLine("Failed to logout: {0}", exception.InnerException.Message);
                var dialog = new MessageDialog(exception.Message);
                await dialog.ShowAsync();
            }

            // Re-enable the button
            LogoutIcon.Foreground = color;
            LogoutIcon.IsTapEnabled = true;
        }

        /// <summary>
        /// Event Handler: Clicked on People
        /// </summary>
        private void People_Click(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PeoplePage));
        }

        private async void RefreshIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!RefreshIcon.IsTapEnabled) return;
            App.CloudProvider.Trace("TasksPage", "RefreshIcon_Click");
            await RefreshTasks();
        }

        private async void AddTaskIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!AddTask.IsTapEnabled) return;
            App.CloudProvider.Trace("PeoplePage", "AddFriendIcon_Click");
            // Disable the Add Task Icon
            var color = AddTask.Foreground;
            AddTask.Foreground = new SolidColorBrush(Colors.Gray);
            AddTask.IsTapEnabled = false;

            var stackPanel = new StackPanel();
            var taskBox = new TextBox
            {
                Text = "",
                PlaceholderText = "Enter New Task",
                Margin = new Windows.UI.Xaml.Thickness(8.0)
            };
            stackPanel.Children.Add(taskBox);

            var dialog = new ContentDialog
            {
                Title = "Add New Task",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel"
            };
            dialog.Content = stackPanel;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var newTask = new TodoItem { Title = taskBox.Text.Trim() };
                StartNetworkActivity();
                await dataTable.InsertAsync(newTask);
                tasks.Add(newTask);
                StopNetworkActivity();
            }

            // Enable the Add Task Icon
            AddTask.Foreground = color;
            AddTask.IsTapEnabled = true;
        }

        private async Task RefreshTasks()
        {
            StartNetworkActivity();
            try
            {
                tasks = await dataTable.ToCollectionAsync();
                TaskListView.ItemsSource = tasks;
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                await new MessageDialog(exception.Message, "Error Loading Tasks").ShowAsync();
            }
            StopNetworkActivity();
        }

        private void StartNetworkActivity()
        {
            RefreshIconRotation.Begin();
            RefreshIcon.IsTapEnabled = false;
        }

        private void StopNetworkActivity()
        {
            RefreshIcon.IsTapEnabled = true;
            RefreshIconRotation.Stop();
        }
    }
}

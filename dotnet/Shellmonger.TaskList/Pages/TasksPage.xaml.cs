using Microsoft.WindowsAzure.MobileServices;
using Shellmonger.TaskList.Services;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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

        /// <summary>
        /// Event Handler: Clicked on Refresh Icon
        /// </summary>
        private async void RefreshIcon_Click(object sender, RoutedEventArgs e)
        {
            if (!RefreshIcon.IsTapEnabled) return;
            await RefreshTasks();
        }

        /// <summary>
        /// Event Handler: Clicked on Add Task Icon
        /// </summary>
        private async void AddTaskIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!AddTask.IsTapEnabled) return;

            // Disable the Add Task Icon
            var color = AddTask.Foreground;
            AddTask.Foreground = new SolidColorBrush(Colors.Gray);
            AddTask.IsTapEnabled = false;

            // Build the contents of the dialog box
            var stackPanel = new StackPanel();
            var taskBox = new TextBox
            {
                Text = "",
                PlaceholderText = "Enter New Task",
                Margin = new Windows.UI.Xaml.Thickness(8.0)
            };
            stackPanel.Children.Add(taskBox);

            // Create the dialog box
            var dialog = new ContentDialog
            {
                Title = "Add New Task",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel"
            };
            dialog.Content = stackPanel;

            // Show the dialog box and handle the response
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var newTask = new TodoItem { Title = taskBox.Text.Trim() };
                StartNetworkActivity();
                try
                {
                    await dataTable.InsertAsync(newTask);
                    tasks.Add(newTask);
                }
                catch (MobileServiceInvalidOperationException exception)
                {
                    var opDialog = new MessageDialog(exception.Message);
                    await opDialog.ShowAsync();
                }
                StopNetworkActivity();
            }

            // Enable the Add Task Icon
            AddTask.Foreground = color;
            AddTask.IsTapEnabled = true;
        }

        /// <summary>
        /// Event Handler: A checkbox on the list was either set or cleared
        /// </summary>
        private async void taskComplete_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            item.Completed = (bool)cb.IsChecked;

            StartNetworkActivity();
            try
            {
                await dataTable.UpdateAsync(item);
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                var dialog = new MessageDialog(exception.Message);
                await dialog.ShowAsync();
            }
            StopNetworkActivity();

            TaskListView.Focus(FocusState.Unfocused);
        }

        /// <summary>
        /// Event Handler: The text box has been updated
        /// </summary>
        private async void taskTitle_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox taskTitle = (TextBox)sender;
            TodoItem item = taskTitle.DataContext as TodoItem;

            // if ESC is pressed, restore the old value
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                taskTitle.Text = item.Title;
                TaskListView.Focus(FocusState.Unfocused);
                return;
            }

            // if Enter is pressed, store the new value
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                item.Title = taskTitle.Text;
                StartNetworkActivity();
                try
                {
                    await dataTable.UpdateAsync(item);
                    TaskListView.Focus(FocusState.Unfocused);
                }
                catch (MobileServiceInvalidOperationException exception)
                {
                    var dialog = new MessageDialog(exception.Message);
                    await dialog.ShowAsync();
                }
                StopNetworkActivity();
                return;
            }
        }


        /// <summary>
        /// Event Handler: Delete a record
        /// </summary>
        private async void taskDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SymbolIcon icon = (SymbolIcon)sender;
            TodoItem item = icon.DataContext as TodoItem;

            StartNetworkActivity();
            try
            {
                await dataTable.DeleteAsync(item);
                tasks.Remove(item);
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                var dialog = new MessageDialog(exception.Message);
                await dialog.ShowAsync();
            }

            StopNetworkActivity();
        }

        /// <summary>
        /// Refresh the Tasks List
        /// </summary>
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

        /// <summary>
        /// Start rotating the refresh icon
        /// </summary>
        private void StartNetworkActivity()
        {
            RefreshIconRotation.Begin();
            RefreshIcon.IsTapEnabled = false;
        }

        /// <summary>
        /// Stop rotating the refresh icon
        /// </summary>
        private void StopNetworkActivity()
        {
            RefreshIcon.IsTapEnabled = true;
            RefreshIconRotation.Stop();
        }

    }
}

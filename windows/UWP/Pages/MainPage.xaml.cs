using Microsoft.WindowsAzure.MobileServices;
using System;
using UWP.Services;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        private MobileServiceCollection<TodoItem, TodoItem> items;

        public MainPage()
        {
            this.InitializeComponent();
        }

        #region Event Handlers
        /// <summary>
        /// When the page is loaded, refresh the data
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RefreshIcon_Click(this, null);
        }

        /// <summary>
        /// Event Handler for the Logout Button
        /// </summary>
        private async void LogoutIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            await AzureMobileApp.LogoutAsync();
            Frame.Navigate(typeof(EntryPage), null);
        }

        /// <summary>
        /// Event Handler for the Refresh Button
        /// </summary>
        private async void RefreshIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            // Disable the Refresh Icon
            RefreshIcon.IsTapEnabled = false;
            var color = RefreshIcon.Foreground;
            RefreshIcon.Foreground = TitleGrid.Background;

            var table = AzureMobileApp.GetTaskTable();
            try
            {
                items = await table.Where(item => item.Complete == false).ToCollectionAsync();
                TaskItems.ItemsSource = items;
                this.SaveButton.IsEnabled = true;
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                await new MessageDialog(exception.Message, "Error Loading Tasks").ShowAsync();
            }

            // Enable the Refresh Icon
            RefreshIcon.IsTapEnabled = true;
            RefreshIcon.Foreground = color;
        }

        /// <summary>
        /// Event Handler for the Save Button
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable the Save Button
            SaveButton.IsEnabled = false;
            TaskInput.IsEnabled = false;

            var task = new TodoItem { Text = TaskInput.Text };
            var table = AzureMobileApp.GetTaskTable();
            await table.InsertAsync(task);
            items.Add(task);

            // Enable the Save Button
            SaveButton.IsEnabled = true;
            TaskInput.IsEnabled = true;
            TaskInput.Text = "";
        }

        /// <summary>
        /// Event Handler for checking the completed box next to an event
        /// </summary>
        private async void TaskCompleted_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            await AzureMobileApp.GetTaskTable().UpdateAsync(item);
            items.Remove(item);
            TaskItems.Focus(FocusState.Unfocused);
        }

        /// <summary>
        /// Event Handler - called when user types a letter into the text input box - handles Enter
        /// </summary>
        private void TaskInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SaveButton.Focus(FocusState.Programmatic);
            }
        }
        #endregion
    }
}

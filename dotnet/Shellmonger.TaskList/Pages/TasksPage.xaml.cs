using Shellmonger.TaskList.Services;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Shellmonger.TaskList.Pages
{
    public sealed partial class TasksPage : Page
    {
        public TasksPage()
        {
            this.InitializeComponent();
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
            RefreshIconRotation.Begin();
            RefreshIcon.IsTapEnabled = false;

            // TODO: Processing
            await Task.Delay(2000);

            RefreshIcon.IsTapEnabled = true;
            RefreshIconRotation.Stop();
        }

        private async void AddTaskIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!AddTask.IsTapEnabled) return;
            App.CloudProvider.Trace("PeoplePage", "AddFriendIcon_Click");
            // Disable the Add Friend Icon
            var color = AddTask.Foreground;
            AddTask.Foreground = new SolidColorBrush(Colors.Gray);
            AddTask.IsTapEnabled = false;

            // TODO: Processing
            await Task.Delay(2000);

            // Enable the Add Friend Icon
            AddTask.Foreground = color;
            AddTask.IsTapEnabled = true;
        }
    }
}

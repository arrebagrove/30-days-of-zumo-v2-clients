using Shellmonger.TaskList.Services;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Shellmonger.TaskList.Pages
{
    public sealed partial class PeoplePage : Page
    {
        public PeoplePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event Handler: Log Out
        /// </summary>
        private async void HangUp_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!LogoutIcon.IsTapEnabled) return;
            App.CloudProvider.Trace("PeoplePage", "HangUp_Click");
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
        private void Tasks_Click(object sender, TappedRoutedEventArgs e)
        {
            App.CloudProvider.Trace("PeoplePage", "Tasks_Click");
            Frame.Navigate(typeof(TasksPage));
        }

        private async void RefreshIcon_Click(object sender, RoutedEventArgs e)
        {
            if (!RefreshIcon.IsTapEnabled) return;
            App.CloudProvider.Trace("PeoplePage", "RefreshIcon_Click");
            // Rotate the Refresh icon (and disable it)
            RefreshIconRotation.Begin();
            RefreshIcon.IsTapEnabled = false;

            // TODO: Processing
            await Task.Delay(2000);

            // Re-enable the refresh icon
            RefreshIcon.IsTapEnabled = true;
            RefreshIconRotation.Stop();
        }

        private async void AddFriendIcon_Click(object sender, TappedRoutedEventArgs e)
        {
            if (!AddFriendIcon.IsTapEnabled) return;
            App.CloudProvider.Trace("PeoplePage", "AddFriendIcon_Click");
            // Disable the Add Friend Icon
            var color = AddFriendIcon.Foreground;
            AddFriendIcon.Foreground = new SolidColorBrush(Colors.Gray);
            AddFriendIcon.IsTapEnabled = false;

            // TODO: Processing
            await Task.Delay(2000);

            // Enable the Add Friend Icon
            AddFriendIcon.Foreground = color;
            AddFriendIcon.IsTapEnabled = true;
        }
    }
}

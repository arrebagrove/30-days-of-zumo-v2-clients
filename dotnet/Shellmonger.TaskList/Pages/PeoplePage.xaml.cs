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
            Frame.Navigate(typeof(TasksPage));
        }

        private async void RefreshIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RefreshIconRotation.Begin();
            RefreshIcon.IsTapEnabled = false;

            // TODO: Processing
            await Task.Delay(2000);

            RefreshIcon.IsTapEnabled = true;
            RefreshIconRotation.Stop();
        }
    }
}

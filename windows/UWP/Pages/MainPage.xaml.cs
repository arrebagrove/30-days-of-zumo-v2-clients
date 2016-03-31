using System;
using UWP.Services;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace UWP.Pages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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

            // Enable the Refresh Icon
            RefreshIcon.IsTapEnabled = true;
        }

        /// <summary>
        /// Event Handler for the Save Button
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Disable the Save Button
            SaveButton.IsEnabled = false;

            // Enable the Save Button
            SaveButton.IsEnabled = true;
            TaskInput.Text = "";
        }
    }
}

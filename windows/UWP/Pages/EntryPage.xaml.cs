using System;
using UWP.Services;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP.Pages
{
    /// <summary>
    /// The Entry Page for our application - this will just allow the user
    /// to click on an "Entry" button
    /// </summary>
    public sealed partial class EntryPage : Page
    {
        public EntryPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event Handler that is called when this page is loaded - enables the login
        /// button so we can login.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            entryButton.IsEnabled = true;
        }

        /// <summary>
        /// Event Handler that is called when the application needs to be initialized
        /// </summary>
        private async void entryButton_Click(object sender, RoutedEventArgs e)
        {
            entryButton.IsEnabled = false;

            try
            {
                await AzureMobileApp.LoginAsync();
                Frame.Navigate(typeof(MainPage), null);

            }
            catch (AzureMobileAppException exception)
            {
                var dialog = new MessageDialog(exception.Message, "Login Failed");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();

            }

            entryButton.IsEnabled = true;


        }
    }
}

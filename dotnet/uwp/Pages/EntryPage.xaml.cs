using Microsoft.WindowsAzure.MobileServices;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EntryPage : Page
    {
        public EntryPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var client = AzureMobileApp.GetClient();

            try
            {
                await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                Frame.Navigate(typeof(MainPage), null);
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                var message = string.Format("Login Failed: {0}", exception.Message);
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}

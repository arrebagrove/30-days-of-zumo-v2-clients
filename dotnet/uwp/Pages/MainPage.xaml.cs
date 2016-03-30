using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uwp
{
    public sealed partial class MainPage : Page
    {
        private MobileServiceCollection<TodoItem, TodoItem> items;

        public MainPage()
        {
            this.InitializeComponent();
        }

        #region UI Event Handlers
        /// <summary>
        /// Event Handler for navigating to this page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ButtonRefresh_Click(this, null);
        }

        /// <summary>
        /// Event Handler for clicking on Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            ButtonRefresh.IsEnabled = false;
            await RefreshTasks();
            ButtonRefresh.IsEnabled = true;
        }

        /// <summary>
        /// Event Handler for clicking on Save next to the new task box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            // Disable the Save Button and the TextInput
            ButtonSave.IsEnabled = false;
            TextInput.IsEnabled = false;

            // Save the new item
            var task = new TodoItem { Text = TextInput.Text };
            var table = AzureMobileApp.GetTaskTable();
            await table.InsertAsync(task);
            items.Add(task);

            // Enable the Save Button and the TextInput
            ButtonSave.IsEnabled = true;
            TextInput.IsEnabled = true;
            TextInput.Text = "";
        }

        /// <summary>
        /// Event Handler for checking the completed box in the task list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            var table = AzureMobileApp.GetTaskTable();

            await table.UpdateAsync(item);
            items.Remove(item);
            ListItems.Focus(FocusState.Unfocused);
        }

        /// <summary>
        /// Event Handler for pressing Enter in the Text Box of the new task item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInput_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ButtonSave.Focus(FocusState.Programmatic);
            }
        }
        #endregion

        #region Service Management Tasks
        private async Task RefreshTasks()
        {
            var table = AzureMobileApp.GetTaskTable();

            try
            {
                items = await table.Where(item => item.Complete == false).ToCollectionAsync();
                ListItems.ItemsSource = items;
                this.ButtonSave.IsEnabled = true;
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                await new MessageDialog(exception.Message, "Error Loading Tasks").ShowAsync();
            }
        }
        #endregion
    }
}

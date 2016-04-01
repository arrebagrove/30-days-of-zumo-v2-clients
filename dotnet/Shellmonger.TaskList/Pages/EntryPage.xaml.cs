using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Shellmonger.TaskList.Pages
{
    public sealed partial class EntryPage : Page
    {
        public EntryPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event Handler for the Entry Button
        /// </summary>
        private void EntryButton_Clicked(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));

        }
    }
}

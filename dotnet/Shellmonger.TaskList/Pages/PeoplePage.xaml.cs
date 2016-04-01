using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


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
        private void HangUp_Click(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryPage));
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

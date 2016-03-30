using Microsoft.WindowsAzure.MobileServices;

namespace uwp
{
    public class AzureMobileApp
    {
        private static MobileServiceClient client = null;
        private static IMobileServiceTable<TodoItem> taskTable = null;

        public static MobileServiceClient GetClient()
        {
            if (client == null)
            {
                client = new MobileServiceClient("https://30-days-of-zumo-v2.azurewebsites.net");
            }
            return client;
        }

        public static IMobileServiceTable<TodoItem> GetTaskTable()
        {
            if (taskTable == null)
            {
                taskTable = GetClient().GetTable<TodoItem>();
            }
            return taskTable;
        }
    }
}

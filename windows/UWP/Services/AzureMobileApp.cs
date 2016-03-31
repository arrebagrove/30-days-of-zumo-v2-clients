using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace UWP.Services
{
    class AzureMobileApp
    {
        private static string AppService = "https://30-days-of-zumo-v2.azurewebsites.net";
        private static MobileServiceClient client = null;

        /// <summary>
        /// Singleton for accessing the Azure App Service Mobile App client
        /// </summary>
        /// <returns></returns>
        public static MobileServiceClient GetClient()
        {
            if (client == null)
            {
                client = new MobileServiceClient(AppService);
            }
            return client;
        }

        /// <summary>
        /// Login to the remote app service
        /// </summary>
        public static async Task LoginAsync()
        {
            var client = GetClient();

            try
            {
                await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                throw new AzureMobileAppException("Login Failed", exception);
            }
        }

        /// <summary>
        /// Log out of the remote app service
        /// </summary>
        public static async Task LogoutAsync()
        {
            var client = GetClient();
            await client.LogoutAsync();
        }
    }
}

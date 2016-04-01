using System.Threading.Tasks;

namespace Shellmonger.TaskList.Services
{
    public interface ICloudDataProvider
    {
        Task LoginAsync();

        Task LogoutAsync();
    }
}

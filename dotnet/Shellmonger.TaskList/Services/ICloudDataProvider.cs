using System.Threading.Tasks;

namespace Shellmonger.TaskList.Services
{
    public interface ICloudDataProvider
    {
        Task LoginAsync();

        Task LogoutAsync();

        void Trace(string className, string message);
    }
}

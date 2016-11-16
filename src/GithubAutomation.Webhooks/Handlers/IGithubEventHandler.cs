using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GithubAutomation.Webhooks.Handlers
{
    public interface IGithubEventHandler
    {
        Task ExecuteAsync(string action, dynamic data);
    }
}
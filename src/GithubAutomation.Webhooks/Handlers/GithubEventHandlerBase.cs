using System.Threading.Tasks;
using System.Web.Configuration;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    public abstract class GithubEventHandlerBase: IGithubEventHandler
    {
        private GitHubClient _client;

        public GithubEventHandlerBase()
        {
            _client = new GitHubClient(new ProductHeaderValue("github-automation-sample", "0.1.0"));
            _client.Credentials = new Credentials(WebConfigurationManager.AppSettings["PersonalAccessToken"]);
        }

        /// <summary>
        /// Gets the github client
        /// </summary>
        protected GitHubClient Github => _client;

        /// <summary>
        /// Executes the event handler
        /// </summary>
        /// <param name="action">Action name provided by github</param>
        /// <param name="data">Data associated with the event</param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(string action, dynamic data);
    }
}
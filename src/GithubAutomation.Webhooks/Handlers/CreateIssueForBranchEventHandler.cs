using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    /// <summary>
    /// Creates a new issue for a specific branch when it is published to github
    /// </summary>
    public class CreateIssueForBranchEventHandler: IGithubEventHandler
    {
        private GitHubClient _client;
        private Regex _refPattern;

        public CreateIssueForBranchEventHandler()
        {
            _client = new GitHubClient(new ProductHeaderValue("github-automation-sample","0.1.0"));
            _client.Credentials = new Credentials(WebConfigurationManager.AppSettings["PersonalAccessToken"]);

            _refPattern = new Regex("^refs/heads/(.*)$");
        }

        /// <summary>
        /// Creates a new issue for a newly created branch
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync(string action, dynamic data)
        {
            if (action != "push")
            {
                return;
            }

            // Please see the 'push' event on github for the data that you can expect here:
            // https://developer.github.com/v3/activity/events/types/#pushevent

            long repositoryId = data.repository.id;
            bool created = data.created;
            string @ref = data.@ref;
            string author = data.sender.login;
            string commitUrl = data.head_commit.url;

            // Make sure that we have a valid branch pattern.
            // Also, make sure that the branch was created.
            if (_refPattern.IsMatch(@ref) && created)
            {
                string branchName = _refPattern.Match(@ref).Groups[1].Value;
                await _client.Issue.Create(repositoryId, new NewIssue($"New branch [{branchName}]")
                {
                    Assignee = author,
                    Body = $"A new branch was created by {author}\r\nCheck out the last commit at {commitUrl}"
                });
            }
        }
    }
}
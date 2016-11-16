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
    public class CreateIssueForBranchEventHandler : GithubEventHandlerBase
    {
        private Regex _refPattern;

        public CreateIssueForBranchEventHandler()
        {
            _refPattern = new Regex("^refs/heads/(.*)$");
        }

        /// <summary>
        /// Creates a new issue for a newly created branch
        /// </summary>
        /// <returns></returns>
        public override async Task ExecuteAsync(string action, dynamic data)
        {
            if (action != "create" || data.ref_type != "branch")
            {
                return;
            }

            // Please see the 'push' event on github for the data that you can expect here:
            // https://developer.github.com/v3/activity/events/types/#pushevent

            long repositoryId = data.repository.id;
            string branchName = data.@ref;
            string author = data.sender.login;

            await Github.Issue.Create(repositoryId, new NewIssue($"New branch [{branchName}]")
            {
                Assignee = author,
                Body = $"A new branch was created by {author}"
            });

        }
    }
}
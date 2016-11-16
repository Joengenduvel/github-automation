using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    public class CreatePullRequestForBranchEventHandler: GithubEventHandlerBase
    {
        public CreatePullRequestForBranchEventHandler()
        {
            
        }

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
            string masterBranch = data.master_branch;

            var pullRequestData = new NewPullRequest($"Merge changes from {branchName}", branchName, masterBranch)
            {
                Body = $"Please merge changes from the branch {branchName}"
            };

            await Github.PullRequest.Create(repositoryId, pullRequestData);
        }
    }
}
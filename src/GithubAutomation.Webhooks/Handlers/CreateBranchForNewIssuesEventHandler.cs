using System.Threading.Tasks;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    public class CreateBranchForNewIssuesEventHandler: GithubEventHandlerBase
    {
        public override async Task ExecuteAsync(string action, dynamic data)
        {
            if (action != "issues" || data.action != "opened")
            {
                return;
            }

            long repositoryId = data.repository.id;
            int issueNumber = data.issue.number;

            var headRef = await Github.Git.Reference.Get(repositoryId, "heads/master");

            var result = await Github.Git.Reference.Create(repositoryId, 
                new NewReference($"refs/heads/issue-{issueNumber}", headRef.Object.Sha));
        }
    }
}
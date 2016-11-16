using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    /// <summary>
    /// Closes related issues automatically when completing a pull request.
    /// </summary>
    public class CloseRelatedIssuesAfterPullrequestCloseEventHandler: GithubEventHandlerBase
    {
        private readonly Regex _branchTitlePattern;
        private readonly Regex _treeReferencePattern;

        /// <summary>
        /// Initializes a new instance of <see cref="CloseRelatedIssuesAfterPullrequestCloseEventHandler"/>
        /// </summary>
        public CloseRelatedIssuesAfterPullrequestCloseEventHandler()
        {
            _branchTitlePattern = new Regex("\\[(.*)\\]$");
            _treeReferencePattern = new Regex("/[a-zA-Z0-9\\-_]/tree/(.*)");
        }

        /// <summary>
        /// Executes the event handler
        /// </summary>
        /// <param name="action"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(string action, dynamic data)
        {
            if (action != "pull_request" || data.action != "closed")
            {
                return;
            }

            long repositoryId = data.pull_request.@base.repo.id;
            string repoName = data.pull_request.@base.repo.full_name;

            int pullRequestId = data.number;
            
            string branchName = data.pull_request.head.@ref;

            var issues = await Github.Issue.GetAllForRepository(repositoryId);
            var relatedIssues = issues.Where(issue =>
            {
                bool isBranchInTitle = _branchTitlePattern.IsMatch(issue.Title) &&
                                       _branchTitlePattern.Match(issue.Title).Groups[1].Value == branchName;

                //TODO: Apply more searches to find references to the branch that was merged through the pull request.
                
                return isBranchInTitle;
            });

            foreach (var issue in relatedIssues)
            {
                await Github.Issue.Comment.Create(repositoryId, 
                    issue.Number, $"Closing this issue because pull request #{pullRequestId}");

                await Github.Issue.Update(repositoryId, issue.Number, new IssueUpdate()
                {
                    State = ItemState.Closed
                });
            }
        }
    }
}
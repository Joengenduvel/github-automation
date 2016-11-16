using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Octokit;

namespace GithubAutomation.Webhooks.Handlers
{
    /// <summary>
    /// Creates a tag when a pull request is closed
    /// </summary>
    public class TagLastCommitAfterPullRequestMergeEventHandler: GithubEventHandlerBase
    {
        /// <summary>
        /// Executes the event handler
        /// </summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data received from github</param>
        /// <returns></returns>
        public override async Task ExecuteAsync(string action, dynamic data)
        {
            if (action != "pull_request")
            {
                return;
            }

            // Please see the 'push' event on github for the data that you can expect here:
            // https://developer.github.com/v3/activity/events/types/#pullrequestevent

            string pullRequestAction = data.action;
            
            if (pullRequestAction != "closed")
            {
                return;
            }

            int pullRequestNumber = data.number;
            long repositoryId = data.pull_request.head.repo.id;
            string commitId = data.pull_request.head.sha;

            var originalCommit = await Github.Git.Commit.Get(repositoryId, commitId);

            var tagResult = await Github.Git.Tag.Create(repositoryId, new NewTag()
            {
                Object = commitId,
                Message = $"Tagged for pull-request #{pullRequestNumber}",
                Tag = $"pull-request-{pullRequestNumber}",
                Tagger = new Committer(originalCommit.Committer.Name,originalCommit.Committer.Email,DateTimeOffset.UtcNow),
                Type = TaggedType.Commit
            });

            Trace.WriteLine($"Created tag {tagResult.Tag} for commit {tagResult.Sha}");
        }
    }
}
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using GithubAutomation.Webhooks.Handlers;

namespace GithubAutomation.Webhooks.WebHooks
{
    /// <summary>
    /// Handles incoming webhook calls from github
    /// </summary>
    public class GithubWebhookHandler : WebHookHandler
    {
        private List<IGithubEventHandler> _eventHandlers;

        /// <summary>
        /// Initializes a new instance of <see cref="GithubWebhookHandler"/>
        /// </summary>
        public GithubWebhookHandler()
        {
            this.Receiver = GitHubWebHookReceiver.ReceiverName;
            _eventHandlers = new List<IGithubEventHandler>()
            {
                new CreateIssueForBranchEventHandler(),
                new TagLastCommitAfterPullRequestMergeEventHandler(),
                new CreatePullRequestForBranchEventHandler()
            };
        }

        /// <summary>
        /// Executes the webhook handler
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            // For more information about GitHub WebHook payloads, please see 
            // 'https://developer.github.com/webhooks/'
            JObject entry = context.GetDataOrDefault<JObject>();

            foreach (var handler in _eventHandlers)
            {
                await handler.ExecuteAsync(context.Actions.First(), entry);
            }
        }
    }
}
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json.Linq;

namespace GithubAutomation.Webhooks.WebHooks
{
    /// <summary>
    /// Handles incoming webhook calls from github
    /// </summary>
    public class GithubWebhookHandler: WebHookHandler
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GithubWebhookHandler"/>
        /// </summary>
        public GithubWebhookHandler()
        {
            this.Receiver = GitHubWebHookReceiver.ReceiverName;
        }

        /// <summary>
        /// Executes the webhook handler
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            // For more information about GitHub WebHook payloads, please see 
            // 'https://developer.github.com/webhooks/'
            JObject entry = context.GetDataOrDefault<JObject>();

            Trace.WriteLine("Received call from github");

            return Task.FromResult(true);
        }
    }
}
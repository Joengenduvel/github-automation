using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using GithubAutomation.Webhooks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace GithubAutomation.Webhooks
{
    /// <summary>
    /// The startup logic for the web application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the request pipeline for the application
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(CreateHttpConfiguration());
        }

        private HttpConfiguration CreateHttpConfiguration()
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.InitializeReceiveGitHubWebHooks();

            return config;
        }
    }
}
# Github automation sample with ASP.NET Webhooks
This repository contains a short sample application that allows you to do the following things:

 - Automatically create a new tag when a pull request is merged
 - Automatically create a branch when you create a new issue
 - Automatically create a new issue when you create a new branch
 - Automatically close the issues associated with a pull request when you merge the pull request

## Quickstart
If you want to test this quickly, fire up the web application and note the port.
Next run [ngrok](https://ngrok.com/download) with the following command:

```
ngrok http YOUR_PORT
```

After that go to github and hook up a new webhook to your repository using 
the url that is displayed by ngrok.

Check the documentation on how to set up a webhook in a repository: 
https://developer.github.com/webhooks/creating/#setting-up-a-webhook

The URL for the receiver is of the kind 

```
http://YOUR_HOST/api/webhooks/incoming/github/YOUR_ID
```

The hostname will be the one ngrok gives you. For production this will be the host of your
webhost running the webhooks. You will need to specify an ID for the webhook.

The github webhook needs to be secured using a secret you generate.
This needs to be a SHA-1 hash. You can generate a secret online using the following website:

http://www.sha1-online.com/

Enter this secret in your web.config like so:

```
<appSettings>
    <add key="MS_WebHookReceiverSecret_GitHub" value="YOUR_ID=YOUR_SECRET"/>
</appSettings>
```

Make sure you use the exact URL as specified here and the secret from your
configuration file otherwise the webhook will not work.

To be able to communicate back to github you must authenticate the application via an access token.
These tokens can be managed here: https://github.com/settings/tokens
Once you have your token, add it ti the web.config

```
<appSettings>
    <add key="MS_WebHookReceiverSecret_GitHub" value="YOUR_ID=YOUR_SECRET"/>
    <add key="PersonalAccessToken" value="YOUR_ACCESS_TOKEN"/>
</appSettings>
```

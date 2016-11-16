# Github automation sample with ASP.NET Webhooks
This repository contains a short sample application that allows you to do the following things:

 - Automatically create a new tag when a pull request is merged
 - Automatically create a branch when you create a new issue
 - Automatically create a new issue when you create a new branch
 - Automatically close the issues associated with a pull request when you merge the pull request

## Quickstart
If you want to test this quickly, fire up the web application and note the port.
Next run [ngrok]() with the following command:

```
ngrok http YOUR_PORT
```

After that go to github and hook up a new webhook to your repository using 
the url that is displayed by ngrok.

Check the documentation on how to set up a webhook in a repository: 
https://developer.github.com/webhooks/creating/#setting-up-a-webhook
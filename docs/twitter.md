# Creating Twitter API credentials

The prerequisites for accessing the Twitter API v2 and searching for tweets are:
- a Twitter Developer account
- a Twitter Project + Application with Elevated access (free, but needs a short approval process), and make sure to keep:
  - API Key
  - API Secret
  - Bearer Token


## If you don't have a Twitter account:
- Access [Twitter](https://twitter.com/i/flow/signup) home page and Sign up for an account.

## If you have a Twitter account, you can now enable it as a developer account:
- Sign up at [developer.twitter.com](https://developer.twitter.com/) with your basic name, location and use case details
- You will be asked to review and accept the [developer agreement terms](https://developer.twitter.com/en/developer-terms/agreement.html)
- You should have now access to the Developer Portal to create a new App and Project with Elevated access

## Creating an Application
We will use the Twitter API v2, which provides some new features and a richer response for the Tweet contents, so we can easily fetch the related attachments of each tweet.

You can follow this [tutorial](https://developer.twitter.com/en/docs/tutorials/step-by-step-guide-to-making-your-first-request-to-the-twitter-api-v2) for a step-by-step guide on how to create an application for the V2 API.

Make sure to write down in a safe space the `API Key`, `API Secret`, and `Bearer Token` values of your new application. We will need them for configuring the Octweet app.

## API
We will be using the [Search V2 API](https://developer.twitter.com/en/docs/twitter-api/tweets/search/introduction), and specifically the [Recent search](https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent). This API provides access to filtered public Tweets over the course of the last week, and needs an elevated developer account.

[Go back to main Readme file](/README.md)

## Resources
- [Twitter API Dashboard](https://developer.twitter.com/en/portal/dashboard)
- [Twitter Developer Agreement](https://developer.twitter.com/en/developer-terms/agreement.html)
- [Recent Serach V2 API Reference](https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent)
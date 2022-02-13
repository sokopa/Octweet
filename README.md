# Octweet

Project repo for Modern Web Applications Post-grad Technoeconomics course - 2021/2022

---

## :building_construction: Setting Up

### :old_key: API Credentials

This application contacts the `Twitter API` and `Google Vision API`. You will need your own credentials in order to access these APIs using this application.

You can access the following pages in order to see more thorough guides on how to create your credentials for each service:
 - [Twitter](/docs/twitter.md)
 - [Google Cloud Vision](/docs/google.md)

### :scroll: Prerequisites

In order to run the application, you will need:
- Docker to be installed
- Credentials for Twitter API and Google Vision API from the section [above](/README.md#api-credentials)

The application is packaged along with a MySQL database and a structured logging service in a Docker Compose file, which accepts configuration in a `.env` file in the root folder of the repository. 

There is a `sample.env` file containing the needed configuration that needs to be updated before running. You can make a copy of this file and rename it to `.env`, and follow the guidance inside it to properly set it up.

<details> <summary><b>Show sample <code>.env</code></b></summary>

```sh
# replace {DB_PASSWORD} with a password of your choosing. Make sure they match in the below two lines.
MYSQL_ROOT_PASSWORD={DB_PASSWORD}
ConnectionStrings__OctweetDB=Server=127.0.0.1;Database=OctweetDB;Uid=root;Pwd={DB_PASSWORD};

# replace {REPLACE_JSON_PATH} with the path of your Google credentials JSON file.
# for Windows users: set this to a similar format like /c/path/file.json
GOOGLE_CREDENTIALS_JSON_PATH={REPLACE_JSON_PATH}

# replace the following values with the actual values of your Twitter application
Twitter__ApiKey={REPLACE_APIKEY}
Twitter__ApiSecret={REPLACE_APISECRET}
Twitter__BearerToken={REPLACE_BEARERTOKEN}

# this is the current configuration for the search that the application will perform. (tweets from account @ukpapers)
# you can uncomment the below line and specify your query.
# Twitter__Query=from:ukpapers

# leave the below lines as-is
DB_HOST=127.0.0.1
NETCORE_ENVIRONMENT=Staging
Google__VisionCredentialsPath=/tmp/keys/googlecredential.json
```

</details>

---

## :runner: Running the app

Now that you have your `.env` file setup, it's time to spin up the application.

From the root folder of the repository, execute `docker-compose up` in your favorite terminal. 

This will start:
- a container running a MySQL DB instance inside the Docker container
- a container running a Seq service instance which will collect and display the application's logs in a structured format
- a container running an `Octweet` application instance

The database and the logs service are set up in the docker-compose file in such a way that their data will be retained upon restarts.

To stop the containers, you can type `Ctrl-C` in the terminal.

---

## :mag: Internals

For more information about the Database schema and the method used, you can refer to [Database Docs](docs/database.md)

For more information about the application's structure and the architecture, you can refer to [Application Docs](docs/application.md)

---

## :wrench: Technologies used

The application is written in [C#](https://docs.microsoft.com/en-us/dotnet/csharp/), with [.NET 6](https://docs.microsoft.com/en-us/dotnet/).

The database that we used was [MySQL](https://www.mysql.com/), and in the docker container that is provided contains an image with version 8.0.27 of MySQL.

For the communication between our client application and the database, we used the native ORM of .NET technology stack, which is [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/). The model of the database was designed in a code-first way, and the DB model is generated and updated when the application starts.

For communicating with the external APIs, we used the following .NET client libraries:
- Google Vision API: `Google.Cloud.Vision.V1` 
    - :package: [NuGet](https://www.nuget.org/packages/Google.Apis.Vision.v1)
    - :octocat: [GitHub](https://github.com/googleapis/google-api-dotnet-client)
    - :book: [Docs](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Vision.V1/latest/index)
- Twitter API: `TweetinviAPI` 
    - :package: [Nuget](https://www.nuget.org/packages/TweetinviAPI/)
    - :octocat: [GitHub](https://github.com/linvi/tweetinvi)
    - :book: [Docs](https://linvi.github.io/tweetinvi/dist/index.html)

For logging purposes, we have two components responsible:

One lives in the client, and is the .NET [Serilog](https://serilog.net/) library. It is a flexible library that works alongside Microsoft's Logging infrastructure (ILogger interface), gathers structured log events and is outputing them to the configured ["Sinks"](https://github.com/serilog/serilog/wiki/Provided-Sinks). We are logging to the `Console` directly, and also to a separate service which will also persist the logs after the application is terminated.

The other part is the aforementioned service. The Docker container is bundled with [Seq](https://datalust.co/seq), a real-time search and analysis server for structured application log data, and we have configured Serilog to output its logs to the `Seq` Sink, contacting the respective HTTP API in the Docker container.

Finally, in order to have the application and the infrastructure ready to be used, we have used [Docker](https://www.docker.com/) to bundle our custom application alongside the services it needs, as mentioned before: the MySQL database and the Seq service. The whole application is configured to run with multiple containers, using [Docker Compose](https://www.docker.com/). You can inspect the contents of the configuration file in [docker-compose.yml](/docker-compose.yml).

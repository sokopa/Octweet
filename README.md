# Octweet
Project repo for Modern Web Applications Post-grad Technoeconomics course - 2021/2022

## Setting Up

### API Credentials

This application contacts the `Twitter API` and `Google Vision API`. You will need your own credentials in order to access these APIs using this application.

You can access the following pages in order to see more thorough guides on how to create your credentials for each service:
 - [Twitter](/docs/twitter.md)
 - [Google Cloud Vision](/docs/google.md)

### Prerequisites

In order to run the application, you will need:
- Docker to be installed
- Credentials for Twitter API and Google Vision API from the section [above](/README.md#api-credentials)

The application is packaged along with a MySQL database and a structured logging service in a Docker Compose file, which accepts configuration in a `.env` file in the root folder of the repository. 

There is a `sample.env` file containing the needed configuration that needs to be updated before running. You can make a copy of this file and rename it to `.env`, and follow the guidance inside it to properly set it up:

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

Now that you have your `.env` file setup, it's time to spin up the application.

From the root folder of the repository, execute `docker-compose up` in your favorite terminal. 

This will start:
- a container running a MySQL DB instance inside the Docker container
- a container running a Seq service instance which will collect and display the application's logs in a structured format
- a container running an `Octweet` application instance

The database and the logs service are set up in the docker-compose file in such a way that their data will be retained upon restarts.

To stop the containers, you can type `Ctrl-C` in the terminal.

--- 
this will be rewritten
### Prerequisites 

- Docker
- Visual Studio, VS Code, or Rider installed
- .NET 6 SDK installed

### Database
The project contains a docker-compose file in the root directory, which sets up a MySQL database.
To have it run in your system, you can spin it up using:

```bash
docker-compose up -d
```

Specifying the `-d` option will run the container as a deamon in the background and leave your terminal to be used for anything else you might want.

Respectively, in order to stop the running container, you can run:

```bash
docker-compose down
```

#### Setting up schema

This project is using Entity Framework Core as an ORM, and for scaffolding the DB.

You will need the Entity Framework CLI, which you can either install globally to your machine, using the command `dotnet tool install dotnet-ef --global` **OR**
 you can run `dotnet tool restore`

By default, the MySQL image running with Docker will not have a database or the tables setup.

The repository, however, will have checked in source control a "Migrations" folder and the respective files, which are produced by Entity Framework Core, and in essence contain instructions for setting up the DB.
To apply them, you will have to execute:

```bash
dotnet ef database update --project Octweet.ConsoleApp
```

The above line will execute the `ef` dotnet tool (Entity Framework - which will be installed by the previous `dotnet tool restore` command)
and update the database specified in appSettings.json connection string (if you haven't changed anything, it will be a db named OctweetDB in localhost MySQL instance running in docker)


### Running the app

Now that you're all set, you can either load the solution in an IDE and run/debug or if you prefer command line:

- `cd` into `Octweet.ConsoleApp`
- run `dotnet run`
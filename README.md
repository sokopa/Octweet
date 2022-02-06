# Octweet
Project repo for Modern Web Applications Post-grad Technoeconomics course - 2021/2022

## Setting Up

### API Credentials

This application contacts the `Twitter API` and `Google Vision API`. You will need your own credentials in order to access these APIs using this application.

You can access the following pages in order to see more thorough guides on how to create your credentials for each service:
 - [Twitter](/docs/twitter.md)

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
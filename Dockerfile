#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Octweet.ConsoleApp/Octweet.ConsoleApp.csproj", "Octweet.ConsoleApp/"]
COPY ["Octweet.Core.Abstractions/Octweet.Core.Abstractions.csproj", "Octweet.Core.Abstractions/"]
COPY ["Octweet.Core/Octweet.Core.csproj", "Octweet.Core/"]
COPY ["Octweet.Data.Abstractions/Octweet.Data.Abstractions.csproj", "Octweet.Data.Abstractions/"]
COPY ["Octweet.Data/Octweet.Data.csproj", "Octweet.Data/"]
RUN dotnet restore "Octweet.ConsoleApp/Octweet.ConsoleApp.csproj"
COPY . .
RUN chmod +x ./devops/wait-for-it.sh ./devops/docker-entrypoint.sh
WORKDIR "/src/Octweet.ConsoleApp"
RUN dotnet build "Octweet.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Octweet.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./devops /app/devops
ENTRYPOINT ["/app/devops/docker-entrypoint.sh"]
CMD ["dotnet", "Octweet.ConsoleApp.dll"]
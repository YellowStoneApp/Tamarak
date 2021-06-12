FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# copy nuget packages into dir
WORKDIR /src
COPY ["MainService.csproj", "./"]
COPY ["nuget.config", "./"]

# I'm not sure why the config file is giving trouble but we no longer have our internal packages to publish
RUN dotnet restore "MainService.csproj" --configfile nuget.config
# RUN dotnet restore "MainService.csproj" 
COPY . .
WORKDIR "/src/."
RUN dotnet build "MainService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainService.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet MainService.dll


#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GlobalBlue.CustomerManager.WebApi/GlobalBlue.CustomerManager.WebApi.csproj", "GlobalBlue.CustomerManager.WebApi/"]
RUN dotnet restore "GlobalBlue.CustomerManager.WebApi/GlobalBlue.CustomerManager.WebApi.csproj"
COPY . .
WORKDIR "/src/GlobalBlue.CustomerManager.WebApi"
RUN dotnet build "GlobalBlue.CustomerManager.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GlobalBlue.CustomerManager.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalBlue.CustomerManager.WebApi.dll"]
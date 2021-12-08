#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
ENV ASPNETCORE_ENVIRONMENT Development
ENV GOOGLE_APPLICATION_CREDENTIALS ./brenda-wallet-firebase.json
WORKDIR /app

EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["nuget.config", "nuget.config"]

COPY ["Brenda.Api/Brenda.Api.csproj", "Brenda.Api/"]
COPY ["Brenda.Core/Brenda.Core.csproj", "Brenda.Core/"]
COPY ["Brenda.Contracts/Brenda.Contracts.csproj", "Brenda.Contracts/"]
COPY ["Brenda.Utils/Brenda.Utils.csproj", "Brenda.Utils/"]
COPY ["Brenda.Infrastructure/Brenda.Infrastructure.csproj", "Brenda.Infrastructure/"]
COPY ["Brenda.Data/Brenda.Data.csproj", "Brenda.Data/"]
COPY ["Brenda.Services/Brenda.Services.csproj", "Brenda.Services/"]
RUN dotnet restore "Brenda.Api/Brenda.Api.csproj"
COPY . .
WORKDIR "/src/Brenda.Api"
RUN dotnet build "Brenda.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Brenda.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Brenda.Api.dll"]
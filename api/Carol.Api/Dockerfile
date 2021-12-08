#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
RUN apt-get update -y && apt-get install -y tzdata
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["Carol.Api/Carol.Api.csproj", "Carol.Api/"]
RUN dotnet restore "Carol.Api/Carol.Api.csproj"
COPY . .
WORKDIR "/src/Carol.Api"
RUN dotnet build "Carol.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carol.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carol.Api.dll"]
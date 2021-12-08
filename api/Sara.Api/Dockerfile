#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["Sara.Api/Sara.Api.csproj", "Sara.Api/"]
RUN dotnet restore "Sara.Api/Sara.Api.csproj"
COPY . .
WORKDIR "/src/Sara.Api"
RUN dotnet build "Sara.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sara.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sara.Api.dll"]
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
RUN apt-get update -y && apt-get install -y tzdata libc6-dev libgdiplus libx11-dev && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["Brenda.Web/Brenda.Web.csproj", "Brenda.Web/"]
RUN dotnet restore "Brenda.Web/Brenda.Web.csproj"
COPY . .
WORKDIR "/src/Brenda.Web"
RUN dotnet build "Brenda.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Brenda.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Brenda.Web.dll"]
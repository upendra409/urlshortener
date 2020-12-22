FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /urlshortener
COPY *.csproj ./
RUN dotnet restore

COPY ./ ./
RUN dotnet publish -c Release -o out

#ENV ASPNETCORE_ENVIRONMENT=Test
#ENV EmailServer=127.0.01
#ENV ConnectionString= Data Source=127.0.01,1500;Initial Catalog=Test.Database;
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /urlshortener
COPY --from=build /urlshortener/out .
ENV ASPNETCORE_ENVIRONMENT Docker
ENV ASPNETCORE_URLS http://*:5200
ENTRYPOINT ["dotnet", "urlshortener.dll"]
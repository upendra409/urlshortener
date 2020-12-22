# UrlShortener

The solution consists of two projects.

  - urlshortener
  - apigateway

Actual functionality of shortening the url and storing it to the database is done by urlshortener. 
# Input
    {
    "longurl" : "abcdef"
    }
# Response
    {
    "shortUrl": "https://urlshortenerdec2020.azurewebsites.net/GPfgzDy",
    "identifier": "9d3625e1-36bd-4773-a8dd-ebc420e5e2d8",
    "createdOn": "2020-12-21T02:18:07.39+00:00"
    }
    
If the url is already present, then it returns the same output along with the original created time. If url is absent in the database then it will create a new record in the database and return the above output.

apigateway is a facade over the service based on ocelot open source project. It provides the following functionality
- rewrite /urlservice/v1/url to /urlservice/url. This means the request to /urlservice/url to apigateway is sent to /urlservice/v1/url to urlshortener service.
- Ratelimiting to prevent any client from making too many requests within a short duration of time. It throws a message as given below
"API calls quota exceeded! maximum admitted 1 per 5s."

The solution also contains a docker compose file, which refers to the following images. The docker compose file can be used locally to set up the solution
- urlshortener
- apigateway
- mysqlserver

mysqlserver is the image for sql server + a few database objects that are required to function.

The solution (urlshortener + sqlserver database) is deployed on Microsoft Azure cloud and the api definition can be viewed using the url
https://urlshortenerdec2020.azurewebsites.net/swagger/index.html. Swagger gives some information about how the api can be tested.

### Docker components
Docker components in the solution consist of the docker files for following
- urlshortener
- apigateway

docker-compose file in the root folder refers to the following images in the public dockerhub repository
- upendra409/apigateway
- upendra409/urlshortener
- upendra409/mysqlserver


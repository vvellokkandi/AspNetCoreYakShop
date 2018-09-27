# AspNetCoreYakShop

YakShop is fictitious shop created for technology demonstration. Following technlogies are used.

- ASP.net MVC Core 2.1, 
- ASP.NET Core API 
- Swagger UI to visualize and interact with API
- Windows Docker Container
- SignalR

# Prerequisites

- Visual Studio 2017
- Docker for Windows
- .NET Core 2.1

# How to run the application
- Download or Clone the code base.
- Build the code using Visual Studio or .NET CLI
- By default 
- - API listens on http://localhost:5050
- - MVC Application listens on http://localhost:8086
- MVC Razor pages interacts with API through JQuery AJAX calls.
- APIs can be accessed and interacted through Swagger UI with URL: http://localhost:5050/index.html
- Application also demostrates use of SignalR to auto refresh all client pages.

## Using Docker Containers
- Windows container images are available at following location
-- https://hub.docker.com/u/vvellokkandi/
- In the code base go to the path "../AspNetCoreYakShop/YakShop/Docker Pull" and run command
- - docker-compose up
- - It creates separate network with subnet: 192.168.112.0/24
- - API listenes at http://192.168.112.10/
- - MVC Application listens at http://192.168.112.11/

License
----

MIT


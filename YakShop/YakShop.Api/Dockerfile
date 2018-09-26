#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat
FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
ARG source
WORKDIR /app
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
COPY ${source:-obj/Docker/Publish} .
ENTRYPOINT ["dotnet", "YakShop.Api.dll", "--environment=Container"]
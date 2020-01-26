FROM node:10 AS node-env
WORKDIR /app
COPY . .
RUN npm install -g @angular/cli
RUN cd vodFrontend && npm install --silent
RUN cd vodFrontend && ng build
FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
WORKDIR /app
COPY --from=node-env /app .
RUN dotnet publish -c Release
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet .\vodApi\bin\Release\netcoreapp2.1\publish\vodApi.dll
FROM node:10
WORKDIR /app
COPY . .
RUN npm install -g @angular/cli
RUN cd vodFrontend && npm install --silent
RUN cd vodFrontend && ng build
FROM mcr.microsoft.com/dotnet/core/sdk:2.1
WORKDIR /app
RUN dotnet publish -c Release
FROM microsoft/dotnet:2.1-aspnetcore-runtime
CMD ASPNETCORE_URLS=http://*:$PORT dotnet .\vodApi\bin\Release\netcoreapp2.1\publish\vodApi.dll
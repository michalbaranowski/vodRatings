FROM node:16 AS node-env
COPY . .
RUN npm install -g @angular/cli
RUN cd vodFrontend && npm install --silent
RUN cd vodFrontend && ng build
FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
COPY --from=node-env . .
RUN dotnet publish -c Release
FROM mcr.microsoft.com/dotnet/core/runtime:2.1
COPY --from=build-env ./vodApi/bin/Release/netcoreapp2.1/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet vodApi.dll
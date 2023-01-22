FROM node:16 AS node-env
COPY . .
RUN npm install -g @angular/cli
RUN cd vodFrontend && npm install --silent
RUN cd vodFrontend && ng build

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
ENV CLR_OPENSSL_VERSION_OVERRIDE 1.0.2
COPY --from=node-env . .

RUN dotnet publish -c Release
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
COPY --from=build-env ./vodApi/bin/Release/netcoreapp2.1/publish .
CMD dotnet vodApi.dll
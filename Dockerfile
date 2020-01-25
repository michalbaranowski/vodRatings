FROM microsoft/dotnet:2.1-aspnetcore-runtime
RUN npm-install.cmd
RUN angular-build.cmd
RUN dotnet publish -c Release
WORKDIR /app
COPY .\vodApi\bin\Release\netcoreapp2.1\publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet vodApi.dll
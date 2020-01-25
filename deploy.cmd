npm-install.cmd &&
angular-build.cmd &&
dotnet publish -c Release &&
copy Dockerfile .\vodApi\bin\Release\netcoreapp2.1\publish
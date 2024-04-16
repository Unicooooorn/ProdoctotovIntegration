FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

ADD ./dotnet-restore.tar ./
RUN dotnet restore "src/ProdoctorovIntegration/ProdoctorovIntegration.Api/ProdoctorovIntegration.Api.csproj" -s https://api.nuget.org/v3/index.json

COPY ./prodoctorovIntegration ./
RUN dotnet publish "src/ProdoctorovIntegration/ProdoctorovIntegration.Api/ProdoctorovIntegration.Api.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "ProdoctorovIntegration.Api.dll"]
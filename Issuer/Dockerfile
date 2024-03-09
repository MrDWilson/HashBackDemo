FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ./Issuer/ ./Issuer/
COPY ./Core ./Core/

WORKDIR /app/Issuer

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/Issuer/out .
ENTRYPOINT ["dotnet", "Issuer.dll"]
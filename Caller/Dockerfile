FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ./Caller/ ./Caller/
COPY ./Core ./Core/

WORKDIR /app/Caller

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/Caller/out .
ENTRYPOINT ["dotnet", "Caller.dll"]
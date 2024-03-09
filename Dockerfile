FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PROJECT_NAME

WORKDIR /app

COPY ./${PROJECT_NAME}/ ./${PROJECT_NAME}/
COPY ./Core ./Core/

WORKDIR /app/${PROJECT_NAME}

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ARG PROJECT_NAME
ENV PROJECT_NAME=$PROJECT_NAME

WORKDIR /app
COPY --from=build /app/${PROJECT_NAME}/out .
ENTRYPOINT ["sh", "-c", "dotnet $PROJECT_NAME.dll"]
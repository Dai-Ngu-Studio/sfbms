# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY BusinessObject/*.csproj ./BusinessObject/
COPY DataAccess/*.csproj ./DataAccess/
COPY Repositories/*.csproj ./Repositories/
COPY SFBMS-API/*.csproj ./SFBMS-API/
RUN dotnet restore

# Copy everything else and build
COPY BusinessObject/. ./BusinessObject/
COPY DataAccess/. ./DataAccess/
COPY Repositories/. ./Repositories/
COPY SFBMS-API/. ./SFBMS-API/
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build-env /app/out .
CMD dotnet SFBMS-API.dll
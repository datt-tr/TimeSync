FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ./src .
RUN dotnet publish FuenfzehnZeitWrapper.csproj -o /publish /p:UserAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT [ "dotnet", "FuenfzehnZeitWrapper.dll" ]

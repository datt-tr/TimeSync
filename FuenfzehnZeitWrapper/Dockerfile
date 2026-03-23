FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ./src .
RUN dotnet restore FuenfzehnZeitWrapper.Api/FuenfzehnZeitWrapper.Api.csproj
RUN dotnet build --no-restore FuenfzehnZeitWrapper.Api/FuenfzehnZeitWrapper.Api.csproj
RUN dotnet publish -c Debug --no-build FuenfzehnZeitWrapper.Api/FuenfzehnZeitWrapper.Api.csproj -o /publish /p:UserAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT [ "dotnet", "FuenfzehnZeitWrapper.Api.dll" ]

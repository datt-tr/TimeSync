FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ./src .
RUN dotnet restore FuenfzehnZeitWrapper.Presentation/FuenfzehnZeitWrapper.Presentation.csproj
RUN dotnet build --no-restore FuenfzehnZeitWrapper.Presentation/FuenfzehnZeitWrapper.Presentation.csproj
RUN dotnet publish -c Debug --no-build FuenfzehnZeitWrapper.Presentation/FuenfzehnZeitWrapper.Presentation.csproj -o /publish /p:UserAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT [ "dotnet", "FuenfzehnZeitWrapper.Presentation.dll" ]

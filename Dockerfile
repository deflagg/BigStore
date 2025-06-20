# 1) Build stage
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS builder
WORKDIR C:/src

# copy just the project file, restore packages
COPY ["BigStore.sln", "./"]
COPY ["./BigStore/BigStore.csproj", "./BigStore"]
COPY ["./BigStore/packages.config", "./BigStore"]

COPY ["./BigStoreBL/BigStoreBL.csproj", "./BigStoreBL"]
COPY ["./SubSystemBL/SubSystemBL.csproj", "./SubSystemBL"]
RUN msbuild BigStore.sln /t:Restore

#ENV NUGET_PACKAGES=C:/src/packages

RUN nuget restore "BigStore.sln"

# copy the rest of your code

COPY ["./BigStore", "./BigStore"]
COPY ["./BigStoreBL", "./BigStoreBL"]
COPY ["./SubSystemBL", "./SubSystemBL"]

# Build source
RUN msbuild BigStore.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:WebPublishMethod=FileSystem /p:PublishUrl=C:/publish

#############################################


# 2) Runtime stage
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
WORKDIR C:/inetpub/wwwroot
COPY --from=builder C:/publish/ ./
ENTRYPOINT ["C:\\ServiceMonitor.exe", "w3svc"]

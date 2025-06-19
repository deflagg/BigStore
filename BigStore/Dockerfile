FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
WORKDIR /inetpub/BigStore

# Copy the project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application files and build the release
COPY . .
RUN dotnet publish -c Release -o /app

####################################################################

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

# Expose the port the application runs on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "blipfeed.dll"]
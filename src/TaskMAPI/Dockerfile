# Use the official .NET Runtime as a base image with arm64 architecture
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set working directory inside container
WORKDIR /app

EXPOSE 80

# Set environment variables for HTTPS
ENV ASPNETCORE_URLS="http://+:80"
ENV ASPNETCORE_ENVIRONMENT="Development"

# Copy published app from local folder to container
COPY src/TaskMAPI/bin/Release/net9.0/publish/ .

# Tell Docker what command to run
ENTRYPOINT ["dotnet", "TaskMAPI.dll"]
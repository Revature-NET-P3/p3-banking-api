# Use the standard Microsoft .NET Core container

FROM microsoft/dotnet

WORKDIR /bankingapi
COPY /bankingapi /bankingapi

# Expose port 5000 for the Web API traffic
EXPOSE 5000

# Restore the necessary packages

# Build and run the dotnet application from within the container

ENTRYPOINT dotnet banking.dll

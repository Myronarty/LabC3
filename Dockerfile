FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet build --configuration Release
RUN dotnet test test3/test3.csproj --no-build --verbosity normal
CMD ["dotnet", "run", "--project", "LabC3/LabC3.csproj"]

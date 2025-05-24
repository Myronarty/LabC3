FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet build --configuration Release
CMD ["dotnet", "run", "--project", "LabC3/LabC3.csproj"]

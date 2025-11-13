# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["example/People.Api/People.Api.csproj", "example/People.Api/"]
COPY ["example/People.IoC/People.IoC.csproj", "example/People.IoC/"]
COPY ["example/People.Application/People.Application.csproj", "example/People.Application/"]
COPY ["example/People.Domain/People.Domain.csproj", "example/People.Domain/"]
COPY ["example/People.Infra.Data/People.Infra.Data.csproj", "example/People.Infra.Data/"]
COPY ["src/Core.Libraries.Application/Core.Libraries.Application.csproj", "src/Core.Libraries.Application/"]
COPY ["src/Core.Libraries.Infra.Data/Core.Libraries.Infra.Data.csproj", "src/Core.Libraries.Infra.Data/"]
COPY ["src/Core.Libraries.Domain/Core.Libraries.Domain.csproj", "src/Core.Libraries.Domain/"]

# Restore dependencies
RUN dotnet restore "example/People.Api/People.Api.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/example/People.Api"
RUN dotnet build "People.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "People.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "People.Api.dll"]


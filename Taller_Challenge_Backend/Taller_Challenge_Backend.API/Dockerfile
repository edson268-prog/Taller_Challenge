# Etapa de Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copied the project files using the paths from the root.
COPY ["Taller_Challenge_Backend.API/Taller_Challenge_Backend.API.csproj", "Taller_Challenge_Backend.API/"]
COPY ["Taller_Challenge_Backend.Domain/Taller_Challenge_Backend.Domain.csproj", "Taller_Challenge_Backend.Domain/"]
COPY ["Taller_Challenge_Backend.Infrastructure/Taller_Challenge_Backend.Infrastructure.csproj", "Taller_Challenge_Backend.Infrastructure/"]

# Restore
RUN dotnet restore "Taller_Challenge_Backend.API/Taller_Challenge_Backend.API.csproj"

# Copied all the contents of the solution
COPY . .

# Changed the working directory to compile the API
WORKDIR "/src/Taller_Challenge_Backend.API"
RUN dotnet build "Taller_Challenge_Backend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Taller_Challenge_Backend.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taller_Challenge_Backend.API.dll"]
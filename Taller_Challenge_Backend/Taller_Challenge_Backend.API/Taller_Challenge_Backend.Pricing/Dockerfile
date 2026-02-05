FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file while maintaining the folder structure.
COPY ["Taller_Challenge_Backend.Pricing/Taller_Challenge_Backend.Pricing.csproj", "Taller_Challenge_Backend.Pricing/"]

# Restore dependencies
RUN dotnet restore "Taller_Challenge_Backend.Pricing/Taller_Challenge_Backend.Pricing.csproj"

# Copied the REST of the files from the Pricing folder
COPY Taller_Challenge_Backend.Pricing/ Taller_Challenge_Backend.Pricing/

# Changed to the project directory to compile
WORKDIR "/src/Taller_Challenge_Backend.Pricing"
RUN dotnet build "Taller_Challenge_Backend.Pricing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Taller_Challenge_Backend.Pricing.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taller_Challenge_Backend.Pricing.dll"]
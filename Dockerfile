FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OleleLibraryAPI/OleleLibraryAPI.csproj OleleLibraryAPI/
RUN dotnet restore OleleLibraryAPI/OleleLibraryAPI.csproj

COPY . .
RUN dotnet publish OleleLibraryAPI/OleleLibraryAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 10000
ENV ENABLE_RENDER_SEED=true

COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-10000} exec dotnet OleleLibraryAPI.dll"]

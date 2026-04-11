FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NatalChart.sln .
COPY src/NatalChart.Core/NatalChart.Core.csproj src/NatalChart.Core/
COPY src/NatalChart.Astrology/NatalChart.Astrology.csproj src/NatalChart.Astrology/
COPY src/NatalChart.Interpretation/NatalChart.Interpretation.csproj src/NatalChart.Interpretation/
COPY src/NatalChart.Infrastructure/NatalChart.Infrastructure.csproj src/NatalChart.Infrastructure/
COPY src/NatalChart.Api/NatalChart.Api.csproj src/NatalChart.Api/
RUN dotnet restore src/NatalChart.Api/NatalChart.Api.csproj

COPY src/ src/
RUN dotnet publish src/NatalChart.Api/NatalChart.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "NatalChart.Api.dll"]

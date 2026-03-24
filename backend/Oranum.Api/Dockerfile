FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Oranum.Api/Oranum.Api.csproj", "Oranum.Api/"]
COPY ["Oranum.Application/Oranum.Application.csproj", "Oranum.Application/"]
COPY ["Oranum.Domain/Oranum.Domain.csproj", "Oranum.Domain/"]
COPY ["Oranum.Infrastructure/Oranum.Infrastructure.csproj", "Oranum.Infrastructure/"]
RUN dotnet restore "Oranum.Api/Oranum.Api.csproj"

COPY . .
WORKDIR /src/Oranum.Api
RUN dotnet publish "Oranum.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Oranum.Api.dll"]

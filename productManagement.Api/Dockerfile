FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["productManagement.Api/productManagement.Api.csproj", "productManagement.Api/"]
COPY ["productManagement.Application/productManagement.Application.csproj", "productManagement.Application/"]
COPY ["productManagement.Domain/productManagement.Domain.csproj", "productManagement.Domain/"]
COPY ["productManagement.Infrastructure/productManagement.Infrastructure.csproj", "productManagement.Infrastructure/"]
RUN dotnet restore "productManagement.Api/productManagement.Api.csproj"
COPY . .
WORKDIR "/src/productManagement.Api"
RUN dotnet build "./productManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./productManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "productManagement.Api.dll"]

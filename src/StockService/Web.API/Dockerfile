#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/StockService/Web.API/Stock.API.csproj", "src/StockService/Web.API/"]
COPY ["src/StockService/Application/Application.csproj", "src/StockService/Application/"]
COPY ["src/SharedLib/SharedLib.csproj", "src/SharedLib/"]
COPY ["src/StockService/Entity/Domain.csproj", "src/StockService/Entity/"]
COPY ["src/StockService/Persistence/Persistence.csproj", "src/StockService/Persistence/"]
RUN dotnet restore "src/StockService/Web.API/Stock.API.csproj"
COPY . .
WORKDIR "/src/src/StockService/Web.API"
RUN dotnet build "Stock.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stock.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.API.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/OrderService/Web.API/Order.API.csproj", "src/OrderService/Web.API/"]
COPY ["src/OrderService/Application/Application.csproj", "src/OrderService/Application/"]
COPY ["src/SharedLib/SharedLib.csproj", "src/SharedLib/"]
COPY ["src/OrderService/Domain/Domain.csproj", "src/OrderService/Domain/"]
COPY ["src/OrderService/Persistence/Persistence.csproj", "src/OrderService/Persistence/"]
RUN dotnet restore "src/OrderService/Web.API/Order.API.csproj"
COPY . .
WORKDIR "/src/src/OrderService/Web.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]
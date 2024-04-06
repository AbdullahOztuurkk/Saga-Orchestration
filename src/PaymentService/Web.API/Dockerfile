#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PaymentService/Web.API/Payment.API.csproj", "src/PaymentService/Web.API/"]
COPY ["src/PaymentService/Application/Application.csproj", "src/PaymentService/Application/"]
COPY ["src/SharedLib/SharedLib.csproj", "src/SharedLib/"]
RUN dotnet restore "src/PaymentService/Web.API/Payment.API.csproj"
COPY . .
WORKDIR "/src/src/PaymentService/Web.API"
RUN dotnet build "Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]
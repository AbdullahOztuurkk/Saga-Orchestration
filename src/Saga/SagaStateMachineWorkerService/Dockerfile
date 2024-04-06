#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Saga/SagaStateMachineWorkerService/SagaStateMachineWorkerService.csproj", "src/Saga/SagaStateMachineWorkerService/"]
COPY ["src/SharedLib/SharedLib.csproj", "src/SharedLib/"]
RUN dotnet restore "src/Saga/SagaStateMachineWorkerService/SagaStateMachineWorkerService.csproj"
COPY . .
WORKDIR "/src/src/Saga/SagaStateMachineWorkerService"
RUN dotnet build "SagaStateMachineWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SagaStateMachineWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SagaStateMachineWorkerService.dll"]
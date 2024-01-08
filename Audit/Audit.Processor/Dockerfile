#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Audit/Audit.Processor/Audit.Worker.csproj", "Audit/Audit.Processor/"]
COPY ["Core/Core.Worker/Core.Worker.csproj", "Core/Core.Worker/"]
COPY ["Core/Core.Infrastructures/Core.Infrastructures.csproj", "Core/Core.Infrastructures/"]
COPY ["Core/Core.Application/Core.Application.csproj", "Core/Core.Application/"]
COPY ["Core/Core.Domain/Core.Domain.csproj", "Core/Core.Domain/"]
COPY ["Audit/Audit.Contracts/Audit.Contracts.csproj", "Audit/Audit.Contracts/"]
COPY ["Audit/Audit.Infrastructures/Audit.Infrastructures.csproj", "Audit/Audit.Infrastructures/"]
COPY ["Audit/Audit.Application/Audit.Application.csproj", "Audit/Audit.Application/"]
COPY ["Audit/Audit.Domain/Audit.Domain.csproj", "Audit/Audit.Domain/"]
RUN dotnet restore "./Audit/Audit.Processor/./Audit.Worker.csproj"
COPY . .
WORKDIR "/src/Audit/Audit.Processor"
RUN dotnet build "./Audit.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Audit.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Audit.Worker.dll"]
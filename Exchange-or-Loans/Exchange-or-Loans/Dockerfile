FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Exchange-or-Loans/Exchange-or-Loans.csproj", "Exchange-or-Loans/"]
RUN dotnet restore "Exchange-or-Loans/Exchange-or-Loans.csproj"
COPY . .
WORKDIR "/src/Exchange-or-Loans"
RUN dotnet build "Exchange-or-Loans.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Exchange-or-Loans.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange-or-Loans.dll"]

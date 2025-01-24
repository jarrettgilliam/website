FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build-api
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/api/Website/Website.csproj", "."]
RUN dotnet restore "Website.csproj"
COPY src/api/Website .
RUN dotnet publish "Website.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM oven/bun AS build-web
WORKDIR /app
COPY src/web/package.json src/web/bun.lock ./
RUN bun install
COPY src/web .
RUN bun run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
COPY --from=build-api /app/publish .
COPY --from=build-web /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "Website.dll"]

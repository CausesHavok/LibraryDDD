# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy build settings and project files first, so the restore layer is cached
# and only re-runs when dependencies change.
COPY Directory.Build.props .editorconfig ./
COPY src/LibraryDDD.API/LibraryDDD.API.csproj src/LibraryDDD.API/
COPY src/LibraryDDD.Application/LibraryDDD.Application.csproj src/LibraryDDD.Application/
COPY src/LibraryDDD.Domain/LibraryDDD.Domain.csproj src/LibraryDDD.Domain/
COPY src/LibraryDDD.Infrastructure/LibraryDDD.Infrastructure.csproj src/LibraryDDD.Infrastructure/
COPY src/LibraryDDD.Shared/LibraryDDD.Shared.csproj src/LibraryDDD.Shared/
RUN dotnet restore src/LibraryDDD.API/LibraryDDD.API.csproj

COPY src/ src/
RUN dotnet publish src/LibraryDDD.API/LibraryDDD.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Listen on 8080 and run as the non-root user that the runtime image provides.
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID

ENTRYPOINT ["dotnet", "LibraryDDD.API.dll"]

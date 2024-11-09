# Based on:
# https://github.com/dotnet/dotnet-docker/blob/8d855073c5125f9f07c37cdd4ef12e62a217dd26/samples/dotnetapp/Dockerfile.chiseled
# https://github.com/dotnet/dotnet-docker/blob/8d855073c5125f9f07c37cdd4ef12e62a217dd26/samples/8.0/aspnetapp/Dockerfile.chiseled

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link *.fsproj .
RUN dotnet restore -a $TARGETARCH

# Copy source code and publish app
COPY --link . .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app

# Generate settings file
RUN sed "s/..\/\(Database\/\)/\1/" appsettings.json >/app/appsettings.json

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled

WORKDIR /app
COPY --link --from=build /app .
ENTRYPOINT ["./ConsorciosCLP"]

# Build front end
FROM node:16-buster AS build-frontend
WORKDIR /app/frontend
COPY WebsiteApp/ .
RUN npm install --only=prod
RUN ./node_modules/.bin/ng build

# Build backend
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm32v7 AS build-backend
WORKDIR /app
COPY ./WebsiteApi/WebsiteApi/ ./
RUN dotnet restore --runtime linux-arm
RUN dotnet publish -c Release -o out --self-contained true --runtime linux-arm /p:PublishTrimmed=true
COPY --from=build-frontend /app/frontend/dist/ ./out/wwwroot

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-backend /app/out .
RUN useradd -u 1000 ryanha
RUN chown -R ryanha .
RUN chgrp -R ryanha .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "WebsiteApi.dll"]

# Change user to not run as root
USER ryanha
# Build front end
FROM node:16-alpine3.14 AS build-frontend
WORKDIR /app/frontend
COPY WebsiteApp/ .
RUN npm install
RUN ./node_modules/.bin/ng build

# Build backend
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-backend
WORKDIR /app
COPY ./WebsiteApi/WebsiteApi/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
COPY --from=build-frontend /app/frontend/dist/ ./out/wwwroot

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-backend /app/out .
ENTRYPOINT ["dotnet", "WebsiteApi.dll"]
# # Etap budowania (build stage) dla aplikacji .NET
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# WORKDIR /app
# COPY . ./

# RUN dotnet restore "SpaAngular.csproj"
# RUN dotnet publish "SpaAngular.csproj" -c Release -o out

# # Etap budowania (build stage) dla aplikacji Angular
# FROM node:latest as node
# WORKDIR /app/ClientApp
# COPY --from=build-env /app/ClientApp .
# RUN npm install
# RUN npm run build --prod

# # Etap uruchomieniowy (runtime stage)
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# EXPOSE 80
# WORKDIR /app

# COPY docker-entrypoint.sh .
# RUN chmod +x docker-entrypoint.sh

# # Kopiowanie plików wynikowych z etapu budowania .NET
# COPY --from=build-env /app/out .

# ENV ASPNETCORE_ENVIRONMENT=Production
# ENTRYPOINT ["./docker-entrypoint.sh"]
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

RUN apt-get update -yq && apt-get install -yq curl
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get update && \
    apt-get install -yq nodejs && \
    rm -rf /var/lib/apt/lists/*

COPY *.csproj ./
RUN dotnet restore "SpaAngular.csproj"

COPY . ./
RUN dotnet publish "SpaAngular.csproj" -c Release -o /out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 80
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT [ "dotnet", "SpaAngular.dll" ]
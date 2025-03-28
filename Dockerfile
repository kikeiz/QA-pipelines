# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY QA/*.csproj ./QA/
RUN dotnet restore ./QA/QA.csproj

COPY . .
RUN dotnet publish ./QA/QA.csproj -c Release -o /out /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Optional: document that this container listens on 8080 internally
EXPOSE 8080

# Set the port the app will listen on inside the container
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /out ./

ENTRYPOINT ["dotnet", "QA.dll"]

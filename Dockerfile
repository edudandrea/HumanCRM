# =========================
# 1️⃣ Build do Angular (Node 20)
# =========================
FROM node:20 AS frontend-build
WORKDIR /app/frontend

COPY Front/HumanCRM-App/package*.json ./
RUN npm install

COPY Front/HumanCRM-App .
RUN npm run build

# =========================
# 2️⃣ Build .NET 10
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS backend-build
WORKDIR /app

COPY Back/HumanCRM-Api/HumanCRM-Api.csproj Back/HumanCRM-Api/
RUN dotnet restore Back/HumanCRM-Api/HumanCRM-Api.csproj

COPY Back ./Back
RUN dotnet publish Back/HumanCRM-Api/HumanCRM-Api.csproj -c Release -o /app/publish

# =========================
# 3️⃣ Runtime .NET
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=backend-build /app/publish .
COPY --from=frontend-build /app/frontend/dist/HumanCRM-App /app/wwwroot

# 🔴 ESSENCIAL PARA O RAILWAY
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HumanCRM-Api.dll"]

# =========================
# 1️⃣ Build do Angular
# =========================
FROM node:18 AS frontend-build
WORKDIR /app/frontend

COPY Front/package*.json ./
RUN npm install

COPY Front .
RUN npm run build --configuration production

# =========================
# 2️⃣ Build do .NET
# =========================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend-build
WORKDIR /app

COPY Back/*.csproj ./Back/
RUN dotnet restore ./Back

COPY Back ./Back
RUN dotnet publish ./Back -c Release -o /app/publish

# =========================
# 3️⃣ Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=backend-build /app/publish .
COPY --from=frontend-build /app/frontend/dist /app/wwwroot

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HumanCRM.Api.dll"]

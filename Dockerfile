# =========================
# 1️⃣ Build do Angular
# =========================
FROM node:18 AS frontend-build
WORKDIR /app/frontend
COPY frontend/package*.json ./
RUN npm install
COPY frontend .
RUN npm run build --configuration production

# =========================
# 2️⃣ Build do .NET
# =========================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend-build
WORKDIR /app
COPY backend/*.csproj ./backend/
RUN dotnet restore backend
COPY backend ./backend
RUN dotnet publish backend -c Release -o /app/publish

# =========================
# 3️⃣ Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=backend-build /app/publish .
COPY --from=frontend-build /app/frontend/dist /app/wwwroot
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HumanCRM.dll"]

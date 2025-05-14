# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# копируем csproj и восстанавливаем зависимостu
COPY Edu_project/*.csproj ./Edu_project/
RUN dotnet restore ./Edu_project/Edu_project.csproj

# копируем остальной код и публикуем
COPY . .
RUN dotnet publish Edu_project/Edu_project.csproj -c Release -o /app/publish

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Edu_project.dll"]

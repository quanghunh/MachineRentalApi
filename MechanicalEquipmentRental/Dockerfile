# Sử dụng image chính thức của .NET SDK để build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file project và restore dependencies
COPY *.sln .
COPY MechanicalEquipmentRental/*.csproj ./MechanicalEquipmentRental/
RUN dotnet restore

# Copy toàn bộ source code và build ứng dụng
COPY . .
WORKDIR /app/MechanicalEquipmentRental
RUN dotnet publish -c Release -o /out

# Sử dụng image .NET Runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Mở cổng 80 và chạy ứng dụng
EXPOSE 80
CMD ["dotnet", "MechanicalEquipmentRental.dll"]

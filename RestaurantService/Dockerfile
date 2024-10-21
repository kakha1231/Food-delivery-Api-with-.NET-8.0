FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5432
EXPOSE 5048

ENV ASPNETCORE_HTTP_PORTS=5048

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RestaurantService/RestaurantService.csproj", "RestaurantService/"]
RUN dotnet restore "RestaurantService/RestaurantService.csproj"
COPY . .
WORKDIR "/src/RestaurantService"
RUN dotnet build "RestaurantService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantService.dll"]

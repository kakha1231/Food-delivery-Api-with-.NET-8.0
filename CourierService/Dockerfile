FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5068

ENV ASPNETCORE_HTTP_PORTS=5068

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CourierService/CourierService.csproj", "CourierService/"]
RUN dotnet restore "CourierService/CourierService.csproj"
COPY . .
WORKDIR "/src/CourierService"
RUN dotnet build "CourierService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourierService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourierService.dll"]

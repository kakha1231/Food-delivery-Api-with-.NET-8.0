FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Food-delivery-Api-with .NET-8.0/Food-delivery-Api-with .NET-8.0.csproj", "Food-delivery-Api-with .NET-8.0/"]
RUN dotnet restore "Food-delivery-Api-with .NET-8.0/Food-delivery-Api-with .NET-8.0.csproj"
COPY . .
WORKDIR "/src/Food-delivery-Api-with .NET-8.0"
RUN dotnet build "Food-delivery-Api-with .NET-8.0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Food-delivery-Api-with .NET-8.0.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Food-delivery-Api-with .NET-8.0.dll"]

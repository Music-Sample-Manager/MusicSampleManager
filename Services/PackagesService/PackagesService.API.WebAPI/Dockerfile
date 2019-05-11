FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Services/PackagesService/PackagesService.API/PackagesService.API.csproj", "Services/PackagesService/PackagesService.API/"]
RUN dotnet restore "Services/PackagesService/PackagesService.API/PackagesService.API.csproj"
COPY . .
WORKDIR "/src/Services/PackagesService/PackagesService.API"
RUN dotnet build "PackagesService.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PackagesService.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PackagesService.API.dll"]
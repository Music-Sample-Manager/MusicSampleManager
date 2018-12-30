FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Website/ContributorWebsite/PublicWebsite.csproj", "Website/ContributorWebsite/"]
COPY ["Core/DataLayer/DataLayer.csproj", "Core/DataLayer/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/PackageDatabase/PackageDatabase.csproj", "Core/PackageDatabase/"]
COPY ["Website/PublicWebsite.APIClient/PublicWebsite.APIClient.csproj", "Website/PublicWebsite.APIClient/"]
RUN dotnet restore "Website/ContributorWebsite/PublicWebsite.csproj"
COPY . .
WORKDIR "/src/Website/ContributorWebsite"
RUN dotnet build "PublicWebsite.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PublicWebsite.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PublicWebsite.dll"]
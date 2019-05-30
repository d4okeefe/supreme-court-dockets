FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SupremeCourtDocketApp/SupremeCourtDocketApp.csproj", "SupremeCourtDocketApp/"]
RUN dotnet restore "SupremeCourtDocketApp/SupremeCourtDocketApp.csproj"
COPY . .
WORKDIR "/src/SupremeCourtDocketApp"
RUN dotnet build "SupremeCourtDocketApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SupremeCourtDocketApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SupremeCourtDocketApp.dll"]
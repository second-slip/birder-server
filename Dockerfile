FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Birder/Birder.csproj", ""]
RUN dotnet restore
COPY . .
RUN dotnet build "Birder.csproj" -c Release -out

FROM build AS publish
RUN dotnet publish "Birder.csproj" -c Release -o out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Birder.dll"]

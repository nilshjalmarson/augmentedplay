FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BioKassa/BioKassa.csproj BioKassa/
RUN dotnet restore BioKassa/BioKassa.csproj

COPY . .
RUN dotnet publish BioKassa/BioKassa.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BioKassa.dll"]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MiniHittegods.Api/MiniHittegods.Api.csproj MiniHittegods.Api/
COPY MiniHittegods.Application/MiniHittegods.Application.csproj MiniHittegods.Application/
COPY MiniHittegods.Domain/MiniHittegods.Domain.csproj MiniHittegods.Domain/

Run dotnet restore MiniHittegods.Api/MiniHittegods.Api.csproj

COPY . .

RUN dotnet publish MiniHittegods.Api/MiniHittegods.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MiniHittegods.Api.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
	&& apt-get install curl gnupg -yq \
	&& curl -sL https://deb.nodesource.com/setup_12.x | bash \
	&& apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
	&& apt-get install curl gnupg -yq \
	&& curl -sL https://deb.nodesource.com/setup_12.x | bash \
	&& apt-get install nodejs -yq
WORKDIR /src
COPY ["FinTrader.Pro.Web/FinTrader.Pro.Web.csproj", "FinTrader.Pro.Web/"]
RUN dotnet restore "FinTrader.Pro.Web/FinTrader.Pro.Web.csproj"
COPY . .
WORKDIR "/src/FinTrader.Pro.Web"
RUN dotnet build "FinTrader.Pro.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinTrader.Pro.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTrader.Pro.Web.dll"]
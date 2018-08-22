FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Vita.Api/Vita.Api.csproj", "Vita.Api/"]
COPY ["Vita.Domain/Vita.Domain.csproj", "Vita.Domain/"]
COPY ["Vita.Contracts/Vita.Contracts.csproj", "Vita.Contracts/"]
COPY ["Vita.Predictor/Vita.Predictor.csproj", "Vita.Predictor/"]
RUN dotnet restore "Vita.Api/Vita.Api.csproj"
COPY . .
WORKDIR "/src/Vita.Api"
RUN dotnet build "Vita.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Vita.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Vita.Api.dll"]
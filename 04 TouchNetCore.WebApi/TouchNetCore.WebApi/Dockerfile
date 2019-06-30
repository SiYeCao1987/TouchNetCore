FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["04 TouchNetCore.WebApi/TouchNetCore.WebApi/TouchNetCore.WebApi.csproj", "04 TouchNetCore.WebApi/TouchNetCore.WebApi/"]
RUN dotnet restore "04 TouchNetCore.WebApi/TouchNetCore.WebApi/TouchNetCore.WebApi.csproj"
COPY . .
WORKDIR "/src/04 TouchNetCore.WebApi/TouchNetCore.WebApi"
RUN dotnet build "TouchNetCore.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TouchNetCore.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TouchNetCore.WebApi.dll"]
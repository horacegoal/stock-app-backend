
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dev
WORKDIR /src

EXPOSE 5097
ENV ASPNETCORE_URLS=http://+:5097
ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["stockAppApi/stockAppApi.csproj", "stockAppApi/"]
RUN dotnet restore "stockAppApi/stockAppApi.csproj"
COPY . .

ENTRYPOINT ["dotnet", "watch", "run", "--always", "--project", "stockAppApi/stockAppApi.csproj", "--urls", "http://+:5097/", "--no-restore"]


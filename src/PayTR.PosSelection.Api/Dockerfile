FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PayTR.PosSelection.Api/PayTR.PosSelection.Api.csproj", "PayTR.PosSelection.Api/"]
COPY ["PayTR.PosSelection.Application/PayTR.PosSelection.Application.csproj", "PayTR.PosSelection.Application/"]
COPY ["PayTR.PosSelection.Domain/PayTR.PosSelection.Domain.csproj", "PayTR.PosSelection.Domain/"]
COPY ["PayTR.PosSelection.Infrastructure/PayTR.PosSelection.Infrastructure.csproj", "PayTR.PosSelection.Infrastructure/"]

RUN dotnet restore "PayTR.PosSelection.Api/PayTR.PosSelection.Api.csproj"

COPY . .
RUN dotnet publish "PayTR.PosSelection.Api/PayTR.PosSelection.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PayTR.PosSelection.Api.dll"]


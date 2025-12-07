# PayTR POS Selection API

E-ticaret ödemelerinde en düşük maliyetli POS sağlayıcısını seçen API.

## Kurulum

### Gereksinimler
- .NET 8 SDK
- Docker (opsiyonel)

### Çalıştırma

**Docker ile:**
```bash
docker-compose up -d
```

**Veya doğrudan:**
```bash
dotnet run --project src/PayTR.PosSelection.Api
```

## API

### Endpoint

```
POST /api/pos/select
```

### Request

```json
{
  "amount": 362.22,
  "installment": 6,
  "currency": "TRY",
  "cardType": "credit",
  "cardBrand": "bonus"
}
```

### Response

```json
{
  "filters": {
    "amount": 362.22,
    "installment": 6,
    "currency": "TRY",
    "cardType": "credit",
    "cardBrand": null
  },
  "overallMin": {
    "posName": "KuveytTurk",
    "cardType": "credit",
    "cardBrand": "saglam",
    "installment": 6,
    "currency": "TRY",
    "commissionRate": 0.026,
    "price": 9.42,
    "payableTotal": 371.64
  }
}
```

### Health Check

```
GET /health
```

## Test

```bash
dotnet test
```

## Teknolojiler

- .NET 8
- ASP.NET Core
- Polly (retry & circuit breaker)
- xUnit
- Docker


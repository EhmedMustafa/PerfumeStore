# PerfumeStore Backend (OMAR PERFUME)

Bu ASP.NET Core 8 + EF Core Web API-dir. Frontend ayrı qovluqdadır:
`c:\Users\User\Downloads\OmarPerfume\` — orada daha geniş `CLAUDE.md` var.

## Sürətli baxış

```
PerfumeStore.API              Web API (controllers, Program.cs, appsettings)
PerfumeStore.Application      DTOs, Services, AutoMapper Profiles, Interfaces
PerfumeStore.Domain           Entities, Enums
PerfumeStore.Infrastructure   AppDbContext, Migrations, Repositories
PerfumeStore.WebUI            Köhnə MVC UI (Vanilla frontend onu əvəz edir)
```

## Qaçırma

```pwsh
cd PerfumeStore.API
dotnet run --launch-profile https     # https://localhost:7285
# və ya VS-də F5 → IIS Express → https://localhost:44380
```

DB: `DESKTOP-K64PT03\SQLEXPRESS` → `PerfumeStoreDB`.

## Migration

```pwsh
dotnet ef migrations add Name --project PerfumeStore.Infrastructure --startup-project PerfumeStore.API
dotnet ef database update --project PerfumeStore.Infrastructure --startup-project PerfumeStore.API
```

## Əsas məqamlar (frontend tərəfi ilə uyğunluq)

- **JWT auth default scheme** — `Program.cs`-də `DefaultAuthenticateScheme = Bearer` təyin olunub (yoxsa Identity cookie konfliktə düşür)
- **`Microsoft.IdentityModel.*` paketləri 8.6.1-ə pin edilib** — versiya konflikti `MissingMethodException` verirdi
- **OrderController `[Authorize]`** — UserId token-dən gəlir, client DTO-da `UserId` YOXDUR
- **OrderService qiyməti `ProductVariant.CurrentPrice`-dan oxuyur** — client qiyməti təyin edə bilməz
- **OrderItem-də snapshot** (ProductName, BrandName, Size, UnitPrice) — məhsul silinsə də sifariş tarixçəsi qalır
- **`ResultOrderDto`-da AppUser YOXDUR** — data leak qarşısı
- **CORS** — development-də `AllowAll`, production-da `Cors:AllowedOrigins` (appsettings)

## Yeni endpoint-lər

- `GET /api/Product/paged?page=&pageSize=&...` — pagination
- `POST /api/Newsletter/subscribe`
- `POST /api/Contact`

## Hələ etməli

Bax `c:\Users\User\Downloads\OmarPerfume\CLAUDE.md` — orada "Bilinən problemlər / hələ etməli" siyahısı var.

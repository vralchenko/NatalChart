# NatalChart

Web application for building natal charts (Western astrology, tropical zodiac, Placidus house system).

**Stack:** .NET 8 Web API + React / TypeScript / Vite + Material UI

## Features

- **Natal Chart** — planet positions, house cusps, aspect detection with orbs, SVG zodiac wheel
- **Interpretations** — planet-in-sign, planet-in-house, aspect texts (366 entries)
- **Synastry** — compatibility analysis between two charts with inter-aspects
- **Transits** — current planetary transits to natal chart
- **Geocoding** — location search with coordinate resolution (Nominatim/OpenStreetMap)
- **Timezone handling** — automatic timezone detection from coordinates (GeoTimeZone + NodaTime)

## Architecture

```
NatalChart/
├── src/
│   ├── NatalChart.Api/              # ASP.NET 8 Web API (controllers, DI, Swagger)
│   ├── NatalChart.Core/             # Domain: models, enums, interfaces
│   ├── NatalChart.Astrology/        # Computation engine (SwissEphNet)
│   ├── NatalChart.Interpretation/   # Text interpretations (JSON data)
│   └── NatalChart.Infrastructure/   # External services (geocoding, timezone)
├── tests/
│   ├── NatalChart.Astrology.Tests/  # 17 tests (ephemeris, houses, aspects, calculator)
│   └── NatalChart.Interpretation.Tests/  # 3 tests
└── client/                          # React + TypeScript (Vite)
    └── src/
        ├── api/          # Axios API client
        ├── types/        # TypeScript types matching backend models
        ├── components/   # ChartWheel, PlanetTable, AspectGrid, BirthDataForm, etc.
        ├── hooks/        # useChart, useSynastry, useTransits (React Query)
        └── pages/        # NatalChartPage, SynastryPage, TransitsPage
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/chart/calculate` | Calculate natal chart |
| POST | `/api/chart/interpret` | Get text interpretations |
| POST | `/api/synastry/calculate` | Synastry (two chart comparison) |
| POST | `/api/transit/calculate` | Transits to natal chart |
| GET | `/api/geocoding/search?query=` | Location search |

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)

### Run Backend

```bash
dotnet run --project src/NatalChart.Api
```

API starts at `http://localhost:5102`. Swagger UI at `http://localhost:5102/swagger`.

### Run Frontend

```bash
cd client
npm install
npm run dev
```

Frontend starts at `http://localhost:5173`.

### Run Tests

```bash
dotnet test
```

## Libraries

### Backend
- **SwissEphNet** — C# port of Swiss Ephemeris (planet positions, house cusps)
- **GeoTimeZone** — timezone lookup by coordinates
- **NodaTime** — accurate historical timezone/DST handling
- **Swashbuckle** — Swagger/OpenAPI

### Frontend
- **Material UI** + **MUI X Date Pickers** + **dayjs** — UI components
- **@tanstack/react-query** — server state management
- **react-router-dom** — client-side routing
- **axios** — HTTP client

## Astrology Details

- **Zodiac:** Tropical (Western)
- **House systems:** Placidus (default), Koch, Equal, Whole Sign
- **Celestial bodies:** Sun, Moon, Mercury, Venus, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto, North Node, Chiron
- **Aspects:** Conjunction (0°), Opposition (180°), Trine (120°), Square (90°), Sextile (60°), Quincunx (150°)
- **Orbs:** Wider for luminaries (Sun/Moon), tighter for outer planets

| Aspect | Luminary Orb | Other Orb |
|--------|-------------|-----------|
| Conjunction | 10° | 8° |
| Opposition | 10° | 8° |
| Trine | 8° | 6° |
| Square | 8° | 6° |
| Sextile | 6° | 4° |
| Quincunx | 3° | 2° |

## Verification

Golden chart test: **January 1, 2000, 12:00 UTC, Greenwich (51.4769, -0.0005)** — Sun in Capricorn ~280.5°, Ascendant in Aries. All positions verified against reference ephemeris data.

# NatalChart

Full-stack web application for building natal charts, synastry and transit analysis.

**Live:** https://natalchart-c4z.pages.dev

**Stack:** .NET 8 Web API + React / TypeScript / Vite + Material UI

## Features

- **Natal Chart** — planet positions, house cusps, aspect detection with configurable orbs, interactive SVG zodiac wheel
- **Synastry** — compatibility analysis between two birth charts with inter-aspects
- **Transits** — current planetary transits to natal chart
- **Interpretations** — 366+ text entries for planets in signs, houses, and aspects (EN / RU, DE / UK fallback to EN)
- **Numerology** — Life Path Number and Birthday Number with descriptions in all 4 languages
- **Multilingual UI** — full English / Russian / German / Ukrainian localization with language switcher
- **PDF Export** — server-side report generation via QuestPDF with full Unicode/Cyrillic support (selectable text, embedded fonts)
- **Geocoding** — city search powered by Nominatim (OpenStreetMap), results in selected language
- **Timezone handling** — automatic timezone detection from coordinates (GeoTimeZone + NodaTime)
- **Birth time optional** — works without exact birth time (houses will be approximate)
- **UX enhancements** — sign trait descriptions per planet, color-coded aspect rows with legend, tooltips, narrative chart summary

## Live Demo

| Service | URL |
|---------|-----|
| Frontend | https://natalchart-c4z.pages.dev |
| API | https://natalchart-api-empty-breeze-8714.fly.dev |
| Swagger | https://natalchart-api-empty-breeze-8714.fly.dev/swagger |

## Architecture

```
NatalChart/
├── src/
│   ├── NatalChart.Api/              # ASP.NET 8 Web API (controllers, DI, Swagger, CORS)
│   │   ├── Controllers/             # Chart, Export, Synastry, Transit, Geocoding
│   │   ├── Helpers/                 # BirthDataHelper (shared timezone logic)
│   │   ├── Services/                # PdfExportService (QuestPDF)
│   │   └── Fonts/                   # NotoSans TTF fonts for PDF
│   ├── NatalChart.Core/             # Domain: models, enums, interfaces
│   ├── NatalChart.Astrology/        # Computation engine (SwissEphNet)
│   ├── NatalChart.Interpretation/   # Text interpretations + numerology (EN/RU/DE/UK JSON data)
│   └── NatalChart.Infrastructure/   # External services (geocoding, timezone)
├── tests/
│   ├── NatalChart.Astrology.Tests/  # 17 tests (ephemeris, houses, aspects, calculator)
│   └── NatalChart.Interpretation.Tests/  # 3 tests
├── client/                          # React + TypeScript (Vite)
│   └── src/
│       ├── api/          # Axios API client
│       ├── types/        # TypeScript types matching backend models
│       ├── components/   # ChartWheel, PlanetTable, AspectGrid, NumerologyPanel, ExportPdfButton, etc.
│       ├── context/      # Language context (EN/RU/DE/UK)
│       ├── hooks/        # useChart, useSynastry, useTransits (React Query)
│       └── pages/        # NatalChartPage, SynastryPage, TransitsPage
├── Dockerfile            # Multi-stage .NET 8 build
└── fly.toml              # Fly.io deployment config
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/chart/calculate` | Calculate natal chart (planets, houses, aspects) |
| POST | `/api/chart/interpret?lang=en` | Get text interpretations (en/ru/de/uk) |
| POST | `/api/chart/numerology?lang=en` | Calculate numerology (Life Path + Birthday Number) |
| POST | `/api/export/pdf` | Generate full PDF report in selected language |
| POST | `/api/synastry/calculate` | Compare two charts with inter-aspects |
| POST | `/api/transit/calculate` | Current transits to natal chart |
| GET | `/api/geocoding/search?query=&lang=` | Location search (Nominatim proxy) |

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

20 tests (17 astrology + 3 interpretation).

## Deployment

### Backend — Fly.io

```bash
flyctl deploy
```

### Frontend — Cloudflare Pages

```bash
cd client
VITE_API_URL=https://your-api.fly.dev npm run build
wrangler pages deploy dist --project-name natalchart
```

## Tech Stack

### Backend
| Library | Purpose |
|---------|---------|
| **SwissEphNet** | C# port of Swiss Ephemeris — planet positions, house cusps |
| **QuestPDF** | Server-side PDF generation with full Unicode support |
| **GeoTimeZone** | Timezone lookup by geographic coordinates |
| **NodaTime** | Accurate historical timezone / DST handling |
| **Swashbuckle** | Swagger / OpenAPI documentation |

### Frontend
| Library | Purpose |
|---------|---------|
| **React 19** + **TypeScript** | UI framework |
| **Vite** | Build tool |
| **Material UI v9** | Component library + date/time pickers |
| **@tanstack/react-query** | Server state management |
| **react-router-dom** | Client-side routing |
| **axios** | HTTP client |

## Astrology Details

- **Zodiac:** Tropical (Western)
- **House systems:** Placidus (default), Koch, Equal, Whole Sign
- **Celestial bodies:** Sun, Moon, Mercury, Venus, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto, North Node, Chiron
- **Aspects:** Conjunction (0°), Opposition (180°), Trine (120°), Square (90°), Sextile (60°), Quincunx (150°)

### Aspect Orbs

| Aspect | Luminary Orb (Sun/Moon) | Other Planets |
|--------|------------------------|---------------|
| Conjunction | 10° | 8° |
| Opposition | 10° | 8° |
| Trine | 8° | 6° |
| Square | 8° | 6° |
| Sextile | 6° | 4° |
| Quincunx | 3° | 2° |

## Testing & Verification

**Golden chart:** January 1, 2000, 12:00 UTC, Greenwich (51.4769, -0.0005) — Sun in Capricorn ~280.5°, Ascendant in Aries. Positions verified against astro.com reference data.

## Author

**Viktor Ralchenko** — [LinkedIn](https://www.linkedin.com/in/vralchenko) | [Portfolio](https://vralchenko.pages.dev) | vralchenko@gmail.com

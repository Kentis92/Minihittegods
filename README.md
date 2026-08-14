# MiniHittegods

Et lite REST API for håndtering av hittegods.

Prosjektet lar brukere registrere gjenstander som er funnet, hente ut registrerte gjenstander, søke og filtrere i listen, markere gjenstander som hentet, levere dem tilbake og slette gjenstander som kan slettes.

Prosjektet er laget som en del av opplæringen min i backend-utvikling, med fokus på TDD, REST API, domeneregler og lagdeling.

## Teknologi

* C# / .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker Compose
* xUnit
* Swagger

## Prosjektstruktur

Prosjektet er delt opp i flere lag:

```text
MiniHittegods
│
├── MiniHittegods.Api
│   └── Controllers, DTOs, database og repositories
│
├── MiniHittegods.Application
│   └── Services og interfaces
│
├── MiniHittegods.Domain
│   └── Entities, enums og domeneregler
│
└── MiniHittegods.Tests
    └── Tester for domain, application og API
```

## Funksjonalitet

API-et har følgende hovedfunksjoner:

* Opprette et nytt hittegods
* Hente alle registrerte gjenstander
* Hente en gjenstand med ID
* Filtrere på status
* Filtrere på kategori
* Søke etter tekst i tittel og beskrivelse
* Markere en gjenstand som hentet
* Levere en hentet gjenstand tilbake
* Slette gjenstander som kan slettes

Ved oppretting må `title` og `foundLocation` være satt. Tittel kan være maksimalt 80 tegn. Ugyldige requests returnerer `400 Bad Request`.

API-et bruker også HTTP-statuskoder for å vise resultatet av handlingen, blant annet `201 Created`, `200 OK`, `204 No Content`, `400 Bad Request`, `404 Not Found` og `409 Conflict`.

## API

Hovedendepunktene er:

```text
POST   /api/items
GET    /api/items
GET    /api/items/{id}
POST   /api/items/{id}/claim
POST   /api/items/{id}/return
DELETE /api/items/{id}
```

`GET /api/items` støtter valgfrie query-parametere:

```text
/api/items?status=Available
/api/items?category=Clothing
/api/items?q=phone
```

`q` søker i både tittel og beskrivelse. Parametrene kan også kombineres.

## Starte prosjektet

Prosjektet bruker Docker Compose for å starte både API-et og PostgreSQL.

Fra rotmappen:

```bash
docker compose up --build
```

Når containerne er startet, er API-et tilgjengelig på:

```text
http://localhost:8080
```

Swagger er tilgjengelig på:

```text
http://localhost:8080/swagger
```

Det skal ikke være nødvendig å starte databasen eller API-et manuelt.

## Eksempel med curl

Opprette et hittegods:

```bash
curl -X POST http://localhost:8080/api/items \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Jakke\",\"description\":\"Svart jakke\",\"category\":\"Clothing\",\"foundLocation\":\"Skien sentrum\"}"
```

Hente gjenstander i en kategori:

```bash
curl "http://localhost:8080/api/items?category=Clothing"
```

Søke etter en gjenstand:

```bash
curl "http://localhost:8080/api/items?q=jakke"
```

## Tester

Alle tester kjøres med:

```bash
dotnet test
```

Testene dekker blant annet:

* Oppretting av gjenstander
* Validering av requests
* Filtrering og søk
* Statusendringer
* Claim og return
* Sletting
* Service-logikk
* Repository-logikk
* API-endepunkter

## Database

Prosjektet bruker PostgreSQL.

Docker Compose setter opp:

* Database: `minihittegods`
* Bruker: `postgres`
* Port: `5432`

## Om prosjektet

Dette er et av prosjektene hvor jeg har jobbet mer strukturert med:

* Test Driven Development
* Separasjon mellom Domain, Application og API
* Entity Framework Core
* PostgreSQL
* REST API-design
* Docker Compose
* Testing av API-et

Målet har vært å bygge et lite, fungerende backend-system med tydelig struktur og forståelse for hvordan de forskjellige delene henger sammen.

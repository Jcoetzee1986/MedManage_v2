# jsreport Setup for MedManage

## Overview

jsreport is used to generate PDF and Excel reports from HTML/Handlebars templates. The MedManage API communicates with jsreport via its HTTP API to render reports on demand.

## Running with Docker (Recommended)

```bash
cd Infrastructure/jsreport
docker-compose up -d
```

The jsreport studio will be available at: http://localhost:5488

Default credentials:
- Username: `admin`
- Password: `password`

> **Important**: Change these credentials in production by updating the `docker-compose.yml` environment variables and the `appsettings.Production.json` JsReport section.

## Running as Windows Service (Alternative)

1. Download jsreport from https://jsreport.net/on-prem
2. Extract to a folder (e.g., `C:\jsreport`)
3. Run `jsreport.exe install` to install as a Windows service
4. Configure authentication in `jsreport.config.json`:

```json
{
  "httpPort": 5488,
  "extensions": {
    "authentication": {
      "admin": {
        "username": "admin",
        "password": "password"
      }
    }
  }
}
```

5. Start the service: `net start jsreport`

## Configuration

The API connects to jsreport using settings in `appsettings.json`:

```json
{
  "JsReport": {
    "ServerUrl": "http://localhost:5488",
    "Username": "admin",
    "Password": "password",
    "TimeoutSeconds": 60
  }
}
```

## Report Templates

Report templates are managed in the jsreport studio. The following templates must be created:

| Template Name | Description | Recipe |
|---|---|---|
| `case-letter` | Case letter for a specific case | chrome-pdf |
| `cases-between-dates` | Cases within a date range | chrome-pdf / html-to-xlsx |
| `wip-extract` | Work In Progress extract | html-to-xlsx |
| `billing-summary` | Billing summary report | html-to-xlsx |

### Template Data Contracts

#### case-letter
```json
{
  "caseId": 123
}
```

#### cases-between-dates
```json
{
  "dateFrom": "2024-01-01T00:00:00",
  "dateTo": "2024-12-31T23:59:59",
  "serviceProviderId": null,
  "caseStatusId": null
}
```

#### wip-extract
```json
{
  "serviceProviderId": null,
  "mainClientId": null,
  "asAtDate": "2024-06-30T00:00:00"
}
```

#### billing-summary
```json
{
  "dateFrom": "2024-01-01T00:00:00",
  "dateTo": "2024-12-31T23:59:59",
  "serviceProviderId": null,
  "billingStatusId": null
}
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/reports/case-letter` | Generate case letter |
| POST | `/api/reports/cases-between-dates` | Generate cases between dates report |
| POST | `/api/reports/wip-extract` | Generate WIP extract |
| POST | `/api/reports/billing-summary` | Generate billing summary |

All endpoints return the generated file (PDF or XLSX) as a binary download.

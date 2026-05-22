# MedManage API Documentation

## Base URL
- Development: `https://localhost:5001/api`
- Production: `https://your-domain.com/api`

## Authentication
All API endpoints (except login) require JWT authentication.

Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Endpoints

### Health Check
```http
GET /api/health
```

**Response:**
```json
{
  "status": "Healthy",
  "timestamp": "2026-04-16T10:00:00Z",
  "version": "1.0.0"
}
```

---

### Authentication

#### Login
```http
POST /api/auth/login
```

**Request Body:**
```json
{
  "username": "string",
  "password": "string",
  "mainClientId": 1
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "string",
    "expiresAt": "2026-04-16T11:00:00Z",
    "user": {
      "userId": "string",
      "username": "string",
      "mainClientId": 1,
      "roles": ["User", "Admin"]
    }
  }
}
```

---

### Reference Data

#### Get All Countries
```http
GET /api/shared/countries
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "countryId": 1,
      "countryName": "South Africa",
      "countryCode": "ZA"
    }
  ]
}
```

---

## Response Structure

### Success Response
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error message",
  "errors": [
    "Detailed error 1",
    "Detailed error 2"
  ]
}
```

### Paginated Response
```json
{
  "data": [ ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalRecords": 100,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

## Error Codes

| Status Code | Description |
|-------------|-------------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

## Multi-Tenancy

All requests are automatically filtered by `MainClientId` extracted from the JWT token. You cannot access data from other clients.

## Rate Limiting

- 100 requests per minute per user
- 1000 requests per hour per client

## Next Steps

As modules are implemented, this documentation will be updated with:
- Case Management endpoints
- Member Management endpoints
- Finance & Billing endpoints
- Tariff Management endpoints
- Reporting endpoints

# JwtDemo

A minimal **.NET 10** Web API demonstrating JWT (JSON Web Token) authentication — token generation, protected endpoints, and the full Bearer flow.

---

## Features

- **`POST /auth/login`** — validates credentials and returns a signed JWT
- **`GET /weatherforecast`** — protected endpoint, requires a valid Bearer token
- Strongly-typed JWT settings bound from `appsettings.json`
- `HmacSha256` signed tokens with configurable expiry, issuer, and audience
- **RapiDoc** at `/rapidoc` for interactive API exploration with JWT authentication

---

## Project Structure

```
JwtDemo/
├── Controllers/
│   ├── AuthController.cs          # Login endpoint — issues JWT
│   └── WeatherForecastController.cs  # Protected endpoint
├── Models/
│   ├── JwtSettings.cs             # Config binding model
│   └── LoginRequest.cs            # Login request DTO
├── Services/
│   └── JwtService.cs              # Token generation logic
├── appsettings.json               # JWT configuration
└── Program.cs                     # Auth middleware registration
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone https://github.com/your-username/JwtDemo.git
cd JwtDemo
dotnet run --project JwtDemo
```

The API will be available at `https://localhost:{port}`.

---

## Usage

### 1. Obtain a token

```bash
curl -X POST https://localhost:{port}/auth/login \
  -H "Content-Type: application/json" \
  -d '{ "username": "admin", "password": "password123" }'
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 2. Call the protected endpoint

```bash
curl https://localhost:{port}/weatherforecast \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

Without a token → **401 Unauthorized**  
With a valid token → weather forecast data ✅

### 3. RapiDoc (development only)

1. Open `https://localhost:{port}/rapidoc` in a browser
2. Call `POST /auth/login` to get a token
3. Click **Authorize** in the RapiDoc header, enter the token and confirm
4. All subsequent requests will include the token automatically

---

## Configuration

JWT options live in `appsettings.json`:

```json
"Jwt": {
  "Key": "ThisIsASecretKeyForJwtDemoChangeInProduction!",
  "Issuer": "JwtDemo",
  "Audience": "JwtDemoUsers",
  "ExpiresInMinutes": 60
}
```

| Setting | Description |
|---|---|
| `Key` | HMAC signing secret (min. 32 chars recommended) |
| `Issuer` | Token issuer claim |
| `Audience` | Intended token audience |
| `ExpiresInMinutes` | Token lifetime in minutes |

---

## ⚠️ Security Notes

> This project is for **demonstration purposes only**.

- Replace the hard-coded `admin` / `password123` credentials with a real user store (e.g. ASP.NET Core Identity).
- Never commit secret keys to source control — use [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or a secrets manager (Azure Key Vault, etc.) in production.
- Use HTTPS in all environments.

---

## License

[MIT](LICENSE)

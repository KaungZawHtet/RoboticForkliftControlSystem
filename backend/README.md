# 🚀 Web API for Robotic Forklift Management System

A modern Web API for Robotic Forklift Management System built with **.NET 9**, following clean architecture
practices and ready for azure deployments.

------------------------------------------------------------------------

## 📋 Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [PostgreSQL](https://www.postgresql.org/) 

------------------------------------------------------------------------

## ⚡ Project Setup

1.  **Clone the repository**

    ``` bash
    git clone https://github.com/KaungZawHtet/RoboticForkliftControlSystem
    cd RoboticForkliftControlSystem/backend
    ```

2.  **Restore dependencies**

    ``` bash
    dotnet restore
    ```

3.  **Set up user secrets (for local development)**

    ``` bash
    dotnet user-secrets init
    dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=yourdb;Username=youruser;Password=yourpassword"
    ```

4.  **Database migration**

    ``` bash
    dotnet ef database update
    ```

------------------------------------------------------------------------

## 🗂️ Configuration

Settings are managed via **appsettings.json** and environment variables.

``` json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=yourdb;Username=youruser;Password=yourpassword"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:5173"
  ]
}
```

------------------------------------------------------------------------

## ▶️ Running the Application

-   **Development mode**

    ``` bash
    dotnet run --project RoboticForkliftControlSystem.Api
    ```



The API will be available at: - `https://localhost:5xxx` -
`http://localhost:5xxx`

------------------------------------------------------------------------

## 🧪 Testing

Run all tests with:

``` bash
dotnet test
```

------------------------------------------------------------------------

## 📖 API Documentation

scalar is enabled by default.

-   Development: <https://localhost:5xxx/scalar>


------------------------------------------------------------------------

## ✅ Health Check

Check if the API is alive:

``` bash
GET /healthz
```

------------------------------------------------------------------------

## 📌 Tech Stack

-   **.NET 8 Web API**
-   **Entity Framework Core**
-   **PostgreSQL**
-   **Serilog** for logging
-   **Scalar / OpenAPI** for docs

## Database migration

Consider to use idempotent script.



```bash
# In Backend dir, run these commands
dotnet ef migrations script --idempotent --project RoboticForkliftControlSystem.Api/RoboticForkliftControlSystem.Api.csproj -o artifacts/migrations-$(date +"%Y%m%d%H%M%S").sql

psql "host=$HOST dbname=$DB user=$USER password=$PASSWORD sslmode=require" -v ON_ERROR_STOP=1 -f artifacts/migrations-your-selected-datetime.sql

```

This is alternative one but prefer idempotent script

```bash
dotnet ef database update --project RoboticForkliftControlSystem.Api/RoboticForkliftControlSystem.Api.csproj --startup-project RoboticForkliftControlSystem.Api/RoboticForkliftControlSystem.Api.csproj
```



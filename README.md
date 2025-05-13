# TrueStoryAPI 🚀

A robust RESTful Web API built with **.NET 8.0** that extends the [Restful-API.dev](https://api.restful-api.dev) mock service with advanced features:
- ✨ **Filtering** & **Pagination**
- ✅ **Data Validation**
- 🔄 **Full CRUD Operations**
- 📊 **Batch Processing**
- 📝 **Comprehensive Logging**

---

## 📋 Table of Contents

1. [✨ Features](#-features)
2. [⚙️ Prerequisites](#%EF%B8%8F-prerequisites)
3. [🔧 Configuration](#-configuration)
4. [🚀 Setup & Running](#-setup--running)
5. [📚 API Documentation](#-api-documentation)
6. [📊 DTO Schemas](#-dto-schemas)
7. [🛠️ Project Structure](#%EF%B8%8F-project-structure)
8. [📝 Logging & Errors](#-logging--error-handling)
9. [🤝 Contributing](#-contributing)
10. [📜 License](#-license)

---

## ✨ Features

- **Modern .NET 8.0** stack
- **Swagger UI** for interactive testing
- **Clean Architecture** with separation of concerns
- **Production-ready** error handling
- **Flexible configuration** via appsettings.json
- **Batch operations** support

---

## ⚙️ Prerequisites

Before you begin, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)
- IDE of choice:
  - Visual Studio 2022+
  - VS Code with C# extensions
  - JetBrains Rider

---

## 🔧 Configuration

### appsettings.json
```json
{
  "MockApi": {
    "BaseUrl": "https://api.restful-api.dev"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### Environment Variables
| Variable | Purpose |
|----------|---------|
| `MockApi__BaseUrl` | Override mock API URL |
| `ASPNETCORE_ENVIRONMENT` | Set to `Production` for prod mode |

---

## 🚀 Setup & Running

### Clone & Setup
```bash
git clone https://github.com/Meekdavid/product-api-ts
cd TrueStoryAPI
dotnet restore
dotnet build
```

### Running the API
```bash
dotnet run
```

### Access Swagger UI
After running, open:  
🔗 [https://localhost:5001/swagger](https://localhost:5001/swagger)  
*(Port may vary - check console output)*

---

## 📚 API Documentation

### Base Endpoint
```
https://localhost:5001/api/products
```

### 📌 Available Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | Get paginated products with optional `name` filter |
| `GET` | `/api/products/batch?id={ids}` | Fetch multiple products by IDs |
| `GET` | `/api/products/{id}` | Get single product |
| `POST` | `/api/products` | Create new product |
| `PUT` | `/api/products/{id}` | Full product update |
| `PATCH` | `/api/products/{id}` | Partial update |
| `DELETE` | `/api/products/{id}` | Remove product |

---

## 📊 DTO Schemas

### Create Product
```json
{
  "name": "string (3-100 chars)",
  "data": {
    "key": "value" // Flexible properties
  }
}
```

### Update Product
```json
{
  "name": "string (3-100 chars)",
  "data": {} // Complete replacement
}
```

### Patch Product
```json
{
  "name?": "optional string",
  "data?": {} // Partial update
}
```

---

## 🛠️ Project Structure

```
TrueStoryAPI/
├── Controllers/       # API endpoints
├── DTOs/             # Data transfer objects
├── Interfaces/       # Service contracts
├── Repositories/     # Data access
├── Models/           # Domain models
├── Program.cs        # Startup
└── appsettings.json  # Configuration
```

---

## 📝 Logging & Error Handling

### Error Responses
| Code | Meaning |
|------|---------|
| 400 | Validation failed |
| 404 | Not found |
| 503 | Mock API unavailable |
| 500 | Server error |

### Logging Features
- Structured logging with `ILogger<T>`
- Automatic request/response logging
- Detailed error messages

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📜 License

None

Permission is hereby granted... [standard MIT text]
```

# TrueStoryAPI

A simple RESTful Web API built with **.NET 8.0** that integrates with the mock API at [https://api.restful-api.dev](https://api.restful-api.dev) and extends it with filtering, paging, validation, and full CRUD functionality.

---

## Table of Contents

1. [Prerequisites](#prerequisites)  
2. [Configuration](#configuration)  
3. [Setup & Run](#setup--run)  
4. [API Documentation](#api-documentation)  
   - [Base URL](#base-url)  
   - [Endpoints](#endpoints)  
   - [DTOs](#dtos)  
   - [Response Codes](#response-codes)  
   - [Examples](#examples)  
5. [Logging & Error Handling](#logging--error-handling)  
6. [Project Structure](#project-structure)  
7. [Contributing](#contributing)  
8. [License](#license)  

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Git](https://git-scm.com/downloads)  
- A code editor or IDE (e.g., Visual Studio 2022+, Visual Studio Code)  

---

## Configuration

1. **appsettings.json**  
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
````

2. **Environment variables**

   * You can override the base URL via `MockApi__BaseUrl` if needed.
   * To run in Production mode, set `ASPNETCORE_ENVIRONMENT=Production`.

---

## Setup & Run

1. **Clone the repo**

   ```bash
   git clone https://github.com/Meekdavid/product-api-ts
   cd TrueStoryAPI
   ```
2. **Restore packages**

   ```bash
   dotnet restore
   ```
3. **Build the project**

   ```bash
   dotnet build
   ```
4. **Run the API**

   ```bash
   dotnet run
   ```
5. **Browse Swagger UI**
   Once running, navigate to `https://localhost:5001/swagger` (or the URL printed in console) to explore and test all endpoints.

---

## API Documentation

### Base URL

```
https://localhost:5001/api/products
```

*(Port may vary. Check console output when you run `dotnet run`.)*

### Endpoints

| Method | Route                                     | Description                                                                                  |
| ------ | ----------------------------------------- | -------------------------------------------------------------------------------------------- |
| GET    | `/api/products`                           | Get paginated list of products.<br/>Supports `?name={substr}`, `?page={n}`, `?pageSize={n}`. |
| GET    | `/api/products/batch?id={id1}&id={id2}&…` | Fetch multiple products by array of IDs.                                                     |
| GET    | `/api/products/{id}`                      | Fetch a single product by ID. Returns `404` if not found.                                    |
| POST   | `/api/products`                           | Create a new product.<br/>Body: [CreateProductDto](#dtos)                                    |
| PUT    | `/api/products/{id}`                      | Replace an existing product.<br/>Body: [UpdateProductDto](#dtos)                             |
| PATCH  | `/api/products/{id}`                      | Partially update a product.<br/>Body: [PatchProductDto](#dtos)                               |
| DELETE | `/api/products/{id}`                      | Delete a product by ID. Returns `204` on success or `404` if not found.                      |

---

### DTOs

#### CreateProductDto

```csharp
public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    public Dictionary<string, object> Data { get; set; } = new();
}
```

#### UpdateProductDto

```csharp
public class UpdateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    public Dictionary<string, object> Data { get; set; } = new();
}
```

#### PatchProductDto

```csharp
public class PatchProductDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; set; }

    public Dictionary<string, object>? Data { get; set; }
}
```

---

### Response Codes

| Status Code                 | Meaning                                                  |
| --------------------------- | -------------------------------------------------------- |
| `200 OK`                    | Request succeeded and data (if any) returned.            |
| `201 Created`               | Resource created. Location header points to `GET /{id}`. |
| `204 No Content`            | Resource deleted successfully.                           |
| `400 Bad Request`           | Validation failed or bad parameters.                     |
| `404 Not Found`             | Resource with specified ID does not exist.               |
| `503 Service Unavailable`   | Upstream mock API is unreachable or returns an error.    |
| `500 Internal Server Error` | Unexpected server-side error.                            |

---

### Examples

#### 1. Get paginated products

```bash
curl "https://localhost:5001/api/products?name=Apple&page=1&pageSize=5"
```

**Response**: `200 OK`

```json
{
  "items": [ /* array of products */ ],
  "pageIndex": 1,
  "pageSize": 5,
  "totalCount": 13,
  "totalPages": 3
}
```

#### 2. Batch fetch by IDs

```bash
curl "https://localhost:5001/api/products/batch?id=3&id=5&id=10"
```

#### 3. Create a product

```bash
curl -X POST "https://localhost:5001/api/products" \
     -H "Content-Type: application/json" \
     -d '{
           "name": "New Gadget",
           "data": {
             "price": 99.99,
             "color": "Red"
           }
         }'
```

**Response**: `201 Created`
Location header: `/api/products/14`

---

## Logging & Error Handling

* Uses ASP.NET Core’s built-in `ILogger<T>` for structured logging.
* Each action wraps calls in `try/catch` blocks:

  * **`HttpRequestException`** → `503 Service Unavailable`
  * **Other exceptions** → `500 Internal Server Error`
* Model binding and data-annotation validation automatically return `400 Bad Request` with detailed error messages.

---

## Project Structure

```
/Controllers
    ProductsController.cs
/DTOs
    CreateProductDto.cs
    UpdateProductDto.cs
    PatchProductDto.cs
/Interfaces
    IProductRepository.cs
/Repositories
    ProductRepository.cs
/Models
    Product.cs
Program.cs
appsettings.json
TrueStoryAPI.csproj
```

---

## Contributing

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/XYZ`)
3. Commit your changes (`git commit -m "Add XYZ"`)
4. Push to your branch (`git push origin feature/XYZ`)
5. Open a Pull Request

---

## License

This project is licensed under the MIT License.

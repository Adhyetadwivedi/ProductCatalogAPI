# ProductApi - Product Management System
A RESTful API built with ASP.NET Core for managing products with stock control functionality. This API supports full CRUD operations and provides endpoints for stock management in a distributed environment.

## 🚀 Features

Complete CRUD Operations for products
Stock Management with increment/decrement functionality
Distributed ID Generation for unique 6-digit product IDs
Thread-Safe Operations using semaphore for concurrent access
Entity Framework Core with Code First approach
Clean Architecture with Repository and Service patterns
Async/Await patterns throughout
Comprehensive Unit Tests with xUnit

## 🛠 Technologies Used

 - .NET 8.0
 - ASP.NET Core Web API
 - Entity Framework Core
 - SQL Server
 - Swagger/OpenAPI
 - xUnit (Testing)
 - Moq (Mocking)

## 📁 Project Structure
```
ProductApi/
├── Controllers/
│   └── ProductsController.cs          # API endpoints
├── Data/
│   └── AppDbContext.cs                # Database context
├── DTOs/
│   └── ProductDto.cs                  # Data transfer objects
├── Interfaces/
│   ├── IProductRepository.cs          # Repository interface
│   ├── IProductService.cs             # Service interface
│   └── IProductIdGenerator.cs         # ID generation interface
├── Models/
│   ├── Product.cs                     # Product entity
│   └── ProductIdTracker.cs            # ID tracking entity
├── Repositories/
│   └── ProductRepository.cs           # Data access layer
├── Services/
│   ├── ProductService.cs              # Business logic layer
│   └── ProductIdGenerator.cs          # Unique ID generation
├── Program.cs                         # Application entry point
└── appsettings.json                   # Configuration

```
## 🔧 Setup Instructions
### Prerequisites

- .NET 8.0 SDK
- SQL Server (Local DB or Express)
- Visual Studio 2022 or VS Code

### Installation Steps

**1. Clone the repository**
```
bashgit clone <repository-url>
cd ProductApi
```

**2. Update Connection String** Update the connection string in **appsettings.json** to match your SQL Server instance:
```
json{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDb;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```

**3. Install Dependencies**
```
bashdotnet restore
```

**4. Create Database and Run Migrations**
```
bashdotnet ef database update
```

**5. Run the Application**
```
bashdotnet run
```

**6. Access Swagger UI**
```
Navigate to **https://localhost:5001/swagger ** to explore the API endpoints.
```

## 📋 API Endpoints
**Product Management**
```
Method  Endpoint        Description
GET     /productsGet    all products
GET     /products/{id}  Get product by ID
POST    /products       Create new product
PUT     /products/{id}  Update existing product
DELETE  /products/{id}  Delete product
```

**Stock Management**
```
Method  Endpoint                                        Description
PUT     /products/decrement-stock/{id}/{quantity}       Decrease product stock
PUT     /products/add-to-stock/{id}/{quantity}Increase  product stock
```
  
**Sample Request/Response** <br>
**Create Product (POST /products)**
```
Request Body:
json{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop with RTX 4070",
  "stockAvailable": 25,
  "price": 1299.99
}
```
```
Response:
json{
  "productId": "100001",
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop with RTX 4070",
  "stockAvailable": 25,
  "price": 1299.99,
  "createdAt": "2025-01-15T10:30:00Z"
}
```

## 🧪 Running Tests
**Run All Tests**
```
bashdotnet test
```

**Run Tests with Coverage**
```
bashdotnet test --collect:"XPlat Code Coverage"
```

**Test Project Structure**
```
ProductApi.Tests/
├── Services/
│   ├── ProductServiceTests.cs
│   └── ProductIdGeneratorTests.cs
└── Controllers/
    └── ProductsControllerTests.cs
```

## 🏗 Architecture & Design Patterns
**Clean Architecture**

- Controllers: Handle HTTP requests and responses
- Services: Contains business logic
- Repositories: Data access layer
- Models: Domain entities
- DTOs: Data transfer objects

## Design Patterns Used

- Repository Pattern: Abstracts data access logic
- Dependency Injection: Promotes loose coupling
- Service Layer Pattern: Encapsulates business logic
- Factory Pattern: Used in ID generation

## 🔒 Key Features
### Distributed ID Generation

- Generates unique 6-digit IDs starting from 100001
- Thread-safe using SemaphoreSlim
- Handles concurrent requests across multiple instances
- Stores last generated ID in database for persistence

### Stock Management

- Prevents overselling with validation
- Atomic operations for stock updates
- Thread-safe increment/decrement operations

### Error Handling

- Proper HTTP status codes
- Validation for business rules
- Graceful handling of edge cases

## 🚀 Production Considerations
This API is designed with production scalability in mind:

- Async Operations: All database operations are asynchronous
- Thread Safety: Critical sections protected with semaphore
- Clean Architecture: Easy to maintain and extend
- Testable Code: Comprehensive unit test coverage
- Logging Ready: Structured for adding logging middleware

## 📝 Database Schema
**Products Table**
```
Column          Type              Description
ProductId       string (PK)       Unique 6-digit identifier
Name            string            Product name
Description     string            Product description
StockAvailable  int               Current stock quantity
Price           decimal(18,2)     Product price
CreatedAt       datetimeCreation  timestamp

```

**ProductIdTrackers Table**
```
Column           Type      Description
Id               int (PK)  Auto-increment primary key
LastGeneratedId  int       Last generated product ID
```

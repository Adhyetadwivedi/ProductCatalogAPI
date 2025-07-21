# Product API

A RESTful API for managing products built with ASP.NET Core 8.

## Features

- Complete CRUD operations for products
- Distributed-safe 6-digit ID generation
- Stock management with proper validation
- Comprehensive error handling
- Input validation with detailed error messages
- Structured logging
- Health checks
- Swagger/OpenAPI documentation
- Unit tests
- Soft delete functionality

## Prerequisites

- .NET 8.0 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

## Getting Started

1. **Clone the repository**
2. **Update connection string** in `appsettings.json`
3. **Run migrations**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
4. Run the application:
	dotnet run
5. Access Swagger UI at: https://localhost:7XXX/swagger

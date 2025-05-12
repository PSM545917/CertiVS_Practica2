# Patient Management System - Practice 3

This is a patient management system that includes two separate API projects:

1. **CertiVS_Practica2**: The main API for patient management
2. **PatientCodeAPI**: A backing service API that generates patient codes based on names and identification numbers

## Project Structure

- **CertiVS_Practica2** - Main API
  - BusinessLogic: Contains domain models and business logic managers
  - Services: Contains external service clients
  - Controllers: API endpoints

- **PatientCodeAPI** - Patient Code Generation Service
  - Models: Request/response DTOs
  - Services: Business logic for code generation
  - Controllers: API endpoints

## Features

- Patient management (CRUD operations)
- Automatic blood group assignment
- Patient code generation (format: first letter of name + first letter of lastname + "-" + CI)
- Error handling and logging
- Configuration through appsettings
- Swagger API documentation

## Technologies

- .NET 6+
- ASP.NET Core Web API
- Dependency Injection
- HTTP Client for service-to-service communication
- File-based storage

## 12 Factor App Implementation

1. **Codebase**: Single codebase tracked in Git with multiple deployments
2. **Dependencies**: Explicitly declared and isolated dependencies through .csproj files
3. **Config**: Configuration stored in environment variables or appsettings.json
4. **Backing Services**: PatientCodeAPI treated as an attached resource via HTTP
5. **Build, Release, Run**: Separation of build and run stages
6. **Logs**: Treated as event streams, using the built-in .NET logging framework
7. **Error Handling**: Comprehensive exception handling throughout the application

## API Endpoints

### CertiVS_Practica2 API

- GET `/api/Patient` - Get all patients
- GET `/api/Patient/{ci}` - Get patient by CI
- POST `/api/Patient` - Create a new patient
- PUT `/api/Patient/{ci}` - Update an existing patient
- DELETE `/api/Patient/{ci}` - Delete a patient

### PatientCodeAPI

- POST `/api/PatientCode/generate` - Generate a patient code

## How to Run

### Prerequisites

- .NET 6+ SDK

### Running Locally

1. Clone the repository
2. Open the solution in Visual Studio or VS Code
3. Make sure both projects are set to start
4. Press F5 to run both projects

### Configuration

The main API connects to the PatientCodeAPI using the URL configured in the `appsettings.json` file:

```json
"ExternalServices": {
  "PatientCodeApiUrl": "https://localhost:7200"
}
```

## Deployment to Azure

Both APIs can be deployed to Azure App Service following these steps:

1. Create Azure App Service resources for both projects
2. Configure connection strings and settings in Azure
3. Deploy using Visual Studio publish or GitHub Actions

## Versioning

This project follows a branching model:

- `master` (main branch) - Production-ready code
- `develop` (main branch) - Development code
- `P3-001` (support branch) - Feature implementation branch
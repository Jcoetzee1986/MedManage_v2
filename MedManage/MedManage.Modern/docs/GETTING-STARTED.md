# Getting Started with MedManage.Modern

This guide will help you set up and run the modernized MedManage application.

## Prerequisites

Before you begin, ensure you have the following installed:

- **Node.js** 18.x or later ([Download](https://nodejs.org/))
- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))  
- **SQL Server 2019+** (or access to existing MedManage database)
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git** for version control

## Project Structure

```
MedManage.Modern/
├── src/                    # .NET Backend
│   ├── MedManage.API/         # Web API
│   ├── MedManage.Core/        # Domain layer
│   ├── MedManage.Infrastructure/  # Data access
│   ├── MedManage.Identity/    # Authentication
│   └── MedManage.Shared/      # Shared DTOs
├── client/                 # Angular Frontend
│   └── medmanage-angular/
├── tests/                  # Unit & Integration tests
└── docs/                   # Documentation
```

## Step 1: Clone and Navigate

```powershell
cd c:\Code\MedManage\MedManage\MedManage.Modern
```

## Step 2: Configure Database Connection

1. Open `src/MedManage.API/appsettings.json`
2. Update the connection string to point to your MedManage database:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=MedManage;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

## Step 3: Set Up .NET Backend

### Restore NuGet Packages

```powershell
dotnet restore
```

### Build the Solution

```powershell
dotnet build
```

### Run the API

```powershell
cd src/MedManage.API
dotnet run
```

The API will start at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

### Test the API

Open your browser and navigate to:
```
https://localhost:5001/swagger
```

You should see the Swagger UI with the Health endpoint.

## Step 4: Set Up Angular Frontend

### Navigate to Angular Project

```powershell
cd client/medmanage-angular
```

### Install Dependencies

```powershell
npm install
```

This will install Angular 17, Angular Material, NgRx, and all other dependencies.

### Run the Angular App

```powershell
npm start
```

The Angular app will start at:
```
http://localhost:4200
```

### Open in Browser

Navigate to `http://localhost:4200` and you should see the login screen.

## Step 5: Scaffold Database Entities (Optional for Phase 1)

To generate Entity Framework Core entities from your existing database:

### Using Package Manager Console (Visual Studio)

```powershell
Scaffold-DbContext "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Context MedManageDbContext -ContextDir Persistence -Force -Project MedManage.Infrastructure
```

### Using dotnet CLI

```powershell
cd src/MedManage.Infrastructure

dotnet ef dbcontext scaffold "Server=.;Database=MedManage;Integrated Security=true;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --context MedManageDbContext --context-dir Persistence --force
```

## Step 6: Run Tests

```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

## Development Workflow

### Running Both API and Angular Together

**Terminal 1 (API):**
```powershell
cd src/MedManage.API
dotnet watch run
```

**Terminal 2 (Angular):**
```powershell
cd client/medmanage-angular
npm start
```

### Making Changes

1. **Backend changes**: The API will auto-reload with `dotnet watch run`
2. **Frontend changes**: Angular will auto-reload with Hot Module Replacement

## Common Issues and Solutions

### Issue: Port Already in Use

**Solution**: Kill the process or change the port in `Properties/launchSettings.json`

```powershell
# Find process using port 5001
netstat -ano | findstr :5001

# Kill process (replace PID)
taskkill /PID <PID> /F
```

### Issue: Database Connection Failed

**Solution**:
1. Verify SQL Server is running
2. Check connection string is correct
3. Ensure your Windows user has access to the database

### Issue: npm install fails

**Solution**:
```powershell
# Clear npm cache
npm cache clean --force

# Delete node_modules and package-lock.json
rm -r node_modules
rm package-lock.json

# Reinstall
npm install
```

### Issue: EF Core Tools Not Found

**Solution**:
```powershell
dotnet tool install --global dotnet-ef
```

## Next Steps

Now that your development environment is set up:

1. ✅ **Phase 1 Complete** - Foundation is ready
2. 📋 **Next**: Implement authentication (Phase 2)
3. 📋 **Then**: Build data access layer (Phase 3)
4. 📋 **Then**: Create first API endpoints (Phase 4)

Refer to the [README.md](../README.md) for the complete implementation plan.

## Useful Commands

### .NET Commands
```powershell
dotnet build                # Build solution
dotnet test                 # Run tests
dotnet run                  # Run project
dotnet watch run            # Run with auto-reload
dotnet ef migrations add    # Create migration
dotnet ef database update   # Apply migrations
```

### Angular Commands
```powershell
npm start                   # Start dev server
npm run build               # Build for production
npm test                    # Run unit tests
npm run lint                # Lint code
ng generate component       # Generate component
ng generate service         # Generate service
```

## Additional Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Angular 17 Documentation](https://angular.io/docs)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Angular Material](https://material.angular.io/)
- [NgRx Store](https://ngrx.io/docs)

## Support

For issues or questions:
1. Check the [README.md](../README.md)
2. Review [API Documentation](api-design.md)
3. Contact the development team

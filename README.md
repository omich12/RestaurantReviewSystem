# Restaurant Review System - ASP.NET Core MVC

A simple and clean ASP.NET Core MVC web application built with .NET 10.0 for managing restaurant information and user reviews.

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 10.0)
- **Database**: SQLite with Entity Framework Core (Code First)
- **UI Framework**: Bootstrap 5
- **ORM**: Entity Framework Core with migrations

## Project Structure

```
RestaurantReviewSystem/
├── Models/
│   ├── Restaurant.cs
│   └── Review.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Controllers/
│   ├── HomeController.cs
│   ├── RestaurantsController.cs
│   └── ReviewsController.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── Privacy.cshtml
│   │   └── Error.cshtml
│   ├── Restaurants/
│   │   ├── Index.cshtml
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   ├── Reviews/
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _ViewStart.cshtml
│       ├── _ViewImports.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── wwwroot/
│   ├── css/
│   └── js/
├── Migrations/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── RestaurantReviewSystem.csproj
└── README.md
```

## Getting Started

### Prerequisites

- .NET 10.0 SDK or higher
- Visual Studio Code, Visual Studio, or another C# IDE

### Installation & Setup

1. **Navigate to project directory**:
   ```bash
   cd RestaurantReviewSystem
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Apply Database Migrations**:
   ```bash
   dotnet ef database update
   ```

   Or if using Package Manager Console in Visual Studio:
   ```
   Update-Database
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Open in browser**:
   Navigate to `https://localhost:7001` (or the port shown in console)

## Database Configuration

The application uses SQLite with the connection string defined in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=RestaurantReview.db"
  }
}
```

The database is automatically created and migrations are applied when the application starts.

## Features

### Restaurant Management

- **List Restaurants**: View all restaurants with their details and average ratings
- **Create Restaurant**: Add new restaurants to the system
- **View Details**: See full restaurant information and all its reviews
- **Edit Restaurant**: Update restaurant information
- **Delete Restaurant**: Remove restaurants (and associated reviews)

### Review Management

- **Add Review**: Add a review with rating (1-5) and comment from restaurant details page
- **Edit Review**: Modify existing reviews
- **Delete Review**: Remove reviews from the system

### Business Logic

- Average rating is calculated dynamically using LINQ
- Proper validation on all inputs using DataAnnotations
- One-to-Many relationship between Restaurant and Review
- Cascading deletes configured in EF Core

## Models

### Restaurant
- **Id** (int, Primary Key)
- **Name** (string, Required, 2-100 chars)
- **Location** (string, Required, 2-100 chars)
- **CuisineType** (string, Required, 2-50 chars)
- **CreatedDate** (DateTime, Default: UtcNow)
- **Reviews** (ICollection<Review>)
- **AverageRating** (Calculated property)

### Review
- **Id** (int, Primary Key)
- **Rating** (int, Required, Range 1-5)
- **Comment** (string, Required, 5-500 chars)
- **RestaurantId** (int, Foreign Key)
- **Restaurant** (Navigation property)

## Database Relationship

- **One Restaurant → Many Reviews** (One-to-Many)
- **One Review → One Restaurant** (Many-to-One)
- Delete behavior: Cascade (deleting a restaurant deletes all its reviews)

## Migrations

The project is configured to automatically apply migrations on startup. To manually manage migrations:

### Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

### List all migrations:
```bash
dotnet ef migrations list
```

### Remove last migration:
```bash
dotnet ef migrations remove
```

## Controllers

### HomeController
- `GET /` - Home page
- `GET /Privacy` - Privacy page

### RestaurantsController
- `GET /Restaurants` - List all restaurants
- `GET /Restaurants/Details/{id}` - View restaurant details
- `GET /Restaurants/Create` - Create form
- `POST /Restaurants/Create` - Create new restaurant
- `GET /Restaurants/Edit/{id}` - Edit form
- `POST /Restaurants/Edit/{id}` - Update restaurant
- `GET /Restaurants/Delete/{id}` - Delete confirmation
- `POST /Restaurants/Delete/{id}` - Delete restaurant

### ReviewsController
- `POST /Reviews/Create` - Add review (from restaurant details)
- `GET /Reviews/Edit/{id}` - Edit review form
- `POST /Reviews/Edit/{id}` - Update review
- `GET /Reviews/Delete/{id}` - Delete confirmation
- `POST /Reviews/Delete/{id}` - Delete review

## UI Components

- **Responsive Bootstrap 5 Layout**
- **Navbar Navigation** with Home and Restaurants links
- **Card-based Restaurant Display** showing name, location, cuisine, and average rating
- **Forms with Validation** error display
- **Tables** for review listings
- **Footer** with copyright information

## Validation Rules

### Restaurant
- Name: Required, 2-100 characters
- Location: Required, 2-100 characters
- CuisineType: Required, 2-50 characters

### Review
- Rating: Required, must be 1-5
- Comment: Required, 5-500 characters
- RestaurantId: Required (must be valid restaurant)

## Sample Data

The database is seeded with sample data on first migration:

**Restaurants**:
- The Italian Kitchen (Downtown, Italian)
- Spice Route (Midtown, Indian)

**Reviews**:
- 5 stars: "Excellent pasta and wonderful service!" (Italian Kitchen)
- 4 stars: "Good food but bit noisy." (Italian Kitchen)
- 5 stars: "Amazing spices and authentic flavors!" (Spice Route)

## Development Tips

1. **Hot Reload**: Use `dotnet watch` for automatic reload on code changes:
   ```bash
   dotnet watch run
   ```

2. **Debugging**: Set breakpoints in Visual Studio and use F5 to debug

3. **Database Inspection**: Use DB Browser for SQLite to inspect the database file

4. **Logging**: Check console output for EF Core SQL queries (configured in appsettings)

## Future Enhancements

- User authentication and authorization
- User profiles and review history
- Restaurant images and galleries
- Search and filtering functionality
- Rating statistics and charts
- Email notifications
- API layer with Swagger documentation

## License

Open source - free to use and modify for educational purposes.

## Support

For issues or questions, refer to the official ASP.NET Core documentation:
- https://learn.microsoft.com/en-us/aspnet/core/
- https://learn.microsoft.com/en-us/ef/core/

# SafeVault

SafeVault is an ASP.NET Core application with authentication, roles, and departments management.

## Prerequisites

- .NET 8 SDK
- Mysql
- Postman for API testing

## Setup and Run

1. **Clone the repository** (if not already done):
   ```
   git clone <repository-url>
   cd SafeVault
   ```

2. **Restore dependencies**:
   ```
   dotnet restore
   ```

3. Set up the database connection string & JWT config in `appsettings.json`:
   
4. **Run migrations** to set up the database:
   ```
   dotnet ef database update
   ```

4. **Build and run the application**:
   ```
   dotnet build
   dotnet run
   ```
   The application will start on the URL specified in `launchSettings.json` (by default is `https://localhost:8080`).

## Testing with Postman

1. **Import Postman Collection**:
    - Open Postman.
    - Import the collection from `assets/SafeVault.postman_collection.json`.

2. **Create Roles**:
    - Use the "Create Role" endpoint to create "Admin" and "Manager" roles.

3. **Obtain JWT Token**:
    - Use the "Login" endpoint to authenticate and get a JWT token.
    - Copy the token from the response.

4. **Set Token in Postman Variables**:
    - In Postman, go to the collection variables.
    - Set the value of the "token" variable to the copied JWT token.
    - Authenticated requests will now use this token (e.g., via Authorization header: `Bearer {{token}}`).

## Additional Notes

- Ensure the database connection string in `appsettings.json` is correct.
- For development, use `appsettings.Development.json` for environment-specific settings.
- The application uses BCrypt for password hashing and JWT for authentication.

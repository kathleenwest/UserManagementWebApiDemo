# User Management Web API

![Picture of Project](./images/Title.jpg)

## Table of Contents
- [User Management Web API](#user-management-web-api)
  - [Table of Contents](#table-of-contents)
  - [Summary](#summary)
  - [How to Try Out the Project Web API](#how-to-try-out-the-project-web-api)
  - [Requirements](#requirements)
  - [Architecture](#architecture)
    - [Project Structure](#project-structure)
  - [Demo Video](#demo-video)
  - [Building and Running the Solution](#building-and-running-the-solution)
    - [Prerequisites](#prerequisites)
    - [Building the Solution](#building-the-solution)
    - [Running the Solution](#running-the-solution)
  - [Database Migrations](#database-migrations)
    - [Creating Migrations](#creating-migrations)
    - [Deploying the SQLite Database](#deploying-the-sqlite-database)
  - [Unit Tests](#unit-tests)
    - [Running Unit Tests](#running-unit-tests)
    - [Test Coverage](#test-coverage)
  - [License](#license)

## Summary

This repository contains a simple User Management Web API built with ASP.NET Core. The API provides CRUD operations for managing user data, including creating, retrieving, updating, and deleting users. The project uses Entity Framework Core with SQLite as the database provider and includes comprehensive unit tests for the controllers, services, and repository layers.

## How to Try Out the Project Web API
You can explore and test the functionality of our Project Web API through our Swagger interface. Follow these steps to get started:

Access the Swagger UI: Open your web browser and navigate to the Swagger UI by clicking on this [link](http://user-management-demo.runasp.net/swagger/index.html).

Explore the Endpoints: Browse through the available API endpoints listed in the Swagger interface. Each endpoint represents a different operation you can perform with the API.

Test the API: To test an endpoint, select it from the list, and then click the "Try it out" button. Fill in any required parameters and click "Execute" to send a request to the API.

View Responses: After executing a request, you can view the response details, including the status code, response body, and any headers returned by the API.

The Swagger UI provides an intuitive way to interact with the API and understand its capabilities without writing any code. Feel free to explore and experiment with different endpoints to get a better understanding of how the API works.

## Requirements

Refer to the [coding-challenge-requirements.pdf](requirements/coding-challenge-requirements.pdf) for detailed requirements.

## Architecture

The project follows a layered architecture with the following components:

- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic and interact with repositories.
- **Repositories**: Handle data access and database operations.
- **Models**: Define the data structures and validation rules.
- **Middleware**: Custom middleware for logging HTTP requests and responses.

### Project Structure

```
UserManagementWebApiDemo/
├── .dockerignore
├── .gitattributes
├── .gitignore
├── UserManagement/
│   ├── Controllers/
│   ├── Data/
│   ├── Middleware/
│   ├── Migrations/
│   ├── Models/
│   ├── Properties/
│   ├── Services/
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── Dockerfile
│   ├── Program.cs
│   ├── UserManagement.csproj
├── UserManagement.Tests/
│   ├── Controllers/
│   ├── Services/
│   ├── UserManagement.Tests.csproj
├── WebApiClient/
│   ├── Program.cs
│   ├── WebApiClient.csproj
├── UserManagement.sln
├── requirements/
│   ├── coding-challenge-requirements.pdf
└── demo/
    ├── sample-data.txt
```

## Demo Video

The demo video showcases the User Management Web API and demonstrates the CRUD operations for managing user data.

[Link to General Demo Video](demo/GeneralWalkThru.mp4)

[Link to Unique E-mail Validation Demo Video](demo/UniqueEmailValidationDemo.mp4)

TODO: Add Production Runtime Demo, Create Professional Demo video with voice over. (weekend project)

## Building and Running the Solution

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)
- TODO: Due to time constraints I was not yet able to test and verify Docker deployment.

### Building the Solution

1. Clone the repository:
    ```sh
    git clone https://github.com/kathleenwest/UserManagementWebApiDemo.git
    ```

2. Restore the dependencies and build the solution:
    ```sh
    dotnet restore
    dotnet build
    ```

### Running the Solution

1. Run the application:
    ```sh
    dotnet run --project UserManagement
    ```

2. The API will be available at `https://localhost:7048` and `http://localhost:5108`.


## Database Migrations

### Creating Migrations

1. Add a new migration:
    ```sh
    dotnet ef migrations add <MigrationName> --project UserManagement --startup-project UserManagement
    ```

2. Apply the migration to the database:
    ```sh
    dotnet ef database update --project UserManagement --startup-project UserManagement
    ```

### Deploying the SQLite Database

1. Ensure the connection string in `appsettings.json` points to the SQLite database file:
    ```json
    {
      "ConnectionStrings": {
        "UserDb": "Data Source=UserDb.db"
      }
    }
    ```

2. Run the application to create and apply the migrations:
    ```sh
    dotnet run --project UserManagement
    ```

## Unit Tests

The project includes unit tests for controllers, services, and repository layers using xUnit and Moq.

### Running Unit Tests

1. Navigate to the test project directory:
    ```sh
    cd UserManagement.Tests
    ```

2. Run the tests:
    ```sh
    dotnet test
    ```

### Test Coverage

The unit tests cover various scenarios, including:

- Creating, retrieving, updating, and deleting users.
- Validating email uniqueness.
- Handling errors and logging.
- TODO: Due to time constraints I added ideas for additional tests in the UserManagement.Tests project.

## License

This project is licensed under the MIT License.

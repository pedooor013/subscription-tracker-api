# 🎬 Streaming Tracker API

> REST API for managing users, streaming subscriptions, and subscription categories, built with C# and ASP.NET Core.

![Status](https://img.shields.io/badge/status-in%20development-yellow)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![C%23](https://img.shields.io/badge/C%23-13-239120?logo=csharp)
![SQL%20Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)

## 📌 About the Project

**Streaming Tracker API** is a personal backend project developed to practice and consolidate concepts from the .NET ecosystem and REST API development.

The API provides functionality for managing users, streaming subscriptions, and subscription categories, while applying concepts such as authentication, data persistence, DTOs, service-layer separation, Entity Framework Core, database migrations, and Docker.

The project is **still under development**. The current repository represents the features implemented so far, while additional improvements and features will be added as development continues.

---

## 🚀 Current Features

### 👤 User Management

- User registration
- User authentication
- Search users by ID
- Search users by username
- Search users by email
- Search users by active status
- Update user information
- Update account active status
- Update password
- Delete users

### 🔐 Authentication

- JWT Bearer authentication
- BCrypt password hashing
- JWT claims containing user ID and username
- Token expiration
- Protected API resources using `[Authorize]`
- Anonymous access for registration and login

### 📺 Subscription Management

- Create subscriptions
- List subscriptions
- Find a subscription by ID
- Filter subscriptions by category
- Update subscriptions
- Delete subscriptions
- Associate subscriptions with users
- Associate subscriptions with categories

### 🏷️ Subscription Categories

- Create categories
- List categories
- Find categories by ID
- Find categories by name
- Update categories
- Delete categories
- Prevent duplicate category names during creation

### 🗄️ Database

- SQL Server 2022
- Entity Framework Core
- EF Core migrations
- Relational entity relationships
- Persistent SQL Server data through Docker volumes

### 📖 API Documentation

The project uses ASP.NET Core OpenAPI support to expose the API contract during development.

---

## 🛠️ Technologies

| Technology | Purpose |
| --- | --- |
| C# | Main programming language |
| .NET 10 | Application platform |
| ASP.NET Core | REST API development |
| Entity Framework Core 10 | ORM and data access |
| SQL Server 2022 | Relational database |
| JWT Bearer | Authentication and authorization |
| BCrypt.Net-Next | Password hashing |
| OpenAPI | API documentation |
| Docker Compose | Local SQL Server environment |
| Git & GitHub | Version control |

---

## 🏗️ Architecture

The project follows a layered approach, separating HTTP handling, application logic, data models, and API contracts.

```text
                    Client
                      │
                      ▼
                ┌────────────┐
                │ Controllers│
                └──────┬─────┘
                       │
                       ▼
                ┌────────────┐
                │    DTOs    │
                └──────┬─────┘
                       │
                       ▼
             ┌──────────────────┐
             │ Service Interfaces│
             └────────┬─────────┘
                      │
                      ▼
             ┌──────────────────────┐
             │Service Implementations│
             └────────┬─────────────┘
                      │
                      ▼
             ┌──────────────────┐
             │ Entity Framework │
             │      Core        │
             └────────┬─────────┘
                      │
                      ▼
                ┌────────────┐
                │ SQL Server │
                └────────────┘
Main Responsibilities
Controllers — Handle HTTP requests, responses, routing, and authorization.
DTOs — Define request and response contracts exposed by the API.
Service Interfaces — Define contracts for application services.
Service Implementations — Contain application and business logic.
Models — Represent the application's persisted entities.
Migrations — Track database schema changes using Entity Framework Core.

Service implementations are registered through dependency injection in Program.cs.

📂 Project Structure
StreamingSubscriptionTrackerAPI/
│
├── Controllers/
│   ├── UserController.cs
│   ├── SubscriptionController.cs
│   └── SubscriptionCategoryController.cs
│
├── DTOs/
│   ├── UserRequestDTO.cs
│   ├── UserResponseDTO.cs
│   ├── UserLoginRequestDTO.cs
│   ├── UserLoginResponseDTO.cs
│   ├── UpdatePasswordRequestDTO.cs
│   ├── UpdateActivedRequestDTO.cs
│   ├── SubscriptionRequestDTO.cs
│   ├── SubscriptionResponseDTO.cs
│   ├── SubscriptionCategoryRequestDTO.cs
│   └── SubscriptionCategoryResponseDTO.cs
│
├── Models/
│   ├── User.cs
│   ├── Subscription.cs
│   ├── SubscriptionCategory.cs
│   └── Context/
│
├── Services/
│   ├── IUserService.cs
│   ├── ISubscriptionService.cs
│   ├── ISubscriptionCategoryService.cs
│   │
│   └── Impl/
│       ├── UserServiceImpl.cs
│       ├── SubscriptionServiceImpl.cs
│       └── SubscriptionCategoryServiceImpl.cs
│
├── Migrations/
│
├── Program.cs
├── docker-compose.yml
├── appsettings.json
├── appsettings.example.json
└── StreamingSubscriptionTrackerAPI.csproj
🔐 Authentication Flow

Authentication is implemented using JWT Bearer tokens.

User
 │
 │ POST /api/User/login
 ▼
UserController
 │
 ▼
UserService
 │
 ├── Find user
 │
 ├── Verify password with BCrypt
 │
 └── Generate JWT
        │
        ▼
      Client
        │
        │ Authorization: Bearer <token>
        ▼
  Protected endpoints

During registration, the user's password is hashed using BCrypt before being persisted.

During login, the submitted password is verified against the stored hash.

After successful authentication, the API generates a JWT containing the user's identity claims.

The token is then used to access protected resources through the Authorization header:

Authorization: Bearer <token>
🗃️ Data Model

The main entities of the application are:

┌──────────────┐
│     User     │
└──────┬───────┘
       │
       │ 1:N
       ▼
┌──────────────┐
│ Subscription │
└──────┬───────┘
       │
       │ N:1
       ▼
┌──────────────────────┐
│ SubscriptionCategory │
└──────────────────────┘
User

A user contains information such as:

Username
Email
Password hash
Active status
Creation date
Subscription

A subscription contains:

Name
Price
Payment due date
User reference
Category reference
Subscription Category

A category contains:

Name
User reference

The database schema is managed using Entity Framework Core migrations.

📚 API Endpoints

All endpoints below require JWT authentication unless explicitly marked as public.

👤 Users

Base route:

/api/User
Method	Endpoint	Authentication	Description
GET	/api/User	🔒	Get all users
GET	/api/User/username/{username}	🔒	Get user by username
GET	/api/User/email/{email}	🔒	Get user by email
GET	/api/User/{id}	🔒	Get user by ID
GET	/api/User/actived/{actived}	🔒	Get users by active status
POST	/api/User	🌐 Public	Create a user
POST	/api/User/login	🌐 Public	Authenticate a user and return a JWT
PUT	/api/User/update/{id}	🔒	Update user information
PUT	/api/User/update/actived/{id}	🔒	Update user active status
PUT	/api/User/update/password/{id}	🔒	Update user password
DELETE	/api/User/delete/{id}	🔒	Delete a user
📺 Subscriptions

Base route:

/api/Subscription
Method	Endpoint	Authentication	Description
GET	/api/Subscription	🔒	Get all subscriptions
GET	/api/Subscription/{id}	🔒	Get subscription by ID
GET	/api/Subscription/category/{id}	🔒	Get subscriptions by category
POST	/api/Subscription	🔒	Create a subscription
PUT	/api/Subscription/{id}	🔒	Update a subscription
DELETE	/api/Subscription/{id}	🔒	Delete a subscription
🏷️ Subscription Categories

Base route:

/api/SubscriptionCategory
Method	Endpoint	Authentication	Description
GET	/api/SubscriptionCategory	🔒	Get all categories
GET	/api/SubscriptionCategory/{id}	🔒	Get category by ID
GET	/api/SubscriptionCategory/name/{name}	🔒	Get category by name
POST	/api/SubscriptionCategory	🔒	Create a category
PUT	/api/SubscriptionCategory/{id}	🔒	Update a category
DELETE	/api/SubscriptionCategory/{id}	🔒	Delete a category
🐳 Running the Project
Prerequisites

Make sure you have the following installed:

.NET 10 SDK
Docker Desktop
Git
1. Clone the Repository
git clone https://github.com/pedooor013/streaming-tracker-api.git
cd streaming-tracker-api
2. Configure the Application

The repository provides an example configuration file:

appsettings.example.json

Create your local appsettings.json based on the example and configure:

SQL Server connection string
JWT signing key

Example:

{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY"
  }
}

⚠️ Never commit real credentials, connection strings, or JWT secret keys to the repository.

3. Start SQL Server with Docker

The project includes a Docker Compose configuration for SQL Server 2022.

Run:

docker compose up -d

The SQL Server container uses port 1433 and stores database data in a persistent Docker volume.

To verify that the container is running:

docker ps
4. Apply Entity Framework Migrations

Make sure the database is running, then execute:

dotnet ef database update

This applies the existing Entity Framework Core migrations to the configured SQL Server database.

5. Run the API

Start the application with:

dotnet run

The API uses ASP.NET Core controller routing and HTTPS redirection.

🗄️ Entity Framework Core Migrations

Database schema changes are tracked through Entity Framework Core migrations stored in:

Migrations/

To create a new migration during development:

dotnet ef migrations add <MigrationName>

To apply migrations:

dotnet ef database update
🔄 Dependency Injection

The application uses ASP.NET Core's built-in Dependency Injection system.

Service interfaces are registered in Program.cs and injected into controllers.

Example:

IUserService
      │
      ▼
UserServiceImpl
      │
      ▼
UserController

This approach helps separate responsibilities and makes the application easier to maintain and evolve.

🧠 Concepts Practiced

This project is being developed as a practical way to study and apply backend development concepts, including:

REST API development
Object-Oriented Programming
Dependency Injection
DTOs
Service Layer separation
Entity Framework Core
LINQ
Relational database modeling
Database migrations
JWT authentication
Password hashing
Authorization
Docker
OpenAPI
Git and GitHub
🚧 Roadmap

The project is currently in development.

Planned improvements include:

 Complete remaining application features
 Improve input validation
 Improve global error handling
 Add automated tests
 Expand API documentation with request/response examples
 Review authorization and resource ownership rules
 Improve configuration and secret management
 Refine the project architecture as requirements evolve
 Add additional subscription-related features
📌 Project Status
🟡 In Development

The current implementation provides the core structure of the API, including:

User management
JWT authentication
Password hashing
Subscription management
Subscription categories
SQL Server persistence
Entity Framework Core migrations
DTO-based API contracts
Service-layer separation
Dockerized database environment

The project is actively being developed, and its architecture and features may evolve as new backend concepts are incorporated.

👨‍💻 Author

Pedro Lopes

Software Engineering student focused on backend development with C# and ASP.NET Core.

GitHub

@pedooor013

📄 License

This project is currently intended as a personal learning and portfolio project.

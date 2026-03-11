# Accident Insurance Capstone
An Angular + ASP.NET Core application for comprehensive accident insurance management with secure JWT authentication and multi-role access control.

## Features

### Core Functionality
- **Authentication & Authorization**: JWT-based login/register with roles for Customer, Agent, Claims Officer, and Admin.
- **Policy Management**: Browse available insurance plans, submit policy requests, and manage active policies.
- **Claims Processing**: Submit accident claims with supporting documents and track their status.
- **Payment System**: Securely process premium payments and view EMI schedules/invoices.
- **ChatBot Support**: Integrated chatbot for quick assistance and insurance-related queries.
- **Interactive Tour**: Onboarding tour for new users using Driver.js.
- **Notification System**: Real-time alerts for policy approvals, claim updates, and payment reminders.

### Role-Based Dashboard
- **Customer**: Browse plans, request policies, add nominees, pay EMIs, and file claims.
- **Agent**: Review policy requests, assess risks, calculate premiums, and manage assigned customers.
- **Claims Officer**: Review and process accident claims, verify documents, and approve/reject settlements.
- **Admin**: Full system oversight, user management, policy type configuration, and comprehensive reporting.

## Technology Stack

### Frontend
- **Angular 21**: Latest standalone component architecture.
- **TypeScript**: Strong typing for robust development.
- **NG-Zorro (Ant Design)**: Premium UI component library.
- **Lucide Icons**: Modern, lightweight icon system.
- **Reactive Forms**: Sophisticated data entry with real-time validation.
- **JWT Authentication**: Secure token-based session management.
- **CSS3**: Custom styling
- **DriverJS**: Tour component


### Backend
- **ASP.NET Core 8**: High-performance Web API framework.
- **Entity Framework Core**: Code-first database management.
- **SQL Server**: Reliable relational data storage.
- **JWT Bearer Auth**: Standardized secure API communication.
- **AutoMapper**: Efficient DTO-to-Entity mapping.
- **Semantic Kernel**: Integration of AI Concepts.
- **Swagger / OpenAPI**: Interactive API documentation and testing.



## Demo Video

[![Watch the demo](https://img.youtube.com/vi/Pb3N4qO56VQ/0.jpg)](https://youtu.be/Pb3N4qO56VQ)

## Setup Instructions

### Prerequisites
- **Node.js**: v22 or higher
- **Angular CLI**: v21 or higher
- **.NET SDK**: 8.0 or higher
- **SQL Server**: Microsoft SQL SERVER
### Installation

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd CapstoneAccident
   ```

2. **Backend Setup**(Package Manager Console)
   ```bash
   cd CapstoneBackend
   # Update the connection string in appsettings.json if necessary
   dotnet ef database update
   dotnet run --project CapStone.API
   ```



   *The API will be running on `https://localhost:7147`*

3. **Frontend Setup**
   ```bash
   cd CapstoneFrontend
   npm install
   ng s -o
   ```
   *The application will be accessible at `http://localhost:4200`*

## Project Structure (Frontend)
```text
src/app/
├── components/          # Feature-specific components
│   ├── login/           # Auth components
│   ├── admin-dashboard/ # Admin features
│   ├── agent-dashboard/ # Agent workflow
│   ├── customer-dashboard/ # Customer interactions
│   └── public-page/     # Landing page & chatbot
├── services/            # API integration & business logic
│   ├── auth/            # JWT & Role management
│   ├── policy/          # Insurance operations
│   └── claims/          # Claims processing
├── models/              # TypeScript interfaces/DTOs
├── guards/              # Route protection
└── interceptors/        # HTTP token injection
```
## Backend
CLEAN ARCHITECTURE :
 ```text
 CapstoneBackend/
├── CapStone.API/            # API layer (controllers & middleware)
│   ├── Controllers/         # REST API endpoints (Admin, Agent, Auth, Customer, Claims, ChatBot)
│   └── Middleware/          # Global exception handling
│
├── CapStone.Application/    # Application/business logic layer
│   ├── DTOs/                # Data Transfer Objects for requests & responses
│   ├── Services/            # Service interfaces for business operations
│   ├── Repositories/        # Repository interfaces & Unit of Work
│   ├── Mappings/            # AutoMapper profiles
│   └── Exceptions/          # Custom application exceptions
│
├── CapStone.Domain/         # Core domain models
│   ├── Entities/            # Database entities (User, Policy, Claim, Payment, etc.)
│   └── Enums/               # Enumerations (UserRole, PolicyStatus, ClaimStatus, etc.)
│
├── CapStone.Infrastructure/ # Data access & service implementations
│   ├── Data/                # DbContext & seed data
│   ├── Repositories/        # Repository implementations
│   ├── Services/            # Business service implementations
│   └── Migrations/          # Entity Framework database migrations
│
└── CapStone.Test/           # Unit tests for controllers, services, and repositories
```








<!-- 
## API Configuration
- **Base URL**: `https://localhost:7147`
- **Swagger Documentation**: `https://localhost:7147/swagger`
- **Authentication**: JWT tokens stored in browser storage and injected via `AuthInterceptor`.

## Security Features
- **Route Guards**: Prevents unauthorized access to role-specific dashboards.
- **CSRF Protection**: Standard ASP.NET Core security middleware.
- **Role-Based Authorization**: Fine-grained access control on both Frontend (UI elements) and Backend (API endpoints). -->

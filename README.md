# FullStack - Backend C#/Angular Challenge

Small application developed for Car repair shop authentication and management

## Features

- Generate security JWT token on login
- Retreive all the orders (All users)
- Retreive order details, apply change of status (Admin Role)
- Create new order (Admin Role)
- Calculate tax and discounts (Admin Role)

## Steps to deploy locally with Docker

- Install docker, you can download it from official website. (https://www.docker.com/)
- Install with WSL2 if you are working in windows.
- Clone the repository code to a folder. (git clone https://github.com/edson268-prog/Taller_Challenge.git)
- Open the folder of the proyect and open file ".env.sample" and modify the DB_PASSWORD variable with your database password.
- Open a terminar command software. (Powershell or I recommend Terminal)
- Navigate to root path from the proyect. (Taller_Challenge folder)
- Execute "docker-compose up --build".
- Open docker and make sure all the containers are running.
https://drive.google.com/file/d/1ariF42i4O1lHvjJX3HryBo608U6EqqMx/view?usp=sharing
- Open the browser and go to "http://localhost:4205/". (or the port you set for frontend)
- Start to work.

## Technical Architecture & Decisions

#### Implementation Approach: Domain-Driven Design (DDD):

The system follows a DDD approach to ensure business logic integrity by using Factory Methods in entities like Order to guarantee a valid state upon instantiation, private setters to enforce encapsulation, and distributing business rules (such as subtotal calculations) directly within the domain models to avoid an anemic model.

- **Domain Layer:** This is the core of the system, containing business Entities (Order, User), Enums, and Interfaces. By isolating business rules here, the system remains independent of external frameworks or databases.
- **Application Logic (API Layer):** The API project is organized by functional features (Auth, Orders) using a Command and Query pattern. This allows for granular control over operations and simplifies the integration of specialized endpoints.
- **Infrastructure Layer:** This layer handles external concerns like Data Persistence (AppDbContext), Repositories, and external Service implementations (PricingService). It uses extension methods for a clean dependency injection setup.

#### Technical Trade-offs:

- **Boilerplate vs Scalability:** Implementing DDD requires more initial files and folders compared to a basic CRUD architecture. However, this trade-off was made to ensure the code remains modular, testable, and consistent, directly addressing the evaluation criteria for code quality.
- **In-Memory vs Real Persistence:** While a real SQL Server instance is used , a DatabaseInitializer was implemented to automate the schema creation and data seeding upon container startup, facilitating an immediate experience.

#### Service Separation & Integration:

The solution is modeled to reflect a transition toward a Microservices Architecture, separating new logic from legacy components.

- Service Isolation:

  - **Orders Service:** Built with the modern .NET 9 stack and DDD to handle the new workshop management logic.
  - **Pricing Service (Legacy):** Built as a simplified, single-layer service using traditional Controllers to represent a pre-existing component.

- Integration Strategy:

  - **Communication:** Integration is achieved via Asynchronous RESTful calls from the Orders Service to the Pricing Service using IHttpClientFactory.
  - **Resilience:** To ensure communication reliability between these distributed services, Polly-based retry policies and transient fault handling are implemented in the Infrastructure layer.
  - **Docker Orchestration:** A unified docker-compose.yml manages the lifecycle of both services and the SQL Server, using internal Docker networking to allow services to discover each other by name.
 
#### Cloud Deployment Outline:

To transition this solution from a local Docker environment to a production-ready cloud infrastructure, the following strategy is proposed:

- **Container Orchestration:** Deploy the Orders and Pricing services into Azure Kubernetes Service to ensure high availability, auto-scaling, and self-healing capabilities.
- **Container Registry:** Store and manage Docker images using a private Azure Container Registry, integrating security scanning for every image build.
- **Managed Database:** Replace the containerized SQL Server with a managed service like Azure SQL Database, enabling automated backups, multi-region replication, and high performance.
- **Secrets Management:** Secure sensitive data (connection strings, API keys) using Azure Key Vault or AWS Secrets Manager, ensuring that no credentials are stored in the source code or environment variables.
- **CI/CD Pipeline:** Implement automated pipelines using GitHub Actions or Azure DevOps to run unit tests, build images, and deploy to staging/production environments automatically on every push.
  
## FAQ

#### Does this application requires a login?

Yes, this application has a login functionality; the user is automatically created as a seed in the docker sqlserver container

#### Personal data is requested within the application?

No, the app will not ask you for personal information.

#### Does the application uses a design pattern?

Yes, the project uses some patterns such as Dependency Injection, IOption pattern using a DDD aproach using patterns like CQRS, Repository (Application → Domain ← Infrastructure).

#### Does the application have unit tests?

Yes, the application has unit tests for Auth Service created with xUnit.

#### How much time was spent creating this project?

Since it was a challenge, the project was completed in a matter of two days

#### Are there any planned future updates or new features?

No, no changes are planned at the moment.

## Tech Stack

**Client:** Angular v18 / Docker

**Server:** .NET Core 9 / Entity Framework / Docker

## Test User

Admin User:
- **Username:** eibanez268
- **Password:** exsquaredadmin123

Visitor User:
- **Username:** amonrroy151
- **Password:** iamavisitor456

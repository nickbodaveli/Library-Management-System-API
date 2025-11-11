# Library Management System API
The project is the Backend API for a Library Management System, built on the .NET 9/C# stack, and aims for complete fulfillment of technical requirements by adhering to the principles of Clean Architecture and CQRS/MediatR. The project follows a full implementation of CQRS. PostgreSQL is used for write operations and persistent storage of transactional data, while MongoDB is used for storing optimized Read Models, and Redis for performance caching.

## Technical Stack and Architecture

###Technologies Used

Categor | Technology/Pattern | Purpose
Backend Framework |.NET 9 / C# | Core API Development.
Architecture | Clean Architecture / DDD | Project Layered Structure.
CQRS | MediatR | Handling Commands and Queries.
Main DB | PostgreSQL (EF Core) | Persistent storage of transactional data.
Caching | Redis | High-performance distributed caching mechanism.
NoSQL | MongoDB | Storage of optimized Read Models.

### Architecture Overview
The project is divided into four layers: Domain, Application, Infrastructure, and API, which ensures strict separation of concerns.

## Local Setup Instructions
To run the project locally, the local installation and running of three services is mandatory: PostgreSQL, Redis, and MongoDB.

### Prerequisites
.NET SDK 9.0+

Git

PostgreSQL Server: Local installation and running.

Redis Server: Local installation and running (Default Port: 6379).

MongoDB Server: Local installation and running (Default Port: 27017).














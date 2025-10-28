# Mz.Healthcare

**Mz.Healthcare** is a sample Web API project demonstrating good practices such as:  
- Building RESTful Web APIs using **.NET 9**
- Integrating **Entity Framework Core** for data access
- Using **Serilog** for structured logging
- Writing **unit tests**
- Running the app via **Docker**

---

## How to Run

### Option 1 — Run with Docker (recommended)
The easiest way to start the application is by using **Docker Compose**.

1. Clone the repository and navigate to the project folder:
   
   git clone https://github.com/mesutzincir/Mz.Healthcare.git
   cd Mz.Healthcare
Build and start the container:

run command "docker compose up -d --build"
Once the container is running, open your browser and visit:
--> http://localhost:5001/swagger/index.html
to explore and test the API endpoints using Swagger UI.

Option 2 — Run Locally with Visual Studio
If you’re using Visual Studio:

Open the solution in Visual Studio.

Press Run or Debug (F5) to start the application.

Check the appsettings.json file to verify or adjust the log file path used by Serilog.

## Features
ASP.NET Core 9 Web API

Entity Framework Core (InMemory or SQL Server)

Serilog file-based logging

Swagger / OpenAPI documentation

Unit test examples

Dockerized deployment
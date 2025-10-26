# Job Portal (ASP.NET Core 8, API + MVC)

This project is a complete job management system built with ASP.NET Core 8. It includes both a Web API for backend services and an MVC frontend for user interaction. The goal was to design a clean, modular, and production-ready architecture that demonstrates secure authentication, role-based access, and modern development practices.

---

## Features

### API (Backend)
- ASP.NET Core Web API with JWT-based authentication
- Entity Framework Core (code-first)
- FluentValidation for input validation
- Swagger (OpenAPI) documentation for testing endpoints
- Repository-style structure with clear separation of layers

### MVC (Frontend)
- ASP.NET Core MVC application consuming API endpoints via HttpClient
- Clean and responsive UI built with Bootstrap 5
- Supports recruiter and job seeker workflows
- View, post, and apply for jobs in real time

---

## Tech Stack

- **Backend:** ASP.NET Core Web API (C#)
- **Frontend:** ASP.NET Core MVC (Razor Views)
- **Database:** SQL Server (EF Core Code-First)
- **Authentication:** JWT Tokens
- **Validation:** FluentValidation
- **UI Framework:** Bootstrap 5

---

## Setup Guide

1. Clone the repository:
   ```bash
   git clone https://github.com/Yameen-21/Job-Portal-API.git
   cd Job-Portal-API
   ```

2. Open the solution (`JobPortalApi.sln`) in Visual Studio.

3. Set both **JobPortalApi** and **JobPortalMvc** as startup projects  
   (Right-click solution → Set Startup Projects → Select “Start” for both).

4. Update the connection string in `appsettings.json` under:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "your-sql-connection-string"
   }
   ```

5. Run EF Core migrations:
   ```bash
   Add-Migration Initial
   Update-Database
   ```

6. Press **Ctrl + F5** to run the project.
   - API will open at: `https://localhost:7123/swagger`
   - MVC web app will open at: `https://localhost:5071`

---

## Roles and Capabilities

| Role | Description |
|------|--------------|
| Recruiter | Create and manage job listings |
| Job Seeker | Register, view jobs, and apply online |

---

## Learning Focus

- Building and integrating ASP.NET Core MVC with Web API  
- Implementing authentication and authorization using JWT  
- Designing clean database models with EF Core  
- Handling validation and error responses  
- Consuming API data securely in an MVC client

---

## Author

**Muhammad Yameen**  
Email: [m.yameen.zada@gmail.com](mailto:m.yameen.zada@gmail.com)  
GitHub: [Yameen-21](https://github.com/Yameen-21)

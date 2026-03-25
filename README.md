# TaskPlanner – Project Overview

TaskPlanner is a small full‑stack task management application with user and role management.  
It is built as a **.NET + React** solution and can serve as a reference for modern full‑stack development.

---

## Tech Stack

### Backend

- **Framework**: ASP.NET Core Web API (.NET 10, C# 14)
- **Projects / Layers**:
  - `Service.Api` – HTTP API, controllers
  - `Logic.Administration` – user management, roles, activation mails
  - `Logic.Tasks` – task logic (overview, details, assignment, notifications)
  - `Data.Database` – EF Core DbContext, migrations (MySQL)
  - `Shared.Models.*` – DTOs & configuration models
- **Authentication & Authorization**:
  - JWT bearer tokens
  - Custom `[JwtAuthentication]` attribute
  - Role‑based access control via `UserRoleEnum` (e.g. `Admin`)
- **Infrastructure**:
  - Configuration binding (`JwtTokenModel`, `ApiOptions`, `EmailSettings`, `DefaultAdminUser`)
  - Central service registration in `Core.Api.Bundels.ServiceRegistration`
  - Endpoint timing & statistics (`ApiControllerBase` + `IEndpointStatisticService`)
  - Swagger/OpenAPI with JWT support

### Frontend

- **Framework**: React (Create React App) + TypeScript
- **Routing**: `react-router-dom`
  - Private routes for authenticated areas (dashboard, task & user administration, logs)
  - Public route for account activation
- **API Access**:
  - `useStatelessApi` – low‑level HTTP client wrapping `fetch`
    - Sends `Authentication: Bearer <token>` header with the JWT
  - `useStateFulApiService` – adds `loading`, `error`, `response` state on top
- **Main Pages**:
  - Dashboard
  - Task administration & task details
  - User administration & user details
  - Log page
  - Account activation

---

## Full‑Stack Flow

1. **Login / JWT**
   - User authenticates and receives a JWT from the backend.
   - React stores the token and passes it to API hooks.

2. **Protected API Calls**
   - Hooks add `Authentication: Bearer <token>` to every request.
   - Controllers are protected with `[JwtAuthentication]` and, where required, with a role:
     - e.g. `[JwtAuthentication(UserRole = UserRoleEnum.Admin)]` for admin‑only endpoints.

3. **Business Logic & Persistence**
   - Controllers delegate to services like `UserAdministration` and `TaskService`.
   - Services use unit‑of‑work abstractions (`IUserUnitOfWork`, `ITaskUnitOfWork`) for database access.
   - Certain actions (new user, task re‑assignment) trigger emails via `IEmailClient`.

4. **UI Rendering**
   - React containers call the API hooks and render:
     - Page models (`TaskPageModel`, `UserAdministrationPageDataModel`, …)
     - Detail models (`TaskDetailsPageModel`, `UserModel`, …)
   - Route guards (`PrivateRoute`) rely on `useAuth()` to check whether a token / session is present.

---

## Running the Application (Short)

- The API base URL is configured in `appsettings.json` (`ApiOptions.ApiBaseAddress`).
- The UI runs on `http://localhost:3000` and calls the API using that base address.

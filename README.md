# PulseCheck 📊

**PulseCheck** is a .NET 8 Web API application that enables users to check in once per day for safety tracking and friend status sharing. It follows Clean Architecture and uses modern practices such as MediatR for CQRS, Entity Framework Core, FluentValidation, and AutoMapper.

---

## 🔧 Features

* ✅ User Registration and Login (JWT-secured)
* ✅ Check-in functionality (limited to once per day)
* ✅ Friend system (add/remove friends, view their check-ins)
* ✅ Self check-in history and friend history
* ✅ OperationResult-wrapped responses for consistency
* ✅ Swagger documentation and Postman collection

---

## 📁 Folder Structure (Clean Architecture)

```
PulseCheck/
│
├── API/                        # Presentation layer (controllers, startup)
├── Application/               # Application logic, CQRS, MediatR, validators
│   ├── Commands/
│   ├── Queries/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Common/                # OperationResult, Behaviors
├── Domain/                    # Core domain models (User, CheckIn, Friendship)
├── Infrastructure/            # EF Core context, repositories, auth, seeding
│   ├── Data/
│   ├── Services/
│   └── Seed/
└── PulseCheck.sln             # Solution file
```

---

## 🧰 Technologies Used

* .NET 8 Web API
* Entity Framework Core
* MediatR (CQRS)
* FluentValidation
* AutoMapper
* SQL Server
* Swagger / Swashbuckle
* Postman (for API testing)

---

## ⚙️ Setup Instructions

### 1. Clone the Repository

```bash
https://github.com/your-username/pulsecheck.git
```

### 2. Navigate to the Project Root

```bash
cd PulseCheck
```

### 3. Apply Migrations and Update Database

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API

dotnet ef database update --project Infrastructure --startup-project API
```

### 4. Run the Application

```bash
dotnet run --project API
```

Access Swagger UI at:

```
https://localhost:<port>/swagger
```

---

## 🔹 Example API Endpoints

### Register

`POST /api/User/register`

```json
{
  "firstName": "Alice",
  "lastName": "Doe",
  "email": "alice@example.com",
  "password": "Password123",
  "phone": "1234567890",
  "emergencyContactName": "Bob",
  "emergencyContactPhone": "0987654321"
}
```

### Login

`POST /api/User/login`

```json
{
  "email": "alice@example.com",
  "password": "Password123"
}
```

### Check In

`POST /api/CheckIn/CheckMeIn`

> Requires JWT Authorization header

---

## 🚀 Import Postman Collection

1. Open Postman
2. Click **Import** > **Link** or **File**
3. Paste URL to the exported Swagger/OpenAPI or import the Postman collection JSON file from `/docs` folder.

---

## 👤 Author

**Mohanad Al-Daghestani**
.NET Developer @ NBI Handelsakademin

Feel free to reach out on [LinkedIn](https://www.linkedin.com/in/al-daghestani/) or GitHub if you have questions!

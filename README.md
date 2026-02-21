# 🏦 Bank API — Secure Enterprise Banking REST API

A production-ready **Banking REST API** built using **ASP.NET Core Web API** and **Clean Architecture** principles. This project demonstrates enterprise-level backend development practices including JWT Authentication, Refresh Token Rotation, Rate Limiting, Repository Pattern, and Clean 3-Tier Architecture.

This system simulates real-world banking backend operations with a strong focus on **Security, Scalability, and Maintainability.**

---

## 🚀 Key Features

* **Clean 3-Tier Architecture** (DataAccessLayer + BusinessLayer + ApiLayer)
* **JWT Authentication** with short-lived **Access Tokens (30 minutes)**
* **Secure Refresh Token System** with **Rotation and Revocation**
* **Rate Limiting Protection** against Brute Force and API Abuse
* **Repository Pattern** for clean and maintainable data access
* **Unit of Work Pattern** for transaction consistency
* **DTO Pattern** to protect internal entities
* **Password Hashing** using **BCrypt** (Industry Standard Security)
* **Policy-Based Authorization**
* **Dependency Injection** across the entire application
* **RESTful API Design**

---

## 🧱 Architecture Overview

This project follows **Clean 3-Tier Architecture** used in Enterprise Systems:

1.  **ApiLayer (Presentation Layer):** Handles HTTP requests, Controllers, Middleware, and API Configuration.
2.  **BusinessLayer (Application Layer):** Contains Business Logic, Authentication, Services, DTOs, and Security Logic.
3.  **DataAccessLayer (Infrastructure Layer):** Handles Database Operations using Entity Framework Core, Repositories, and Unit of Work.

---

## 📁 Project Structure

### 🔹 ApiLayer
Responsible for Exposing REST Endpoints, Handling HTTP Requests, and Applying Security Policies.
* **Controllers:** `AuthController.cs`, `AccountsController.cs`, `ApplicationsController.cs`
* **Configuration:** `Program.cs` (Middleware pipeline & Dependency Injection)

### 🔹 BusinessLayer
The core engine of the application containing all business rules and security logic.
* **Modules:** Authentication, Authorization, Security, Services, Tokens, DTOs.
* **Key Services:** `AuthService`, `TokenService`, `RefreshTokenService`, `AccountService`.

### 🔹 DataAccessLayer
Handles database communication and ensures data integrity.
* **Components:** `ApplicationDbContext`, `GenericRepository`, `UserRepository`, `UnitOfWork`.

---

## 🔐 Authentication Flow

Implementing **Secure JWT Authentication** with **Refresh Token Rotation**.

### **Access Token**
* **Type:** Short-Lived JWT
* **Expiration:** 30 minutes
* **Contains:** UserId, Username, Roles, Expiration.

### **Refresh Token**
* **Type:** Secure Long-Lived Token stored in Database.
* **Security:** Implements **Token Rotation** (One-time use) and **Revocation**.

---

## 🚪 Logout Flow
Logout securely revokes the Refresh Token in the database, ensuring the user cannot generate new access tokens once they sign out.

---

## 🛡️ Rate Limiting Protection
Protects the API against Brute Force attacks.
* **Login Endpoint:** 5 Requests per Minute per IP.
* **Global API Limit:** 100 Requests per Minute per IP.
* **Response:** `429 Too Many Requests`.

---

## 🧠 Design Patterns Used

* **Repository Pattern:** For clean data abstraction and loose coupling.
* **Unit of Work Pattern:** Ensures atomic operations and transaction consistency.
* **Dependency Injection:** For scalable and testable architecture.
* **DTO Pattern:** To provide secure API contracts and prevent entity exposure.

---

## ⚙️ Technologies Used

* **Backend:** ASP.NET Core Web API, C#
* **Database:** SQL Server, Entity Framework Core
* **Security:** JWT, Refresh Tokens, BCrypt Hashing, Rate Limiting Middleware
* **Tools:** Swagger / OpenAPI, Visual Studio

---

## ▶️ How to Run the Project

1.  **Clone the repo:**
    ```bash
    git clone [https://github.com/yourusername/BankApi.git](https://github.com/yourusername/BankApi.git)
    ```
2.  **Update Connection String** in `appsettings.json`.
3.  **Update Database:**
    ```bash
    dotnet ef database update
    ```
4.  **Run Project** and navigate to `/swagger`.

---

## 💼 Portfolio Value
This project demonstrates strong knowledge of **ASP.NET Core**, **Enterprise Security**, and **Scalable Backend Design**. It is a **Production-Ready** project suitable for Backend Developer interviews.

---

**✍️ Author**
Backend Developer specializing in ASP.NET Core, REST APIs, and Secure Backend Systems.
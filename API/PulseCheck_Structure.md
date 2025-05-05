# 📁 PulseCheck - Project Structure with Dependency Injection (DI)

## 🧩 Solution Structure
```
PulseCheck.sln
│
├── 📁 API (Presentation Layer)
│   ├── Controllers/
│   │   ├── UserController.cs                --> Depends on IUserService (via DI)
│   │   ├── CheckInController.cs             --> Depends on ICheckInService (via DI)
│   │   ├── FriendshipController.cs          --> Depends on IFriendshipService (via DI)
│   ├── Program.cs                           --> Builds service container & pipeline
│   ├── appsettings.json                     --> Configuration for JWT, DB, etc.
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs   --> Registered in Program.cs
│   ├── Helpers/
│   │   ├── JwtHelper.cs                     --> Used in JWT config setup
│   │   └── SwaggerHelper.cs                 --> Extension method for Swagger setup
│   ├── Extensions/
│   │   └── ApiServiceCollectionExtensions.cs --> Centralized DI for API-specific services
│   └── PulseCheck.API.csproj

├── 📁 Application (Application Layer)
│   ├── DTOs/
│   │   ├── UserDtos/
│   │   │   ├── UserCreateDto.cs
│   │   │   ├── UserDto.cs
│   │   │   ├── LoginDto.cs
│   │   │   └── LoginResponseDto.cs
│   │   ├── CheckInDtos/
│   │   │   ├── CheckInCreateDto.cs
│   │   │   └── CheckInDto.cs
│   │   └── FriendshipDtos/
│   │       └── FriendshipDto.cs
│
│   ├── Common/
│   │   └── OperationResult.cs               --> Shared response type
│
│   ├── Interfaces/
│   │   └── ITokenService.cs                 --> Implemented by TokenService (Infrastructure)
│
│   ├── Behaviors/
│   │   ├── ValidationBehavior.cs            --> Registered as MediatR PipelineBehavior
│   │   └── LoggingBehavior.cs               --> Registered as MediatR PipelineBehavior
│
│   ├── Users/
│   │   ├── Commands/
│   │   │   ├── RegisterUserCommand.cs       --> Handled by MediatR
│   │   │   ├── LoginUserCommand.cs
│   │   ├── Queries/
│   │   │   └── GetUserByIdQuery.cs
│
│   ├── CheckIns/
│   │   ├── Commands/
│   │   │   └── CreateCheckInCommand.cs
│   │   ├── Queries/
│   │   │   └── GetCheckInsByUserQuery.cs
│
│   ├── Friendships/
│   │   ├── Commands/
│   │   │   └── CreateFriendshipCommand.cs
│   │   ├── Queries/
│   │   │   └── GetFriendshipsByUserQuery.cs
│
│   └── PulseCheck.Application.csproj

├── 📁 Domain (Domain Layer)
│   ├── Models/
│   │   ├── User.cs
│   │   ├── CheckIn.cs
│   │   └── Friendship.cs
│   ├── Enums/
│   └── PulseCheck.Domain.csproj

├── 📁 Infrastructure (Infrastructure Layer)
│   ├── Data/
│   │   ├── AppDbContext.cs                  --> Registered in DI (EF Core)
│   │   └── Migrations/
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   │   ├── IUserRepository.cs
│   │   │   ├── ICheckInRepository.cs
│   │   │   └── IFriendshipRepository.cs
│   │   └── Implementations/
│   │       ├── UserRepository.cs            --> Registered as IUserRepository
│   │       ├── CheckInRepository.cs         --> Registered as ICheckInRepository
│   │       └── FriendshipRepository.cs      --> Registered as IFriendshipRepository
│   ├── Services/
│   │   └── TokenService.cs                  --> Registered as ITokenService
│   ├── Helpers/
│   │   └── JwtSettings.cs                   --> Used in JWT binding
│   ├── Extensions/
│   │   └── InfrastructureServiceCollectionExtensions.cs --> Registers all infra services
│   └── PulseCheck.Infrastructure.csproj
```

---

## 📌 Summary

- ✅ All dependencies are injected using **constructor injection**
- ✅ Application uses **MediatR** for CQRS (registered in Application DI)
- ✅ Infrastructure handles **EF Core, JWT, and repositories**
- ✅ API handles controllers, middleware, and app entrypoint

---

> Each folder follows Clean Architecture separation and injects dependencies using DI for flexibility and testability.
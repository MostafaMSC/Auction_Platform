
┌─────────────────────────────┐
│   Presentation Layer (API)  │
│ • Controllers (Users, Auctions, Bids)│
│ • Middleware (Auth, Access) │
│ • wwwroot/uploads           │
└───────────────▲─────────────┘
                │ Depends on
┌───────────────┴─────────────────┐
│     Application Layer           │
│ • Commands / Queries (CQRS)     │
│ • DTOs (Data Transfer Objects)  │
│ • Interfaces (to Infrastructure)│
│ • Validation / EventHandlers    │
└───────────────▲─────────────────┘
                │ Depends on
┌───────────────┴─────────────────────┐
│       Domain Layer                  │
│ • Entities (Auction, User, Bid)     │
│ • ValueObjects (Money)              │
│ • Domain Events                     │
│ • Repository Interfaces             │
│ • Constants & Exceptions            │
└───────────────▲─────────────────────┘
                │ Implemented by
┌───────────────┴────────────────────┐
│   Infrastructure Layer             │
│• Repositories (AuctionRepo, etc.)  │
│• Services (Auth, FileStorage)      │
│• DB Context (ApplicationDbContext) │
│• Identity & JWT Management         │
└────────────────────────────────────┘

منطق العمل الخاص بالتطبيق، يشمل: Application Layer
Commands / Queries: تنفيذ العمليات (CQRS pattern)
DTOs: نقل البيانات بين الطبقات
Interfaces: عقود الخدمات التي تنفذها الطبقة التحتية
Validation: التحقق من صحة البيانات قبل المعالجة
├── Application/
│   ├── Abstraction/ واجهات الخدمات المشتركة والتحقق من البيانات
│   │   ├── IAuthService.cs
│   │   └── ValidationBehavior.cs
│   ├── Commands/    أوامر CQRS (تنفيذ العمليات)
│   │   ├── Auctions/أوامر المزادات
│   │   │   ├── CloseAuctionCommand.cs
│   │   │   ├── CreateAuctionCommand.cs
│   │   │   ├── StartAuctionCommand.cs
│   │   │   └── UpdateAuctionCommand.cs
│   │   ├── Auth/   أوامر تسجيل الدخول/الخروج/تغيير كلمة السر
│   │   │   ├── ChangePasswordCommand.cs
│   │   │   ├── DeleteUserCommand.cs
│   │   │   ├── LoginUserCommand.cs
│   │   │   ├── LogoutCommand.cs
│   │   │   ├── RefreshTokenCommand.cs
│   │   │   ├── RegisterUserCommand.cs
│   │   │   └── VerifyUserCommand.cs
│   │   ├── Bids/   أوامر المزايدات
│   │   │   ├── ChooseWinnerCommand.cs
│   │   │   └── PlaceBidCommand.cs
│   │   ├── Categories/ أوامر الاصناف
│   │   │   ├── CreateCategoryCommand.cs
│   │   │   └── DeleteCategoryCommand.cs
│   │   ├── Notification/   إنشاء الإشعارات
│   │   │   └── CreateNotificationCommand.cs
│   │   ├── Projects/   أوامر المشاريع
│   │   │   ├── CreateProjectCommand.cs
│   │   │   ├── DeleteProjectCommand.cs
│   │   │   └── SubmitProjectCommand.cs
│   │   └── Users/    تحديث الملف الشخصي للمستخدم
│   │       ├── UpdateProfileCommand.cs
│   │       └── UpdateProfileResult.cs
│   ├── DTOs/    نقل البيانات بين الطبقات (Data Transfer Objects)
│   │   ├── AuctionDto.cs
│   │   ├── BidDto.cs
│   │   ├── CategoryDto.cs
│   │   ├── NotificationDto.cs
│   │   ├── ProjectDto.cs
│   │   ├── RegisterUserDto.cs
│   │   ├── ResultDto.cs
│   │   ├── RoleDto.cs
│   │   ├── UpdateAuctionDto.cs
│   │   └── UserDto.cs
│   ├── EventHandler/   معالجات الأحداث (Domain Events)
│   ├── Extensions/  امتدادات مساعدة (Mapping)
│   │   └── AuctionMappingExtensions.cs
│   ├── Handlers/ معالجات الأوامر والاستعلامات
│   │   ├── AuctionHandler/
│   │   │   ├── CloseAuctionCommandHandler.cs
│   │   │   ├── CreateAuctionCommandHandler.cs
│   │   │   ├── GetAllAuctionsQueryHandlers.cs
│   │   │   ├── GetAuctionByIdQueryHandler.cs
│   │   │   ├── GetAuctionsByStatusQueryHandler.cs
│   │   │   ├── PlaceBidHandler.cs
│   │   │   ├── StartAuctionCommandHandler.cs
│   │   │   └── UpdateAuctionCommandHandler.cs
│   │   ├── Bids/
│   │   │   ├── ChooseWinnerCommandHandler.cs
│   │   │   ├── GetBidsQueryHandler.cs
│   │   │   └── GetMyBidsQueryHandler.cs
│   │   ├── Categories/
│   │   │   ├── CreateCategoryCommandHandler.cs
│   │   │   └── DeleteCategoryCommandHandler.cs
│   │   ├── Notification/
│   │   │   ├── CreateNotificationCommandHandler.cs
│   │   │   └── GetUserNotificationsQueryHandler.cs
│   │   ├── Projects/
│   │   │   ├── CreateProjectCommandHandler.cs
│   │   │   ├── DeleteProjectCommandHandler.cs
│   │   │   ├── GetAllProjectsQueryHandler.cs
│   │   │   ├── GetProjectByIdQueryHandler.cs
│   │   │   ├── GetProjectByStatusQueryHandler.cs
│   │   │   └── SubmitProjectCommandHandler.cs
│   │   └── Users/
│   │       ├── DeleteUserCommandHandler.cs
│   │       ├── GetAllUsersQueryHandler.cs
│   │       ├── GetUserProfileQueryHandler.cs
│   │       ├── GetVerificationStatusQueryHandler.cs
│   │       ├── LoginUserCommandHandler.cs
│   │       ├── LogoutUserCommandHandler.cs
│   │       ├── RefreshTokenCommandHandler.cs
│   │       ├── RegisterUserCommandHandler.cs
│   │       ├── UpdateProfileCommandHandler.cs
│   │       └── VerifyUserCommandHandler.cs
│   ├── Interfaces/  واجهات الخدمات التي تنفذها Infrastructure
│   │   ├── IFileStorageService.cs
│   │   ├── IJwtTokenService.cs
│   │   ├── INotificationService.cs
│   │   ├── IUserAuthenticationService.cs
│   │   └── IUserRegistrationService.cs
│   ├── Queries/   استعلامات CQRS
│   │   ├── Auctions/
│   │   │   ├── GetAllAuctionsQuery.cs
│   │   │   ├── GetAuctionByIdQuery.cs
│   │   │   └── GetAuctionsByStatusQuery.cs
│   │   ├── Bids/
│   │   │   ├── GetBidsQuery.cs
│   │   │   └── GetMyBidsQuery.cs
│   │   ├── Categories/
│   │   │   └── GetAllCatgegoriesQuery.cs
│   │   ├── Notification/
│   │   │   └── GetUserNotificationsQuery.cs
│   │   ├── Projects/
│   │   │   ├── GetAllProjectsCommand.cs
│   │   │   ├── GetProjectByIdQuery.cs
│   │   │   └── GetProjectByStateQuery.cs
│   │   └── Users/
│   │       ├── GetAllUsersQuery.cs
│   │       ├── GetUserProfileQuery.cs
│   │       └── GetVerificationStatusQuery.cs
│   ├── Validation/     Validators للأوامر والاستعلامات
│   │   ├── CreateAuctionCommandValidator.cs
│   │   ├── CreateBidCommandValidator.cs
│   │   ├── CreateProjectCommandValidator.cs
│   │   ├── GetUserProfileQueryValidator.cs
│   │   ├── UpdateProfileCommandValidator.cs
│   │   └── VerifyUserRequestValidator.cs
│   ├── DependencyInjection.cs
│   └── application.csproj

Infrastructure Layer
تنفيذ الخدمات الخارجية والتخزين، يشمل:
Repositories → تنفيذ واجهات الـ Repository الخاصة بالـ Domain Layer
Services → مثل LocalFileStorageService, NotificationService, JwtTokenService
Identity → إدارة المستخدمين والـ JWT
DbContext → ApplicationDbContext وتعريف العلاقات والجداول
DependencyInjection → إضافة كل الخدمات والـ Repositories إلى DI
ملاحظات:

تعتمد على Domain Layer لتطبيق الـ Interfaces
Presentation وApplication تستخدم هذه الخدمات عبر الـ DI

├── Infrastructure/
│   ├── Authorizations/    الخدمات المرتبطة بالهوية والتوثيق
│   │   ├── ClaimsTransformer.cs
│   │   ├── UserAuthenticationService.cs
│   │   └── UserRegistrationService.cs
│   ├── BackgrounService/   خدمات الخلفية مثل AuctionBackgroundService
│   │   └── AuctionBackgroundService.cs
│   ├── Configuration/
│   │   └── JwtSettings.cs
│   ├── Data/     ApplicationDbContext وتهيئة قاعدة البيانات
│   │   ├── ApplicationDbContext.cs
│   │   ├── DatabaseConfiguration.cs
│   │   └── SeedData.cs
│   ├── Exceptions/   استثناءات خاصة بالبنية التحتية
│   │   └── InfrastructureException.cs
│   ├── Extensions/    امتدادات مساعدة (Claims etc)
│   │   └── ClaimIdentitiyExtensions.cs
│   ├── Repositories/      تنفيذ واجهات Repository من Domain
│   │   ├── AuctionRepository.cs
│   │   ├── BidRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── JwtTokenService.cs
│   │   ├── NotificationRepository.cs
│   │   ├── ProjectRepository.cs
│   │   ├── UserRepository.cs
│   │   └── VerficationDocumentRepository.cs
│   ├── Services/    خدمات عامة (AuthService, FileStorageService)
│   │   ├── AuthService.cs
│   │   ├── DependencyInjection.cs
│   │   ├── FileStorageService.cs
│   │   ├── NotificationService.cs
│   │   └── RoleService.cs
│   ├── ApplicationDbContextFactory.cs
│   └── infrastructure.csproj

 واجهة المستخدم (API Layer)
واجهة المستخدم (API) التي يتفاعل معها العميل مثل React أو Postman.
المحتوى:
Controllers/ → جميع الـ Controllers الخاصة بالـ API (UsersController, AuctionsController…)
Program.cs → إعداد الخدمات، الـ Middleware، CORS، Swagger، JWT Authentication
SeedData.cs → لتهيئة البيانات الأولية مثل Roles وUsers
ملاحظات:
لا تحتوي على أي منطق عمل (Business Logic)، فقط تستدعي الخدمات من Application Layer.

├── Presentation/
│   ├── Controller/  Controllers للـ API
│   │   ├── AccessController.cs  الحصول على الادوار والصلاحيات
│   │   ├── AuctionsController.cs  انشاء وتعديل وغلق المزادات
│   │   ├── AuthController.cs تسجيل دخول المستخدمين وا Refresh Token
│   │   ├── BidController.cs   اضافة وتحميل العروض
│   │   ├── CategoryController.cs    التعامل مع الاصناف المختلفة للمشروع
│   │   ├── NotificationsController.cs  اضافة الاشعارات للمستخدمين
│   │   ├── ProjectsController.cs   اضافة وتقديم المشاريع للمزادات
│   │   └── UsersController.cs    توثيق المستخدمين وعرض الملف الشخصي
│   ├── Middleware/     Middleware مثل AuctionAccessMiddleware
│   │   └── AuctionAccessMiddleware.cs  منع المستخدمين غير الموثقين من استخدام النظام
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── wwwroot/   ملفات ثابتة ورفع الملفات (uploads)
│   │   └── uploads/ الملفات المرفوعة للتوثيق تخزن هنا
│   ├── Program.cs   إعداد الخدمات، Middleware، JWT، Swagger، CORS, Background Service
│   ├── appsettings.Development.json  إعدادات خاصة ببيئة التطوير
│   └── appsettings.json إعدادات عامة للتطبيق، تستخدم كقاعدة لجميع البيئات  

Domain Layer
قلب المشروع ومنطق الأعمال الأساسي، يشمل:
Entities → الكيانات الأساسية مثل Auction, Bid, Project, User
ValueObjects → كائنات لا تتغير (مثل Money)
Events → Domain Events لإشعارات التغييرات
Repositories (Interfaces) → واجهات للتعامل مع قاعدة البيانات
Constants / Exceptions → الثوابت واستثناءات النطاق
ملاحظات:
هذه الطبقة لا تعتمد على أي شيء خارجي، مستقلة تمامًا
كل منطق عمل أساسي (مثل PlaceBid، StartAuction) موجود هنا

├── domain/
│   ├── Abstractions/    كائنات أساسية Base Entities
│   │   ├── Entity.cs
│   │   └── SoftDeleteableEntity.cs   الحذف الافتراضي للبيانات المهمة
│   ├── Constants/  الثوابت مثل AuctionStatus, RolePermissions
│   │   ├── AccountType.cs  Seller , Buyer , Admin
│   │   ├── AuctionStatus.cs
│   │   ├── BidStatus.cs
│   │   ├── ProjectStatus.cs
│   │   ├── RolePermissions.cs
│   │   └── VerificationStatus.cs   Pending , Approved , Rejected
│   ├── Entities/     الكيانات الأساسية (Auction, Bid, User …)
│   │   ├── Auction.cs  كلاس خاص بكل ما يتعلق بالمزادات
│   │   ├── Bids.cs    العروض على المزاد
│   │   ├── Categories.cs
│   │   ├── Notification.cs
│   │   ├── Project.cs
│   │   ├── RefreshToken.cs
│   │   ├── User.cs
│   │   └── VerificationDocs.cs

تُستخدم هذه الأحداث لإعلام النظام عن تغييرات مهمة داخل الـ Domain، بحيث يمكن للـ Handlers أو الـ Services التعامل معها بشكل منفصل
│   ├── Events/    ← أحداث الـ Domain (Domain Events) التي تمثل تغييرات مهمة داخل النظام ويجب إعلام أجزاء أخرى بها
│   │   ├── AuctionEvents/   ← أحداث مرتبطة بالمزادات مثل إنشاء المزاد أو إغلاقه أو إضافة عرض
│   │   │   ├── AuctionCloasedEvent.cs      ← حدث إغلاق المزاد
│   │   │   ├── AuctionCreatedEvent.cs      ← حدث إنشاء مزاد جديد
│   │   │   └── BidPlacedEvents.cs          ← حدث تقديم عرض (Bid) على المزاد
│   │   ├── ProjectEvents/   ← أحداث مرتبطة بالمشاريع
│   │   │   ├── ProjectApprovedEvent.cs     ← حدث الموافقة على المشروع
│   │   │   ├── ProjectCreatedEvents.cs     ← حدث إنشاء مشروع جديد
│   │   │   └── ProjectSubmitedEvent.cs     ← حدث تقديم المشروع (Submission)
│   │   ├── UserEvents/      ← أحداث مرتبطة بالمستخدمين
│   │   │   ├── UserRejectedEvent.cs        ← حدث رفض المستخدم
│   │   │   └── UserVerifiedEvent.cs        ← حدث التحقق من المستخدم
│   │   ├── IDomainEvent.cs    ← الواجهة الأساسية لجميع أحداث الـ Domain
│   │   └── NotificationCreatedEvent.cs    ← حدث إنشاء إشعار داخل النظام

│   ├── Exceptions/    استثناءات Domain خاصة بالمنطق
│   │   ├── DomainException.cs
│   │   ├── ForbidException.cs
│   │   ├── InvalidException.cs
│   │   └── NotFoundException.cs
│   ├── Repositories/    واجهات Repository للتعامل مع البيانات
│   │   ├── IAuctionRepository.cs
│   │   ├── IBidsRepository.cs
│   │   ├── ICategoryReoistory.cs
│   │   ├── INotificationRepository.cs
│   │   ├── IProjectRepository.cs
│   │   ├── IUserRepositoy.cs
│   │   └── IVerficationDocRepository.cs
│   ├── ValueObject/    ValueObjects مثل Money
│   │   └── Money.cs
│   ├── AuctionSystem.sln
│   └── domain.csproj
└── AuctionSystem.sln

العلاقات بين الطبقات
Presentation Layer  ---> يعتمد على ---> Application Layer ---> يعتمد على ---> Domain Layer
Application Layer  ---> يعتمد على ---> Infrastructure Layer (عبر Interfaces)
Domain Layer  ---> مستقل تماماً

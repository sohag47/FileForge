# FileForge

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) installed
- SQL Server or any compatible database
- IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))

### Database CMD
1. Create Migration:
   ```bash
   Add-Migration InitialCreate
   ```
2. Database Migrate:
```bash
Update-Database
```
3. DB Connection String
```bash
Server=localhost\\SQLEXPRESS;Database=cms_backend_db;Trusted_Connection=True;TrustServerCertificate=True;
```
4. Server name
```bash
.\SQLEXPRESS 
```

### Remove Database
```bash
Drop-Database
```
### Remove Migrations
```bash
Remove-Migration
```
---


# 📦 Tools & Packages
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="BCrypt.Net-Next" Version="4.1.0" />
    <PackageReference Include="FluentValidation" Version="12.1.1" />
    <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.1.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.7" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.7" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.7" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.7">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.7" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.7">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="SharpGrip.FluentValidation.AutoValidation.Mvc" Version="2.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.1.7" />
  </ItemGroup>

  <ItemGroup>
    <Folder Include="Migrations\" />
  </ItemGroup>

</Project>

```

### generate a secure key for jwt  
```bash
# for Windows
[Convert]::ToBase64String((1..32 | ForEach-Object { Get-Random -Maximum 256 }))

# for Linux/macOS/Git Bash
using System.Security.Cryptography;
var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
Console.WriteLine(key);

# run final command
openssl rand -base64 32
```

---
# 🚀 Suggested Roadmap

### ✅ Basic 
- [x] Create api using `app.MapGet` `app.MapPost` `app.MapPut` `app.MapDelete` in `Program.cs`
- [x] Serve static files from `wwwroot`
- [x] Create a controller and set api end pointes
- [x] Implement CURD Operation 
- [x] Bulk Create & Update
- [x] Image Uploads
- [ ] Relationships `OneToOne` `ManytoMnay` `OneToMany` `ManyToOne`


### ✅ Mid Level
- [x] Add DTO for `request` & `response`
- [x] Add FluentValidation with DTO
- [x] Add Custom Error Response
- [x] Setup Standard and central API Response
- [x] Setup Service layer for implement Database Logics
- [x] Add `AddBusinessServices` to hold all the services
- [x] Add `AddValidationConfigurations` to hold all the validator rules
- [x] Registration, Login, Logout with JWT
- [x] Manage Role based authorization
- [ ] Change Password
- [ ] Forget Password



using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyTeProject.BackEnd.Entities;
using MyTeProject.BackEnd.Entities.Admin;
using MyTeProject.BackEnd.Entities.ExpenseEntities;
using MyTeProject.BackEnd.Entities.User;
using MyTeProject.BackEnd.Entities.WBSEntities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Entity Framework
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole<int>>(config =>
    {
        config.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(10);
    options.SlidingExpiration = true;
});

// Roles Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequiredManagerRole", policy => policy.RequireRole("Admin", "Manager"));
});

// Swagger with Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please, inform a JWT Token in Bearer format {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

// Cross Domain
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Role and User creation
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var dbContext = services.GetRequiredService<AppDbContext>();


    await SeedRolesAsync(roleManager);
    await SeedWBSTypes(dbContext);
    await SeedWBS(dbContext);
    await SeedExpenseType(dbContext);
    await SeedHiringRegime(dbContext);
    await SeedDepartment(dbContext);
    await SeedLocation(dbContext);
    await SeedUsersAsync(userManager, dbContext);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cross Domain Definition
app.UseCors("Cors");

// Authorization and Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// Creating Roles and users, if they don't exist
static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
{
    var roles = new[] { "Admin", "Manager", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));
    }
}

static async Task SeedWBSTypes(AppDbContext dbContext)
{
    var wbsTypes = new[] { new WBSType { Description = "Chargeable" }, new WBSType { Description = "Non-Chargeable" } };
    foreach (var type in wbsTypes)
    {
        if (!dbContext.WBSType.Any(e => e.Description == type.Description))
            dbContext.WBSType.Add(type);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedWBS(AppDbContext dbContext)
{
    var chargeable = await dbContext.WBSType.Where(e => e.Description == "Chargeable").FirstOrDefaultAsync();

    var nonChargeable = await dbContext.WBSType.Where(e => e.Description == "Non-Chargeable").FirstOrDefaultAsync();

    var wbsEntities = new[] {
        new WBS {
            ChargeCode = "900X00",
            Description = "Vacation",
            WBSType = nonChargeable
        },
        new WBS {
            ChargeCode = "A991005",
            Description = "Unassigned Time",
            WBSType = nonChargeable
        },
        new WBS {
            ChargeCode = "957X05",
            Description = "Day-Off",
            WBSType = nonChargeable
        },
        new WBS {
            ChargeCode = "103021A",
            Description = "Implementation and Delevopment",
            WBSType = chargeable
        },
    };
    foreach (var wbs in wbsEntities)
    {
        if (!dbContext.WBS.Any(e => e.ChargeCode == wbs.ChargeCode))
            dbContext.WBS.Add(wbs);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedExpenseType(AppDbContext dbContext)
{
    var expenseTypes = new[] {
        new ExpenseType {
            Description = "Alimentation"
        },
        new ExpenseType {
            Description = "Transportation"
        }
    };
    foreach (var type in expenseTypes)
    {
        if (!dbContext.ExpenseType.Any(e => e.Description == type.Description))
            dbContext.ExpenseType.Add(type);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedHiringRegime(AppDbContext dbContext)
{
    var hiringRegimes = new[] {
        new HiringRegime
        {
            Description = "CLT" ,
            AcceptOvertime = true,
            WorkSchedule = 8
        },
        new HiringRegime
        {
            Description = "Intern",
            AcceptOvertime = false,
            WorkSchedule = 6
        }
    };
    foreach (var hiringRegime in hiringRegimes)
    {
        if (!dbContext.HiringRegime.Any(e => e.Description == hiringRegime.Description))
            dbContext.HiringRegime.Add(hiringRegime);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedDepartment(AppDbContext dbContext)
{
    var departments = new[] {
        new Department
        {
            Name = "IT",
            ContactEmail = "it@avanade.com"
        },
        new Department
        {
            Name = "HR",
            ContactEmail = "hr@avanade.com"
        },
        new Department
        {
            Name = "Financial",
            ContactEmail = "financial@avanade.com"
        }
    };
    foreach (var department in departments)
    {
        if (!dbContext.Department.Any(e => e.Name == department.Name))
            dbContext.Department.Add(department);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedLocation(AppDbContext dbContext)
{
    var locations = new[]
    {
        new Location
        {
            State = "PE",
            City = "Recife"
        },
        new Location
        {
            State = "SP",
            City = "São Paulo"
        }
    };
    foreach (var location in locations)
    {
        if (!dbContext.Location.Any(e => e.State == location.State && e.City == location.City))
            dbContext.Location.Add(location);
    }
    await dbContext.SaveChangesAsync();
}

static async Task SeedUsersAsync(UserManager<AppUser> userManager, AppDbContext dbContext)
{
    HiringRegime? clt = await dbContext.HiringRegime.Where(e => e.Description == "CLT").FirstOrDefaultAsync();
    HiringRegime? intern = await dbContext.HiringRegime.Where(e => e.Description == "Intern").FirstOrDefaultAsync();


    await CreateUser(userManager, dbContext, "Admin", "admin@avanade.com", "Admin", clt, "Admin123!");
    await CreateUser(userManager, dbContext, "Manager", "manager@avanade.com", "Manager", clt, "Manager123!");
    await CreateUser(userManager, dbContext, "User", "user@avanade.com", "User", clt, "User123!");
    await CreateUser(userManager, dbContext, "Intern", "intern@avanade.com", "User", intern, "Intern123!");

}

static async Task CreateUser(UserManager<AppUser> userManager, AppDbContext dbContext, string name, string email, string role, HiringRegime hiringRegime, string password)
{
    Department? it = await dbContext.Department.Where(e => e.Name == "IT").FirstOrDefaultAsync();
    Location? sp = await dbContext.Location.Where(e => e.State == "SP" && e.City == "São Paulo").FirstOrDefaultAsync();
    var adminUser = new AppUser
    {
        UserName = name,
        Email = email,
        HiringRegime = hiringRegime,
        Department = it,
        Location = sp,
        Active = true,
        AdmissionDate = new DateOnly(2024, 1, 1),
    };

    if (userManager.Users.Any(u => u.Email == adminUser.Email))
        return;

    var userInsertResult = await userManager.CreateAsync(adminUser, password);

    if (userInsertResult.Succeeded)
    {
        var roleInsertResult = await userManager.AddToRoleAsync(adminUser, role);

        if (!roleInsertResult.Succeeded)
        {
            Console.WriteLine("Erro ao adicionar o usuário Admin à role Admin: " + string.Join(", ", userInsertResult.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        Console.WriteLine("Erro ao adicionar o usuário Admin: " + string.Join(", ", userInsertResult.Errors.Select(e => e.Description)));
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Login.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Connection string for the database
var connectionString = builder.Configuration.GetConnectionString("ApplicationUserDbContextConnection")
                       ?? throw new InvalidOperationException("Connection string 'ApplicationUserDbContextConnection' not found.");

// Register the database context for ApplicationUserDbContext
builder.Services.AddDbContext<ApplicationUserDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register Identity services (User and Role)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Disable email confirmation for login
})
.AddRoles<IdentityRole>() // Enable role support
.AddEntityFrameworkStores<ApplicationUserDbContext>();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and users in the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Define roles
        string[] roleNames = { "Admin", "Student", "Faculty" };

        foreach (var roleName in roleNames)
        {
            // Check if the role exists, if not, create it
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin user
        await SeedUser(userManager, "admin10@gmail.com", "Admin10%", "Admin");

        // Seed Faculty user
        await SeedUser(userManager, "faculty@example.com", "Faculty@123", "Faculty");

        // Seed Student user
        await SeedUser(userManager, "student@example.com", "Student@123", "Student");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during role/user seeding: {ex.Message}");
    }
}

// Helper method to seed users
static async Task SeedUser(UserManager<ApplicationUser> userManager, string email, string password, string role)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true // Confirm email to bypass email confirmation
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
            Console.WriteLine($"User '{email}' added to role '{role}'.");
        }
        else
        {
            Console.WriteLine($"Error creating user '{email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
    else
    {
        Console.WriteLine($"User '{email}' already exists.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

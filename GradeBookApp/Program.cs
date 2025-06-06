using GradeBookApp.Components;
using GradeBookApp.Components.Account;
using GradeBookApp.Data;
using GradeBookApp.Data.Entities;
using GradeBookApp.Data.Seed;
using GradeBookApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// === 0. Konfiguracja plików JSON + środowiskowego
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// === 1. Rejestracja sekcji DatabaseSettings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// === 2. Fabryka DbContext
builder.Services.AddDbContextFactory<ApplicationDbContext>((sp, options) =>
{
    var monitor = sp.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();
    bool useBackup = monitor.CurrentValue.UseBackup;
    Console.WriteLine($"[DbContextFactory BEFORE] UseBackup = {useBackup}");

    var config = sp.GetRequiredService<IConfiguration>();
    string connString = useBackup
        ? config.GetConnectionString("Backup")
        : config.GetConnectionString("Primary");

    Console.WriteLine($"[DbContextFactory] Łączę się z bazą: {(useBackup ? "Backup" : "Primary")} ({connString})");
    options.UseNpgsql(connString);
});

// === 3. ApplicationDbContext tylko przez fabrykę
builder.Services.AddScoped<ApplicationDbContext>(sp =>
    sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

// === 4. Identity (cookie-based auth)
//     Używamy klasy ApplicationUser (dziedziczy po IdentityUser)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.AllowedForNewUsers = false;
    options.Tokens.AuthenticatorTokenProvider = null;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// === (opcjonalnie) konfiguracja ścieżek cookie auth
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// === 5. Rejestracja pozostałych serwisów (po konfiguracji Identity)
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<SubjectService>();           // SubjectService wstrzykuje UserManager<ApplicationUser>
builder.Services.AddScoped<TeacherSubjectService>();
builder.Services.AddScoped<StudentClassService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StudentDataService>();
builder.Services.AddScoped<TeacherActionsService>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// === 6. Blazor + MVC + AuthenticationState
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddHttpContextAccessor();

// === 7. HttpClient z cookies (jeśli wysyłasz żądania lokalnie)
builder.Services.AddScoped<HttpClient>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var handler = new HttpClientHandler();

    if (httpContextAccessor.HttpContext?.Request?.Cookies != null)
    {
        handler.CookieContainer = new System.Net.CookieContainer();

        foreach (var cookie in httpContextAccessor.HttpContext.Request.Cookies)
        {
            handler.CookieContainer.Add(new Uri("https://localhost:7264"), new System.Net.Cookie(cookie.Key, cookie.Value));
        }

        handler.UseCookies = true;
    }

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:7264") // dostosuj port do swojego projektu
    };
});

var app = builder.Build();

// === 8. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // musi być przed UseAuthorization()
app.UseAuthorization();

app.UseAntiforgery();

// Mapowanie endpointów
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapAdditionalIdentityEndpoints();

// === 9. Migracje + seedy
using (var scope = app.Services.CreateScope())
{
    var ctxFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    await using var dbContext = ctxFactory.CreateDbContext();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    Console.WriteLine("[Program] Migrate (UNCONDITIONAL)");
    await dbContext.Database.MigrateAsync();

    Console.WriteLine("[Program] DbSeeder.SeedAsync (UNCONDITIONAL) start");
    await DbSeeder.SeedAsync(dbContext, userManager, roleManager);
    Console.WriteLine("[Program] DbSeeder.SeedAsync (UNCONDITIONAL) end");
}

// === 9. Migracje + Seedy (Z CZYSZCZENIEM BAZY)
// using (var scope = app.Services.CreateScope())
// {
//     var ctxFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
//     await using var dbContext = ctxFactory.CreateDbContext();
//
//     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//
//     Console.WriteLine("[Program] Czyszczenie bazy danych...");
//     await dbContext.Database.EnsureDeletedAsync(); // <--- CZYŚCIMY
//     Console.WriteLine("[Program] Baza usunięta.");
//
//     Console.WriteLine("[Program] Wykonywanie migracji...");
//     await dbContext.Database.MigrateAsync();
//     Console.WriteLine("[Program] Migracje zastosowane.");
//
//     Console.WriteLine("[Program] Seed danych (start)...");
//     await DbSeeder.SeedAsync(dbContext, userManager, roleManager);
//     Console.WriteLine("[Program] Seed danych (koniec).");
// }


Console.WriteLine("[Program] Uruchamiam aplikację");
app.Run();

public class DatabaseSettings
{
    public bool UseBackup { get; set; }
}

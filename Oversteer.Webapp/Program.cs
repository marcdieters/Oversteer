using BlazorTable;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;
using Oversteer.Models;
using Oversteer.Webapp;
using Oversteer.Webapp.Areas.Identity;
using Oversteer.Webapp.Data;
using Oversteer.Webapp.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Blazored.LocalStorage;
using Serilog;
using Serilog.Events;
using static Org.BouncyCastle.Math.EC.ECCurve;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Logging
var path = builder.Configuration.GetValue<string>("Logging:FilePath");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(path)
    .CreateLogger();

var url = builder.Configuration.GetValue<string>("oversteer:url");

builder.Services.AddScoped<HttpClient>(s =>
{
    return new HttpClient { BaseAddress = new Uri(url) };
});

var ApiOrigin = "Open";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ApiOrigin, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString), ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddServerSideBlazor();
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredLocalStorage();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
    options.LoginPath = new PathString("/Account");
    options.ReturnUrlParameter = "returnURL";
    //other properties
});

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ISwalService, SwalService>();
builder.Services.AddScoped<IClipboardService, ClipboardService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRaceSimService, RaceSimService>();
builder.Services.AddScoped<IDLCService, DLCService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddBlazorTable();
builder.Services.AddSweetAlert2(options =>
{
    options.DefaultOptions = new SweetAlertOptions();
    options.DefaultOptions.ConfirmButtonColor = "#1976D2";
    options.DefaultOptions.DenyButtonColor = "#424242";
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = "oversteer.online",
        ValidIssuer = "oversteer.online",
        LifetimeValidator = LifetimeValidator,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1"))
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(ApiOrigin);
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
{
    if (expires != null)
    {
        return expires > DateTime.UtcNow;
    }
    return false;
}

// Needed for EF code first migrations
public class MysqlEntityFrameworkDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddEntityFrameworkMySQL();
        new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)
            .TryAddCoreServices();
    }
}


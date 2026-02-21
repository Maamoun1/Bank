using ApiBank.Helpers;
using BusinessLayer.Security;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using BusinessLayer.Authentication.Services;
using BusinessLayer.Authorization.Handlers;
using BusinessLayer.Tokens.Service;
using BusinessLayer.Authorization.Requirements;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//add CORS
builder.Services.AddCors(options =>
{

    options.AddPolicy("BankApiCoresPloicy", policy =>
    {
        policy
        .WithOrigins(
            "https://localhost:7272",
            "http://localhost:5049"
            )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
    
  });


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank API", Version = "v1" });


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Description = "Jwt Authorization header using the bearer schema.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {

        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    // Global limiter (apply automatically)
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    // Login limiter (strict)
    options.AddPolicy("login", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        Console.WriteLine($"Login attempt from IP: {ip}");

        return RateLimitPartition.GetFixedWindowLimiter(
            ip,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            message = "Too many requests. Please try again later."
        });
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var jwtKey = builder.Configuration["Jwt:Key"];

if(string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("Jwt:Key is missing in configuration");
}

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization( options =>
{
    options.AddPolicy("SameUserPolicy", policy =>
    {
        policy.Requirements.Add(new SameUserRequirement());
    });

});

builder.Services.AddSingleton<IAuthorizationHandler, SameUserHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SameUserHandler>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IReadOnlyRepostitory<>), typeof(ReadOnlyRepository<>));
builder.Services.AddScoped(typeof(IReadOnlyService<>), typeof(ReadOnlyService<>));
builder.Services.AddScoped(typeof(IGenericeService<>), typeof(GenericService<>));
builder.Services.AddScoped<IPersonRepostitory, PersonRepository>();
builder.Services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
builder.Services.AddScoped<IAccountsTypesRepository, AccountTypeRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Use CORS
app.UseCors("BankApiCoresPloicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

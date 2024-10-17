using Metete.Api.Data;
using Metete.Api.Infraestructure.Middlewares;
using Metete.Api.Infraestructure.Smtp;
using Metete.Api.Infraestructure.Transformers;
using Metete.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Configuration;
using System.Reflection;
using System.Text;


var customAllowSpecificOrigins = "_customAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Metete API", Version = "v1" });

    c.CustomSchemaIds(s => s.FullName?.Replace("+", "."));

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Logging
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// DbContext
builder.Services.AddDbContext<MeteteContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("Metete")!,
    new MySqlServerVersion(new Version(8, 0, 29)), x => x.UseNetTopologySuite()));

// Autommaper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// HttpContext
builder.Services.AddHttpContextAccessor();

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: customAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

// Global exception handler
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Authorization
builder.Services.AddAuthorization();

builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));

});

// Mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

// Custom services
builder.Services.AddTransient(typeof(ITokenService), typeof(TokenService));

// Named http clients
builder.Services.AddHttpClient("GoogleMaps", httpClient =>
{
    string baseUrl = builder.Configuration["GoogleMaps:ApiBaseUrl"]!;
    baseUrl += baseUrl.EndsWith("/") ? string.Empty : "/";

    httpClient.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

// Migrate any database changes on startup (includes initial db creation)
await using (var scope = app.Services.CreateAsyncScope())
{
    using var dataContext = scope.ServiceProvider.GetRequiredService<MeteteContext>();
    await dataContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

// Logging
app.UseSerilogRequestLogging();

// Static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration["UserFilesBasePath"]!),
    RequestPath = "/files"
});

// Global cors policy
app.UseCors(customAllowSpecificOrigins);

// Global exception handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();

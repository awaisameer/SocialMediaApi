using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SocialMediaApp.Services;
using System.Text;
using SocialMediaApp;

var builder = WebApplication.CreateBuilder(args);

// Bind MongoDB settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

// Bind JWT settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register MongoDB client as a singleton (this is correct because MongoDB client is thread-safe and can be reused)
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

// Register MongoDBService as a scoped service (as it is tied to each request's lifetime)
builder.Services.AddScoped<IUserService, MongoDBService>();

// Register AuthService (if it's used for JWT token generation)
builder.Services.AddScoped<AuthService>(); // Add this line to register AuthService
builder.Services.AddScoped<IPostService, PostService>();

// JWT Authentication setup
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: Add CORS support if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger setup
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS if needed
// app.UseCors("AllowAll");

app.UseAuthentication();  // Ensure authentication middleware is added
app.UseAuthorization();
app.MapControllers();
app.Run();

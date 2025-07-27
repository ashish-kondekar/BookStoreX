using Microsoft.OpenApi.Models;
using Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

// JWT Configuration
builder.Services.AddJwtAuthentication(builder.Configuration);

// Authorization Policies (Optional, if using RBAC/ABAC)
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//});

// Reverse Proxy
var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
                .LoadFromConfig(reverseProxyConfig);

// Custom Services
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway API", Version = "v1" });

    // Add JWT Bearer definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter your JWT token directly below. 
                        Do not include the 'Bearer' prefix. 
                        Example: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Add security requirement globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware order is important!
app.UseHttpsRedirection();

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

app.UseHealthChecks("/health");
app.MapControllers();     // For Auth/Login endpoints
app.MapReverseProxy();    // For proxying downstream calls

app.Run();
using System.Text;
using CDI_Tool;
using CDI_Tool.Authentication;
using CDI_Tool.Data;
using CDI_Tool.Repository;
using CDI_Tool.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
if (jwtOptions == null)
{
    throw new ArgumentNullException(nameof(jwtOptions));
}
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
    builder => { builder.RequireRole("Admin"); });
    options.AddPolicy("User",
    builder => { builder.RequireRole("Admin", "User"); });

});

builder.Services.AddAuthentication()
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            };

        });

builder.Services.AddScoped<UserAccessToken>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<LogRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LogService>();


builder.Services.AddDbContext<CDIDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SearchTool")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();

var allowedOrigins = new List<string>
{
    "https://medisearchtool.com",
    "https://pharmacy.medisearchtool.com",
    "http://localhost:5173",
        "http://localhost:5174",
        "http://127.0.0.1:8000",
        "http://localhost:8080",
        "https://cdi-drug-tool.vercel.app",

};

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins.ToArray())
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


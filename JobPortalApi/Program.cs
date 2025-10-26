using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobPortalApi.Data;
using JobPortalApi.Services;
using JobPortalApi.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services BEFORE building
builder.Services.AddControllersWithViews()
	.AddNewtonsoftJson();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation()
	.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

// EF Core
builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(o =>
	{
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwt["Issuer"],
			ValidAudience = jwt["Audience"],
			IssuerSigningKey = key,
			ClockSkew = TimeSpan.Zero
		};
	});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<JwtService>();

// Swagger configuration (added before Build)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI for development
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// App pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
// --- Seed Database ---
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.EnsureCreated();
	DatabaseSeeder.Seed(db);
}

app.Run();

using DataAcccessEFCore.Contracts;
using DataAcccessEFCore.Data;
using DataAcccessEFCore.Repositories;
using EmployeesAPI.Maps;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ModelsForAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddCors(policy => policy.AddPolicy("CorsPolicy", builder =>
               builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin()
               ));

builder.Services.AddDbContext<EmployeeDB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")

    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();

builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();
builder.Services.AddScoped<IRepository<Gender>, Repository<Gender>>();

builder.Services.AddAutoMapper(typeof(Map));

builder.Services.Configure<RouteOptions>(option => {
    option.LowercaseUrls = true;
    option.LowercaseQueryStrings = true;
    option.AppendTrailingSlash = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

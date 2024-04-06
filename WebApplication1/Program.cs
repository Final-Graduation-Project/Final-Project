using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using WebApplication1.Service.StaffMembers;
using WebApplication1.Service.Student;
using WebApplication1;
using WebApplication1.Services.Event;
using WebApplication1.Service.concilMember;
using WebApplication1.Services.Dijkstra;
using WebApplication1.map;
using Ninject.Planning.Bindings;


var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connStr));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
});

// Add JWT authentication
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
    };

});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add scoped services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStaffMemberService, StaffMemberService>();
builder.Services.AddScoped<IEventServer, EventServer>();
builder.Services.AddScoped<IconcilMemberService, concilMemberService>();
builder.Services.AddScoped<IDijkstraService, DijkstraService>();


var buildings = new List<Building>
{
    new Building("Eng", 432, 800),
    new Building("Pharmacy", 540, 380),
    new Building("Art", 390, 440),
    new Building("IT", 860 , 315),
    new Building("zane", 900 , 344),
    new Building("tarbeya", 877 , 383),
    new Building("oqoq", 865 , 425),
    new Building("olom", 910 , 900),
    new Building("business", 1337 , 632),
    new Building("mojama", 730 , 630),
    new Building("adab", 680 , 444),
};

var distances = new List<Distance>
{
    new Distance("IT", "zane", 20),
    new Distance("IT", "Pharmacy", 120),
    new Distance("Pharmacy", "Art", 110),
    new Distance("Pharmacy", "adab", 120),
    new Distance("adab", "mojama", 150),
    new Distance("zane", "tarbeya", 30),
    new Distance("tarbeya", "oqoq", 45),
    new Distance("mojama", "business", 300),
    new Distance("business", "olom", 180),
    new Distance("olom", "Eng", 83),
    new Distance("Eng", "Art", 400),
    new Distance("mojama", "oqoq", 100),
    new Distance("olom", "mojama", 120),
};

var graph = new Dictionary<string, Dictionary<string, double>>();

foreach (var building in buildings)
{
    graph[building.Name] = new Dictionary<string, double>();
}

foreach (var distance in distances)
{
    if (graph.ContainsKey(distance.From) && graph.ContainsKey(distance.To))
    {
        graph[distance.From][distance.To] = distance.DistanceValue;
        graph[distance.To][distance.From] = distance.DistanceValue;
    }
}

builder.Services.AddSingleton(graph);




builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
});

var app = builder.Build();

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
 app.UseCors("AllowAll");
app.MapControllers();

app.Run();

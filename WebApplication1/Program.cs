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
using WebApplication1.Service.Probosal;
using WebApplication1.Service.OfficeHour;
using WebApplication1.Services;


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
builder.Services.AddScoped<IProbosalService, ProbosalService>();
builder.Services.AddScoped<IOfficeHour, OfficeHourServer>();
builder.Services.AddScoped<IMessageService, MessageService>();




var buildings = new List<Building>
{
        new Building("AL.Juraysi", 0,0 ),
        new Building("SCI", 0,0 ),
        new Building("Aggad", 0,0 ),
        new Building("N.Shaheen", 0,0 ),
        new Building("Bahrain", 0,0 ),
        new Building("Khoury", 0,0 ),
        new Building("Masruji", 0,0 ),
        new Building("PNH", 0,0 ),
        new Building("Aweidah", 0,0 ),
        new Building("GYM", 0,0 ),
        new Building("Masri", 0,0 ),
        new Building("Bamieh", 0,0 ),
        new Building("Alsadik", 0,0 ),
        new Building("IOL", 0,0 ),
        new Building("KNH", 0,0 ),
        new Building("Alghanim", 0,0 ),
        new Building("NSA", 0,0 ),
        new Building("المجمع", 0,0 ),
        new Building("العمادة", 0,0 ),
        new Building("الرئاسة", 0,0 ),
        new Building("العيادة", 0,0 ),
        new Building("zane", 0,0 ),
        new Building("البوك ستور", 0,0 ),
        new Building("A.Shaheen", 0,0 ),

};

var distances = new List<Distance>
{
    new Distance("Masri", "zane", 40.2),
    new Distance("Masri","PNH",142),
    new Distance("Bamieh","zane",71),
    new Distance("Bamieh","Alsadik",74.5),
    new Distance("Alsadik","العمادة",108.4),
    new Distance("Alsadik","IOL",129),
    new Distance("Alsadik","الرئاسة",129),
    new Distance("Bamieh","A.Shaheen",350),
    new Distance("Masri","Masruji",102.5),
    new Distance("PNH","Masruji",60.3),
    new Distance("PNH","Aweidah",88),
    new Distance("Masruji","A.Shaheen",126.5),
    new Distance("Aweidah","Aggad",476.3),
    new Distance("A.Shaheen","المجمع",162),
    new Distance("المجمع","AL.Juraysi",325.5),
    new Distance("العمادة","AL.Juraysi",378.5),
    new Distance("Aggad","SCI",89.2),
    new Distance("SCI","N.Shaheen",114.5),
    new Distance("N.Shaheen","AL.Juraysi",64),
    new Distance("Aggad","المجمع",100.5),
    new Distance("A.Shaheen","Bahrain",81.2),
    new Distance("Masruji","Bahrain",61.5),
    new Distance("Masri","Bahrain",174),
    new Distance("العيادة","SCI",70),
    new Distance("العيادة","N.Shaheen",54.5),
    new Distance("العيادة","Alsadik",360),
    new Distance("Alsadik","البوك ستور",44),
    new Distance("A.Shaheen","البوك ستور",417.3),
    new Distance("GYM","Aggad",273.8),
    new Distance("GYM","A.Shaheen",51),
    new Distance("GYM","Aweidah",162),
    new Distance("Khoury","Masruji",47),
    new Distance("Khoury","A.Shaheen",128.2),
    new Distance("Khoury","Bahrain",26),
    new Distance("KNH","SCI",85),
    new Distance("AL.Juraysi","KNH",235),
    new Distance("KNH","Aggad",98.6),
    new Distance("Alsadik","KNH",178.5),
    new Distance("Alghanim","Aggad",169),
    new Distance("Alghanim","Alsadik",206.5),
    new Distance("Alghanim","SCI",104.2),
    new Distance("Alghanim","AL.Juraysi",91.2),
    new Distance("NSA","Masri",53),
    new Distance("NSA","Masruji",46.4),
    new Distance("NSA","PNH",99),





    
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

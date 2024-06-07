using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Models;
using WebApplication1.Services.Event;
using WebApplication1.Services;
using WebApplication1.map;
using WebApplication1.Service.concilMember;
using WebApplication1.Service.EmailConfirmation;
using WebApplication1.Service.StaffMembers;
using WebApplication1.Service.Student;
using WebApplication1.Services.Dijkstra;
using WebApplication1.Service.Probosal;
using WebApplication1.Service.OfficeHour;
using Microsoft.AspNetCore.Http.Features;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 52428800; // 50 MB
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://your-client-url")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            services.AddControllers();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IStaffMemberService, StaffMemberService>();
            services.AddTransient<IEventServer, EventServer>();
            services.AddTransient<IconcilMemberService, concilMemberService>();
            services.AddTransient<IDijkstraService, DijkstraService>();
            services.AddTransient<IProbosalService, ProbosalService>();
            services.AddTransient<IOfficeHour, OfficeHourServer>();

            // Register messaging service
            services.AddTransient<IMessageService, MessageService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            // Register IWebHostEnvironment
            services.AddSingleton(env => (IWebHostEnvironment)env.GetRequiredService(typeof(IWebHostEnvironment)));
            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Ensure the 'uploads' folder exists in 'wwwroot'
            var uploadsPath = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "messages",
                    pattern: "api/messages/{action}/{id?}", // Adjust pattern as needed
                    defaults: new { controller = "Messages" }); // Assuming your controller is named MessagesController

                endpoints.MapControllerRoute(
                    name: "uploadImage",
                    pattern: "api/uploadImage", // Adjust pattern as needed
                    defaults: new { controller = "Messages", action = "UploadImage" }); // Assuming your action is named UploadImage

                // Other endpoint mappings...

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}

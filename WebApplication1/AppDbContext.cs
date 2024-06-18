using Microsoft.EntityFrameworkCore;
using WebApplication1.Table;

namespace WebApplication1;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<OfficeHour> OfficeHours { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<News> News { get; set; }


    public DbSet<studentConcilMember> studentConcilMembers { get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasKey(m => m.MessageId);
    }





}
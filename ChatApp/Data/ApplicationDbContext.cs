using Microsoft.EntityFrameworkCore;
using ChatApp.Entity;

namespace ChatApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Message>()
                .HasOne(m => m.FromUser)        
                .WithMany(u => u.SentMessages)  
                .HasForeignKey(m => m.FromId)   
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ToUser)          
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ToId)     
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.CreatedByUser)        
                .WithMany()                         
                .HasForeignKey(a => a.CreatedByUserId) 
                .OnDelete(DeleteBehavior.Restrict);    

            base.OnModelCreating(modelBuilder);
        }
    }
}
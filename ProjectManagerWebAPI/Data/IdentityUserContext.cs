using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Data
{
    public class IdentityUserContext : IdentityDbContext<User>
    {
        private readonly UserManager<User> _userManager;

        public IdentityUserContext(DbContextOptions<IdentityUserContext> options) : base(options) { }
        
        protected override async void OnModelCreating(ModelBuilder builder)
{
            builder.Entity<TaskUser>().HasKey(e => new { e.TasksId, e.UsersId });
            builder.Entity<User>()
                .HasMany(u => u.TaskUsers)
                .WithOne(u => u.Users)
                .HasForeignKey(u => u.UsersId);

            base.OnModelCreating(builder);
            /*builder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithMany(u => u.Users)
                .UsingEntity("TaskUser");
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name= "Admin",
                    NormalizedName ="ADMIN"
                },
                new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "USER"
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);
            
            base.OnModelCreating(builder);*/
            
        }


    }
}

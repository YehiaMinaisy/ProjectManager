using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Data;

public partial class ProjectManagerDatabaseContext : DbContext
{
    public ProjectManagerDatabaseContext()
    {
    }

    public ProjectManagerDatabaseContext(DbContextOptions<ProjectManagerDatabaseContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Models.Task> Tasks { get; set; }

    public virtual DbSet<TaskUser> TaskUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.ProjectDescription).IsRequired();
            entity.Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasIndex(e => e.ParentId, "IX_Tasks_ParentId");

            entity.HasIndex(e => e.ProjectId, "IX_Tasks_ProjectId");

            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(d => d.Parent).WithMany(p => p.SubTasks).HasForeignKey(d => d.ParentId);

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks).HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<TaskUser>(entity =>
        {
            entity.HasKey(e => new { e.TasksId, e.UsersId });

            entity.ToTable("TaskUser");

            entity.HasIndex(e => e.UsersId, "IX_TaskUser_UsersId");

            entity.HasOne(d => d.Tasks).WithMany(p => p.TaskUsers).HasForeignKey(d => d.TasksId);

            entity.HasOne(d => d.Users).WithMany(p => p.TaskUsers).HasForeignKey(d => d.UsersId);
        });

        base.OnModelCreating(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

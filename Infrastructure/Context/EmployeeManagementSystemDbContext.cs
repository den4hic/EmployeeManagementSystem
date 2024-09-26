using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class EmployeeManagementSystemDbContext : IdentityContext
{

    public EmployeeManagementSystemDbContext(DbContextOptions<EmployeeManagementSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Domain.Entities.Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07F754F81E");

            entity.ToTable("Employee");

            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_User");
        });

        builder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC0733BFF013");

            entity.ToTable("Manager");

            entity.Property(e => e.Department).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.Managers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_User");
        });

        builder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC0775EB0870");

            entity.ToTable("Project");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Status).WithMany(p => p.Projects)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Project_Status");

            entity.HasMany(d => d.Employees).WithMany(p => p.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectEmployee",
                    r => r.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProjectEmployee_Employee"),
                    l => l.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProjectEmployee_Project"),
                    j =>
                    {
                        j.HasKey("ProjectId", "EmployeeId").HasName("PK__ProjectE__71B7BA018DEBC603");
                        j.ToTable("ProjectEmployee");
                    });

            //entity.HasMany(d => d.Managers).WithMany(p => p.Projects)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "ProjectManager",
            //        r => r.HasOne<Manager>().WithMany()
            //            .HasForeignKey("ManagerId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_ProjectManager_Manager"),
            //        l => l.HasOne<Project>().WithMany()
            //            .HasForeignKey("ProjectId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_ProjectManager_Project"),
            //        j =>
            //        {
            //            j.HasKey("ProjectId", "ManagerId").HasName("PK__ProjectM__75A0945E642EB1EF");
            //            j.ToTable("ProjectManager");
            //        });
        });

        builder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07F5CE7DF3");

            entity.ToTable("Status");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        builder.Entity<Domain.Entities.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Task__3214EC0702DF9774");

            entity.ToTable("Task");

            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.AssignedToEmployee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.AssignedToEmployeeId)
                .HasConstraintName("FK_Task_Employee");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Project");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Status");
        });

        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0756D47F41");

            entity.ToTable("User");

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RefreshToken).HasMaxLength(50);

            entity.HasOne<IdentityUser>()
                  .WithMany()
                  .HasForeignKey(d => d.AspNetUserId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_User_AspNetUsers");
        });

        builder.Entity<ProjectManager>(entity =>
        {
            entity.HasKey(pm => new { pm.ProjectId, pm.ManagerId }); // Composite Key

            entity.ToTable("ProjectManager");

            // Встановлюємо зв'язок між Project і ProjectManager
            entity.HasOne(pm => pm.Project)
                  .WithMany(p => p.ProjectManagers)
                  .HasForeignKey(pm => pm.ProjectId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            // Встановлюємо зв'язок між Manager і ProjectManager
            entity.HasOne(pm => pm.Manager)
                  .WithMany(m => m.ProjectManagers)
                  .HasForeignKey(pm => pm.ManagerId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(builder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

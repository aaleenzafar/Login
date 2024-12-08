using Login.Areas.Identity.Data;
using Login.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Login.Areas.Identity.Data;

public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options)
        : base(options)
    {
    }

    public DbSet<Faqs> FaQss { get; set; }
    public DbSet<Competition> Competitions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(255);
        builder.Property(x => x.Role).HasMaxLength(255);
        builder.Property(x => x.RollNo).HasMaxLength(255);
        builder.Property(x => x.Class).HasMaxLength(255);
        builder.Property(x => x.Section).HasMaxLength(255);
        builder.Property(x => x.AdmissionDate).HasMaxLength(255);
        builder.Property(x => x.EmployeeNo).HasMaxLength(255);
        builder.Property(x => x.Specification).HasMaxLength(255);
        builder.Property(x => x.JoiningDate).HasMaxLength(255);
    }
}
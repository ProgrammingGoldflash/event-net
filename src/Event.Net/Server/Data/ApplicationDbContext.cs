using Duende.IdentityServer.EntityFramework.Options;
using Event.Net.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;

namespace Event.Net.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Models.Event> Events { get; set; }
        public DbSet<Models.Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Models.Review>()
                .HasOne(e => e.Event)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.EventId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Models.Event>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Event)
                .HasForeignKey(e => e.EventId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Event>()
                .HasOne(e => e.User)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Models.Review>()
                .HasOne(e => e.User)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<Review>()
            .Property(s => s.Created)
            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Models.Event>()
            .Property(s => s.Created)
            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Models.Event>()
            .Property(s => s.LastUpdated)
            .ValueGeneratedOnUpdate();
        }
    }
}
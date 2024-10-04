using Core.Concrete.Entities;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace DataAccess.Concrete.EntityFramework.Context
{
    public class PostgreSqlDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IConfiguration Configuration { get; set; }


        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private PostgreSqlDbContext(DbContextOptions options, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Configuration.GetConnectionString("Postgre");

            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Postgre"), opt => opt.CommandTimeout(60)));
            }

        }

        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;

            #region Automatically set "CreateAt" and "UpdateAt" properties on a new entity
            var addedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .ToList();

            foreach (var entry in addedEntities)
            {
                if (entry.Metadata.FindProperty("CreateAt") != null)
                {
                    entry.Property("CreateAt").CurrentValue = currentDate;
                }
                if (entry.Metadata.FindProperty("UpdateAt") != null)
                {
                    entry.Property("UpdateAt").CurrentValue = currentDate;
                }
                if (entry.Metadata.FindProperty("IsActive") != null)
                {
                    entry.Property("IsActive").CurrentValue = true;
                }
            }
            #endregion

            #region Automatically set "UpdateAt" property on an existing entity
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in modifiedEntities)
            {
                if (entry.Metadata.FindProperty("UpdateAt") != null)
                {
                    entry.Property("UpdateAt").CurrentValue = currentDate;
                }
            }
            #endregion

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User's relationships
            modelBuilder.Entity<UserOperationClaim>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId); 

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne<OperationClaim>()
                .WithMany()
                .HasForeignKey(x => x.OperationClaimId);
            #endregion

            modelBuilder.Entity<Book>()
                .HasOne<Genre>()
                .WithMany()
                .HasForeignKey(x => x.GenreId);

        }
    }
}

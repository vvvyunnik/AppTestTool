using MVCTestTools.Entities;
using System.Data.Entity;

namespace MVCTestTools.Contex
{
    public class MVCTestToolsContext : DbContext
    {
        public MVCTestToolsContext()
        {
            Database.SetInitializer<MVCTestToolsContext>(new CreateDatabaseIfNotExists<MVCTestToolsContext>());
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>()
                    .HasRequired<Application>(s => s.Application)
                    .WithMany(s => s.Tests);


            modelBuilder.Entity<Admin>()
                .HasMany<Application>(s => s.Applications)
                .WithMany(c => c.Admins)
                .Map(cs =>
                {
                    cs.MapLeftKey("AdminRefId");
                    cs.MapRightKey("AppRefId");
                    cs.ToTable("AdminApplication");
                });
        }
    }
}
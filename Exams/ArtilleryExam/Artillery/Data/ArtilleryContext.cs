namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() { }

        public ArtilleryContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Gun> Guns { get; set; }

        public virtual DbSet<CountryGun> CountriesGuns { get; set; }

        public virtual DbSet<Manufacturer> Manufacturers { get; set; } 

        public virtual DbSet<Shell> Shells { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(m => m.ManufacturerName)
                .IsUnique();

            modelBuilder.Entity<CountryGun>()
                .HasKey(cg => new { cg.CountryId, cg.GunId });
        }
    }
}

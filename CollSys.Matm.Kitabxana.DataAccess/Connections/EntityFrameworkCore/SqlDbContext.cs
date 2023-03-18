using CollSys.Matm.Kitabxana.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace CollSys.Matm.Kitabxana.DataAccess.Connections.EntityFrameworkCore
{

    public class SqlDbContext : DbContext
    {
        // Tables
        public DbSet<CurrencyModel> Currencies { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<MaterialModel> Materials { get; set; }
        public DbSet<YazarModel> Yazarlar { get; set; }
        public DbSet<MetbeeModel> Metbeeler { get; set; }
        public DbSet<MeasurementUnitModel> MeasurementUnitModels { get; set; }
        public DbSet<RegionModel> Regions { get; set; }
        public DbSet<SaxlanmaVeziyyetiModel> SaxlanmaVeziyyetleri { get; set; }
        public DbSet<ExhibitModel> Exhibits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Server=<YOUR_SERVER_NAME>; Database=<YOUR_DATABASE_NAME>; User Id=<YOUR_DATABASE_USERNAME>; Password=<YOUR_DATABASE_PASSWORD>; Pooling=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indexing and making values unique
            modelBuilder.Entity<ExhibitModel>().HasIndex(c => new { c.InventarNo, c.Format }).IsUnique();

            modelBuilder.Entity<CurrencyModel>().HasIndex(c => c.Currency).IsUnique();
            modelBuilder.Entity<MaterialModel>().HasIndex(c => c.Material).IsUnique();
            modelBuilder.Entity<YazarModel>().HasIndex(c => c.Yazar).IsUnique();
            modelBuilder.Entity<MetbeeModel>().HasIndex(c => c.Metbee).IsUnique();
            modelBuilder.Entity<MeasurementUnitModel>().HasIndex(c => c.MeasurementUnit).IsUnique();
            modelBuilder.Entity<RegionModel>().HasIndex(c => c.Region).IsUnique();
            modelBuilder.Entity<SaxlanmaVeziyyetiModel>().HasIndex(c => c.SaxlanmaVeziyyeti).IsUnique();


            // Relations between tables
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.Currency).WithMany(c => c.Exhibits).HasForeignKey(c => c.CurrencyId);
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.Material).WithMany(c => c.Exhibits).HasForeignKey(c => c.MaterialId);
            
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.Yazar).WithMany(c => c.Exhibits).HasForeignKey(c => c.YazarId);
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.Metbee).WithMany(c => c.Exhibits).HasForeignKey(c => c.MetbeeId);

            modelBuilder.Entity<ExhibitModel>().HasMany(c => c.Images).WithOne(c => c.Exhibit).IsRequired().HasForeignKey(c => c.ExhibitId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.EnUnit).WithMany(c => c.EnExhibits).HasForeignKey(c => c.EnUnitId);
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.UzunluqUnit).WithMany(c => c.UzunluqExhibits).HasForeignKey(c => c.UzunluqUnitId);
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.DiametrUnit).WithMany(c => c.DiametrExhibits).HasForeignKey(c => c.DiametrUnitId);
            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.HundurlukUnit).WithMany(c => c.HundurlukExhibits).HasForeignKey(c => c.HundurlukUnitId);

            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.IstehsalYeri).WithMany(c => c.IstehsalExhibits).HasForeignKey(c => c.IstehsalYeriId);

            modelBuilder.Entity<ExhibitModel>().HasOne(c => c.SaxlanmaVeziyyeti).WithMany(c => c.Exhibits).HasForeignKey(c => c.SaxlanmaVeziyyetiId);

        }
    }

}

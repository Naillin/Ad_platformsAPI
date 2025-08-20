using ad_platformsAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ad_platformsAPI.DataWork
{
	public class AppDbContext : DbContext
	{
		public DbSet<AdLocation> AdLocations { get; set; }
		public DbSet<AdPlatform> AdPlatforms { get; set; }
		public DbSet<AdPlatformLocation> AdPlatformLocations { get; set; }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlite("Data Source=mqtt_data.db").LogTo(message => Debug.WriteLine(message), LogLevel.Information);
		//}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Конфигурация связи многие-ко-многим
			modelBuilder.Entity<AdPlatformLocation>()
				.HasKey(pl => new { pl.AdPlatformId, pl.AdLocationId });

			modelBuilder.Entity<AdPlatformLocation>()
				.HasOne(pl => pl.AdPlatform)
				.WithMany(p => p.AdPlatformLocations)
				.HasForeignKey(pl => pl.AdPlatformId);

			modelBuilder.Entity<AdPlatformLocation>()
				.HasOne(pl => pl.AdLocation)
				.WithMany(l => l.AdPlatformLocations)
				.HasForeignKey(pl => pl.AdLocationId);

			// Индексы для оптимизации поиска
			modelBuilder.Entity<AdLocation>()
				.HasIndex(l => l.Path)
				.IsUnique();

			modelBuilder.Entity<AdPlatform>()
				.HasIndex(p => p.Name)
				.IsUnique();
		}
	}
}

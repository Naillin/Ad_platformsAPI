namespace ad_platformsAPI.Core.Entities
{
	public class AdPlatform
	{
		public int Id { get; set; }

		public string? Name { get; set; }


		// Навигационные свойства
		public virtual ICollection<AdPlatformLocation> AdPlatformLocations { get; set; }

		public AdPlatform()
		{
			AdPlatformLocations = new HashSet<AdPlatformLocation>();
		}
	}
}

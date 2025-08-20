namespace ad_platformsAPI.Core.Entities
{
	public class AdPlatformLocation
	{
		public int AdPlatformId { get; set; }

		public int AdLocationId { get; set; }


		// Навигационные свойства
		public virtual AdPlatform? AdPlatform { get; set; }

		public virtual AdLocation? AdLocation { get; set; }
	}
}

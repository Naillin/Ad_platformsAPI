namespace ad_platformsAPI.Core.Entities
{
	public class AdLocation
	{
		public int Id { get; set; }

		public string? Path { get; set; }

		// Навигационные свойства
		public virtual ICollection<AdPlatformLocation> AdPlatformLocations { get; set; }

		public AdLocation()
		{
			AdPlatformLocations = new HashSet<AdPlatformLocation>();
		}

		public int GetDepth()
		{
			if (string.IsNullOrWhiteSpace(Path))
				return 0;

			string normalizedPath = Path.Trim('/');

			if (string.IsNullOrEmpty(normalizedPath))
				return 0;

			string[] parts = normalizedPath.Split(
				new[] { '/' },
				StringSplitOptions.RemoveEmptyEntries
			);

			return parts.Length;
		}
	}
}

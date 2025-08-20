using ad_platformsAPI.Core.Entities;

namespace ad_platformsAPI.Core.Interfaces
{
	public interface IDataRepository
	{
		List<AdPlatform> Platforms { get; }
		List<AdLocation> Locations { get; }
		List<AdPlatformLocation> PlatformLocations { get; }

		Task<bool> LoadData();
	}
}

using ad_platformsAPI.Core.Entities;
using ad_platformsAPI.Core.Interfaces;

namespace ad_platformsAPI.DataWork.Repositories
{
	public abstract class DataRepository : IDataRepository
	{
		public List<AdPlatform> Platforms { get; protected set; } = new();
		public List<AdLocation> Locations { get; protected set; } = new();
		public List<AdPlatformLocation> PlatformLocations { get; protected set; } = new();

		public abstract Task<bool> LoadData();
	}
}

using ad_platformsAPI.Core.Entities;

namespace ad_platformsAPI.DataWork.Repositories
{
	public class FileRepository : DataRepository
	{
		private string _pathToFile = string.Empty;

		public FileRepository(string pathToFile = "data.txt") => _pathToFile = pathToFile;

		public override async Task<bool> LoadData()
		{
			bool result = false;

			try
			{
				var (platforms, locations, platformLocations) = await LoadDataFromFile();
				Platforms = platforms;
				Locations = locations;
				PlatformLocations = platformLocations;
				result = true;
			}
			catch
			{
				result = false;
			}

			return result;
		}

		public async Task<(List<AdPlatform>, List<AdLocation>, List<AdPlatformLocation>)> LoadDataFromFile()
		{
			string[] lines = await File.ReadAllLinesAsync(_pathToFile);

			List<AdPlatform> listAdPlatforms = new List<AdPlatform>();
			Dictionary<string, AdLocation> locationMap = new Dictionary<string, AdLocation>();
			List<AdPlatformLocation> listAdPlatformLocation = new List<AdPlatformLocation>();

			int autoIdPlatform = 1;
			int autoIdLocation = 1;

			foreach (var rawLine in lines)
			{
				if (string.IsNullOrWhiteSpace(rawLine))
					continue;

				string[] data = rawLine.Split(':', 2);
				if (data.Length != 2)
					continue;

				string platformName = data[0].Trim();
				string[] locations = data[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

				var adPlatform = new AdPlatform
				{
					Id = autoIdPlatform++,
					Name = platformName
				};

				foreach (var location in locations)
				{
					string path = location.Trim();
					if (!locationMap.TryGetValue(path, out var adLocation))
					{
						adLocation = new AdLocation
						{
							Id = autoIdLocation++,
							Path = path
						};
						locationMap[path] = adLocation;
					}

					var adPlatformLocation = new AdPlatformLocation
					{
						AdPlatformId = adPlatform.Id,
						AdLocationId = adLocation.Id,
						AdPlatform = adPlatform,
						AdLocation = adLocation
					};

					adPlatform.AdPlatformLocations.Add(adPlatformLocation);
					adLocation.AdPlatformLocations.Add(adPlatformLocation);
					listAdPlatformLocation.Add(adPlatformLocation);
				}

				listAdPlatforms.Add(adPlatform);
			}

			return (listAdPlatforms, locationMap.Values.ToList(), listAdPlatformLocation);
		}
	}
}

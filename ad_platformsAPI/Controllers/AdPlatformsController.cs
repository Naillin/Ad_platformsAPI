using ad_platformsAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ad_platformsAPI.Controllers
{
	public class AdPlatformsController : ControllerBase
	{
		private readonly IDataRepository _dataRepository;
		private readonly LoggerManager loggerAdPlatformsController;

		public AdPlatformsController(IDataRepository dataRepository, ILogger<AdPlatformsController> logger)
		{
			_dataRepository = dataRepository;
			loggerAdPlatformsController = new LoggerManager(logger, "AdPlatformsController");
		}

		// 1. Загрузка данных из файла
		[HttpPost("load-data")]
		public async Task<IActionResult> LoadData()
		{
			bool isSuccess = await _dataRepository.LoadData();

			if (isSuccess)
			{
				loggerAdPlatformsController.LogInformation("Data loaded successfully [{Platforms}] for [{Locations}]", _dataRepository.Platforms.Select(p => p.Name).ToList(), _dataRepository.Locations.Select(p => p.Path).ToList());

				return Ok(new { message = "Data loaded successfully" });
			}
			else
			{
				loggerAdPlatformsController.LogError("Failed to load data");
				return StatusCode(500, new { message = "Failed to load data" });
			}
		}

		// 2. Получение списка площадок по локации
		[HttpGet("by-location")]
		public IActionResult GetByLocation([FromQuery] string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				loggerAdPlatformsController.LogWarning("Empty path parameter provided");
				return BadRequest("Path parameter cannot be empty");
			}

			path = path.Trim();
			if (!path.StartsWith("/"))
				path = "/" + path;

			var platforms = _dataRepository.PlatformLocations
				.Where(pl => path.StartsWith(pl.AdLocation!.Path!, StringComparison.Ordinal))
				.Select(pl => pl.AdPlatform!.Name)
				.Distinct()
				.ToList();

			loggerAdPlatformsController.LogInformation("Send platforms for {Path} - {Platforms}", path, platforms.Count != 0 ? platforms : "empty");
			return Ok(platforms);
		}
	}
}

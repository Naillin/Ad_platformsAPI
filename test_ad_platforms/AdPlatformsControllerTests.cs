using ad_platformsAPI.Controllers;
using ad_platformsAPI.Core.Entities;
using ad_platformsAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace test_ad_platforms
{
	public class AdPlatformsControllerTests
	{
		[Fact]
		public async Task Load_ShouldCallRepositoryAndReturnOk()
		{
			// Arrange
			var repoMock = new Mock<IDataRepository>();
			repoMock.Setup(r => r.LoadData()).ReturnsAsync(true);
			repoMock.SetupGet(r => r.Platforms).Returns(new List<AdPlatform>());
			repoMock.SetupGet(r => r.Locations).Returns(new List<AdLocation>());
			repoMock.SetupGet(r => r.PlatformLocations).Returns(new List<AdPlatformLocation>());

			var loggerMock = new Mock<ILogger<AdPlatformsController>>();
			var controller = new AdPlatformsController(repoMock.Object, loggerMock.Object);

			// Act
			var result = await controller.LoadData();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Contains("successfully", okResult.Value!.ToString());
			repoMock.Verify(r => r.LoadData(), Times.Once);
		}

		[Fact]
		public void GetByLocation_ShouldReturnCorrectPlatforms()
		{
			// Arrange
			var location = new AdLocation { Id = 1, Path = "/a" };
			var platform = new AdPlatform { Id = 1, Name = "Platform1" };
			var link = new AdPlatformLocation { AdPlatform = platform, AdLocation = location };

			platform.AdPlatformLocations.Add(link);
			location.AdPlatformLocations.Add(link);

			var repoMock = new Mock<IDataRepository>();
			repoMock.Setup(r => r.PlatformLocations).Returns(new List<AdPlatformLocation> { link });

			var loggerMock = new Mock<ILogger<AdPlatformsController>>();
			var controller = new AdPlatformsController(repoMock.Object, loggerMock.Object);

			// Act
			var result = controller.GetByLocation("/a/test");

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var platforms = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
			Assert.Single(platforms);
			Assert.Contains("Platform1", platforms);
		}
	}
}

using ad_platformsAPI.DataWork.Repositories;

namespace test_ad_platforms
{
	public class FileRepositoryTests
	{
		[Fact]
		public async Task LoadDataFromFile_ShouldParseFileCorrectly()
		{
			// Arrange
			string tempFile = Path.GetTempFileName();
			await File.WriteAllLinesAsync(tempFile, new[]
			{
				"Platform1: /a, /b",
				"Platform2: /b, /c"
			});

			var repo = new FileRepository(tempFile);

			// Act
			var (platforms, locations, platformLocations) = await repo.LoadDataFromFile();

			// Assert
			Assert.Equal(2, platforms.Count);
			Assert.Equal(3, locations.Count);
			Assert.Equal(4, platformLocations.Count);
		}
	}
}

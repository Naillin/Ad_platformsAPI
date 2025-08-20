using ad_platformsAPI.Core.Entities;

namespace test_ad_platforms
{
	public class AdLocationTests
	{
		[Theory]
		[InlineData("/", 0)]
		[InlineData("/home", 1)]
		[InlineData("/home/about", 2)]
		[InlineData("/home/about/us", 3)]
		[InlineData("", 0)]
		[InlineData("   ", 0)]
		public void GetDepth_ShouldReturnExpectedDepth(string path, int expectedDepth)
		{
			var location = new AdLocation { Path = path };

			int depth = location.GetDepth();

			Assert.Equal(expectedDepth, depth);
		}
	}
}

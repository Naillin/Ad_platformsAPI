using Microsoft.EntityFrameworkCore;

namespace ad_platformsAPI.DataWork.Repositories
{
	public class DatabaseRepository : DataRepository
	{
		private readonly IDbContextFactory<AppDbContext> _factory;

		public DatabaseRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

		public override Task<bool> LoadData()
		{
			throw new NotImplementedException();
		}
	}
}

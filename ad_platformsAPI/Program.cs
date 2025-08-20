using ad_platformsAPI.Core.Interfaces;
using ad_platformsAPI.DataWork;
using ad_platformsAPI.DataWork.Repositories;

namespace ad_platformsAPI
{
    public class Program
    {
		public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			// Чтение конфига
			var repoSection = builder.Configuration.GetSection("RepositorySettings");
			int dataMethod = repoSection.GetValue<int>("DataMethod");          // 0 = файл, 1 = БД
			string filePath = repoSection.GetValue<string>("FilePath") ?? "data.txt";
			string connectionStrings = repoSection.GetValue<string>("ConnectionStrings") ?? "Server=.;Database=AdDb;Trusted_Connection=True;TrustServerCertificate=True";

			// Регистрация сервисов контроллеров и Swagger
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Регистрация репозиториев и БД в DI в зависимости от источника данных
			if (dataMethod == 0)
			{
				// Источник — файл. Данные храним в памяти, хотим, чтобы они не «забывались» между запросами.
				// Поэтому Singleton.
				builder.Services.AddSingleton<IDataRepository>(sp => new FileRepository(filePath));
			}
			else if (dataMethod == 1)
			{
				// Источник — БД. Репозиторий на запрос (Scoped), контекст — через фабрику.
				builder.Services.AddDbContextFactory<AppDbContext>(optionsAction =>
				{
					//optionsAction.UseSqlite(connectionStrings).LogTo(message => Debug.WriteLine(message), LogLevel.Information);
				});
				builder.Services.AddScoped<IDataRepository, DatabaseRepository>();
			}
			else
			{
				throw new NotImplementedException("Unknown Repository Settings:DataMethod (use 0 for file, 1 for database).");
			}

			var app = builder.Build();

			// 4) Middleware-пайплайн
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			//app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
    }
}

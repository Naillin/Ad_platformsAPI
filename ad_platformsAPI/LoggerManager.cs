namespace ad_platformsAPI
{
	public class LoggerManager : ILogger
	{
		private readonly ILogger _innerLogger;
		private readonly string _moduleName;

		public LoggerManager(ILogger innerLogger, string moduleName = "default")
		{
			_innerLogger = innerLogger ?? throw new ArgumentNullException(nameof(innerLogger));
			_moduleName = moduleName;
		}

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			return _innerLogger.BeginScope(state);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return _innerLogger.IsEnabled(logLevel);
		}

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception?, string> formatter)
		{
			// Форматируем сообщение
			string message = formatter(state, exception);
			string formattedMessage = $"[{_moduleName}] {message}";

			// Логируем во внутренний логгер
			_innerLogger.Log(logLevel, eventId, state, exception, (s, e) => formattedMessage);
		}

		public void LogInformation(string text) => Log(LogLevel.Information, 0, text, null, (s, _) => s.ToString()!);
		public void LogWarning(string text) => Log(LogLevel.Warning, 0, text, null, (s, _) => s.ToString()!);
		public void LogError(string text, Exception? ex = null) => Log(LogLevel.Error, 0, text, ex, (s, _) => s.ToString()!);
	}
}

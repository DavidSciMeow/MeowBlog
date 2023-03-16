using System.Collections.Concurrent;

namespace MeowBlog
{
    public static class MainUtil
    {
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="s">记录字</param>
        /// <param name="front">前景色</param>
        /// <param name="rear">背景色</param>
        public static void Log(string s, ConsoleColor front = ConsoleColor.White, ConsoleColor rear = ConsoleColor.Black)
        {
            Console.ForegroundColor = front;
            Console.BackgroundColor = rear;
            Console.WriteLine($"[BlogLog] {DateTime.Now:T} :: {s}");
            Console.ResetColor();
        }
        /// <summary>
        /// 字符串扩展
        /// </summary>
        /// <param name="s">记录字</param>
        /// <param name="_logintensenties">等级 0(debug)/1(info)/2(warn)/3(err)</param>
        public static void ToLog(this string s,int _logintensenties = 0)
        {
            if(ProgramProperties.Loglevel <= _logintensenties)
            {
                switch (_logintensenties)
                {
                    case 1: Log(s, ConsoleColor.Blue); break;
                    case 2: Log(s, ConsoleColor.Yellow); break;
                    case 3: Log(s, ConsoleColor.Red); break;
                    default: Log(s, ConsoleColor.White); break;
                }
            }
        }
    }
    public class ColoredConsoleLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public int EventId { get; set; } = 0;
        public ConsoleColor Color { get; set; } = ConsoleColor.Yellow;
    }
    public class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ColoredConsoleLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, ColoredConsoleLogger> _loggers = new();
        public ColoredConsoleLoggerProvider(ColoredConsoleLoggerConfiguration config)
        {
            _config = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new ColoredConsoleLogger(name, _config));
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _loggers.Clear();
        }
    }
    public class ColoredConsoleLogger : ILogger
    {
        private static object _lock = new();
        private readonly string _name;
        private readonly ColoredConsoleLoggerConfiguration _config;
        public ColoredConsoleLogger(string name, ColoredConsoleLoggerConfiguration config)
        {
            _name = name;
            _config = config;
        }
        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel == _config.LogLevel;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            lock (_lock)
            {
                if (_config.EventId == 0 || _config.EventId == eventId.Id)
                {
                    $"[{eventId.Id}] {_name} \n- {formatter(state, exception ?? new Exception())}".ToLog((int)logLevel-1);
                }
            }
        }
    }
}

using SupportBank;
using NLog;
using NLog.Config;
using NLog.Targets;

ILogger logger = LogManager.GetCurrentClassLogger();

var config = new LoggingConfiguration();
var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
config.AddTarget("File Logger", target);
config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
LogManager.Configuration = config;

logger.Info("Application started");

App.Run();

logger.Info("Application stopped");
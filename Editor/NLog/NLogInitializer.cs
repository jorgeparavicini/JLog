using JetBrains.Annotations;
using NLog;
using NLog.Config;
using UnityEditor.Callbacks;
using Logger = NLog.Logger;

namespace UTerm.Editor.NLog
{
    //[InitializeOnLoad]
    public static class NLogInitializer
    {
        public static string ConfigPath { get; } = "Packages/UTerm/NLog.config";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [DidReloadScripts]
        [UsedImplicitly]
        public static void Initialize()
        {
            Logger.Debug($"Initializing with Configuration file: {ConfigPath}");
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigPath);
            Logger.Info("Initialized Log Configuration");
        }
    }
}
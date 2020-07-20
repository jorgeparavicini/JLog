using NLog;
using UnityEngine;
using Logger = NLog.Logger;

namespace JLog.Runtime
{
    public static class JLog
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static string ConfigPath { get; set; } = "Packages/JLog/NLog.config";

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(ConfigPath);
            Debug.Log("Hue");
            Logger.Error("Hue");
        }
    }
}
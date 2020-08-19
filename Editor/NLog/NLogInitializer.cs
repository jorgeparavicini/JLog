using JetBrains.Annotations;
using NLog;
using NLog.Config;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UTerm.Editor.NLog
{
    //[InitializeOnLoad]
    public static class NLogInitializer
    {
        public static string ConfigPath { get; } = "Packages/UTerm/NLog.config";

        [DidReloadScripts]
        [UsedImplicitly]
        public static void Initialize()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigPath);
            LogManager.GetCurrentClassLogger().Debug("hello");
        }
    }
}
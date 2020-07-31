using System.Linq;
using NLog;
using NLog.Config;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UTerm.Editor
{
    //[InitializeOnLoad]
    public static class NLog
    {
        public static string ConfigPath { get; set; } = "Packages/UTerm/NLog.config";

        [DidReloadScripts]
        public static void Initialize()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigPath);
            
            //LogManager.GetCurrentClassLogger().Debug("hello");
            //LogManager.GetCurrentClassLogger().Debug("hello");
            //LogManager.GetCurrentClassLogger().Debug("hello");
            //LogManager.GetCurrentClassLogger().Debug("hello");
            //LogManager.GetCurrentClassLogger().Debug("hello");
        }

        /*static NLog()
        {
            Debug.Log("Wut");
            Initialize();
        }*/
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using NLog;
using NLog.Config;
using NLog.Targets;
using Logger = NLog.Logger;

namespace UTerm.Editor.NLog
{
    /// <summary>
    /// The  target gets destroyed and recreated whenever the config file changes.
    /// This construction however, is sent from another thread. It's difficult to synchronize this with the Unity API.
    /// Hence we serialize the data in this class on its own.
    /// </summary>
    [Target("UnityTerminal")]
    public class NLogTarget : TargetWithLayout
    {

        #region Fields & Properties

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public List<NLogEntry> Logs { get; private set; }= new List<NLogEntry>();
        
        [RequiredParameter] public string TabName { get; set; }
        
        private string PersistencePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "Packages/UTerm/Editor/Data/", Name.GetHashCode().ToString());

        #endregion

        #region Events

        public static event Action<NLogTarget> TargetInitialized = delegate { };
        public static event Action<NLogTarget> TargetClosed = delegate { };
        public event Action<LogEventInfo> DidWrite = delegate { };

        #endregion
        
        #region Target Overrides

        protected override void InitializeTarget()
        {
            Logger.Debug($"Initializing target: {Name}");
            base.InitializeTarget();

            Deserialize();
            TargetInitialized(this);
            Logger.Debug($"Initialized target: {Name}");
        }

        protected override void CloseTarget()
        {
            Logger.Debug($"Closing target: {Name}");
            base.CloseTarget();

            Serialize();
            TargetClosed(this);
            Logger.Debug($"Closed target: {Name}");
        }

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);

            Logs.Add(NLogEntry.FromLog(logEvent, Layout));
            DidWrite(logEvent);
        }
        
        #endregion

        #region Serialization

        private void Serialize()
        {
            var serializer = new XmlSerializer(typeof(List<NLogEntry>));
            using var writer = new StreamWriter(PersistencePath);
            serializer.Serialize(writer, Logs);
        }

        private void Deserialize()
        {
            if (!File.Exists(PersistencePath)) return;
            var serializer = new XmlSerializer(typeof(List<NLogEntry>));
            using var fs = new FileStream(PersistencePath, FileMode.Open);
            Logs = (List<NLogEntry>) serializer.Deserialize(fs);
        }

        #endregion


    }
}
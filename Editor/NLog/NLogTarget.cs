using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NLog;
using NLog.Config;
using NLog.Targets;
using UnityEngine;

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

        private List<LogEventInfo> _logs = new List<LogEventInfo>();
        private string PersistencePath =>
            Path.Combine(Directory.GetCurrentDirectory(), "Packages/UTerm/Editor/Data/", Name.GetHashCode().ToString());
        public ReadOnlyCollection<LogEventInfo> Logs => _logs.AsReadOnly();
        [RequiredParameter] public string TabName { get; set; }

        #endregion

        #region Events

        public static event Action<NLogTarget> TargetInitialized = delegate { };
        public static event Action<NLogTarget> TargetClosed = delegate { };
        public event Action<LogEventInfo> DidWrite = delegate { };

        #endregion
        
        #region Target Overrides

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            Deserialize();
            TargetInitialized(this);
        }

        protected override void CloseTarget()
        {
            base.CloseTarget();

            Serialize();
            TargetClosed(this);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);

            _logs.Add(logEvent);
            DidWrite(logEvent);
        }
        
        #endregion

        #region Serialization

        private void Serialize()
        {
            var serializer = new XmlSerializer(typeof(List<NLogEntry>));
            using var writer = new StreamWriter(PersistencePath);
            var data = _logs.Select(NLogEntry.FromLog).ToList();
            serializer.Serialize(writer, data);
        }

        private void Deserialize()
        {
            if (!File.Exists(PersistencePath)) return;
            var serializer = new XmlSerializer(typeof(List<NLogEntry>));
            using var fs = new FileStream(PersistencePath, FileMode.Open);
            _logs = ((List<NLogEntry>) serializer.Deserialize(fs)).Select(entry => entry.ToLog()).ToList();
        }

        #endregion


    }
}
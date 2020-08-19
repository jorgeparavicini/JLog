using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;
using UnityEngine;
using UTerm.Editor.Terminal;

namespace UTerm.Editor.NLog
{
    [Target("UnityTerminal")]
    public class NLogTarget : TargetWithLayout
    {
        
        [RequiredParameter]
        public string TabName { get; set; }
        
        public static event Action<NLogTarget> TargetInitialized = delegate { };
        public event Action<LogEventInfo> DidWrite = delegate {  };

        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            TargetInitialized(this);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);
            DidWrite(logEvent);
        }
    }
}
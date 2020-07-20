using System.Collections.Generic;
using NLog;
using NLog.Targets;
using UnityEngine;

namespace JLog.Editor
{
    
    [Target("UnityTerminal")]
    public class TerminalTarget : TargetWithLayout
    {
        
        private readonly List<TerminalEntry> _entries = new List<TerminalEntry>();

        public TerminalTarget()
        {
            //Terminal.RegisterTerminalTarget(this);
            Debug.Log("Init");
        }
        
        // TODO: Convert to async
        protected override void Write(LogEventInfo logEvent)
        {
            Debug.Log(Layout.Render(logEvent));
            _entries.Add(new TerminalEntry(Layout.Render(logEvent)));
        }

        public void Clear()
        {
            Debug.Log("Clear");
            _entries.Clear();
        }
    }
}
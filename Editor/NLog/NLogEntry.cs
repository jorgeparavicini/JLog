using System;
using NLog;
using NLog.Layouts;
using UnityEngine;

namespace UTerm.Editor.NLog
{
    [Serializable]
    public class NLogEntry
    {
        [field: SerializeField] public string LogLevelName { get; set; }
        [field: SerializeField] public string LoggerName { get; set; }
        [field: SerializeField] public string RenderedMessage { get; set; }

        public static NLogEntry FromLog(LogEventInfo info, Layout layout)
        {
            return new NLogEntry
            {
                LogLevelName = info.Level.Name,
                LoggerName = info.LoggerName,
                RenderedMessage = layout.Render(info)
            };
        }
    }
}
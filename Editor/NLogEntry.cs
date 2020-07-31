using NLog;
using NLog.Layouts;
using UnityEngine;

namespace UTerm.Editor
{
    public class NLogEntry : ISerializationCallbackReceiver
    {
        public LogEventInfo LogEvent { get; }
        
        public Layout Layout { get; }

        public string RenderedMessage => Layout.Render(LogEvent);

        public NLogEntry(LogEventInfo logEvent, Layout layout)
        {
            LogEvent = logEvent;
            Layout = layout;
        }

        public void OnBeforeSerialize()
        {
            Debug.Log("Before Serialization");
        }

        public void OnAfterDeserialize()
        {
            Debug.Log("After Serialization");
        }
    }
}
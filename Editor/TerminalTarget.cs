using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;
using UIX;
using UnityEngine;
using UTerm.Editor.UI;

namespace UTerm.Editor
{
    [Target("UnityTerminal")]
    public class TerminalTarget : TargetWithLayout
    {
        private NLogTab _tab;

        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            var tab = Terminal.Window.GetTabWithName(Name);
            switch (tab)
            {
                case null:
                    _tab = new NLogTab {TabName = Name};
                    Terminal.Window.AddTab(_tab);
                    break;
                case NLogTab nlogTab:
                    _tab = nlogTab;
                    break;
                default:
                    throw new InvalidOperationException(
                        "Can not create Terminal tab as there is already a different tab registered with the same name.");
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            _tab.AddEntry(new NLogEntry(logEvent, Layout));
        }
    }
}
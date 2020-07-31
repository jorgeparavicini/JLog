using System;
using UIX;
using UnityEngine;

namespace UTerm.Editor.UI
{
    [Serializable]
    public class TerminalTab : Tab
    {
        [field: SerializeField]
        public bool Active { get; set; }
    }
}
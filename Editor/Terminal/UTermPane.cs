using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTerm.Editor.Terminal
{
    [Serializable]
    public abstract class UTermPane : ScriptableObject
    {
        /// <summary>
        /// The name of the pane that should be displayed in the tab view of the terminal.
        /// </summary>
        [field: SerializeField]
        public string TabName { get; set; } = "New UTerm Pane";

        /// <summary>
        /// The state representing whether the Pane is currently displayed in the Tab View.
        /// If false the pane is only registered and can be displayed through the (+) sign dropdown.
        /// </summary>
        [field: SerializeField]
        public bool Active { get; internal set; } = true;

        /// <summary>
        /// Used to identify panes after serialization. Each pane must have a unique Uid.
        /// </summary>
        [field: SerializeField]
        public string Uid { get; protected set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// The view to be displayed when the Pane is activated.
        /// </summary>
        public abstract VisualElement View { get; }
    }
}
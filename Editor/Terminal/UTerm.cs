using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UIX;
using UIX.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTerm.Editor.Terminal
{
    public class UTerm : EditorWindow
    {
        #region Fields & Properties
        
        [SerializeField] private List<UTermPane> panes = new List<UTermPane>();

        // UI Resources
        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_Terminal");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_TerminalStyles");
        
        // UI Views
        private TabView TabView => rootVisualElement.Q<TabView>();
        
        public static UTerm Window => GetWindow<UTerm>();

        #endregion

        #region Window Initialization

        [MenuItem("Window/UTerm")]
        [UsedImplicitly]
        private static void ShowWindow()
        {
            Window.titleContent = new GUIContent("UTerm", Resources.Load<Texture>("img/terminal_white"));
        }

        private void OnEnable()
        {
            RootTree.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(StyleSheet);
            
            panes.ForEach(t =>
            {
                if (t.Active) DisplayPane(t);
            });
        }

        #endregion

        #region Public Methods

        [NotNull]
        public UTermPane GetPaneAt(int index)
        {
            if (index >= panes.Count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range");
            return panes[index];
        }

        [CanBeNull]
        public UTermPane GetPaneWithId(string paneUid)
        {
            return panes.Find(pane => pane.Uid.Equals(paneUid));
        }

        public void RegisterPane([NotNull] UTermPane pane)
        {
            if (panes.Contains(pane))
                throw new ArgumentException($"Tab {pane} is already in tablist", nameof(pane));
            panes.Add(pane);
            if (pane.Active) DisplayPane(pane);
            
        }
        
        #endregion
        
        #region Private Methods

        private void DisplayPane([NotNull] UTermPane pane)
        {
            if (TabView.Children().Any(child => child.name == pane.Uid)) return;

            var tab = new Tab {name = pane.Uid};
            tab.Add(pane.View);
            tab.AddToClassList(UixResources.ExpandClassName);
            // Bind the tab name
            tab.BindTitle(new SerializedObject(pane), "<TabName>k__BackingField");

            TabView.AddTab(tab);
        }

        #endregion
    }
}
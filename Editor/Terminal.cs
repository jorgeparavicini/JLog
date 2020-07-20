using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    public class Terminal : EditorWindow
    {
        #region Properties

        //TODO: Make Private after testing
        public static Terminal Window => GetWindow<Terminal>();

        private VisualElement TabBar => rootVisualElement.Q("jlog-terminal-tab-bar");

        #endregion

        #region UiResources

        private static readonly Lazy<VisualTreeAsset> RootTree =
            new Lazy<VisualTreeAsset>(() => Resources.Load<VisualTreeAsset>("Terminal_Main"));

        private static readonly Lazy<StyleSheet> StyleSheet =
            new Lazy<StyleSheet>(() => Resources.Load<StyleSheet>("Terminal_Main"));

        private static readonly Lazy<VisualTreeAsset> TabPreviewTree = 
            new Lazy<VisualTreeAsset>(() => Resources.Load<VisualTreeAsset>("Terminal_TabPreview"));
        
        #endregion

        #region Fields

        private readonly List<TerminalTab> _tabs = new List<TerminalTab>();

        #endregion

        #region Initializer

        [MenuItem("Window/Terminal")]
        public static void ShowWindow()
        {
            Window.titleContent = new GUIContent("Terminal");
        }

        #endregion

        #region Window Configuration

        private void OnEnable()
        {
            // The root element of the window.
            var root = rootVisualElement;
            root.styleSheets.Add(StyleSheet.Value);
            RootTree.Value.CloneTree(root);
        }

        #endregion

        #region Public Methods

        public void AddTab(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (_tabs.Contains(tab)) throw new ArgumentException("Tab has already been added", nameof(tab));
            _tabs.Add(tab);
            AddTabPreview(tab);
        }

        public void RemoveTab(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab))
                throw new ArgumentException("Tab has not been added to Terminal Window", nameof(tab));
            _tabs.Remove(tab);
        }

        public void RemoveTabAt(int tabNr)
        {
            if (tabNr < 0) throw new ArgumentException("Tab to be removed can not have negative index", nameof(tabNr));
            if (tabNr >= _tabs.Count) throw new ArgumentException("Tab to be removed is out of range", nameof(tabNr));
            _tabs.RemoveAt(tabNr);
        }

        #endregion

        #region Private Methods

        private void AddTabPreview(TerminalTab tab)
        {
            var preview = new VisualElement();
            TabPreviewTree.Value.CloneTree(preview);
            TabBar.Add(preview);
        }

        private void SelectTab(TerminalTab tab)
        {
            
        }

        // TODO: Make private
        public void RemoveAllTabPreviews()
        {
            TabBar.Clear();
        }

        #endregion
        
    }
}
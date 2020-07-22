using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    [Serializable]
    public class Terminal : EditorWindow
    {
        #region Properties

        //TODO: Make Private after testing
        public static Terminal Window => GetWindow<Terminal>();

        private VisualElement TabBar => rootVisualElement.Q("jlog-terminal-tab-bar");
        private VisualElement ContentRoot => rootVisualElement.Q("jlog-terminal-content");

        #endregion

        #region UiResources

        private static readonly Lazy<VisualTreeAsset> RootTree =
            new Lazy<VisualTreeAsset>(() => Resources.Load<VisualTreeAsset>("Terminal_Main"));

        private static readonly Lazy<StyleSheet> StyleSheet =
            new Lazy<StyleSheet>(() => Resources.Load<StyleSheet>("Terminal_MainStyle"));

        #endregion

        #region Fields
        
        private readonly List<TerminalTab> _tabs = new List<TerminalTab>();
        // TODO: Serialize this field after fixing selection error
        [SerializeField]
        private int _selectedTabNr;

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

            // Re add existing tabs.
            _selectedTabNr = 0;
            _tabs.ForEach(AddTabHeader);
            if (_tabs.Count == 0) AddNewTab();
            SelectTab(0);
        }

        #endregion

        #region Public Methods

        public void AddTab(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (_tabs.Contains(tab)) throw new ArgumentException("Tab has already been added", nameof(tab));
            if (string.IsNullOrWhiteSpace(tab.TabName))
            {
                tab.TabName = $"Tab {_tabs.Count + 1}";
            }

            _tabs.Add(tab);
            AddTabHeader(tab);
            SelectTab(tab);
        }

        public void AddNewTab()
        {
            var tab = CreateInstance<TerminalTab>();
            AddTab(tab);
        }

        public void RemoveTab(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab))
                throw new ArgumentException("Tab has not been added to Terminal Window", nameof(tab));

            var index = _tabs.IndexOf(tab);
            RemoveTabAt(index);
        }

        public void RemoveTabAt(int tabNr)
        {
            if (tabNr < 0) 
                throw new ArgumentException("Tab to be removed can not have negative index", nameof(tabNr));
            if (tabNr >= _tabs.Count) 
                throw new ArgumentException("Tab to be removed is out of range", nameof(tabNr));
            if (_tabs.Count <= 0) throw new InvalidOperationException("There are no tabs open.");
            if (_tabs.Count == 1) return;
            if (_selectedTabNr == tabNr)
            {
                if (tabNr == _tabs.Count - 1) SelectTab(tabNr - 1);
                else SelectTab(tabNr + 1);
            }
            else
            {
                _selectedTabNr -= 1;
            }
            _tabs.RemoveAt(tabNr);
            RemoveTabHeaderAt(tabNr);
        }

        public void RemoveAllTabs()
        {
            _tabs.Clear();
            RemoveAllTabHeaders();
            RemoveTabContent();
        }

        public void SelectTab(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab))
                throw new ArgumentException(
                    "Tab has not been added to terminal, but is tried to be selected",
                    nameof(tab));
            var index = _tabs.IndexOf(tab);
            if (_tabs.Count >= 1)
            {
                TabBar.Query<TabHeader>().AtIndex(_selectedTabNr).Selected = false;
            }
            TabBar.Query<TabHeader>().AtIndex(index).Selected = true;
            _selectedTabNr = index;
            
            SetTabContent(tab);
        }

        public void SelectTab(int tabNr)
        {
            if (tabNr < 0) throw new ArgumentException("Can not select negative indexed tab", nameof(tabNr));
            if (tabNr >= _tabs.Count)
                throw new ArgumentException("Tab index to be selected exceeds the amount of available tabs",
                    nameof(tabNr));
            
            SelectTab(_tabs[tabNr]);
        }

        public void Breakpoint()
        {
            Debug.Log("Hue");
        }

        #endregion

        #region Private Methods

        private void AddTabHeader(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab)) throw new ArgumentException("Tab has not been added to the registered tabs");

            var header = new TabHeader(tab);
            TabBar.Add(header);
            header.Select += () => SelectTab(tab);
            header.Close += () => RemoveTab(tab);
        }

        private void RemoveTabHeader(VisualElement header)
        {
            TabBar.Remove(header);
        }

        private void RemoveTabHeaderAt(int tabNr)
        {
            if (tabNr < 0)
                throw new ArgumentException("Preview to be removed can not be negative", nameof(tabNr));
            if (tabNr >= TabBar.childCount)
                throw new ArgumentException("Preview to be removed is out of bounds", nameof(tabNr));

            TabBar.RemoveAt(tabNr);
        }

        private void RemoveAllTabHeaders()
        {
            TabBar.Clear();
        }

        private void SetTabContent(TerminalTab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab)) throw new ArgumentException("Tab has not been added to the registered tabs");

            RemoveTabContent();
            ContentRoot.Add(tab.Content);
        }

        private void RemoveTabContent()
        {
            ContentRoot.Clear();
        }

        #endregion
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UIX;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTerm.Editor.UI
{
    public class Terminal : EditorWindow
    {
        #region Serializable Fields

        [SerializeField] private List<TerminalTab> tabs = new List<TerminalTab>();

        #endregion

        #region UI Resources

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_Terminal");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_TerminalStyles");

        #endregion

        #region Computed Properties

        public static Terminal Window => GetWindow<Terminal>();
        private TabView View => rootVisualElement.Q<TabView>();

        #endregion

        #region Properties

        public bool ShouldAddNewTabsToView { get; set; } = true;

        #endregion

        #region Initialization

        [MenuItem("Window/Terminal")]
        [UsedImplicitly]
        private static void ShowWindow()
        {
            
            Window.titleContent = new GUIContent("Terminal");
        }

        private void OnEnable()
        {
            
            RootTree.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(StyleSheet);
            
            tabs.ForEach(View.AddTab);
        }

        #endregion

        #region Public Methods
        
        [CanBeNull]
        public Tab GetTabWithName(string tabName)
        {
            return tabs.FirstOrDefault(t => t.TabName == tabName);
        }

        public void AddTab(TerminalTab tab)
        {
            if (tabs.Any(t => t.TabName == tab.TabName))
                throw new ArgumentException("A tab with the same name already exists.", nameof(tab));

            
            tabs.Add(tab);
            if (ShouldAddNewTabsToView) View.AddTab(tab);
        }
        
        #endregion
        
    }
}
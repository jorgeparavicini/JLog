using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    public class Terminal : EditorWindow
    {
        #region Constants
        
        private const string ClearButtonName = "Clear";
        
        #endregion
        
        #region Properties

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("Terminal_Main");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("Terminal_Main");
        private static Terminal Window => GetWindow<Terminal>();
        
        #endregion

        #region Fields

        private readonly List<TerminalTarget> _targets = new List<TerminalTarget>();

        #endregion
        
        #region initializer
        
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
            root.styleSheets.Add(StyleSheet);
            RootTree.CloneTree(root);

            root.Q<Button>(ClearButtonName).clickable.clicked += Clear_OnClick;
        }

        private static void Clear_OnClick(TerminalTarget target)
        {
            target.Clear();
        }
        
        #endregion

        #region Static Methods

        public static void RegisterTerminalTarget(TerminalTarget target)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (Window._targets.Contains(target))
            {
                Debug.LogError($"Target {target} already registered");
                return;
            }
            Window._targets.Add(target);
        }

        public static void UnregisterTerminalTarget(TerminalTarget target)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (!Window._targets.Contains(target))
            {
                Debug.LogError($"Target {target} has not been registered, and can therefore not be removed");
                return;
            }

            Window._targets.Remove(target);
        }

        #endregion
    }
}
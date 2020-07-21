using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    public class TabHeader : VisualElement
    {
        #region Fields

        private bool _selected;

        #endregion
        
        #region UI Resources

        private const string SelectName = "jlog-terminal-tab-header-select";
        private const string CloseName = "jlog-terminal-tab-header-close";
        private static VisualTreeAsset TabHeaderTree => Resources.Load<VisualTreeAsset>("Terminal_TabHeader");
        private static StyleSheet HeaderStyleSheet => Resources.Load<StyleSheet>("Terminal_TabHeaderStyle");

        #endregion

        #region Events

        public event Action Select = delegate { };
        public event Action Close = delegate { };

        #endregion

        #region Properties

        private VisualElement Content => Children().First();

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (Selected) Content.AddToClassList("selected");
                else Content.RemoveFromClassList("selected");
            }
        }

        #endregion

        #region Constructors

        // ReSharper disable once SuggestBaseTypeForParameter
        public TabHeader(TerminalTab tab)
        {
            TabHeaderTree.CloneTree(this);
            styleSheets.Add(HeaderStyleSheet);
            this.Bind(new SerializedObject(tab));

            this.Q<Button>(SelectName).clickable.clicked += () => Select();
            this.Q<Button>(CloseName).clickable.clicked += () => Close();
        }


        #endregion

    }
}
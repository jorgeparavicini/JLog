using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    [Serializable]
    public class TerminalTab : ScriptableObject
    {

        #region UI Resources

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("Terminal_Tab");

        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("Terminal_TabStyle");

        #endregion

        #region Serialized Fields

        [SerializeField] private string _tabName;

        #endregion

        #region Properties

        public virtual VisualElement Content { get; private set; }

        public string TabName
        {
            get => _tabName;
            set => _tabName = value;
        }

        #endregion

        #region Unity Events

        private void OnEnable()
        {
            var c = new VisualElement();
            c.style.flexGrow = new StyleFloat(1);
            Content = PopulateContent(c);
        }


        #endregion

        #region Virtual Methods

        protected virtual VisualElement PopulateContent(VisualElement content)
        {
            RootTree.CloneTree(content);
            content.styleSheets.Add(StyleSheet);
            content.Bind(new SerializedObject(this));
            return content;
        }
        
        #endregion

    }
}
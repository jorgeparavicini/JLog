using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    [Serializable]
    public class TerminalTab : ScriptableObject
    {

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
            Content = CreateContent();
        }


        #endregion

        #region Virtual Methods

        protected virtual VisualElement CreateContent()
        {
            return new VisualElement();
        }


        #endregion

    }
}
using System;
using System.Collections.Generic;
using UIX;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTerm.Editor.UI
{
    [Serializable]
    public class NLogTab : TerminalTab, ISerializationCallbackReceiver
    {

        #region Constants

        private const string ListViewName = "uterm-nlog-tab";

        #endregion

        #region UI Resources

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_NLogTab");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_NLogTabStyles");

        #endregion
        
        #region Serializable Fields

        [SerializeField] private List<NLogEntry> entries = new List<NLogEntry>();

        #endregion
        
        #region Constructors

        public NLogTab()
        {
            RootTree.CloneTree(this);
            styleSheets.Add(StyleSheet);
            InitializeListView();
        }
        

        #endregion
        
        #region Public Methods

        public void AddEntry(NLogEntry entry)
        {
            entries.Add(entry);
            this.Q<ListView>(ListViewName).Refresh();
            
        }

        #endregion

        #region Private Methods

        private void InitializeListView()
        {
            var listView = this.Q<ListView>(ListViewName);
            listView.itemsSource = entries;
            listView.makeItem = () => new TerminalTabLine();
            listView.bindItem = (element, i) => ((TerminalTabLine) element).Text = entries[i].RenderedMessage;
            listView.itemHeight = 24;
        }

        #endregion

        #region Serialization

        public void OnBeforeSerialize()
        {
            
            Debug.Log("Before");
        }

        public void OnAfterDeserialize()
        {
            Debug.Log("After");
        }

        #endregion


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using UIX.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UTerm.Editor.Terminal;
using UTerm.Editor.UI;

namespace UTerm.Editor.NLog
{
    [InitializeOnLoad]
    [Serializable]
    public class NLogPane : UTermPane
    {
        #region Fields & Properties

        private const string ListViewName = "uterm-nlog-tab";
        private const string NotInitializedLabelName = "uterm-nlog-tab__not-initialized";

        [NonSerialized] private VisualElement _view;
        [NonSerialized] private NLogTarget _target;

        [SerializeField] private List<NLogEntry> entries = new List<NLogEntry>();

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_NLogTab");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_NLogTabStyles");
        
        public override VisualElement View => _view;
        public bool Initialized => _target != null;

        #endregion

        #region Initialization

        static NLogPane()
        {
            NLogTarget.TargetInitialized += NLogTargetOnTargetInitialized;
        }

        private static void NLogTargetOnTargetInitialized(NLogTarget target)
        {
            var existingPane = Terminal.UTerm.Window.GetPaneWithId(target.Name);
            switch (existingPane)
            {
                // UTerm will handle the creation of this pane.
                case NLogPane pane: 
                    pane.Initialize(target);
                    break;
                case { }:
                    throw new InvalidOperationException(
                        $"Can not create new NLogPane as there is already a pane with the same name registered. Target name: {target.Name}");
                case null:
                    var newPane = CreateInstance<NLogPane>();
                    newPane.Uid = target.Name;
                    newPane.Initialize(target);
                    Terminal.UTerm.Window.RegisterPane(newPane);
                    break;
            }
        }

        private void OnEnable()
        {
            _view = new VisualElement();
            RootTree.CloneTree(_view);
            
            _view.AddToClassList(UixResources.ExpandClassName);
            _view.styleSheets.Add(StyleSheet); 
            HideListView();
        }

        private void Initialize(NLogTarget target)
        {
            if (Initialized) throw new InvalidOperationException("Pane has already been initialized");
            if (target.Name != Uid)
                throw new ArgumentException("The passed targets name does not match this panes uid", nameof(target));
            
            _target = target;
            TabName = _target.TabName;
            
            InitializeListView();
            DisplayListView();
            HideNotInitializedWarning();
            
            _target.DidWrite += TargetOnDidWrite;
        }

        private void DisplayListView()
        {
            _view.Q<VisualElement>(ListViewName).style.display = DisplayStyle.Flex;
        }

        private void HideListView()
        {
            _view.Q<VisualElement>(ListViewName).style.display = DisplayStyle.None;
        }
        
        private void InitializeListView()
        {
            if (_target is null) throw new InvalidOperationException("Can not initialize List View without a target");

            var listView = _view.Q<ListView>(ListViewName);
            listView.itemsSource = entries;
            listView.makeItem = () => new TerminalTabLine();
            listView.bindItem = (element, i) => ((TerminalTabLine) element).Text = entries[i].RenderedMessage;
            listView.itemHeight = 24;
        }

        private void HideNotInitializedWarning()
        {
            _view.Q<Label>(NotInitializedLabelName).RemoveFromHierarchy();
        }
        
        #endregion

        #region Target Events

        private void TargetOnDidWrite(LogEventInfo obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
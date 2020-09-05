using System;
using System.Collections.Generic;
using NLog;
using UIX.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UTerm.Editor.Terminal;

namespace UTerm.Editor.NLog
{
    [InitializeOnLoad]
    [Serializable]
    public class NLogPane : UTermPane
    {
        #region Fields & Properties

        private const string ListViewName = "uterm-nlog__list";
        private const string NotInitializedLabelName = "uterm-nlog__not-initialized";
        private static Queue<Action> _nLogQueue = new Queue<Action>();

        [NonSerialized] private VisualElement _view;
        [NonSerialized] private ListView _listView;
        [NonSerialized] private NLogTarget _target;

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_NLogTab");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_NLogTabStyles");

        private readonly Lazy<StyleSheet> _nlogEntryStyleSheet =
            new Lazy<StyleSheet>(() => Resources.Load<StyleSheet>("UTerm_NLogEntryStyles"));


        public override VisualElement View => _view ??= new VisualElement();
        public bool Initialized => _target != null;

        public Dictionary<LogLevel, StyleColor> foregroundColors = new Dictionary<LogLevel, StyleColor>
        {
            [LogLevel.Trace] = new StyleColor(new Color(66, 66, 66)),
            [LogLevel.Debug] = new StyleColor(Color.gray),
            [LogLevel.Info] = new StyleColor(Color.white),
            [LogLevel.Warn] = new StyleColor(Color.magenta),
            [LogLevel.Error] = new StyleColor(Color.yellow),
            [LogLevel.Fatal] = new StyleColor(Color.red)
        };

        #endregion

        #region Initialization

        static NLogPane()
        {
            EditorApplication.update += NLogDispatcher;
            NLogTarget.TargetInitialized += NLogTargetOnTargetInitialized;
        }

        private static void NLogDispatcher()
        {
            if (_nLogQueue.Count == 0) return;
            lock (_nLogQueue)
            {
                while (_nLogQueue.Count > 0)
                {
                    _nLogQueue.Dequeue()();
                }
            }
        }

        private static void NLogTargetOnTargetInitialized(NLogTarget target)
        {
            lock (_nLogQueue)
            {
                _nLogQueue.Enqueue(() =>
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
                });
            }
        }

        private void OnEnable()
        {
            RootTree.CloneTree(View);
            View.AddToClassList(UixResources.ExpandClassName);
            View.styleSheets.Add(StyleSheet);
            _listView = View.Q<ListView>(ListViewName);
            HideListView();

            View.Q<Button>("uterm-nlog__clear").clicked += () =>
            {
                _target.Logs.Clear();
                _listView.Refresh();
            };
        }

        private void Initialize(NLogTarget target)
        {
            if (target.Name != Uid)
                throw new ArgumentException("The passed targets name does not match this panes uid", nameof(target));
            if (!Initialized)
            {
                DisplayListView();
                HideNotInitializedWarning();
            }
            else
            {
                // Unsubscribe the old target
                _target.DidWrite -= TargetOnDidWrite;
            }


            _target = target;
            TabName = _target.TabName;
            InitializeListView();
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

            _listView.itemsSource = _target.Logs;
            _listView.makeItem = () => new Label();
            _listView.bindItem = (element, i) =>
            {
                var label = (Label) element;
                label.styleSheets.Add(_nlogEntryStyleSheet.Value);
                label.text = _target.Logs[i].RenderedMessage;
                label.style.color =
                    foregroundColors.TryGetValue(LogLevel.FromString(_target.Logs[i].LogLevelName), out var color)
                        ? color
                        : new StyleColor(Color.white);
            };
            _listView.itemHeight = 24;
            _listView.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
        }

        private void HideNotInitializedWarning()
        {
            _view.Q<Label>(NotInitializedLabelName).RemoveFromHierarchy();
        }

        #endregion

        #region Target Events

        private void TargetOnDidWrite(LogEventInfo _)
        {
            _listView.Refresh();
        }

        #endregion
    }
}
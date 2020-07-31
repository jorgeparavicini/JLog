using UnityEngine;
using UnityEngine.UIElements;

namespace UTerm.Editor.UI
{
    public class TerminalTabLine : VisualElement
    {
        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UTerm_TerminalTabLine");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UTerm_TerminalTabLineStyles");

        private readonly Label _text;
        
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        
        public TerminalTabLine()
        {
            RootTree.CloneTree(this);
            styleSheets.Add(StyleSheet);
            _text = this.Q<Label>("uix-terminal-entry__text");
        }
        
    }
}
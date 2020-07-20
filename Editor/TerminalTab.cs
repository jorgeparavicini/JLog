using UnityEngine;
using UnityEngine.UIElements;

namespace JLog.Editor
{
    public class TerminalTab : VisualElement
    {
        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("Terminal_Tab");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("Terminal_Tab");
        
        public string Name { get; set; }
        
        public TerminalTab(string name)
        {
            RootTree.CloneTree(this);
            styleSheets.Add(StyleSheet);
            Name = name;
        }
    }
}
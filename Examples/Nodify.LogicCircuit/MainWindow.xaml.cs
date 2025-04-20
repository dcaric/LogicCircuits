//using System.Windows;
using Avalonia.Controls;

namespace Nodify.LogicCircuit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            EditorGestures.Mappings.Editor.Cutting.Value = MultiGesture.None;
        }
    }
}

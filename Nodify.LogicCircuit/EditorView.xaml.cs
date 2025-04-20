/*using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;*/
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Nodify.LogicCircuit
{
    public partial class EditorView : UserControl
    {
        public EditorView()
        {
            InitializeComponent();

            PointerPressedEvent.AddClassHandler<NodifyEditor>(CloseOperationsMenuPointerPressed);
            ItemContainer.DragStartedEvent.AddClassHandler<ItemContainer>(CloseOperationsMenu);
            PointerReleasedEvent.AddClassHandler<NodifyEditor>(OpenOperationsMenu);
            Editor.AddHandler(DragDrop.DropEvent, OnDropNode);
        }
        
        private void OpenOperationsMenu(object? sender, PointerReleasedEventArgs e)
        {
            if (!e.Handled && e.Source is NodifyEditor editor && !editor.IsPanning && editor.DataContext is LogicCircuitViewModel logicCircuit &&
                e.InitialPressMouseButton == MouseButton.Right)
            {
                e.Handled = true;
                logicCircuit.OperationsMenu.OpenAt(editor.MouseLocation);
            }
        }

        private void CloseOperationsMenuPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
                CloseOperationsMenu(sender, e);
        }
        
        private void CloseOperationsMenu(object? sender, RoutedEventArgs e)
        {
            ItemContainer? itemContainer = sender as ItemContainer;
            NodifyEditor? editor = sender as NodifyEditor ?? itemContainer?.Editor;

            if (!e.Handled && editor?.DataContext is LogicCircuitViewModel logicCircuit)
            {
                logicCircuit.OperationsMenu.Close();
            }
        }

        private void OnDropNode(object? sender, DragEventArgs e)
        {
            NodifyEditor? editor = (e.Source as NodifyEditor) ?? (e.Source as Control)?.GetLogicalParent() as NodifyEditor;
            if(editor != null && editor.DataContext is LogicCircuitViewModel logicCircuit
                && e.Data.Get(typeof(OperationInfoViewModel).FullName) is OperationInfoViewModel operation)
            {
                OperationViewModel op = OperationFactory.GetOperation(operation);
                op.Location = editor.GetLocationInsideEditor(e);
                logicCircuit.Operations.Add(op);

                e.Handled = true;
            }
        }
        
        private void OnNodeDrag(object? sender, MouseEventArgs e)
        {
            if(leftButtonPressed && ((Control)sender).DataContext is OperationInfoViewModel operation)
            {
                var data = new DataObject();
                data.Set(typeof(OperationInfoViewModel).FullName, operation);
                DragDrop.DoDragDrop(e, data, DragDropEffects.Copy);
            }
        }

        private void OnNodePressed(object? sender, PointerPressedEventArgs e)
        {
            leftButtonPressed = e.GetCurrentPoint(this).Properties.PointerUpdateKind ==
                                PointerUpdateKind.LeftButtonPressed;
        }

        private void OnNodeExited(object? sender, PointerEventArgs e)
        {
            leftButtonPressed = false;
        }
        
        private bool leftButtonPressed;
    }
}

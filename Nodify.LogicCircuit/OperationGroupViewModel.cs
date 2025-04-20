// -----------------------------------------------------------------------------
// OperationGroupViewModel.cs
//
//  Purpose:
// Represents a **grouping node** in the logic circuit editor.
// Unlike functional nodes (AND, OR, Expression, etc.), this node does **not** perform
// logic operations — it's used to visually organize and cluster related nodes.
//
//  Key Feature:
//
// -GroupSize
//     → Specifies the width and height of the visual group block
//     → Can be set to fit a bounding box around selected nodes
//     → Used by the UI to render a background container or frame
//
//  Use Case:
//
// - User selects multiple nodes and groups them together
// - A Group node is created with the bounding box of the selection
// - Helps with visual clarity and layout in large graphs
//
//  Notes:
//
// - Inherits from OperationViewModel, so it appears like any node
// - Usually has no connectors or logic
// - Supports selection and dragging like other nodes
//
//  Potential Future Features:
// - Add group label
// - Collapse/expand contained nodes
// - Visual highlighting or background colors
//
// -----------------------------------------------------------------------------

using System.Windows;

namespace Nodify.LogicCircuit
{
    public class OperationGroupViewModel : OperationViewModel
    {
        private Size _size;
        public Size GroupSize
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }
    }
}
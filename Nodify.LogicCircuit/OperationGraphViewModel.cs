// -----------------------------------------------------------------------------
// OperationGraphViewModel.cs
//
//  Purpose:
// A specialized nested circuit node that supports **expand/collapse** behavior.
// It stores and adjusts its size dynamically and acts like a visual logic
// container or group that can optionally hide or show its inner circuit contents.
//
// This ViewModel inherits from `LogicCircuitOperationViewModel`,
// so it behaves like a regular subcircuit node — but adds UI state logic
// for expand/collapse and manual sizing.
//
//  Key Features:
//
// - `IsExpanded`
//     ? Determines whether the node is expanded (show inner logic)
//     ? When collapsed, node resizes to 0x0 to visually shrink
//     ? When expanded again, restores previous size
//
// - `DesiredSize`
//     ? The preferred size of this node when rendered in the UI
//     ? Set manually or restored from `_prevSize`
//     ? Can be used by the UI to adjust layout or container space
//
// - `InnerLogicCircuit.Operations`
//     ? On construction, sets default positions of inner input/output nodes
//     ? Places them visually for better layout when first opened
//
//  Example Use Case:
// - A "Math Block" node containing a reusable logic pattern
// - The user double-clicks it to see internal logic
// - The UI can visually collapse the block to save space
//
//  Usage in UI:
// - Usually displayed with a toggle (arrow or expander button)
// - Collapsing may hide internal connectors/logic for clarity
//
// -----------------------------------------------------------------------------
// Notes:
// - `_prevSize` is used to remember layout before collapsing
// - `DesiredSize = new Size(0, 0)` simulates collapse
// - This node supports UI interactivity but keeps inner logic intact
// -----------------------------------------------------------------------------
// Feature	                        Role
// IsExpanded                       Controls whether inner circuit is shown or hidden
// DesiredSize	                    Tells the UI how big the node should appear
// InnerLogicCircuit.Operations	    Default positioning of inner input/output
// Inherits from	                LogicCircuitOperationViewModel (nested logic node)
// -----------------------------------------------------------------------------



using System.Windows;

namespace Nodify.LogicCircuit
{
    public class OperationGraphViewModel : LogicCircuitOperationViewModel
    {
        private Size _size;
        public Size DesiredSize
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }
        
        private Size _prevSize;

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (SetProperty(ref _isExpanded, value))
                {
                    if (_isExpanded)
                    {
                        DesiredSize = _prevSize;
                    }
                    else
                    {
                        _prevSize = Size;
                        // Fit content
                        DesiredSize = new Size(0,0);
                    }
                }
            }
        }

        public OperationGraphViewModel()
        {
            InnerLogicCircuit.Operations[0].Location = new Point(50, 50);
            InnerLogicCircuit.Operations[1].Location = new Point(200, 50);
        }
    }
}
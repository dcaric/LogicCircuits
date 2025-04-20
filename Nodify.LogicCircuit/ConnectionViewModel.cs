// -----------------------------------------------
// ConnectionViewModel.cs
//
// This class represents a visual connection (or wire)
// between two node connectors in a node-based editor.
//
// It stores references to:
//   - An input connector (target)
//   - An output connector (source)
//
// It inherits from ObservableObject so that property
// changes notify the UI (for binding in GraphView).
//
// Used by GraphView to draw lines between connectors.
//
// GraphView is the core visual canvas control provided
// by the Nodify library. It's a WPF control that:
// Hosts and displays nodes
// Lets you drag them around
// Shows and manages connections (lines/wires) between them
// Handles zooming, panning, selection, etc.
// -----------------------------------------------

namespace Nodify.LogicCircuit
{
    public class ConnectionViewModel : ObservableObject
    {
        private ConnectorViewModel _input = default!;
        public ConnectorViewModel Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        private ConnectorViewModel _output = default!;
        public ConnectorViewModel Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }
    }
}

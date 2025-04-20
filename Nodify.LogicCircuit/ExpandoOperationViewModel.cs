// -----------------------------------------------------------------------------
// ExpandoOperationViewModel.cs
//
//  Purpose:
// This ViewModel represents a special type of logic node that supports a
// **dynamic number of input connectors**. It is designed to "expand" and
// adapt to the user’s needs — for example, summing multiple values,
// performing a multi-input logic AND, etc.
//
// This is useful in node-based editors when you want the user to decide
// how many inputs a node should have.
//
//  Key Features:
//
// - Inherits from `OperationViewModel` to behave like a standard node
// - Adds/removes inputs dynamically using two commands:
//     → `AddInputCommand`
//     → `RemoveInputCommand`
//
// - Exposes constraints via `MinInput` and `MaxInput` to control the bounds
//   of how many inputs the node is allowed to have.
//
// - Reactively updates button/command availability when inputs are changed.
//
//  Constructor:
// - Sets up `RequeryCommand`s for adding/removing inputs
// - Hooks `Input.WhenAdded` and `Input.WhenRemoved` events to update command
//   executability (CanExecute)
//
//  Properties:
//
// - `MinInput`: Minimum number of allowed inputs (default = 0)
// - `MaxInput`: Maximum number of allowed inputs (default = unlimited)
//
//  Usage in UI:
// - Typically paired with "+" and "−" buttons on the node
// - Each press of "+" calls `AddInputCommand` to add a connector
// - Pressing "−" removes the last input, respecting the `MinInput` bound
//
//  Example Use Case:
// - A SUM node that can add 2, 3, or 10 numbers
// - A custom OR gate with 2 to N inputs
//
// Thing                            Description
// ExpandoOperationViewModel	    A node that allows the user to add/remove input connectors
// AddInputCommand	                Adds a new input connector
// RemoveInputCommand               Removes the last input connector
// MinInput / MaxInput	            Boundaries for input count
// Event hooks	                    Keep UI buttons enabled/disabled appropriately
// -----------------------------------------------------------------------------


namespace Nodify.LogicCircuit
{
    public class ExpandoOperationViewModel : OperationViewModel
    {
        public ExpandoOperationViewModel()
        {
            AddInputCommand = new RequeryCommand(
                () => Input.Add(new ConnectorViewModel()),
                () => Input.Count < MaxInput);

            RemoveInputCommand = new RequeryCommand(
                () => Input.RemoveAt(Input.Count - 1),
                () => Input.Count > MinInput);

            Input.WhenAdded(_ => AddInputCommand.RaiseCanExecuteChanged());
            Input.WhenRemoved(_ => AddInputCommand.RaiseCanExecuteChanged());
            Input.WhenAdded(_ => RemoveInputCommand.RaiseCanExecuteChanged());
            Input.WhenRemoved(_ => RemoveInputCommand.RaiseCanExecuteChanged());
        }

        public INodifyCommand AddInputCommand { get; }
        public INodifyCommand RemoveInputCommand { get; }

        private uint _minInput = 0;
        public uint MinInput
        {
            get => _minInput;
            set => SetProperty(ref _minInput, value);
        }

        private uint _maxInput = uint.MaxValue;
        public uint MaxInput
        {
            get => _maxInput;
            set => SetProperty(ref _maxInput, value);
        }
    }
}

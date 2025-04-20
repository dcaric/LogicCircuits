// -----------------------------------------------------------------------------
// LogicCircuitInputOperationViewModel.cs
//
//  Purpose:
// This ViewModel represents a **dynamic input node** inside a nested logic circuit.
// It’s used in composite (nested) nodes to simulate external inputs being passed in.
//
// This node exposes **one or more output connectors**, which serve as "input parameters"
// for the rest of the sub-circuit. Its output values are set externally by the
// parent node (`LogicCircuitOperationViewModel`).
//
//  Key Features:
//
// - Inherits from `OperationViewModel`
// - Replaces base `Output` with a collection of dynamic outputs
// - Exposes two commands:
//     → `AddOutputCommand`    → Adds a new output (simulates a new input param)
//     → `RemoveOutputCommand` → Removes the last output (if more than 1)
//
//  Behavior:
// - Initializes with one default output labeled "In 0"
// - Each output represents an external input passed into the nested circuit
// - Allows dynamic adjustment of input count (e.g., In 0, In 1, In 2, ...)
// - Limited to a maximum of 10 inputs
//
//  Used By:
// - `LogicCircuitOperationViewModel`
//     → When inputs are passed into a nested sub-circuit,
//       their values are routed to these outputs.
//
//  UI Integration:
// - The commands are usually bound to "+" and "−" buttons in the UI
// - Output connector names are updated dynamically
//
// -----------------------------------------------------------------------------
// Example Use Case:
// - A reusable circuit (like "Half Adder") defines inputs: A, B
// - The parent circuit assigns values to those inputs via this ViewModel’s outputs
//
// In effect, this node acts as a bridge between outer inputs and internal logic.
// -----------------------------------------------------------------------------

namespace Nodify.LogicCircuit
{
    public class LogicCircuitInputOperationViewModel : OperationViewModel
    {
        public LogicCircuitInputOperationViewModel()
        {
            AddOutputCommand = new RequeryCommand(
                () => Output.Add(new ConnectorViewModel
                {
                    Title = $"In {Output.Count}"
                }),
                () => Output.Count < 10);

            RemoveOutputCommand = new RequeryCommand(
                () => Output.RemoveAt(Output.Count - 1),
                () => Output.Count > 1);

            Output.Add(new ConnectorViewModel
            {
                Title = $"In {Output.Count}"
            });
        }

        public new NodifyObservableCollection<ConnectorViewModel> Output { get; set; } =
            new NodifyObservableCollection<ConnectorViewModel>();

        public INodifyCommand AddOutputCommand { get; }
        public INodifyCommand RemoveOutputCommand { get; }
    }
}

// -----------------------------------------------------------------------------
// LogicCircuitOperationViewModel.cs
//
//  Purpose:
// This ViewModel defines a special type of node — a **composite logic circuit**.
// It's a "black box" node that **contains its own inner graph**, allowing users
// to build **nested, reusable circuits** (e.g., Half Adder, Full Adder, etc.).
//
// This class extends `OperationViewModel` to behave like a normal node on the
// main canvas, but internally holds its own `LogicCircuitViewModel`.
//
//  Key Features:
//
// - `InnerLogicCircuit`
//     → Contains a self-contained logic graph (its own Operations, Connections)
//
// - `InnerInput`
//     → A special input node inside the nested graph (visually labeled "Input Parameters")
//     → Exposes outputs that simulate incoming values from the outside
//
// - `InnerOutput`
//     → A special output node inside the nested graph (visually labeled "Output Parameters")
//     → Has a single input, used to route data back to the parent circuit
//
// - `Input` (inherited)
//     → Populated based on `InnerInput.Output`, dynamically syncs labels
//
// - `Output` (inherited)
//     → Connected to `InnerOutput.Input[0]`, so nested calculation result is exposed
//
//  Constructor:
// - Adds the inner input/output nodes to `InnerLogicCircuit`
// - Wires up syncing between outer `Input` and `InnerInput.Output`
// - Registers change listeners to keep input names aligned
// - Observes value on `InnerOutput.Input[0]`, pushes it to this node’s `Output`
//
//  Method Override:
// - `OnInputValueChanged()`
//     → Propagates parent node input values into the nested circuit
//     → Updates `InnerInput.Output[i].Value` with values from this node’s `Input[i]`
//
//  Result:
// This node behaves like a regular logic gate (from the outside),
// but executes an entire sub-circuit under the hood — enabling hierarchical design.
//
// -----------------------------------------------------------------------------
// Example Use Case:
// - Create a node called "Half Adder"
// - Inside, wire up an XOR and AND gate with labeled inputs/outputs
// - Add it to a higher-level circuit like "Full Adder" as a black box
//
// This is similar to how modular circuits are built in FPGA or logic simulators.
// -----------------------------------------------------------------------------

using System.Windows;

namespace Nodify.LogicCircuit
{
    public class LogicCircuitOperationViewModel : OperationViewModel
    {
        public LogicCircuitViewModel InnerLogicCircuit { get; } = new LogicCircuitViewModel();

        private OperationViewModel InnerOutput { get; } = new OperationViewModel
        {
            Title = "Output Parameters",
            Input = { new ConnectorViewModel() },
            Location = new Point(500, 300),
            IsReadOnly = true
        };

        private LogicCircuitInputOperationViewModel InnerInput { get; } = new LogicCircuitInputOperationViewModel
        {
            Title = "Input Parameters",
            Location = new Point(300, 300),
            IsReadOnly = true
        };

        public LogicCircuitOperationViewModel()
        {
            InnerLogicCircuit.Operations.Add(InnerInput);
            InnerLogicCircuit.Operations.Add(InnerOutput);

            Output = new ConnectorViewModel();

            InnerOutput.Input[0].ValueObservers.Add(Output);

            InnerInput.Output.ForEach(x => Input.Add(new ConnectorViewModel
            {
                Title = x.Title
            }));

            InnerInput.Output
                .WhenAdded(x => Input.Add(new ConnectorViewModel
                {
                    Title = x.Title
                }))
                .WhenRemoved(x => Input.RemoveOne(i => i.Title == x.Title));
        }

        protected override void OnInputValueChanged()
        {
            for (var i = 0; i < Input.Count; i++)
            {
                InnerInput.Output[i].Value = Input[i].Value;
            }
        }
    }
}

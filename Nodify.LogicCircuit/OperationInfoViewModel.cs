// -----------------------------------------------------------------------------
// OperationInfoViewModel.cs
//
// Contains metadata about a logic operation (node) that can be added to the graph.
// This is used primarily in the operations menu (toolbox) to define the types of
// operations available to the user.
//
//  OperationType Enum:
// Describes the type of operation node to create.
//
//   - Normal       → Fixed number of inputs and outputs (e.g., AND, OR)
//   - Expando      → Dynamic number of inputs (e.g., Sum, Multi-input gate)
//   - Expression   → Custom expression node (e.g., user-defined math)
//   - LogicCircuit → Composite logic node (can contain nested circuits)
//   - Group        → Visual/organizational grouping node
//   - Graph        → Placeholder for a complex node (e.g., operation container)
//
//  OperationInfoViewModel:
// Used to represent a single node template in the UI toolbox.
//
//   - `Title`        → Display name (e.g., "AND Gate")
//   - `Type`         → The type of node to render (from OperationType enum)
//   - `Operation`    → The logic/function to execute (optional, set dynamically)
//   - `Input`        → List of input names (used for connector labeling)
//   - `MinInput`     → Minimum number of inputs required
//   - `MaxInput`     → Maximum number of inputs allowed
//
//  Used in:
// - OperationsMenuViewModel (list of available operations)
// - OperationFactory (to generate node templates dynamically)
// - Node creation and rendering templates (e.g., in the editor canvas)
//
// -----------------------------------------------------------------------------


using System.Collections.Generic;

namespace Nodify.LogicCircuit
{
    public enum OperationType
    {
        Normal,
        Expando,
        Expression,
        LogicCircuit,
        Group,
        Graph,
        ComplexCircuits
    }

    public class OperationInfoViewModel
    {
        public string? Title { get; set; }
        public OperationType Type { get; set; }
        public IOperation? Operation { get; set; }
        public List<string?> Input { get; } = new List<string?>();

        public string[]? InputLabels { get; set; }

        public List<string?> Output { get; set; } = new(); // new multiple outputs
        public string[]? OutputLabels { get; set; }

        public uint MinInput { get; set; }
        public uint MaxInput { get; set; }
    }
}

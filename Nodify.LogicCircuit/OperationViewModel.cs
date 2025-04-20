// -----------------------------------------------------------------------------
// OperationViewModel.cs
//
// Represents a single logic operation node in the visual node editor.
//
//  Purpose:
// This class models a logic gate or mathematical operation in the editor.
// It connects to input/output connectors, listens for input value changes,
// and updates its output accordingly by executing the assigned IOperation.
//
//  Key Features:
// - Holds visual properties (location, size, title, selection)
// - Contains input/output connectors (used for wiring up in the UI)
// - Reacts to value changes in inputs to trigger re-evaluation
// - Supports executing operations like AND, OR, NOT, custom expressions
//
//  Constructor:
// - Subscribes to input connector changes (added/removed)
// - Hooks up change listeners to track value updates
//
//  Properties:
// - `Input`: A collection of input connectors
// - `Output`: A single output connector
// - `Operation`: The logic operation to execute
// - `Title`: Node label (e.g. "AND", "NOT")
// - `Location`, `Size`: Position/size of the node in the editor
// - `IsSelected`: Used to track UI selection state
// - `IsReadOnly`: Optional flag to make the node immutable
//
//  Methods:
// - `OnInputValueChanged()`:
//     Triggered when any input's value changes
//     → Collects input values
//     → Executes the `IOperation`
//     → Assigns the result to `Output.Value`
//     → Used for live evaluation in the graph
//
// - `OnInputValueChanged(object?, PropertyChangedEventArgs)`:
//     Called by input connector change notifications
//     → Filters only on `Value` changes
//     → Triggers main re-evaluation
//
// -----------------------------------------------------------------------------
// Example:
//   A NOT node receives input value 0
//   -> This triggers PropertyChanged on the input
//   -> OnInputValueChanged runs
//   -> Operation.Execute([0]) = 1
//   -> Output.Value becomes 1
//
// -----------------------------------------------------------------------------


using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public class OperationViewModel : ObservableObject
    {
        public OperationViewModel()
        {
            Input.WhenAdded(x =>
            {
                x.Operation = this;
                x.IsInput = true;
                x.PropertyChanged += OnInputValueChanged;
            })
            .WhenRemoved(x =>
            {
                x.PropertyChanged -= OnInputValueChanged;
            });
        }

        private void OnInputValueChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConnectorViewModel.Value))
            {
                OnInputValueChanged();
            }
        }

        private Point _location;
        public Point Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private Size _size;
        public Size Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public bool IsReadOnly { get; set; }

        private IOperation? _operation;
        public IOperation? Operation
        {
            get => _operation;
            set => SetProperty(ref _operation, value)
                .Then(OnInputValueChanged);
        }

        public NodifyObservableCollection<ConnectorViewModel> Input { get; } = new NodifyObservableCollection<ConnectorViewModel>();

        private ConnectorViewModel? _output;
        public ConnectorViewModel? Output
        {
            get => _output;
            set
            {
                if (SetProperty(ref _output, value) && _output != null)
                {
                    _output.Operation = this;
                }
            }
        }

        protected virtual void OnInputValueChanged()
        {
            if (Output != null && Operation != null)
            {
                try
                {
                    var input = Input.Select(i => i.Value).ToArray();
                    Output.Value = Operation?.Execute(input) ?? 0;
                }
                catch
                {

                }
            }
        }
    }
}

// -----------------------------------------------------------------------------
//  HART OF THE LOGIC
//
// LogicCircuitViewModel.cs
//
//  Purpose:
// This ViewModel represents the **entire logic circuit graph**.
// It manages:
// - Nodes (logic operations)
// - Connections between nodes
// - Selection
// - Connection building (with drag-to-connect support)
// - Integration with the operations menu (toolbox)
//
// It acts as the **core engine** behind the visual editor surface.
//
//  Key Responsibilities:
//
// 1. Operations (nodes):
//    - Stored in `Operations`
//    - Reactively listens to input/output changes
//    - Automatically removes connections if inputs/outputs are removed
//
// 2. Connections (wires):
//    - Stored in `Connections`
//    - Automatically sets `IsConnected` flags and synchronizes values
//    - Manages `ValueObservers` to propagate logic values
//
// 3. Selection:
//    - Tracked via `SelectedOperations`
//    - Used for deleting or grouping nodes (future functionality)
//
// 4. Pending Connection (live wire drag):
//    - Stored in `PendingConnection`
//    - Used when user starts dragging a wire
//    - If wire is dropped without a target, opens the toolbox (`OperationsMenu`)
//
// 5. Commands (bindable actions for the UI):
//    - `StartConnectionCommand`     → begins dragging a wire from a connector
//    - `CreateConnectionCommand`    → finalizes a wire between two connectors
//    - `DisconnectConnectorCommand`→ removes all connections to a connector
//    - `DeleteSelectionCommand`     → deletes selected nodes
//    - `GroupSelectionCommand`      → (commented out) groups nodes visually
//
//  Constructor:
// - Wires up reactive listeners on `Operations` and `Connections` collections
// - Subscribes to add/remove events to manage connector state and observers
// - Initializes commands and the `OperationsMenu`
//
// -----------------------------------------------------------------------------
//  Internal Methods:
//
// - `CanCreateConnection(source, target)`:
//     Validates that the two connectors are different, on different nodes,
//     and one is input while the other is output.
//
// - `CreateConnection(source, target)`:
//     If target is null, opens the operations menu at drag location.
//     If valid, removes any existing connection and creates a new one.
//
// - `DisconnectConnector(connector)`:
//     Removes all connections related to a given input/output.
//
// - `DeleteSelection()`:
//     Deletes all currently selected nodes and their connections.
//
// -----------------------------------------------------------------------------
//  Used In:
// - Main editor canvas view
// - EditorViewModel (host of this LogicCircuit)
// - OperationsMenuViewModel (to inject nodes on user request)
//
// -----------------------------------------------------------------------------


using System.Linq;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public class LogicCircuitViewModel : ObservableObject
    {
        public LogicCircuitViewModel()
        {
            CreateConnectionCommand = new DelegateCommand<ConnectorViewModel>(
                _ => CreateConnection(PendingConnection.Source, PendingConnection.Target),
                _ => CanCreateConnection(PendingConnection.Source, PendingConnection.Target));
            StartConnectionCommand = new DelegateCommand<ConnectorViewModel>(_ => PendingConnection.IsVisible = true, (c) => !(c.IsConnected && c.IsInput));
            DisconnectConnectorCommand = new DelegateCommand<ConnectorViewModel>(DisconnectConnector);
            DeleteSelectionCommand = new DelegateCommand(DeleteSelection);
            GroupSelectionCommand = new DelegateCommand(GroupSelectedOperations, () => SelectedOperations.Count > 0);

            Connections.WhenAdded(c =>
            {
                c.Input.IsConnected = true;
                c.Output.IsConnected = true;

                c.Input.Value = c.Output.Value;

                c.Output.ValueObservers.Add(c.Input);
            })
            .WhenRemoved(c =>
            {
                var ic = Connections.Count(con => con.Input == c.Input || con.Output == c.Input);
                var oc = Connections.Count(con => con.Input == c.Output || con.Output == c.Output);

                if (ic == 0)
                {
                    c.Input.IsConnected = false;
                }

                if (oc == 0)
                {
                    c.Output.IsConnected = false;
                }

                c.Output.ValueObservers.Remove(c.Input);
            });

            Operations.WhenAdded(x =>
            {
                x.Input.WhenRemoved(RemoveConnection);

                if (x is LogicCircuitInputOperationViewModel ci)
                {
                    ci.Output.WhenRemoved(RemoveConnection);
                }

                void RemoveConnection(ConnectorViewModel i)
                {
                    var c = Connections.Where(con => con.Input == i || con.Output == i).ToArray();
                    c.ForEach(con => Connections.Remove(con));
                }
            })
            .WhenRemoved(x =>
            {
                foreach (var input in x.Input)
                {
                    DisconnectConnector(input);
                }

                foreach (var output in x.Output)
                {
                    DisconnectConnector(output);
                }
            });

            OperationsMenu = new OperationsMenuViewModel(this);
        }

        private NodifyObservableCollection<OperationViewModel> _operations = new NodifyObservableCollection<OperationViewModel>();
        public NodifyObservableCollection<OperationViewModel> Operations
        {
            get => _operations;
            set => SetProperty(ref _operations, value);
        }

        private NodifyObservableCollection<OperationViewModel> _selectedOperations = new NodifyObservableCollection<OperationViewModel>();
        public NodifyObservableCollection<OperationViewModel> SelectedOperations
        {
            get => _selectedOperations;
            set => SetProperty(ref _selectedOperations, value);
        }

        public NodifyObservableCollection<ConnectionViewModel> Connections { get; } = new NodifyObservableCollection<ConnectionViewModel>();
        public PendingConnectionViewModel PendingConnection { get; set; } = new PendingConnectionViewModel();
        public OperationsMenuViewModel OperationsMenu { get; set; }

        public INodifyCommand StartConnectionCommand { get; }
        public INodifyCommand CreateConnectionCommand { get; }
        public INodifyCommand DisconnectConnectorCommand { get; }
        public INodifyCommand DeleteSelectionCommand { get; }
        public INodifyCommand GroupSelectionCommand { get; }

        private void DisconnectConnector(ConnectorViewModel connector)
        {
            var connections = Connections.Where(c => c.Input == connector || c.Output == connector).ToList();
            connections.ForEach(c => Connections.Remove(c));
        }

        internal bool CanCreateConnection(ConnectorViewModel source, ConnectorViewModel? target)
            => target == null || (source != target && source.Operation != target.Operation && source.IsInput != target.IsInput);

        internal void CreateConnection(ConnectorViewModel source, ConnectorViewModel? target)
        {
            if (target == null)
            {
                PendingConnection.IsVisible = true;
                OperationsMenu.OpenAt(PendingConnection.TargetLocation);
                OperationsMenu.Closed += OnOperationsMenuClosed;
                return;
            }

            var input = source.IsInput ? source : target;
            var output = target.IsInput ? source : target;

            PendingConnection.IsVisible = false;

            DisconnectConnector(input);

            Connections.Add(new ConnectionViewModel
            {
                Input = input,
                Output = output
            });
        }

        private void OnOperationsMenuClosed()
        {
            PendingConnection.IsVisible = false;
            OperationsMenu.Closed -= OnOperationsMenuClosed;
        }

        private void DeleteSelection()
        {
            var selected = SelectedOperations.ToList();
            selected.ForEach(o => Operations.Remove(o));
        }

        private void GroupSelectedOperations()
        {
            var selected = SelectedOperations.ToList();
            var bounding = selected.GetBoundingBox(50);

            Operations.Add(new OperationGroupViewModel
            {
                Title = "Operations",
                Location = bounding.Position,
                GroupSize = new Size(bounding.Width, bounding.Height)
            });
        }
    }
}


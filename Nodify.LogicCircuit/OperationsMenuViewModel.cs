// -----------------------------------------------------------------------------
// OperationsMenuViewModel.cs
//
// This ViewModel controls the "Add Operation" menu (toolbox or popup panel)
// that allows users to create and insert new logic nodes (e.g., AND, OR, NOT)
// into the graph editor.
//
//  Purpose:
// - Manages the list of available operations (nodes) shown in the menu
// - Controls visibility and location of the menu
// - Handles user actions like clicking to add a node
//
//  Main Features:
//
// 1. `AvailableOperations`
//    - A list of available logic operations the user can choose from
//    - Populated via `OperationFactory.GetOperationsInfo(...)`
//    - Displayed in the UI toolbox for drag/drop or click-to-insert
//
// 2. `IsVisible`
//    - Controls whether the operations menu is shown
//    - When set to `false`, triggers the `Closed` event
//
// 3. `Location`
//    - X/Y position of the menu on the editor canvas
//    - Set when opening the menu (e.g., right-clicked location)
//
// 4. `OpenAt(Point)`
//    - Opens the menu at the given canvas position
//
// 5. `Close()`
//    - Hides the menu and notifies listeners
//
// 6. `CreateOperationCommand`
//    - Command bound to UI interactions (click or drag node from menu)
//    - When executed, it:
//        → Uses the `OperationFactory` to create the actual node
//        → Places it at the menu’s location
//        → Adds it to the graph (`logicCircuit.Operations`)
//        → If a pending connection exists, tries to auto-connect it
//        → Closes the menu after insertion
//
//  Dependencies:
// - `LogicCircuitViewModel` (the graph the new node will be added to)
// - `OperationFactory` (used to dynamically generate node view models)
// - `OperationInfoViewModel` (describes the operation’s metadata)
//
// -----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public class OperationsMenuViewModel : ObservableObject
    {
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                SetProperty(ref _isVisible, value);
                if (!value)
                {
                    Closed?.Invoke();
                }
            }
        }

        private Point _location;
        public Point Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public event Action? Closed;

        public void OpenAt(Point targetLocation)
        {
            Close();
            Location = targetLocation;
            IsVisible = true;
        }

        public void Close()
        {
            IsVisible = false;
        }

        public NodifyObservableCollection<OperationInfoViewModel> AvailableOperations { get; }
        public INodifyCommand CreateOperationCommand { get; }
        private readonly LogicCircuitViewModel _logicCircuit;

        public OperationsMenuViewModel(LogicCircuitViewModel logicCircuit)
        {
            _logicCircuit = logicCircuit;
            List<OperationInfoViewModel> operations = new List<OperationInfoViewModel>
            {
                /*
                new OperationInfoViewModel
                {
                    Type = OperationType.Graph,
                    Title = "Operation Graph",
                },
                new OperationInfoViewModel
                {
                    Type = OperationType.LogicCircuit,
                    Title = "LogicCircuit"
                },
                new OperationInfoViewModel
                {
                    Type = OperationType.Expression,
                    Title = "Custom",
                }
                */
            };
            operations.AddRange(OperationFactory.GetOperationsInfo(typeof(OperationsContainer)));

            AvailableOperations = new NodifyObservableCollection<OperationInfoViewModel>(operations);
            CreateOperationCommand = new DelegateCommand<OperationInfoViewModel>(CreateOperation);
        }

        private void CreateOperation(OperationInfoViewModel operationInfo)
        {
            OperationViewModel op = OperationFactory.GetOperation(operationInfo);
            op.Location = Location;

            _logicCircuit.Operations.Add(op);

            var pending = _logicCircuit.PendingConnection;
            if (pending.IsVisible)
            {
                var connector = pending.Source.IsInput ? op.Output : op.Input.FirstOrDefault();
                if (connector != null && _logicCircuit.CanCreateConnection(pending.Source, connector))
                {
                    _logicCircuit.CreateConnection(pending.Source, connector);
                }
            }
            Close();
        }
    }
}

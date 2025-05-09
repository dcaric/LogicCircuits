﻿// -----------------------------------------------------------------------------
// EditorViewModel.cs
//
// This class acts as the ViewModel for a visual logic editor surface.
//
// Responsibilities:
// - Holds the active logic circuit (nodes, connections, toolbox)
// - Provides a command to open nested (inner) logic circuits
// - Supports parent/child navigation for hierarchical circuit design
// - Exposes metadata like unique ID and editor name
//
// Used by the EditorView (UI - EditorView.xaml) to bind data, handle commands, and track circuit state.
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Nodify.LogicCircuit
{
    public class EditorViewModel : ObservableObject
    {
        public event Action<EditorViewModel, LogicCircuitViewModel>? OnOpenInnerLogicCircuit;

        public EditorViewModel? Parent { get; set; }

        public EditorViewModel()
        {
            LogicCircuit = new LogicCircuitViewModel();
            OpenLogicCircuitCommand = new DelegateCommand<LogicCircuitViewModel>(logicCircuit =>
            {
                OnOpenInnerLogicCircuit?.Invoke(this, logicCircuit);
            });
        }

        public INodifyCommand OpenLogicCircuitCommand { get; }

        public Guid Id { get; } = Guid.NewGuid();

        private LogicCircuitViewModel _logicCircuit = default!;
        public LogicCircuitViewModel LogicCircuit 
        {
            get => _logicCircuit;
            set => SetProperty(ref _logicCircuit, value);
        }

        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}

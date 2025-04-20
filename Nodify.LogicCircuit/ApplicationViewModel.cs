// -----------------------------------------------------------------------------
// ApplicationViewModel.cs
//
//  Purpose:
// This is the top-level ViewModel for the entire application.
// It manages one or more logic circuit editors (`EditorViewModel`) —
// including support for nested (inner) editors representing subcircuits.
//
//  Key Responsibilities:
//
// 1. `Editors`:
//     - A collection of all active editors (main + subcircuits)
//     - Supports nesting via the `Parent` reference in `EditorViewModel`
//     - Used as a tab list or panel switcher in the UI
//
// 2. `SelectedEditor`:
//     - Points to the currently active editor (for display and interaction)
//
// 3. `AddEditorCommand`:
//     - Adds a new top-level editor (e.g., user clicks "New Circuit")
//
// 4. `CloseEditorCommand`:
//     - Removes an editor by `Id`
//     - Also removes any child editors related to that parent
//
// 5. `OnOpenInnerLogicCircuit()`:
//     - Event handler for `EditorViewModel.OnOpenInnerLogicCircuit`
//     - Opens a sub-circuit in a new editor tab or panel
//     - Reuses an existing editor if the circuit was already opened
//
// 6. `AutoSelectNewEditor`:
//     - If true, newly added editors will automatically be selected
//     - Helps streamline workflow in tab-based or multi-editor UI
//
//  Behavior:
//
// - On startup, a default editor is added (`Editor 1`)
// - When a logic node representing a nested circuit is double-clicked,
//   `OnOpenInnerLogicCircuit()` is called:
//     → Creates and shows a new editor for that nested circuit
//     → Avoids duplication if already opened
//
//  UI Integration:
// - Each editor corresponds to a logic canvas (like a tab or window)
// - Can be used with a `TabControl`, `ListBox`, or similar to switch views
//
// -----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows.Input;

namespace Nodify.LogicCircuit
{
    public class ApplicationViewModel : ObservableObject
    {
        public NodifyObservableCollection<EditorViewModel> Editors { get; } = new NodifyObservableCollection<EditorViewModel>();

        public ApplicationViewModel()
        {
            AddEditorCommand = new DelegateCommand(() => Editors.Add(new EditorViewModel
            {
                Name = $"Editor {Editors.Count + 1}"
            }));
            CloseEditorCommand = new DelegateCommand<Guid>(
                id => Editors.RemoveOne(editor => editor.Id == id),
                _ => Editors.Count > 0 && SelectedEditor != null);
            Editors.WhenAdded((editor) =>
            {
                if (AutoSelectNewEditor || Editors.Count == 1)
                {
                    SelectedEditor = editor;
                }
                editor.OnOpenInnerLogicCircuit += OnOpenInnerLogicCircuit;
            })
            .WhenRemoved((editor) =>
            {
                editor.OnOpenInnerLogicCircuit -= OnOpenInnerLogicCircuit;
                var childEditors = Editors.Where(ed => ed.Parent == editor).ToList();
                childEditors.ForEach(ed => Editors.Remove(ed));
            });
            Editors.Add(new EditorViewModel
            {
                Name = $"Editor {Editors.Count + 1}"
            });
        }

        private void OnOpenInnerLogicCircuit(EditorViewModel parentEditor, LogicCircuitViewModel logicCircuit)
        {
            var editor = Editors.FirstOrDefault(e => e.LogicCircuit == logicCircuit);
            if (editor != null)
            {
                SelectedEditor = editor;
            }
            else
            {
                var childEditor = new EditorViewModel
                {
                    Parent = parentEditor,
                    LogicCircuit = logicCircuit,
                    Name = $"[Inner] Editor {Editors.Count + 1}"
                };
                Editors.Add(childEditor);
            }
        }

        public ICommand AddEditorCommand { get; }
        public ICommand CloseEditorCommand { get; }

        private EditorViewModel? _selectedEditor;
        public EditorViewModel? SelectedEditor
        {
            get => _selectedEditor;
            set => SetProperty(ref _selectedEditor, value);
        }

        private bool _autoSelectNewEditor = true;
        public bool AutoSelectNewEditor
        {
            get => _autoSelectNewEditor;
            set => SetProperty(ref _autoSelectNewEditor , value); 
        }
    }
}

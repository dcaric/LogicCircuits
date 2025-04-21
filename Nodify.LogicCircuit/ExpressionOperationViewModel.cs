// -----------------------------------------------------------------------------
// ExpressionOperationViewModel.cs
//
//  Purpose:
// This ViewModel represents a node where users can define a **custom math or logic
// expression** as a string. The node automatically detects and generates inputs
// based on the variables in the expression, and computes an output value.
//
//  Example Expression:
//     Expression = "a + b * c"
//     → Automatically creates inputs: a, b, c
//     → Computes result using input values
//
//  Key Features:
//
// - Inherits from `OperationViewModel`
// - Accepts an `Expression` string (e.g., "a + b")
// - Parses the string into an executable expression tree (`MathExpr`)
// - Automatically manages input connectors based on detected variables
// - On value change, substitutes input values into the expression
//   and assigns the result to `Output.Value`
//
//  Dependencies:
// - Uses the `StringMath` library to parse and evaluate math expressions
//   (`ToMathExpr()`, `Substitute()`, `.Result`, etc.)
//
//  Properties:
// - `Expression`:
//     - The raw expression string
//     - On change, triggers regeneration of inputs and recalculation
//
// - `_expr` (private):
//     - Holds the compiled expression object for reuse
//
//  Method: `GenerateInput()`
// - Parses the expression
// - Removes obsolete inputs no longer referenced
// - Adds new inputs for new variables
// - Triggers output recalculation
//
//  Method: `OnInputValueChanged()` (override)
// - Substitutes values from current inputs into `_expr`
// - Sets the final result into `Output.Value`
//
//  Use Cases:
// - Dynamic math or logic node
// - User-defined formulas in visual workflows
// - Educational tools for expression evaluation
//
// Concept          Role
// Expression	    User-entered math/logic expression
// MathExpr     	Parsed version of the expression
// Input	        Auto-generated from variables in the expression
// Output	        Evaluated result of the expression
// Use case	        Highly flexible dynamic node (like a formula block)
// -----------------------------------------------------------------------------


using StringMath;
using System.Collections.Generic;
using System.Linq;

namespace Nodify.LogicCircuit
{
    public class ExpressionOperationViewModel : OperationViewModel
    {
        private MathExpr? _expr;
        private string? _expression;
        public string? Expression
        {
            get => _expression;
            set => SetProperty(ref _expression, value)
                .Then(GenerateInput);
        }

        private void GenerateInput()
        {
            try
            {
                _expr = Expression!.ToMathExpr();
                ConnectorViewModel[]? toRemove = Input.Where(i => !_expr.LocalVariables.Contains(i.Title)).ToArray();
                toRemove.ForEach(i => Input.Remove(i));
                HashSet<string> existingVars = Input.Select(s => s.Title).Where(s => s != null).ToHashSet()!;

                foreach (string variable in _expr.LocalVariables.Except(existingVars))
                {
                    Input.Add(new ConnectorViewModel
                    {
                        Title = variable
                    });
                }

                OnInputValueChanged();
            }
            catch
            {

            }
        }

        /*protected override void OnInputValueChanged()
        {
            if (Output != null && _expr != null)
            {
                try
                {
                    Input.ForEach(i => _expr.Substitute(i.Title!, i.Value));
                    Output.Value = _expr.Result;
                }
                catch
                {

                }
            }
        }*/

        protected override void OnInputValueChanged()
        {
            if (_expr != null && Output.Count > 0)
            {
                try
                {
                    Input.ForEach(i => _expr.Substitute(i.Title!, i.Value));
                    Output[0].Value = _expr.Result;
                }
                catch
                {
                    // Handle parse/eval errors if needed
                }
            }
        }

    }
}

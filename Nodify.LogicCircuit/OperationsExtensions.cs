// -----------------------------------------------------------------------------
// OperationsExtensions.cs
//
//  Purpose:
// Provides extension methods for working with collections of `OperationViewModel`
// nodes — specifically, a method to calculate a **bounding box** around them.
//
//  Method: `GetBoundingBox(...)`
//
// Calculates the rectangular area that fully contains all provided nodes.
// This is commonly used when:
// - Creating a visual group node around selected nodes
// - Aligning or scaling layouts
// - Snapping to a visual grid
//
//  Parameters:
// - `nodes`: IEnumerable<OperationViewModel> — the nodes to include
// - `padding`: Optional space to add around the bounding area (default = 0)
// - `gridCellSize`: Optional size for grid snapping (default = 15)
//
//  Internal Logic:
// - Loops through all nodes
// - Uses fixed dimensions (`width = 200`, `height = 100`) as estimated node sizes
// - Computes the minimum and maximum X/Y positions to enclose all nodes
// - Adds optional `padding`
// - Snaps the top-left corner to the nearest grid cell for alignment
//
//  Returns:
// - A `Rect` struct representing the full bounding area
//
//  Use Case Example:
// ```csharp
// var box = selectedNodes.GetBoundingBox(padding: 50);
// var groupNode = new OperationGroupViewModel { GroupSize = new Size(box.Width, box.Height) };
// ```
// -----------------------------------------------------------------------------
// Function	            Description
// GetBoundingBox()     Calculates the rectangular area around multiple nodes
// Adds padding?        Yes — configurable
// Snaps to grid?       Yes — using gridCellSize
// Used for?            Grouping, layout, UI alignment
// -----------------------------------------------------------------------------


using System.Collections.Generic;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public static class OperationsExtensions
    {
        public static Rect GetBoundingBox(this IEnumerable<OperationViewModel> nodes, double padding = 0, int gridCellSize = 15)
        {
            var minX = double.MaxValue;
            var minY = double.MaxValue;

            var maxX = double.MinValue;
            var maxY = double.MinValue;

            const int width = 200; //node.Width
            const int height = 100; //node.Height

            foreach (var node in nodes)
            {
                if (node.Location.X < minX)
                {
                    minX = node.Location.X;
                }

                if (node.Location.Y < minY)
                {
                    minY = node.Location.Y;
                }

                var sizeX = node.Location.X + width;
                if (sizeX > maxX)
                {
                    maxX = sizeX;
                }

                var sizeY = node.Location.Y + height;
                if (sizeY > maxY)
                {
                    maxY = sizeY;
                }
            }

            var result = new Rect(minX - padding, minY - padding, maxX - minX + padding * 2, maxY - minY + padding * 2);
            result = new Rect((int)result.X / gridCellSize * gridCellSize,
                (int)result.Y / gridCellSize * gridCellSize,
                result.Width,
                result.Height);
            return result;
        }
    }
}
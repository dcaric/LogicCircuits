using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodify.LogicCircuit
{
    public class AndNand : IOperation
    {
        public double[] Execute(double[] input)
        {
            // Simple demo: first output is AND of a and b, second is NAND
            var a = input.ElementAtOrDefault(0);
            var b = input.ElementAtOrDefault(1);

            double and = (a == 1 && b == 1) ? 1 : 0;
            double nand = and == 1 ? 0 : 1;

            return new double[] { and, nand };
        }

        object IOperation.Execute(double[] input) => Execute(input); // explicit for interface
    }
}

using System;

namespace Nodify.LogicCircuit
{
    public class BinaryOperation : IOperation
    {
        private readonly Func<double, double, double> _func;

        public BinaryOperation(Func<double, double, double> func)
            => _func = func;

        public object Execute(params double[] operands)
            => _func.Invoke(operands[0], operands[1]);
    }
}

using System;

namespace Nodify.LogicCircuit
{
    public class ValueOperation : IOperation
    {
        private readonly Func<double> _func;

        public ValueOperation(Func<double> func) => _func = func;

        public object Execute(params double[] operands)
            => _func();
    }
}

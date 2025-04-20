namespace Nodify.LogicCircuit
{
    public interface IOperation
    {
        double Execute(params double[] operands);
    }
}

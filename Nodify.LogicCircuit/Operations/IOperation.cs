namespace Nodify.LogicCircuit
{
    // old code for single output
    /*public interface IOperation
    {
        double Execute(params double[] operands);
    }*/

    // update to support multiple outputs
    public interface IOperation
    {
        object Execute(params double[] input);
    }
}

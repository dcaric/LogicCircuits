using System;
using System.Linq;

public struct BitDouble
{
    private double _value;

    public BitDouble(double value)
    {
        if (value != 0.0 && value != 1.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Only 0.0 or 1.0 allowed.");

        _value = value;
    }

    public static implicit operator double(BitDouble b) => b._value;
    public static explicit operator BitDouble(double d) => new BitDouble(d);

    public override string ToString() => _value.ToString();
}

namespace Nodify.LogicCircuit
{
    public static class OperationsContainer
    {
        /*
        [Operation(MinInput = 2, MaxInput = 10, GenerateInputNames = false)]
        public static double Add(params double[] operands)
            => operands.Sum();

        [Operation(MinInput = 2, MaxInput = 10, GenerateInputNames = false)]
        public static double Multiply(params double[] operands)
            => operands.Aggregate((x, y) => x * y);

        public static double Divide(double a, double b)
            => a / b;

        public static double Subtract(double a, double b)
            => a - b;

        public static double Pow(double value, double exp)
            => (double)Math.Pow((double)value, (double)exp);

        [Operation(GenerateInputNames = false)]
        public static double Abs(double value)
            => Math.Abs(value);

        public static double PI()
            => (double)Math.PI;


        */

        //************************************************************************************
        // Logic functions 
        //************************************************************************************
        public static double Not(double a)
            => a == 1 ? 0 : 1;

        public static double And(double a, double b)
        {
            if (a == 1 && b == 1) return 1;
            else return 0;
        }

        [Operation(MinInput = 2, MaxInput = 2, OutputLabels = new[] { "AND", "NAND" }, InputLabels = new[] { "In1", "In2" })]
        public static IOperation AndNand()
                => new AndNand();

        public static double And4(double a, double b, double c, double d)
        {
            if (a == 1 && b == 1 && c == 1 && d == 1) return 1;
            else return 0;
        }
        //************************************************************************************

    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OperationAttribute : Attribute
    {
        public uint MaxInput { get; set; }
        public uint MinInput { get; set; }
        public bool GenerateInputNames { get; set; }

        // Labels for output connectors
        public string[]? OutputLabels { get; set; }

        // Labels for input connectors
        public string[]? InputLabels { get; set; }
    }
}

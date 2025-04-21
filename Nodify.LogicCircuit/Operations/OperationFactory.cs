/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public static class OperationFactory
    {
        public static List<OperationInfoViewModel> GetOperationsInfo(Type container)
        {
            List<OperationInfoViewModel> result = new List<OperationInfoViewModel>();

            foreach (var method in container.GetMethods())
            {
                if (method.IsStatic)
                {
                    OperationInfoViewModel op = new OperationInfoViewModel
                    {
                        Title = method.Name
                    };

                    var attr = method.GetCustomAttribute<OperationAttribute>();
                    var para = method.GetParameters();

                    bool generateInputNames = true;

                    op.Type = OperationType.Normal;

                    if (para.Length == 2)
                    {
                        var delType = typeof(Func<double, double, double>);
                        var del = (Func<double, double, double>)Delegate.CreateDelegate(delType, method);

                        op.Operation = new BinaryOperation(del);
                    }
                    else if (para.Length == 1)
                    {
                        if (para[0].ParameterType.IsArray)
                        {
                            op.Type = OperationType.Expando;

                            var delType = typeof(Func<double[], double>);
                            var del = (Func<double[], double>)Delegate.CreateDelegate(delType, method);

                            op.Operation = new ParamsOperation(del);
                            op.MaxInput = int.MaxValue;
                        }
                        else
                        {
                            var delType = typeof(Func<double, double>);
                            var del = (Func<double, double>)Delegate.CreateDelegate(delType, method);

                            op.Operation = new UnaryOperation(del);
                        }
                    }


                    else if (para.Length == 0)
                    {
                        if (method.ReturnType == typeof(IOperation))
                        {
                            op.Operation = (IOperation)method.Invoke(null, null)!;
                        }
                        else
                        {
                            var delType = typeof(Func<double>);
                            var del = (Func<double>)Delegate.CreateDelegate(delType, method);
                            op.Operation = new ValueOperation(del);
                        }
                    }


                    if (attr != null)
                    {
                        op.MinInput = attr.MinInput;
                        op.MaxInput = attr.MaxInput;
                        generateInputNames = attr.GenerateInputNames;
                    }
                    else
                    {
                        op.MinInput = (uint)para.Length;
                        op.MaxInput = (uint)para.Length;
                    }

                    foreach (var param in para)
                    {
                        op.Input.Add(generateInputNames ? param.Name : null);
                    }

                    for (int i = op.Input.Count; i < op.MinInput; i++)
                    {
                        op.Input.Add(null);
                    }

                    result.Add(op);
                }
            }

            return result;
        }

        public static OperationViewModel GetOperation(OperationInfoViewModel info)
        {
            var input = info.Input.Select(i => new ConnectorViewModel
            {
                Title = i
            });

            var op = new OperationViewModel();

            

            switch (info.Type)
            {
                case OperationType.Expression:
                    var expr = new ExpressionOperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation,
                        Expression = "1 + sin {a} + cos {b}"
                    };
                    expr.Output.Add(new ConnectorViewModel());
                    return expr;

                case OperationType.LogicCircuit:
                    return new LogicCircuitOperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation,
                    };

                case OperationType.Expando:
                    var expando = new ExpandoOperationViewModel
                    {
                        MaxInput = info.MaxInput,
                        MinInput = info.MinInput,
                        Title = info.Title,
                        Operation = info.Operation
                    };
                    expando.Input.AddRange(input);
                    expando.Output.Add(new ConnectorViewModel());
                    return expando;

                case OperationType.Group:
                    return new OperationGroupViewModel
                    {
                        Title = info.Title,
                    };

                case OperationType.Graph:
                    return new OperationGraphViewModel
                    {
                        Title = info.Title,
                        DesiredSize = new Size(420, 250)
                    };

                case OperationType.AndANnd:
                    op = new OperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation
                    };
                    op.Input.AddRange(input);

                    // Check if this operation returns multiple outputs
                    if (info.Operation is AndNand)
                    {
                        op.Output.Add(new ConnectorViewModel { Title = "AND" });
                        op.Output.Add(new ConnectorViewModel { Title = "NAND" });
                    }
                    else
                    {
                        op.Output.Add(new ConnectorViewModel());
                    }

                    return op;
                default:
                    op = new OperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation
                    };
                    op.Input.AddRange(input);
                    op.Output.Add(new ConnectorViewModel());
                    return op;

            }
        }

    }
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Nodify.LogicCircuit
{
    public static class OperationFactory
    {
        public static List<OperationInfoViewModel> GetOperationsInfo(Type container)
        {
            List<OperationInfoViewModel> result = new List<OperationInfoViewModel>();

            foreach (var method in container.GetMethods())
            {
                if (method.IsStatic)
                {
                    OperationInfoViewModel op = new OperationInfoViewModel
                    {
                        Title = method.Name
                    };

                    var attr = method.GetCustomAttribute<OperationAttribute>();
                    var para = method.GetParameters();

                    bool generateInputNames = true;
                    op.Type = OperationType.Normal;

                    if (para.Length == 2)
                    {
                        var delType = typeof(Func<double, double, double>);
                        var del = (Func<double, double, double>)Delegate.CreateDelegate(delType, method);
                        op.Operation = new BinaryOperation(del);
                    }
                    else if (para.Length == 1)
                    {
                        if (para[0].ParameterType.IsArray)
                        {
                            op.Type = OperationType.Expando;
                            var delType = typeof(Func<double[], double>);
                            var del = (Func<double[], double>)Delegate.CreateDelegate(delType, method);
                            op.Operation = new ParamsOperation(del);
                            op.MaxInput = int.MaxValue;
                        }
                        else
                        {
                            var delType = typeof(Func<double, double>);
                            var del = (Func<double, double>)Delegate.CreateDelegate(delType, method);
                            op.Operation = new UnaryOperation(del);
                        }
                    }
                    else if (para.Length == 0)
                    {
                        if (method.ReturnType == typeof(IOperation))
                        {
                            var instance = (IOperation)method.Invoke(null, null)!;
                            op.Operation = instance;


                            if (attr != null)
                            {
                                op.MinInput = attr.MinInput;
                                op.MaxInput = attr.MaxInput;
                                generateInputNames = attr.GenerateInputNames;

                           
                                op.OutputLabels = attr.OutputLabels;
                                op.InputLabels = attr.InputLabels;

                            }

                            // Detect special multi-output operation
                            if (instance is AndNand)
                            {
                                op.Type = OperationType.ComplexCircuits; // type for the Jogic Circuit
                            }
                        }
                        else
                        {
                            var delType = typeof(Func<double>);
                            var del = (Func<double>)Delegate.CreateDelegate(delType, method);
                            op.Operation = new ValueOperation(del);
                        }
                    }

                    if (attr != null)
                    {
                        op.MinInput = attr.MinInput;
                        op.MaxInput = attr.MaxInput;
                        generateInputNames = attr.GenerateInputNames;
                    }
                    else
                    {
                        op.MinInput = (uint)para.Length;
                        op.MaxInput = (uint)para.Length;
                    }

                    foreach (var param in para)
                    {
                        op.Input.Add(generateInputNames ? param.Name : null);
                    }

                    for (int i = op.Input.Count; i < op.MinInput; i++)
                    {
                        op.Input.Add(null);
                    }

                    result.Add(op);
                }
            }

            return result;
        }

        public static OperationViewModel GetOperation(OperationInfoViewModel info)
        {
            var input = info.Input.Select(i => new ConnectorViewModel
            {
                Title = i
            });

            switch (info.Type)
            {
                case OperationType.Expression:
                    var expr = new ExpressionOperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation,
                        Expression = "1 + sin {a} + cos {b}"
                    };
                    expr.Output.Add(new ConnectorViewModel());
                    return expr;

                case OperationType.LogicCircuit:
                    return new LogicCircuitOperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation,
                    };

                case OperationType.Expando:
                    var expando = new ExpandoOperationViewModel
                    {
                        MaxInput = info.MaxInput,
                        MinInput = info.MinInput,
                        Title = info.Title,
                        Operation = info.Operation
                    };
                    expando.Input.AddRange(input);
                    expando.Output.Add(new ConnectorViewModel());
                    return expando;

                case OperationType.Group:
                    return new OperationGroupViewModel
                    {
                        Title = info.Title,
                    };

                case OperationType.Graph:
                    return new OperationGraphViewModel
                    {
                        Title = info.Title,
                        DesiredSize = new Size(420, 250)
                    };

                case OperationType.ComplexCircuits:
                    {
                        var multi = new OperationViewModel
                        {
                            Title = info.Title,
                            Operation = info.Operation
                        };

                        if (info.OutputLabels != null && info.OutputLabels.Any())
                        {
                            foreach (var label in info.OutputLabels)
                            {
                                multi.Output.Add(new ConnectorViewModel { Title = label });
                            }
                        }

                        if (info.InputLabels != null && info.InputLabels.Any())
                        {
                            foreach (var label in info.InputLabels)
                            {
                                multi.Input.Add(new ConnectorViewModel { Title = label });
                            }
                        }

                        else
                        {
                            // Fallback: unnamed outputs
                            multi.Output.Add(new ConnectorViewModel());
                            multi.Output.Add(new ConnectorViewModel());
                        }

                        return multi;
                    }

                default:
                    var op = new OperationViewModel
                    {
                        Title = info.Title,
                        Operation = info.Operation
                    };
                    op.Input.AddRange(input);
                    op.Output.Add(new ConnectorViewModel());
                    return op;
            }
        }
    }
}

using System.Windows;

namespace Nodify.LogicCircuit
{
    public class LogicCircuitOperationViewModel : OperationViewModel
    {
        public LogicCircuitViewModel InnerLogicCircuit { get; } = new LogicCircuitViewModel();

        private OperationViewModel InnerOutput { get; } = new OperationViewModel
        {
            Title = "Output Parameters",
            Input = { new ConnectorViewModel() },
            Location = new Point(500, 300),
            IsReadOnly = true
        };

        private LogicCircuitInputOperationViewModel InnerInput { get; } = new LogicCircuitInputOperationViewModel
        {
            Title = "Input Parameters",
            Location = new Point(300, 300),
            IsReadOnly = true
        };

        public LogicCircuitOperationViewModel()
        {
            InnerLogicCircuit.Operations.Add(InnerInput);
            InnerLogicCircuit.Operations.Add(InnerOutput);

            Output = new ConnectorViewModel();

            InnerOutput.Input[0].ValueObservers.Add(Output);

            InnerInput.Output.ForEach(x => Input.Add(new ConnectorViewModel
            {
                Title = x.Title
            }));

            InnerInput.Output
                .WhenAdded(x => Input.Add(new ConnectorViewModel
                {
                    Title = x.Title
                }))
                .WhenRemoved(x => Input.RemoveOne(i => i.Title == x.Title));
        }

        protected override void OnInputValueChanged()
        {
            for (var i = 0; i < Input.Count; i++)
            {
                InnerInput.Output[i].Value = Input[i].Value;
            }
        }
    }
}

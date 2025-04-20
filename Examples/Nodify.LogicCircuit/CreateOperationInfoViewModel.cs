using System.Windows;

namespace Nodify.LogicCircuit
{
    public class CreateOperationInfoViewModel
    {
        public CreateOperationInfoViewModel(OperationInfoViewModel info, Point location)
        {
            Info = info;
            Location = location;
        }

        public OperationInfoViewModel Info { get; }
        public Point Location { get; }
    }
}

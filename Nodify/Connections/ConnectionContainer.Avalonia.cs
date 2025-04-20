namespace Nodify;

internal partial class ConnectionContainer
{
    static ConnectionContainer()
    {
        IsSelectedProperty.Changed.AddClassHandler<ConnectionContainer>(OnIsSelectedChanged);
        SelectableMixin.Attach<ConnectionContainer>(IsSelectedProperty);
    }
}
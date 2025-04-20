namespace Nodify;

internal partial class ConnectionsMultiSelector
{
    static ConnectionsMultiSelector()
    {
        SelectedItemsProperty.Changed.AddClassHandler<ConnectionsMultiSelector>(OnSelectedItemsSourceChanged);
        CanSelectMultipleItemsProperty.Changed.AddClassHandler<ConnectionsMultiSelector>(OnCanSelectMultipleItemsChanged);
    }

    protected override Type StyleKeyOverride => typeof(ItemsControl);
}
namespace Nodify;

internal class BindableStyleClasses
{
    static BindableStyleClasses()
    {
        ClassesProperty.Changed.AddClassHandler<StyledElement>(HandleClassesChanged);
    }

    public static readonly AttachedProperty<object?> ClassesProperty = AvaloniaProperty.RegisterAttached<BindableStyleClasses, StyledElement, object?>(
        "Classes");

    private static void HandleClassesChanged(StyledElement element, AvaloniaPropertyChangedEventArgs e)
    {
        element.Classes.Clear();
        if (e.NewValue != null)
            element.Classes.Add(e.NewValue.ToString());
    }

    public static void SetClasses(StyledElement element, object? classes)
    {
        element.SetValue(ClassesProperty, classes);
    }


    public static object? GetClasses(StyledElement element)
    {
        return element.GetValue(ClassesProperty);
    }
}
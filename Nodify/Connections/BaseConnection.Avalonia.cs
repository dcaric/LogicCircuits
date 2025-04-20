using System.Threading;

namespace Nodify;

public partial class BaseConnection
{
    private CancellationTokenSource? animationTokenSource;

    static BaseConnection()
    {
        AffectsRender<BaseConnection>(SourceProperty, TargetProperty, SourceOffsetProperty, TargetOffsetProperty,
            SourceOffsetModeProperty, TargetOffsetModeProperty, DirectionProperty, SpacingProperty, ArrowSizeProperty, ArrowEndsProperty, ArrowShapeProperty, TextProperty, DirectionalArrowsCountProperty, DirectionalArrowsOffsetProperty, OutlineThicknessProperty, OutlineBrushProperty,
            IsAnimatingDirectionalArrowsProperty, DirectionalArrowsAnimationDurationProperty);
        AffectsGeometry<BaseConnection>(SourceProperty, TargetProperty, SourceOffsetProperty, TargetOffsetProperty,
            SourceOffsetModeProperty, TargetOffsetModeProperty, DirectionProperty, SpacingProperty, ArrowSizeProperty, ArrowEndsProperty, ArrowShapeProperty, SourceOrientationProperty, TargetOrientationProperty, DirectionalArrowsCountProperty, DirectionalArrowsOffsetProperty, OutlineThicknessProperty, OutlineBrushProperty);
        OutlineBrushProperty.Changed.AddClassHandler<BaseConnection>(OnOutlinePenChanged);
        OutlineThicknessProperty.Changed.AddClassHandler<BaseConnection>(OnOutlinePenChanged);
        IsAnimatingDirectionalArrowsProperty.Changed.AddClassHandler<BaseConnection>(OnIsAnimatingDirectionalArrowsChanged);
        DirectionalArrowsAnimationDurationProperty.Changed.AddClassHandler<BaseConnection>(OnDirectionalArrowsAnimationDurationChanged);
        IsSelectedProperty.Changed.AddClassHandler<Control>(OnIsSelectedChanged);
    }
}
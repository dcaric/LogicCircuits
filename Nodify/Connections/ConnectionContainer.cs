﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nodify
{
    internal partial class ConnectionContainer : ContentPresenter
    {
        #region Dependency properties

        public static readonly DirectProperty<ConnectionContainer, bool> IsSelectableProperty = AvaloniaProperty.RegisterDirect<ConnectionContainer, bool>(nameof(IsSelectable), x => x.IsSelectable, (x, val) => x.IsSelectable = val);
        public static readonly StyledProperty<bool> IsSelectedProperty = SelectingItemsControl.IsSelectedProperty.AddOwner<ConnectionContainer>(new StyledPropertyMetadata<bool>(defaultBindingMode: BindingMode.TwoWay));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var elem = (ConnectionContainer)d;
            bool result = elem.IsSelectable && (bool)e.NewValue;
            elem.IsSelected = result;
            elem.OnSelectedChanged(result);
        }

        /// <summary>
        /// Gets or sets whether this <see cref="ConnectionContainer"/> can be selected.
        /// </summary>
        public bool IsSelectable
        {
            get => BaseConnection.GetIsSelectable(Connection ?? this);
            set => BaseConnection.SetIsSelectable(Connection ?? this, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="ConnectionContainer"/> is selected.
        /// Can only be set if <see cref="IsSelectable"/> is true.
        /// </summary>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        #endregion

        #region Routed events

        public static readonly RoutedEvent SelectedEvent = RoutedEvent.Register<RoutedEventArgs>(nameof(Selected), RoutingStrategies.Bubble, typeof(ConnectionContainer));
        public static readonly RoutedEvent UnselectedEvent = RoutedEvent.Register<RoutedEventArgs>(nameof(Unselected), RoutingStrategies.Bubble, typeof(ConnectionContainer));

        /// <summary>
        /// Occurs when this <see cref="ConnectionContainer"/> is selected.
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }

        /// <summary>
        /// Occurs when this <see cref="ConnectionContainer"/> is unselected.
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add => AddHandler(UnselectedEvent, value);
            remove => RemoveHandler(UnselectedEvent, value);
        }

        #endregion

        private ConnectionsMultiSelector Selector { get; }

        private UIElement? _connection;
        private UIElement? Connection => _connection ??= BaseConnection.PrioritizeBaseConnectionForSelection
            ? this.GetChildOfType<BaseConnection>() ?? this.GetChildOfType<UIElement>()
            : this.GetChildOfType<UIElement>();

        internal ConnectionContainer(ConnectionsMultiSelector selector)
        {
            Selector = selector;
        }

        /// <summary>
        /// Raises the <see cref="SelectedEvent"/> or <see cref="UnselectedEvent"/> based on <paramref name="newValue"/>.
        /// Called when the <see cref="IsSelected"/> value is changed.
        /// </summary>
        /// <param name="newValue">True if selected, false otherwise.</param>
        protected void OnSelectedChanged(bool newValue)
        {
            BaseConnection.SetIsSelected(Connection, newValue);

            RaiseEvent(new RoutedEventArgs(newValue ? SelectedEvent : UnselectedEvent, this));
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (IsSelectable && EditorGestures.Mappings.Connection.Selection.Select.Matches(e.Source, e))
            {
                e.Handled = true;
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            EditorGestures.ConnectionGestures gestures = EditorGestures.Mappings.Connection;
            if (gestures.Selection.Select.Matches(e.Source, e))
            {
                if (gestures.Selection.Append.Matches(e.Source, e))
                {
                    SetCurrentValue(IsSelectedProperty, true);
                }
                else if (gestures.Selection.Invert.Matches(e.Source, e))
                {
                    SetCurrentValue(IsSelectedProperty, !IsSelected);
                }
                else if (gestures.Selection.Remove.Matches(e.Source, e))
                {
                    SetCurrentValue(IsSelectedProperty, false);
                }
                else
                {
                    // Allow context menu on selection
                    if (!(e.InitialPressMouseButton == MouseButton.Right && !e.GetCurrentPoint(this).Properties.IsRightButtonPressed) || !IsSelected)
                    {
                        Selector.UnselectAll();
                    }

                    SetCurrentValue(IsSelectedProperty, true);
                }
            }
        }
    }
}

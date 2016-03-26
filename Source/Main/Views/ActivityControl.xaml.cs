namespace Main.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Models;

    /// <summary>
    /// Interaction logic for ActivityControl.xaml
    /// </summary>
    public partial class ActivityControl : UserControl
    {
        public static readonly DependencyProperty ActivityProperty = DependencyProperty.Register("Activity", typeof(Activity), typeof(ActivityControl), new PropertyMetadata(null));
        public static readonly RoutedEvent MovedEvent = EventManager.RegisterRoutedEvent("Moved", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ActivityControl));
        private Point _dragStartPosition;
        private bool _dragging;
        private DragAdorner _dragAdorner;

        public ActivityControl()
        {
            InitializeComponent();
            PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            PreviewMouseMove += OnPreviewMouseMove;

            Loaded += (sender, args) =>
            {
                Window.GetWindow(this).PreviewDragOver += OnDragOver;
            };
        }

        public event RoutedEventHandler Moved
        {
            add { AddHandler(MovedEvent, value); }
            remove { RemoveHandler(MovedEvent, value); }
        }

        public Activity Activity
        {
            get { return (Activity)GetValue(ActivityProperty); }
            set { SetValue(ActivityProperty, value); }
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!_dragging)
                return;

            var mousePosition = mouseEventArgs.GetPosition(null);
            var difference = _dragStartPosition - mousePosition;

            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed && (Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var activityControl = (ActivityControl) sender;
                var source = GetParentControl(activityControl);
                var dictionary = new Dictionary<string, object> { { "activity", activityControl.Activity } };

                if (source != null)
                    dictionary.Add("source", source);

                var dragData = new DataObject("data", dictionary);

                ShowDragAdorner();

                var dragDropEffects = DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);

                _dragging = false;
                _dragAdorner.Destroy();
                
                if (dragDropEffects.HasFlag(DragDropEffects.Move))
                    RaiseEvent(new RoutedEventArgs(MovedEvent, this));
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _dragStartPosition = mouseButtonEventArgs.GetPosition(null);
            _dragging = true;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (_dragAdorner != null)
                _dragAdorner.UpdatePosition(e.GetPosition(this).X, e.GetPosition(this).Y);
        }

        private void ShowDragAdorner()
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer((Visual)Window.GetWindow(this).FindName("AdornerLayer"));
            _dragAdorner = new DragAdorner(this, adornerLayer) { Visibility = Visibility.Visible, Opacity = 0.8 };
        }

        private ActivityListControl GetParentControl(DependencyObject element)
        {
            var control = element;
            while (!(control is ActivityListControl))
            {
                control = VisualTreeHelper.GetParent(control);
                if (control == null)
                    return null;
            }

            return control as ActivityListControl;
        }
    }
}

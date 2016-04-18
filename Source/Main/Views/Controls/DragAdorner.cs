namespace Main.Views.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class DragAdorner : Adorner
    {
        private readonly ContentPresenter _contentPresenter;
        private readonly AdornerLayer _adornerLayer;
        private double _x, _y;
        private readonly double _width, _height;

        public DragAdorner(FrameworkElement adornedElement, AdornerLayer adornerLayer) : base(adornedElement)
        {
            _width = adornedElement.ActualWidth;
            _height = adornedElement.ActualHeight;

            _adornerLayer = adornerLayer;

            var grid = new Grid
            {
                Width = _width,
                Height = _height,
                Background = new ImageBrush(BitmapFrame.Create(RenderBitmap(adornedElement)))
            };

            _contentPresenter = new ContentPresenter() { Content = grid };
            _adornerLayer.Add(this);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(constraint);
            return _contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top)
        {
            _x = left - _width / 2;
            _y = top - _height / 2;
            if (_adornerLayer != null && AdornedElement != null && _contentPresenter.Content != null && _adornerLayer.GetAdorners(AdornedElement) != null)
                _adornerLayer.Update(AdornedElement);
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_x, _y));
            return result;
        }

        public void Destroy()
        {
            _adornerLayer.Remove(this);
        }

        // Whole lot of work to take a snapshot of an element
        public RenderTargetBitmap RenderBitmap(FrameworkElement element)
        {
            var width = element.ActualWidth;
            var height = element.ActualHeight;
            var elementBrush = new VisualBrush(element);
            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();
            dc.DrawRectangle(elementBrush, null, new Rect(0, 0, width, height));
            dc.Close();
            var bitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);

            return bitmap;
        }
    }
}

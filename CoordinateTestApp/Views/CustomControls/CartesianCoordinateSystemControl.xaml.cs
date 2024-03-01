using CoordinateTestApp.Data.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Point = CoordinateTestApp.Data.Models.Point;

namespace CoordinateTestApp.Views.CustomControls
{
	/// <summary>
	/// Логика взаимодействия для CartesianCoordinateSystemControl.xaml
	/// </summary>
	public partial class CartesianCoordinateSystemControl : UserControl
	{
		private const int LabelIndent = 7;
		private static Type ownerType = typeof(CartesianCoordinateSystemControl);
		private readonly IList<Path> UIPoints;

		private double CanvasWidth => canvas.ActualWidth;

		private double CanvasHeight => canvas.ActualHeight;

		private double XAbsoluteStep => CanvasWidth / 2 / Math.Max(AxisLength.Left, AxisLength.Right) * XRelativeStep;

		private double YAbsoluteStep => CanvasHeight / 2 / Math.Max(AxisLength.Top, AxisLength.Bottom) * YRelativeStep;

		#region PointRadiusProperty
		public static readonly DependencyProperty PointRadiusProperty =
			DependencyProperty.Register(nameof(PointRadius), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					3.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double PointRadius
		{
			get => (double)GetValue(PointRadiusProperty);
			set => SetValue(PointRadiusProperty, value);
		}
		#endregion

		#region MouseRelativeXProperty
		public static readonly DependencyProperty MouseRelativeXProperty =
			DependencyProperty.Register(nameof(MouseRelativeX), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					0.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double MouseRelativeX
		{
			get => (double)GetValue(MouseRelativeXProperty);
			set => SetValue(MouseRelativeXProperty, value);
		}
		#endregion

		#region MouseRelativeYProperty
		public static readonly DependencyProperty MouseRelativeYProperty =
			DependencyProperty.Register(nameof(MouseRelativeY), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					0.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double MouseRelativeY
		{
			get => (double)GetValue(MouseRelativeYProperty);
			set => SetValue(MouseRelativeYProperty, value);
		}
		#endregion

		#region LabelForegroundProperty
		public static readonly DependencyProperty LabelForegroundProperty =
			DependencyProperty.Register(nameof(LabelForeground), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.Black,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush LabelForeground
		{
			get => (Brush)GetValue(LabelForegroundProperty);
			set => SetValue(LabelForegroundProperty, value);
		}
		#endregion

		#region LabelFontSizeProperty
		public static readonly DependencyProperty LabelFontSizeProperty =
			DependencyProperty.Register(nameof(LabelFontSize), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					12.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double LabelFontSize
		{
			get => (double)GetValue(LabelFontSizeProperty);
			set => SetValue(LabelFontSizeProperty, value);
		}
		#endregion

		#region PointBackgroundProperty
		public static readonly DependencyProperty PointBackgroundProperty =
			DependencyProperty.Register(nameof(PointBackground), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.Black,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush PointBackground
		{
			get => (Brush)GetValue(PointBackgroundProperty);
			set => SetValue(PointBackgroundProperty, value);
		}
		#endregion

		#region XRelativeStepProperty
		public static readonly DependencyProperty XRelativeStepProperty =
			DependencyProperty.Register(nameof(XRelativeStep), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					1.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double XRelativeStep
		{
			get => (double)GetValue(XRelativeStepProperty);
			set => SetValue(XRelativeStepProperty, value);
		}
		#endregion

		#region YRelativeStepProperty
		public static readonly DependencyProperty YRelativeStepProperty =
			DependencyProperty.Register(nameof(YRelativeStep), typeof(double), ownerType,
				new FrameworkPropertyMetadata
				(
					1.0,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public double YRelativeStep
		{
			get => (double)GetValue(YRelativeStepProperty);
			set => SetValue(YRelativeStepProperty, value);
		}
		#endregion

		#region AxisLengthProperty
		public static readonly DependencyProperty AxisLengthProperty =
			DependencyProperty.Register(nameof(AxisLength), typeof(AxisLength), ownerType,
				new FrameworkPropertyMetadata
				(
					new AxisLength(10),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				),
				IsAxisValid);

		public AxisLength AxisLength
		{
			get => (AxisLength)GetValue(AxisLengthProperty);
			set => SetValue(AxisLengthProperty, value);
		}
		#endregion

		#region PointsProperty
		public static readonly DependencyProperty PointsProperty =
			DependencyProperty.Register(nameof(Points), typeof(IList<Point>), ownerType,
				new FrameworkPropertyMetadata
				(
					null,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
					new PropertyChangedCallback(PointsChangedEvent)
				));

		public IList<Point> Points
		{
			get => (IList<Point>)GetValue(PointsProperty);
			set => SetValue(PointsProperty, value);
		}
		#endregion

		#region IsDrawnHelperXDivisionsProperty		
		public static readonly DependencyProperty IsDrawnHelperXDivisionsProperty =
			DependencyProperty.Register(nameof(IsDrawnHelperXDivisions), typeof(bool), ownerType,
				new FrameworkPropertyMetadata
				(
					true,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public bool IsDrawnHelperXDivisions
		{
			get => (bool)GetValue(IsDrawnHelperXDivisionsProperty);
			set => SetValue(IsDrawnHelperXDivisionsProperty, value);
		}
		#endregion

		#region IsDrawnHelperYDivisionsProperty		
		public static readonly DependencyProperty IsDrawnHelperYDivisionsProperty =
			DependencyProperty.Register(nameof(IsDrawnHelperYDivisions), typeof(bool), ownerType,
				new FrameworkPropertyMetadata
				(
					true,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public bool IsDrawnHelperYDivisions
		{
			get => (bool)GetValue(IsDrawnHelperYDivisionsProperty);
			set => SetValue(IsDrawnHelperYDivisionsProperty, value);
		}
		#endregion

		#region XDivisionColorProperty
		public static readonly DependencyProperty XDivisionColorProperty =
			DependencyProperty.Register(nameof(XDivisionColor), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.Black,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush XDivisionColor
		{
			get => (Brush)GetValue(XDivisionColorProperty);
			set => SetValue(XDivisionColorProperty, value);
		}
		#endregion

		#region YDivisionColorProperty
		public static readonly DependencyProperty YDivisionColorProperty =
			DependencyProperty.Register(nameof(YDivisionColor), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.Black,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush YDivisionColor
		{
			get => (Brush)GetValue(YDivisionColorProperty);
			set => SetValue(YDivisionColorProperty, value);
		}
		#endregion

		#region HelperXDivisionColorProperty
		public static readonly DependencyProperty HelperXDivisionColorProperty =
			DependencyProperty.Register(nameof(HelperXDivisionColor), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.LightGray,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush HelperXDivisionColor
		{
			get => (Brush)GetValue(HelperXDivisionColorProperty);
			set => SetValue(HelperXDivisionColorProperty, value);
		}
		#endregion

		#region HelperYDivisionColorProperty
		public static readonly DependencyProperty HelperYDivisionColorProperty =
			DependencyProperty.Register(nameof(HelperYDivisionColor), typeof(Brush), ownerType,
				new FrameworkPropertyMetadata
				(
					Brushes.LightGray,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
				));

		public Brush HelperYDivisionColor
		{
			get => (Brush)GetValue(HelperYDivisionColorProperty);
			set => SetValue(HelperYDivisionColorProperty, value);
		}
		#endregion

		public CartesianCoordinateSystemControl()
		{
			InitializeComponent();
			UIPoints = new List<Path>();
			SizeChanged += ControlSizeChanged;
			DrawCoordinateSystem();
		}

		#region Axis
		private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
		{
			DrawCoordinateSystem();
			DrawPoints(Points);
		}

		private void DrawCoordinateSystem()
		{
			canvas.Children.Clear();

			if (IsDrawnHelperXDivisions)
			{
				DrawHelperXDivisions(-CanvasWidth / 2);
				DrawHelperXDivisions(CanvasWidth / 2);
			}
			if (IsDrawnHelperYDivisions)
			{
				DrawHelperYDivisions(-CanvasHeight / 2);
				DrawHelperYDivisions(CanvasHeight / 2);
			}

			DrawYDivisions(AxisLength.Top);
			DrawYDivisions(-AxisLength.Bottom);
			DrawXDivisions(-AxisLength.Left);
			DrawXDivisions(AxisLength.Right);

			DrawXLabels(-AxisLength.Left);
			DrawXLabels(AxisLength.Right);
			DrawYLabels(AxisLength.Top);
			DrawYLabels(-AxisLength.Bottom);

			DrawXAxis();
			DrawYAxis();
		}

		private void DrawXAxis()
		{
			Line xAxis = new Line();
			xAxis.Stroke = Brushes.Black;
			xAxis.X1 = 0;
			xAxis.Y1 = CanvasHeight / 2;
			xAxis.X2 = CanvasWidth;
			xAxis.Y2 = CanvasHeight / 2;

			canvas.Children.Add(xAxis);
		}

		private void DrawXDivisions(double length)
		{
			if (XAbsoluteStep == 0) return;
			double xAbsoluteStep = length < 0 ? -XAbsoluteStep : XAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / XRelativeStep));

			for (int i = 0; i <= stepsCount; i++)
			{
				Line xDivision = new Line();
				xDivision.Stroke = XDivisionColor;
				xDivision.StrokeThickness = 1;
				xDivision.X1 = CanvasWidth / 2 + i * xAbsoluteStep;
				xDivision.Y1 = CanvasHeight / 2 - 5;
				xDivision.X2 = CanvasWidth / 2 + i * xAbsoluteStep;
				xDivision.Y2 = CanvasHeight / 2 + 5;
				canvas.Children.Add(xDivision);
			}
		}

		private void DrawHelperXDivisions(double length)
		{
			if (XAbsoluteStep == 0) return;
			double xAbsoluteStep = length < 0 ? -XAbsoluteStep : XAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / xAbsoluteStep));

			for (int i = 0; i <= stepsCount; i++)
			{
				Line xHelpDivision = new Line();
				xHelpDivision.Stroke = HelperXDivisionColor;
				xHelpDivision.StrokeThickness = 1;
				xHelpDivision.X1 = CanvasWidth / 2 + i * xAbsoluteStep;
				xHelpDivision.Y1 = 0;
				xHelpDivision.X2 = CanvasWidth / 2 + i * xAbsoluteStep;
				xHelpDivision.Y2 = CanvasHeight;
				canvas.Children.Add(xHelpDivision);
			}
		}

		private void DrawXLabels(double length)
		{
			if (XAbsoluteStep == 0) return;
			double xAbsoluteStep = length < 0 ? -XAbsoluteStep : XAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / XRelativeStep));
			double label = 0;

			for (int i = 0; i <= stepsCount; i++)
			{
				if (label == 0)
				{
					label += (length < 0) ? -XRelativeStep : XRelativeStep;
					continue;
				}

				TextBlock textX = CreateLabel(label.ToString());
				textX.SetValue(Canvas.TopProperty, CanvasHeight / 2 + LabelIndent);
				textX.SetValue(Canvas.LeftProperty, CanvasWidth / 2 + i * xAbsoluteStep);

				canvas.Children.Add(textX);
				label += (length < 0) ? -XRelativeStep : XRelativeStep;
			}
		}

		private void DrawYAxis()
		{
			Line yAxis = new Line();
			yAxis.Stroke = Brushes.Black;
			yAxis.X1 = CanvasWidth / 2;
			yAxis.Y1 = 0;
			yAxis.X2 = CanvasWidth / 2;
			yAxis.Y2 = CanvasHeight;

			canvas.Children.Add(yAxis);
		}

		private void DrawYDivisions(double length)
		{
			if (YAbsoluteStep == 0) return;
			double yAbsoluteStep = length < 0 ? -YAbsoluteStep : YAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / YRelativeStep));

			for (int i = 0; i <= stepsCount; i++)
			{
				Line yDivision = new Line();
				yDivision.Stroke = XDivisionColor;
				yDivision.StrokeThickness = 2;
				yDivision.X1 = CanvasWidth / 2 - 5;
				yDivision.Y1 = CanvasHeight / 2 - i * yAbsoluteStep;
				yDivision.X2 = CanvasWidth / 2 + 5;
				yDivision.Y2 = CanvasHeight / 2 - i * yAbsoluteStep;
				canvas.Children.Add(yDivision);
			}
		}

		private void DrawHelperYDivisions(double length)
		{
			if (YAbsoluteStep == 0) return;
			double yAbsoluteStep = length < 0 ? -YAbsoluteStep : YAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / yAbsoluteStep));

			for (int i = 0; i <= stepsCount; i++)
			{
				Line yHelpDivision = new Line();
				yHelpDivision.Stroke = HelperYDivisionColor;
				yHelpDivision.StrokeThickness = 1;
				yHelpDivision.X1 = 0;
				yHelpDivision.Y1 = CanvasHeight / 2 - i * yAbsoluteStep;
				yHelpDivision.X2 = CanvasWidth;
				yHelpDivision.Y2 = CanvasHeight / 2 - i * yAbsoluteStep;
				canvas.Children.Add(yHelpDivision);
			}
		}

		private void DrawYLabels(double length)
		{
			if (YAbsoluteStep == 0) return;
			double yAbsoluteStep = length < 0 ? -YAbsoluteStep : YAbsoluteStep;
			int stepsCount = Math.Abs((int)(length / YRelativeStep));
			double label = 0;

			for (int i = 0; i <= stepsCount; i++)
			{
				if (label == 0)
				{
					label += (length < 0) ? -YRelativeStep : YRelativeStep;
					continue;
				}

				TextBlock textY = CreateLabel(label.ToString());
				textY.SetValue(Canvas.BottomProperty, CanvasHeight / 2 + i * yAbsoluteStep - LabelIndent);
				textY.SetValue(Canvas.LeftProperty, CanvasWidth / 2 + LabelIndent);

				canvas.Children.Add(textY);
				label += (length < 0) ? -YRelativeStep : YRelativeStep;
			}
		}
		#endregion

		#region Points		
		private static void PointsChangedEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is CartesianCoordinateSystemControl control)
			{
				if (e.OldValue is ObservableCollection<Point> oldPoints)
				{
					oldPoints.CollectionChanged -= control.PointsChangedEvent;
					control.ErasePoints(oldPoints);
				}

				if (e.NewValue is ObservableCollection<Point> newPoints)
				{
					newPoints.CollectionChanged += control.PointsChangedEvent;
					control.DrawPoints(newPoints);
				}
			}
		}

		private void PointsChangedEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					DrawPoint((Point)e.NewItems?[0]);
					break;
				case NotifyCollectionChangedAction.Replace:
					ErasePoint((Point)e.OldItems?[0]);
					DrawPoint((Point)e.NewItems?[0]);
					break;
				case NotifyCollectionChangedAction.Remove:
					ErasePoint((Point)e.OldItems?[0]);
					break;
				case NotifyCollectionChangedAction.Reset:
					ErasePoints(UIPoints.Select(p => (Point)p.Tag));
					break;
				default:
					break;
			}
		}

		private void DrawPoints(IEnumerable<Point> points)
		{
			if (points == null) return;
			foreach (Point point in points)
			{
				DrawPoint(point);
			}
		}

		private void DrawPoint(Point point)
		{
			double pointAbsoluteX = CanvasWidth / 2 + point.X * XAbsoluteStep;
			double pointAbsoluteY = CanvasHeight / 2 - point.Y * YAbsoluteStep;			
			var toolTip = new ToolTip();
			toolTip.Content = ToolTip = $"({point.X}; {point.Y})";
			toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
			Path path = new Path() { Fill = PointBackground, Tag = point, ToolTip = toolTip };
			path.Data = new EllipseGeometry
				(new System.Windows.Point(pointAbsoluteX, pointAbsoluteY), PointRadius, PointRadius);

			UIPoints.Add(path);
			canvas.Children.Add(path);
		}

		private void ErasePoints(IEnumerable<Point> points)
		{
			if (points == null) return;
			foreach (Point point in points)
			{
				ErasePoint(point);
			}
		}

		private void ErasePoint(Point point)
		{
			List<Path> uiPointsToDel = new List<Path>();
			foreach (Path uiPoint in UIPoints)
			{
				if (uiPoint.Tag != null && (Point)uiPoint.Tag == point)
				{
					canvas.Children.Remove(uiPoint);
				}
			}

			uiPointsToDel.ForEach(e => UIPoints.Remove(e));
		}
		#endregion

		#region Validation
		private static bool IsAxisValid(object value)
		{
			return ((AxisLength)value).IsValid();
		}
		#endregion

		private TextBlock CreateLabel(string labelText)
		{
			return new TextBlock()
			{
				Text = labelText,
				FontSize = LabelFontSize,
				Foreground = LabelForeground
			};
		}

		private void CanvasMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{			
			System.Windows.Point mousePoint = e.GetPosition(this);
			double xAbsolute = (mousePoint.X - CanvasWidth / 2) / XAbsoluteStep;
			double yAbsolute = -(mousePoint.Y - CanvasHeight / 2) / YAbsoluteStep;
			if (MouseRelativeX != xAbsolute)
			{
				MouseRelativeX = xAbsolute;
			}

			if (MouseRelativeY != yAbsolute)
			{
				MouseRelativeY = yAbsolute;
			}
		}		
	}
}

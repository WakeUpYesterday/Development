using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TestWPF.Classes;
using TestWPF.Functions;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for ZoomDev.xaml
    /// </summary>
    public partial class ZoomDev : Window
    {

        private bool ButtonDownIsCalled = false;
        private bool ButtonUpIsCalled = false;
        private bool ButtonLeftIsCalled = false;
        private bool ButtonRightIsCalled = false;
        private bool ZoomFlag = false;
        private int i = 0;
        private double Zoom = 0.000001;

        private double SZoom = 1;
        private DispatcherTimer drawingTimer;
        private Random rnd = new Random();
        private double CurrentX, CurrentY;
        private double CurrentLng, CurrentLat;

        private double Edge = 50;

        private double DeferedY = 0;
        private double DeferedX = 0;

        private Polygon myPolygon;

        private Path Path;

        Point RecentlyPoint { set; get; }
        Point CurrentPoint { set; get; }

        double LastLinesDistanceY { set; get; }
        double LastLinesDistanceX { set; get; }

        double LastLinesDegree { set; get; }
        double CurrentSlope { set; get; }
        double RecentlySlope { set; get; }

        private int currIndex = 0;

        private List<PointLatLon> PointLatLonList = new List<PointLatLon>();

        private List<PointLatLon> RecentlyPointLatLonList = new List<PointLatLon>();

        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;

        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        private Point origZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point origContentMouseDownPoint;

        /// <summary>
        /// Records which mouse button clicked during mouse dragging.
        /// </summary>
        private MouseButton mouseButtonDown;
        public ZoomDev()
        {
            InitializeComponent();
            SetTimer();
            SetPointLatLonList();
            SetPolygon();

            Console.WriteLine(zoomAndPanControl.ContentScale);
        }

        private void SetPolygon()
        {
            myPolygon = new Polygon();
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            myPolygon.StrokeThickness = 2;
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;
            System.Windows.Point Point1 = new System.Windows.Point(1, 50);
            System.Windows.Point Point2 = new System.Windows.Point(10, 80);
            System.Windows.Point Point3 = new System.Windows.Point(50, 50);
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPolygon.Points = myPointCollection;

            // this.content.Children.Add(myPolygon);

        }

        /// <summary>
        /// Event raised when the Window has loaded.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //HelpTextWindow helpTextWindow = new HelpTextWindow();
            //helpTextWindow.Left = this.Left + this.Width + 5;
            //helpTextWindow.Top = this.Top;
            //helpTextWindow.Owner = this;
            //helpTextWindow.Show();
        }

        /// <summary>
        /// Event raised on mouse down in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            content.Focus();
            Keyboard.Focus(content);

            mouseButtonDown = e.ChangedButton;
            origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
            origContentMouseDownPoint = e.GetPosition(content);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
                (e.ChangedButton == MouseButton.Left ||
                 e.ChangedButton == MouseButton.Right))
            {
                // Shift + left- or right-down initiates zooming mode.
                mouseHandlingMode = MouseHandlingMode.Zooming;
            }
            else if (mouseButtonDown == MouseButton.Left)
            {
                // Just a plain old left-down initiates panning mode.
                mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                // Capture the mouse so that we eventually receive the mouse up event.
                zoomAndPanControl.CaptureMouse();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                if (mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (mouseButtonDown == MouseButton.Left)
                    {
                        // Shift + left-click zooms in on the content.
                        ZoomIn();
                    }
                    else if (mouseButtonDown == MouseButton.Right)
                    {
                        // Shift + left-click zooms out from the content.
                        ZoomOut();
                    }
                }

                zoomAndPanControl.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHandlingMode == MouseHandlingMode.Panning)
            {
                //
                // The user is left-dragging the mouse.
                // Pan the viewport by the appropriate amount.
                //
                Point curContentMousePoint = e.GetPosition(content);
                Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised by rotating the mouse wheel
        /// </summary>
        private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                ZoomIn();
            }
            else if (e.Delta < 0)
            {
                ZoomOut();
            }
        }

        /// <summary>
        /// The 'ZoomIn' command (bound to the plus key) was executed.
        /// </summary>
        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn();
        }

        /// <summary>
        /// The 'ZoomOut' command (bound to the minus key) was executed.
        /// </summary>
        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut();
        }

        /// <summary>
        /// Zoom the viewport out by a small increment.
        /// </summary>
        private void ZoomOut()
        {
            zoomAndPanControl.ContentScale -= 0.1;
        }

        /// <summary>
        /// Zoom the viewport in by a small increment.
        /// </summary>
        private void ZoomIn()
        {
            zoomAndPanControl.ContentScale += 0.1;
        }

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            content.Focus();
            Keyboard.Focus(content);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                //
                // When the shift key is held down special zooming logic is executed in content_MouseDown,
                // so don't handle mouse input here.
                //
                return;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                //
                // We are in some other mouse handling mode, don't do anything.
                return;
            }

            mouseHandlingMode = MouseHandlingMode.DraggingRectangles;
            origContentMouseDownPoint = e.GetPosition(content);

            Rectangle rectangle = (Rectangle)sender;
            rectangle.CaptureMouse();

            e.Handled = true;
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            {
                //
                // We are not in rectangle dragging mode.
                //
                return;
            }

            mouseHandlingMode = MouseHandlingMode.None;

            Rectangle rectangle = (Rectangle)sender;
            rectangle.ReleaseMouseCapture();

            e.Handled = true;
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            {
                //
                // We are not in rectangle dragging mode, so don't do anything.
                //
                return;
            }

            Point curContentPoint = e.GetPosition(content);
            Vector rectangleDragVector = curContentPoint - origContentMouseDownPoint;

            //
            // When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
            //

            origContentMouseDownPoint = curContentPoint;

            Rectangle rectangle = (Rectangle)sender;
            Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) + rectangleDragVector.X);
            Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + rectangleDragVector.Y);

            e.Handled = true;
        }

        #region lining

        private void CheckEdges(Point p)
        {
            if (p.Y + this.DeferedY >= this.content.Height - this.Edge)
            {
                BtnUp_OnClick(new object(), new RoutedEventArgs());
            }
            if (p.Y + this.DeferedY <= this.Edge)
            {
                BtnDown_OnClick(new object(), new RoutedEventArgs());
            }
            if (p.X + this.DeferedX >= this.content.Width - this.Edge)
            {
                BtnLeft_OnClick(new object(), new RoutedEventArgs());
            }
            if (p.X + this.DeferedX <= this.Edge)
            {
                BtnRight_OnClick(new object(), new RoutedEventArgs());

            }
        }

        private void drawingTimer_Tick(object sender, EventArgs e)
        {

            this.currIndex++;

            if (this.currIndex == this.PointLatLonList.Count)
            {
                drawingTimer.IsEnabled = false;
                this.currIndex = 0;
                return;
            }
            RecentlyPointLatLonList.Add(PointLatLonList[currIndex]);
            double yMinus = (PointLatLonList[currIndex].Latitude.Value - CurrentLat) / (Zoom);
            double xMinus = (PointLatLonList[currIndex].Longitude.Value - CurrentLng) / (Zoom);

            //y de ki değişim ters olduğu için çıkarma işlemi yapıldı.
            Lining(new Point(this.CurrentX, this.CurrentY), new Point(this.CurrentX += xMinus, this.CurrentY -= yMinus),Brushes.Yellow);

            this.CurrentLat = PointLatLonList[currIndex].Latitude.Value;
            this.CurrentLng = PointLatLonList[currIndex].Longitude.Value;

        }
        private void BtnUp_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedY -= 2;
            foreach (var item in this.content.Children.OfType<Line>())
            {
                if (item.Y1 <= this.Edge)
                {
                    ZoomExOut();
                    break;
                }
                item.Y1 -= 2;
                item.Y2 -= 2;
            }

        }
        private void BtnLeft_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedX -= 2;
            foreach (var item in this.content.Children.OfType<Line>())
            {
                if (item.X1 <= this.Edge)
                {
                    ZoomExOut();
                    break;
                }
                item.X1 -= 2;
                item.X2 -= 2;
            }

        }

        private void BtnDown_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedY += 2;
            foreach (var item in this.content.Children.OfType<Line>())
            {
                if (item.Y2 >= this.content.Height - this.Edge)
                {
                    ZoomExOut();
                    break;
                }

                item.Y1 += 2;
                item.Y2 += 2;
            }

        }

        private void BtnRight_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedX += 2;
            foreach (var item in this.content.Children.OfType<Line>())
            {
                if (item.X2 >= this.content.Width - this.Edge)
                {
                    ZoomExOut();
                    break;
                }

                item.X1 += 2;
                item.X2 += 2;
            }
        }
        private void SetPointLatLonList()
        {
            try
            {
                PointLatLonList = GpsOperations.GetPointLatLonList();

                CurrentLng = PointLatLonList[0].Longitude.Value;
                CurrentLat = PointLatLonList[0].Latitude.Value;

                RecentlyPointLatLonList.Add(PointLatLonList[0]);


                this.CurrentX = this.content.Width / 2;
                //this.CurrentY = 10;
                this.CurrentY = this.content.Height / 2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (!this.drawingTimer.IsEnabled)
                this.drawingTimer.Start();
        }
        
        private void ReDrawLines()
        {
            double cx = this.content.Width / 2;
            double cy= this.content.Height / 2;

  
            double yMinus = 0;
            double xMinus = 0;
           
            for(int i = 0; i < RecentlyPointLatLonList.Count - 1; i++)
            {
                 yMinus = (RecentlyPointLatLonList[i+1].Latitude.Value - RecentlyPointLatLonList[i].Latitude.Value) / (Zoom);
                 xMinus = (RecentlyPointLatLonList[i+1].Longitude.Value - RecentlyPointLatLonList[i].Longitude.Value) / (Zoom);
                 Lining(new Point(cx, cy), new Point(cx += xMinus, cy -= yMinus), Brushes.Red);

                Console.WriteLine(cx +","+ cy);
            }
            //this.currIndex = RecentlyPointLatLonList.Count - 1;    
            this.CurrentX = cx;
            this.CurrentY = cy;
        
        }

        private void ClearLines()
        {

            ZoomFlag = true;
            this.content.Children.Clear();
           
        }

        private void ZoomExOut()
        {      
            drawingTimer.IsEnabled = false;
            ClearLines();
            Zoom += 0.000001;
            ReDrawLines();
            System.Threading.Thread.Sleep(1000);
            drawingTimer.IsEnabled = true;
        }

        private void ZoomOutEx_Click(object sender, RoutedEventArgs e)
        {
            ZoomExOut();
        }

        private void ZoomInEx_Click(object sender, RoutedEventArgs e)
        {
            ClearLines();
            Zoom -= 0.000001;
            ReDrawLines();
        }

        public void SetTimer()
        {
            drawingTimer = new DispatcherTimer();
            this.drawingTimer.Tick += new EventHandler(drawingTimer_Tick);
            this.drawingTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }

        public void Lining(Point p1, Point p2,SolidColorBrush b)
        {
            this.CheckEdges(p2);

            Line line = new Line();

            line.Stroke = b;
            line.StrokeThickness = 4;

            line.X1 = (Math.Floor(Math.Floor(p1.X))) + this.DeferedX;
            line.Y1 = (Math.Floor(Math.Floor(p1.Y))) + this.DeferedY;
            line.X2 = (Math.Floor(Math.Floor(p2.X))) + this.DeferedX;
            line.Y2 = (Math.Floor(Math.Floor(p2.Y))) + this.DeferedY;

            content.Children.Add(line);
        }
        #endregion

    }
}

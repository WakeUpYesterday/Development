using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private double Zoom = 0.000003;
        private double ZoomIncDec = 0.000001;

        private DispatcherTimer drawingTimer;
        private Random rnd = new Random();
        private double CurrentX, CurrentY;

        private double BeforeZoomFirstX, BeforeZoomFirstY;
        private double BeforeZoomMaxX, BeforeZoomMaxY;
        private double BeforeZoomMinX, BeforeZoomMinY;


        private double CurrentLng, CurrentLat;

        private double Edge = 100;

        private int currIndex = 0;

        private List<PointLatLon> PointLatLonList = new List<PointLatLon>();

        private List<PointLatLon> RecentlyPointLatLonList = new List<PointLatLon>();


        public ZoomDev()
        {
            InitializeComponent();
            SetTimer();
            SetPointLatLonList();

        }

        #region lining


        private void drawingTimer_Tick(object sender, EventArgs e)
        {
            this.currIndex++;

            if (this.currIndex == this.PointLatLonList.Count)
            {
                drawingTimer.IsEnabled = false;
                this.currIndex = 0;
                Console.WriteLine(this.content.Children.OfType<Line>().ToList().Count());
                return;
            }
            RecentlyPointLatLonList.Add(PointLatLonList[currIndex]);
            double yMinus = (PointLatLonList[currIndex].Latitude.Value - CurrentLat) / (Zoom);
            double xMinus = (PointLatLonList[currIndex].Longitude.Value - CurrentLng) / (Zoom);

            //y de ki değişim ters olduğu için çıkarma işlemi yapıldı.
            Lining(new Point(this.CurrentX, this.CurrentY), new Point(this.CurrentX += xMinus, this.CurrentY -= yMinus), Brushes.Yellow);

            this.CurrentLat = PointLatLonList[currIndex].Latitude.Value;
            this.CurrentLng = PointLatLonList[currIndex].Longitude.Value;

        }
        private void SetPointLatLonList()
        {
            try
            {
                PointLatLonList = GpsOperations.GetPointLatLonList();
                Console.WriteLine(PointLatLonList.Count);
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


        public void Lining(Point p1, Point p2, SolidColorBrush b)
        {
            AddLine(p1.X, p1.Y, p2.X, p2.Y);

            var llist = content.Children.OfType<Line>().ToList();
            var maxX = llist.Max(m => m.X1);
            var maxY = llist.Max(m => m.Y1);

            var isZoomable = false;
            if (maxY > this.content.Height - this.Edge || maxX > this.content.Width - this.Edge)
            {
                isZoomable = true;
                ZoomExOut();
            }

            if (!isZoomable)
            {
                GoCenter(false);
            }
        }
        private void ReDrawLines(double cx, double cy)
        {
            for (int i = 0; i < RecentlyPointLatLonList.Count - 1; i++)
            {
                var yMinus = (RecentlyPointLatLonList[i + 1].Latitude.Value - RecentlyPointLatLonList[i].Latitude.Value) / (Zoom);
                var xMinus = (RecentlyPointLatLonList[i + 1].Longitude.Value - RecentlyPointLatLonList[i].Longitude.Value) / (Zoom);

                AddLine(cx, cy, cx += xMinus, cy -= yMinus);
            }

            GoCenter(true);
        }

        private void AddLine(double x1, double y1, double x2, double y2)
        {
            Line line1 = new Line() { Stroke = Brushes.Orange, StrokeThickness = 4 };
            line1.X1 = Math.Floor(x1);
            line1.Y1 = Math.Floor(y1);
            line1.X2 = Math.Floor(x2);
            line1.Y2 = Math.Floor(y2);
            content.Children.Add(line1);
        }
        private void ClearLines()
        {
            this.content.Children.Clear();
        }

        private void ZoomExOut()
        {
            drawingTimer.IsEnabled = false;
            var line = this.content.Children.OfType<Line>().ToList();
            if (line != null)
            {
                ClearLines();
                Zoom += ZoomIncDec;
                ReDrawLines(line[0].X1, line[0].Y1);
            }
            drawingTimer.IsEnabled = true;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (!this.drawingTimer.IsEnabled)
                this.drawingTimer.Start();
        }
        private void ZoomOutEx_Click(object sender, RoutedEventArgs e)
        {
            ZoomExOut();
        }
        private void ZoomInEx_Click(object sender, RoutedEventArgs e)
        {
            drawingTimer.IsEnabled = false;
            var line = this.content.Children.OfType<Line>().ToList();
            if (line != null)
            {
                ClearLines();
                Zoom -= ZoomIncDec;
                ReDrawLines(line[0].X1, line[0].Y1);
            }
            drawingTimer.IsEnabled = true;
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            drawingTimer.IsEnabled = !drawingTimer.IsEnabled;
        }

        public void SetTimer()
        {
            drawingTimer = new DispatcherTimer();
            this.drawingTimer.Tick += new EventHandler(drawingTimer_Tick);
            this.drawingTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }

        private void GoCenter(bool isZoomable)
        {
            List<Point> points = GetPointsFromLines();
            Point p = GetFiledCenter2(points);
            p = GetDistancePair(p);

            var list = this.content.Children.OfType<Line>();
            foreach (var item in list)
            {
                item.X1 += p.X;
                item.X2 += p.X;
                item.Y1 += p.Y;
                item.Y2 += p.Y;
            }
            if (isZoomable)
            {
                this.CurrentX = list.LastOrDefault().X2;
                this.CurrentY = list.LastOrDefault().Y2;
            }
            else
            {
                this.CurrentX += p.X;
                this.CurrentY += p.Y;
            }
        }
        private Point GetFiledCenter2(List<Point> points)
        {
            var maxX = points.Max(m => m.X);
            var maxY = points.Max(m => m.Y);
            var minX = points.Min(m => m.X);
            var minY = points.Min(m => m.Y);

            return new Point((maxX + minX) / 2, (maxY + minY) / 2);
        }
        private Point GetDistancePair(Point centerPoint)
        {
            double xEx = this.content.Width / 2 - centerPoint.X;
            double yEx = this.content.Height / 2 - centerPoint.Y;

            return new Point(xEx, yEx);
        }
        private List<Point> GetPointsFromLines()
        {
            List<Line> lines = this.content.Children.OfType<Line>().ToList();

            List<Point> points = new List<Point>();

            Point point;
            point = new Point(lines[0].X1, lines[0].Y1);
            points.Add(point);
            foreach (var item in lines)
            {
                point = new Point(item.X2, item.Y2);
                points.Add(point);
            }

            return points;
        }

        #endregion

    }
}

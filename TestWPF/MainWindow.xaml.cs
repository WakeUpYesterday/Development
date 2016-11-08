using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestWPF
{
    using System.Linq;
    using System.Windows.Threading;
    using Classes;
    using Functions;

    using MessageBox = System.Windows.MessageBox;
    using System.Windows.Controls;
    public partial class MainWindow : Window
    {

        private int i = 0;
        private double Zoom = 0.000005;

        private double SZoom = 1;
        private DispatcherTimer drawingTimer;
        private Random rnd = new Random();
        private double CurrentX, CurrentY;
        private double CurrentLng, CurrentLat;

        private double Edge = 50;

        private double DeferedY = 0;
        private double DeferedX = 0;

        private Polygon Polygon;

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
        public MainWindow()
        {
            InitializeComponent();
            SetTimer();
            SetPointLatLonList();
            var dc = new ABC();
            dc.Name = 5;           

            this.DataContext = dc;
        }
        private void SetPointLatLonList()
        {
            try
            {
                PointLatLonList = GpsOperations.GetPointLatLonList();

                CurrentLng = PointLatLonList[0].Longitude.Value;
                CurrentLat = PointLatLonList[0].Latitude.Value;

                this.CurrentX = this.MainCanvas.Width / 3;
                //this.CurrentY = 10;
                this.CurrentY = this.MainCanvas.Height / 3;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SetTimer()
        {
            drawingTimer = new DispatcherTimer();
            this.drawingTimer.Tick += new EventHandler(drawingTimer_Tick);
            this.drawingTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        public void Lining(Point p1, Point p2)
        {
            this.CheckEdges(p2);

            Line line = new Line();

            line.Stroke = Brushes.Yellow;
            line.StrokeThickness = 1;          

            line.X1 = Math.Floor(Math.Floor(p1.X) * this.SZoom) + this.DeferedX;
            line.Y1 = Math.Floor(Math.Floor(p1.Y) * this.SZoom) + this.DeferedY;
            line.X2 = Math.Floor(Math.Floor(p2.X) * this.SZoom) + this.DeferedX;
            line.Y2 = Math.Floor(Math.Floor(p2.Y) * this.SZoom) + this.DeferedY;           

            MainCanvas.Children.Add(line);
        }




        public void CreatePath2()
        {
            Path A = GetPath6();
            A.Stroke = Brushes.White;
            A.StrokeThickness = 1;

            this.MainCanvas.Children.Clear();
            this.MainCanvas.Children.Add(A);



            //Polyline Polyline = new Polyline();
            //Polyline.Points = new PointCollection(GetPoints());

            //Polyline.Stroke = Brushes.Blue;
            //Polyline.StrokeThickness = 3;

            //this.MainCanvas.Children.Add(Polyline);
        }

        private Path GetPath6()
        {
            Path Path = new Path();           

            Path.Data = GetPathGeometry5(); Console.WriteLine(Path.Data.ToString());
          //  Path.Data =  Geometry.Parse("M 100,200 C 100,25 400,350 400,175"); Console.WriteLine(Path.Data.ToString());




            return Path;
        }

        private PathGeometry GetPathGeometry5()
        {
            PathGeometry PathGeometry = new PathGeometry();

            PathGeometry.AddGeometry(new PathGeometry(GetPathFigureCollection4()));

            //PathGeometry.Figures = GetPathFigureCollection4();

            return PathGeometry;
        }

        private PathFigureCollection GetPathFigureCollection4()
        {
            PathFigureCollection PathFigureCollection = new PathFigureCollection();

            PathFigureCollection.Add(GetPathFigure3());

            return PathFigureCollection;
        }
        private PathFigure GetPathFigure3()
        {
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = GetPoints()[0];
            pathFigure.Segments = GetPathSegmentCollection2();

            return pathFigure;
        }

        private PathSegmentCollection GetPathSegmentCollection2()
        {
            PathSegmentCollection PathSegmentCollection = new PathSegmentCollection();

            PathSegmentCollection.Add(GetPolyBezierSegment1());

            return PathSegmentCollection;
        }


        private PolyBezierSegment GetPolyBezierSegment1()
        {
            PolyBezierSegment polyBezierSegment = new PolyBezierSegment();
            PointCollection pointCollection = new PointCollection(GetPoints());
            polyBezierSegment.Points = pointCollection;

            return polyBezierSegment;
        }






        private void CheckEdges(Point p)
        {
            if (p.Y + this.DeferedY >= this.MainCanvas.Height - this.Edge)
            {
                BtnUp_OnClick(new object(), new RoutedEventArgs());
            }
            if (p.Y + this.DeferedY <= this.Edge)
            {
                BtnDown_OnClick(new object(), new RoutedEventArgs());
            }
            if (p.X + this.DeferedX >= this.MainCanvas.Width - this.Edge)
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

            double yMinus = (PointLatLonList[currIndex].Latitude.Value - CurrentLat) / Zoom;
            double xMinus = (PointLatLonList[currIndex].Longitude.Value - CurrentLng) / Zoom;

            //y de ki değişim ters olduğu için çıkarma işlemi yapıldı.
            Lining(new Point(this.CurrentX, this.CurrentY), new Point(this.CurrentX += xMinus, this.CurrentY -= yMinus));

            this.CurrentLat = PointLatLonList[currIndex].Latitude.Value;
            this.CurrentLng = PointLatLonList[currIndex].Longitude.Value;

        }
        private void BtnUp_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedY -= 2;
            foreach (var item in this.MainCanvas.Children.OfType<Line>())
            {
                if (item.Y1 - this.Edge < 0)
                {
                    return;
                }

                item.Y1 -= 2;
                item.Y2 -= 2;
            }

        }
        private void BtnLeft_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedX -= 2;
            foreach (var item in this.MainCanvas.Children.OfType<Line>())
            {
                item.X1 -= 2;
                item.X2 -= 2;
            }

        }

        private void BtnDown_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedY += 2;
            foreach (var item in this.MainCanvas.Children.OfType<Line>())
            {

                item.Y1 += 2;
                item.Y2 += 2;
            }

        }

        private void BtnRight_OnClick(object sender, RoutedEventArgs e)
        {
            this.DeferedX += 2;
            foreach (var item in this.MainCanvas.Children.OfType<Line>())
            {
                item.X1 += 2;
                item.X2 += 2;
            }
        }

        


        private void BtnGetList_OnClick(object sender, RoutedEventArgs e)
        {

            if (this.MainCanvas.Children.OfType<Line>().ToList().Count > 0 && !this.drawingTimer.IsEnabled)
            {

                Polygon = GetPolygon();
                Point polygonCenter = GetPlygonCenter(Polygon);
                SignCenter(polygonCenter);
                ToCenterPolygon(Polygon, polygonCenter);
            }
        }

        private void ToCenterPolygon(Polygon polygon, Point polygonCenter)
        {

            double xEx = this.MainCanvas.Width / 2 - polygonCenter.X;
            double yEx = this.MainCanvas.Height / 2 - polygonCenter.Y;

            var List = polygon.Points.ToList();
            var removedLines = MainCanvas.Children.OfType<Line>().ToList();
            Point point;

            polygon.Points.Clear();
            foreach (var item in List)
            {
                point = new Point(item.X + xEx, item.Y + yEx);
                polygon.Points.Add(point);
            }

            foreach (var item in removedLines)
            {
                MainCanvas.Children.Remove(item);
            }


            MainCanvas.Children.Add(polygon);
        }

        private void SignCenter(Point polygonCenter)
        {
            Line l1 = new Line();
            l1.Stroke = Brushes.Yellow;
            l1.StrokeThickness = 2;
            l1.X1 = polygonCenter.X - 6;
            l1.X2 = polygonCenter.X + 6;

            l1.Y1 = l1.Y2 = polygonCenter.Y;

            Line l2 = new Line();
            l2.Stroke = Brushes.Yellow;
            l2.StrokeThickness = 2;
            l2.Y1 = polygonCenter.Y - 6;
            l2.Y2 = polygonCenter.Y + 6;

            l2.X1 = l2.X2 = polygonCenter.X;

            MainCanvas.Children.Add(l1);
            MainCanvas.Children.Add(l2);

        }

        private Point GetPlygonCenter(Polygon polygon)
        {

            Point centroid = polygon.Points.Aggregate(
                new { xSum = 0.0, ySum = 0.0, n = 0 },
                (acc, p) => new
                {
                    xSum = acc.xSum + p.X,
                    ySum = acc.ySum + p.Y,
                    n = acc.n + 1
                },
                acc => new Point(acc.xSum / acc.n, acc.ySum / acc.n));

            return centroid;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (!this.drawingTimer.IsEnabled)
                this.drawingTimer.Start();
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {

            //var Lines = MainCanvas.Children.OfType<Line>().ToList();

            //foreach (var item in Lines)
            //{
            //    item.X1 *= 0.8;
            //    item.Y1 *= 0.8;
            //    item.X2 *= 0.8;
            //    item.Y2 *= 0.8;
            //}


            CreatePath2();

         

        }

        private Polygon GetPolygon()
        {
            Polygon polygon = new Polygon();
            PointCollection pointCollection = new PointCollection(GetPoints());
            polygon.Points = pointCollection;
            polygon.Stroke = Brushes.White;
            polygon.StrokeThickness = 2;

            return polygon;
        }

        private List<Point> GetPoints()
        {
            List<Line> lines = MainCanvas.Children.OfType<Line>().ToList();

            List<Point> points = new List<Point>();
            int i = 0;
            Point point;

            if (lines.Count > 0)
            {
                point = new Point((int)lines[0].X1, (int)lines[0].Y1);
                foreach (var item in lines)
                {
                    point = new Point((int)item.X2, (int)item.Y2);
                    points.Add(point);
                }
                point = new Point((int)lines[lines.Count - 1].X1, (int)lines[lines.Count - 1].Y1);
            }
          
            return points;

        }
    }


    public class ABC
    {
        public long Name { set; get; }
    }
}

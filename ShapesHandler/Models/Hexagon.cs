using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;

namespace ShapesHandler.Models
{
    public class Hexagon: CustomShape
    {

        public float Side { get; set; }

        public Hexagon() {}
        
        public Hexagon(string name, Point center, float side)
        {
            this.Name = name;
            this.Type = ShapeType.Hexagon;
            this.Center = center;
            this.XLabel = (float)center.X;
            this.YLabel = (float)center.Y;
            this.Side = side;
            this.WidthC = side*2;
            this.HeightC = (float)(side*Math.Sqrt(3));
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return ShapePathGeometry();
            }
        }

        private Geometry ShapePathGeometry()
        {
            Point pointLe = new Point(Center.X - Side, Center.Y); //Left Point
            Point pointTL = new Point(Center.X - Side / 2, Center.Y - Side * Math.Sqrt(3)/ 2); //TopLeft Point
            Point pointTR = new Point(Center.X + Side / 2, Center.Y - Side * Math.Sqrt(3) / 2);
            Point pointRi = new Point(Center.X + Side, Center.Y); //Right Point
            Point pointBR = new Point(Center.X + Side / 2, Center.Y + Side * Math.Sqrt(3) / 2);
            Point pointBL = new Point(Center.X - Side / 2, Center.Y + Side * Math.Sqrt(3) / 2);
            PathFigure hexPathFigure = new PathFigure();
            hexPathFigure.StartPoint = pointLe;
            LineSegment leftTopSegment = new LineSegment();
            leftTopSegment.Point = pointTL;
            LineSegment topSegment = new LineSegment();
            topSegment.Point = pointTR;
            LineSegment rightTopSegment = new LineSegment();
            rightTopSegment.Point = pointRi;
            LineSegment rightBottomSegment = new LineSegment();
            rightBottomSegment.Point = pointBR;
            LineSegment bottomSegment = new LineSegment();
            bottomSegment.Point = pointBL;
            LineSegment leftBottomSegment = new LineSegment();
            leftBottomSegment.Point = pointLe;
            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(leftTopSegment);
            segmentCollection.Add(topSegment);
            segmentCollection.Add(rightTopSegment);
            segmentCollection.Add(rightBottomSegment);
            segmentCollection.Add(bottomSegment);
            segmentCollection.Add(leftBottomSegment);
            hexPathFigure.Segments = segmentCollection;
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(hexPathFigure);
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pathFigureCollection;

            //Add pathgeometry and ellipsis center point
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(pathGeometry);
            EllipseGeometry centre = new EllipseGeometry(Center, 2.0f, 2.0f);
            geometryGroup.Children.Add(centre);
            //Add Center Label
            string centerLabel = null;
            if (this.IsCopy) centerLabel = ""; 
            else centerLabel = XLabel.ToString() + " , " + YLabel.ToString();
            FormattedText ft = new FormattedText(centerLabel, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 13, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X, Center.Y - 7.0f));
            geometryGroup.Children.Add(centerText);
            //Add Name Label
            FormattedText ft2 = new FormattedText(Name, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 14, Brushes.Black);
            Geometry nameText = ft2.BuildGeometry(new Point(Center.X, Center.Y + Side * Math.Sqrt(3)/ 2 + 3.0f));
            geometryGroup.Children.Add(nameText);

            return geometryGroup;
        }

    }
}

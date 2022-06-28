using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

namespace ShapesHandler.Models
{
    public class Rectangle:Shape
    {
        //public string Name { get; set; }
        public ShapeType Type { get; set; }
        public Point Center { get; set; }
        public float WidthC { get; set; } //Width Custom
        public float HeightC { get; set; } //Height Custom
        public PathGeometry Path { get; set; }
        public FormattedText Text { get; set; }
        public GeometryGroup PathGroup { get; set; }

        public Rectangle(string Name, Point Center, float Width, float Height)
        {
            this.Name = Name;
            this.Type = ShapeType.Rectangle;
            this.Center = Center;
            this.WidthC = Width;
            this.HeightC = Height;
            //this.Stroke = Brushes.Blue;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {   /*
                //GeometryGroup geometryGroup = new GeometryGroup();
                Point pointTL = new Point(Center.X - Width / 2, Center.Y - Height / 2);
                Point pointTR = new Point(Center.X + Width / 2, Center.Y - Height / 2);
                Point pointBR = new Point(Center.X + Width / 2, Center.Y + Height / 2);
                Point pointBL = new Point(Center.X - Width / 2, Center.Y + Height / 2);
                PathFigure rectPathFigure = new PathFigure();
                rectPathFigure.StartPoint = pointTL;
                LineSegment topSegment = new LineSegment();
                topSegment.Point = pointTR;
                LineSegment rightSegment = new LineSegment();
                rightSegment.Point = pointBR;
                LineSegment bottomSegment = new LineSegment();
                bottomSegment.Point = pointBL;
                LineSegment leftSegment = new LineSegment();
                leftSegment.Point = pointTL;
                PathSegmentCollection segmentCollection = new PathSegmentCollection();
                segmentCollection.Add(topSegment);
                segmentCollection.Add(rightSegment);
                segmentCollection.Add(bottomSegment);
                segmentCollection.Add(leftSegment);
                rectPathFigure.Segments = segmentCollection;
                PathFigureCollection pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(rectPathFigure);
                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = pathFigureCollection;

                //Add pathgeometry and ellipsis point
                GeometryGroup geometryGroup = new GeometryGroup();
                geometryGroup.Children.Add(pathGeometry);
                EllipseGeometry centre = new EllipseGeometry(Center, 2.0f, 2.0f);
                geometryGroup.Children.Add(centre);
                return geometryGroup; */
                return ShapePathGeometry();
            }
        }

        private Geometry ShapePathGeometry()
        {   
            Point pointTL = new Point(Center.X - WidthC / 2, Center.Y - HeightC / 2);
            Point pointTR = new Point(Center.X + WidthC / 2, Center.Y - HeightC / 2);
            Point pointBR = new Point(Center.X + WidthC / 2, Center.Y + HeightC / 2);
            Point pointBL = new Point(Center.X - WidthC / 2, Center.Y + HeightC / 2);
            PathFigure rectPathFigure = new PathFigure();
            rectPathFigure.StartPoint = pointTL;
            LineSegment topSegment = new LineSegment();
            topSegment.Point = pointTR;
            LineSegment rightSegment = new LineSegment();
            rightSegment.Point = pointBR;
            LineSegment bottomSegment = new LineSegment();
            bottomSegment.Point = pointBL;
            LineSegment leftSegment = new LineSegment();
            leftSegment.Point = pointTL;
            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(topSegment);
            segmentCollection.Add(rightSegment);
            segmentCollection.Add(bottomSegment);
            segmentCollection.Add(leftSegment);
            rectPathFigure.Segments = segmentCollection;
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(rectPathFigure);
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pathFigureCollection;

            //Add pathgeometry and ellipsis point
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(pathGeometry);
            EllipseGeometry centre = new EllipseGeometry(Center, 2.0f, 2.0f);
            geometryGroup.Children.Add(centre); 
            //GeometryGroup geometryGroup = new GeometryGroup();
            //Add Center Label
            string centerLabel = Center.X.ToString() + " , " + Center.Y.ToString();
            FormattedText ft = new FormattedText(centerLabel, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Verdana"), 10, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X,Center.Y-7.0f));
            geometryGroup.Children.Add(centerText);
            //Add Name Label
            FormattedText ft2 = new FormattedText(Name, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 10, Brushes.Black);
            Geometry nameText = ft2.BuildGeometry(new Point(Center.X, Center.Y + HeightC / 2 + 3.0f));
            geometryGroup.Children.Add(nameText);

            return geometryGroup;
        }
        
    }
}

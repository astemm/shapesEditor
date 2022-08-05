using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;

namespace ShapesHandler.Models
{
    public class Circle: CustomShape
    {

        public float Radius { get; set; }

        public Circle() {}
        
        public Circle(string name, Point center, float radius)
        {
            this.Name = name;
            this.Type = ShapeType.Circle;
            this.Center = center;
            this.XLabel = (float)center.X;
            this.YLabel = (float)center.Y;
            this.Radius = radius;
            this.WidthC = radius*2;
            this.HeightC = radius*2;
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
            //Circle comprises of 18 connecting points, one starting on the right and 17 else
            Point[] pointsArray=new Point[18];
            Point pointRi = new Point(Center.X + Radius, Center.Y); //Rightmost point - cos 0
            double angle = 0;
            for (int i = 0; i < 17; i++)
            {
                angle = (Math.PI * (i+1) * 20 )/ 180;
                pointsArray[i].X = Center.X + Radius * Math.Cos(angle);
                pointsArray[i].Y = Center.Y - Radius * Math.Sin(angle);
            }
            pointsArray[17] = pointRi;

            PathFigure circlePathFigure = new PathFigure();
            circlePathFigure.StartPoint = pointRi;
            LineSegment[] lineSegments = new LineSegment[18];

            for (int i = 0; i < 18; i++)
            {
                LineSegment lineSegment = new LineSegment();
                lineSegment.Point=pointsArray[i];
                lineSegments[i] = lineSegment;
            }

            PathSegmentCollection segmentCollection = new PathSegmentCollection(lineSegments.ToList());
            circlePathFigure.Segments = segmentCollection;
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(circlePathFigure);
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
            else centerLabel = Math.Round((double)XLabel).ToString() + ", " + Math.Round((double)YLabel).ToString();
            Trace.WriteLine(Center.X + " " + Center.Y);
            FormattedText ft = new FormattedText(centerLabel, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 13, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X, Center.Y - 13.0f));
            geometryGroup.Children.Add(centerText);
            //Add Name Label
            FormattedText ft2 = new FormattedText(Name, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 14, Brushes.Black);
            Geometry nameText = ft2.BuildGeometry(new Point(Center.X, Center.Y + Radius + 3.0f));
            geometryGroup.Children.Add(nameText);

            return geometryGroup;
        }
    }
}

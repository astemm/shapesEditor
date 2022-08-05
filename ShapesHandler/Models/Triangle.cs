using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Diagnostics;

namespace ShapesHandler.Models
{
    public class Triangle: CustomShape
    {
        public Point[] vertexes { get; set; }

        public Triangle() {}

        public Triangle(string name, Point[] vertexes)
        {
            this.Name = name;
            this.Type = ShapeType.Triangle;
            this.vertexes = vertexes;
            this.Center = FindCentrePoint();
            this.XLabel = (float)Center.X;
            this.YLabel = (float)Center.Y;
            FindWidthAndHeight();
        }

        public Point FindCentrePoint()
        {
            Point sideMidPoint = new Point(); //find mid of first side (0;1)
            sideMidPoint.X = (vertexes[0].X + vertexes[1].X) / 2;
            sideMidPoint.Y = (vertexes[0].Y + vertexes[1].Y) / 2;
            Vector mediane = new Vector(); //find mediane of opposite vertex to sidemidpoint
            mediane.X = vertexes[2].X - sideMidPoint.X;
            mediane.Y = vertexes[2].Y - sideMidPoint.Y;
            Point centrePoint=new Point();
            centrePoint.X = sideMidPoint.X + mediane.X / 3; 
            centrePoint.X = Math.Round(centrePoint.X, 1);
            centrePoint.Y = sideMidPoint.Y + mediane.Y / 3; 
            centrePoint.Y = Math.Round(centrePoint.Y, 1);
            //point of medianes crossing on the distance of 1/3 mediane length to the appropriate side
            return centrePoint;
        }

        private void FindWidthAndHeight()
        {
            List<double> xPoints = new List<double>();
            xPoints.Add(vertexes[0].X); xPoints.Add(vertexes[1].X); xPoints.Add(vertexes[2].X);
            List<double> yPoints = new List<double>();
            yPoints.Add(vertexes[0].Y); yPoints.Add(vertexes[1].Y); yPoints.Add(vertexes[2].Y);
            double xMin = xPoints.Min(); double xMax = xPoints.Max();
            double yMin = yPoints.Min(); double yMax = yPoints.Max();
            this.WidthC = (float)(xMax - xMin);
            this.HeightC = (float)(yMax - yMin);
        }

        public Point FindTranslationDimension()
        {
            List<double> xPoints = new List<double>();
            xPoints.Add(vertexes[0].X); xPoints.Add(vertexes[1].X); xPoints.Add(vertexes[2].X);
            List<double> yPoints = new List<double>();
            yPoints.Add(vertexes[0].Y); yPoints.Add(vertexes[1].Y); yPoints.Add(vertexes[2].Y);
            double xMin = xPoints.Min(); double xMax = xPoints.Max();
            double yMin = yPoints.Min(); double yMax = yPoints.Max();
            Point translationPoint = new Point(xMin,yMin);
            return translationPoint;
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
            Point firstPoint = vertexes[0];
            Point secondPoint = vertexes[1];
            Point thirdPoint = vertexes[2];
            PathFigure triaPathFigure = new PathFigure();
            triaPathFigure.StartPoint = firstPoint;
            LineSegment firstSegment = new LineSegment();
            firstSegment.Point = secondPoint;
            LineSegment secondSegment = new LineSegment();
            secondSegment.Point = thirdPoint;
            LineSegment thirdSegment = new LineSegment();
            thirdSegment.Point = firstPoint;
            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(firstSegment);
            segmentCollection.Add(secondSegment);
            segmentCollection.Add(thirdSegment);
            triaPathFigure.Segments = segmentCollection;
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(triaPathFigure);
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
            FormattedText ft = new FormattedText(centerLabel, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 14, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X, Center.Y - 13.0f));
            geometryGroup.Children.Add(centerText);

            //Add Name Label
            FormattedText ft2 = new FormattedText(Name, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 14, Brushes.Black);
            Geometry nameText = ft2.BuildGeometry(new Point(Center.X, Center.Y + HeightC / 2 + 1.0f));
            geometryGroup.Children.Add(nameText);

            return geometryGroup;
        }

    }
}

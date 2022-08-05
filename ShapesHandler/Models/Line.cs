using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace ShapesHandler.Models
{
    public class Line:CustomShape
    {
        public Point[] ends { get; set; }

        public Line() {}

        public Line(string name, Point[] ends)
        {
            this.Name = name;
            this.Type = ShapeType.Line;
            this.ends = ends;
            this.Center = new Point((ends[0].X + ends[1].X) / 2, (ends[0].Y + ends[1].Y) / 2);
            this.XLabel = (float)Center.X;
            this.YLabel = (float)Center.Y;
        }

        public Point FindTranslationDimension()
        {
            double xMin = Math.Min(ends[0].X,ends[1].X);
            double yMin = Math.Min(ends[0].Y,ends[1].Y);
            Point translationPoint = new Point(xMin, yMin);
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
            Point firstPoint = ends[0];
            Point secondPoint = ends[1];
            PathFigure linePathFigure = new PathFigure();
            linePathFigure.StartPoint = firstPoint;
            LineSegment firstSegment = new LineSegment();
            firstSegment.Point = secondPoint;
            PathSegmentCollection segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(firstSegment);

            linePathFigure.Segments = segmentCollection;
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(linePathFigure);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;


namespace ShapesHandler.Models
{
    public class Rhomb: CustomShape
    {
        public Rhomb() {}
        
        public Rhomb(string name, Point center, float width, float height)
        {
            this.Name = name;
            this.Type = ShapeType.Rhomb;
            this.Center = center;
            this.XLabel = (float)center.X;
            this.YLabel = (float)center.Y;
            this.WidthC = (float)width; //first diagonale
            this.HeightC = (float)height; //second diagonale
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
            Point pointL = new Point(Center.X - WidthC / 2, Center.Y); //Left
            Point pointT = new Point(Center.X, Center.Y - HeightC / 2);
            Point pointR = new Point(Center.X + WidthC / 2, Center.Y);
            Point pointB = new Point(Center.X, Center.Y + HeightC / 2);
            PathFigure rectPathFigure = new PathFigure();
            rectPathFigure.StartPoint = pointL;
            LineSegment topSegment = new LineSegment();
            topSegment.Point = pointT;
            LineSegment rightSegment = new LineSegment();
            rightSegment.Point = pointR;
            LineSegment bottomSegment = new LineSegment();
            bottomSegment.Point = pointB;
            LineSegment leftSegment = new LineSegment();
            leftSegment.Point = pointL;
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
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 12, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X, Center.Y - 12.0f));
            geometryGroup.Children.Add(centerText);
            //Add Name Label

            FormattedText ft2 = new FormattedText(Name, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 14, Brushes.Black);
            Geometry nameText = ft2.BuildGeometry(new Point(Center.X, Center.Y + HeightC / 2 + 3.0f));
            geometryGroup.Children.Add(nameText);

            return geometryGroup;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace ShapesHandler.Models
{
    public class Rectangle : CustomShape
    {

        public Rectangle() {}
        
        public Rectangle(string name, Point center, float width, float height)
        {
            this.Name = name;
            this.Type = ShapeType.Rectangle;
            this.Center = center;
            this.XLabel = (float)center.X;
            this.YLabel = (float)center.Y;
            this.WidthC = (float)width;
            this.HeightC = (float)height;
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
            Point pointTL = new Point(Center.X - WidthC / 2, Center.Y - HeightC / 2); //TopLeft
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

            //Add pathgeometry and ellipsis center point
            GeometryGroup geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(pathGeometry);
            EllipseGeometry centre = new EllipseGeometry(Center, 2.0f, 2.0f);
            geometryGroup.Children.Add(centre); 
            //Add Center Label
            string centerLabel=null;
            if (this.IsCopy) centerLabel = "";
            else centerLabel = Math.Round((double)XLabel).ToString() + ", " + Math.Round((double)YLabel).ToString();
            FormattedText ft = new FormattedText(centerLabel, Thread.CurrentThread.CurrentCulture,
            System.Windows.FlowDirection.LeftToRight, new Typeface("Times New Roman"), 13, Brushes.Black);
            Geometry centerText = ft.BuildGeometry(new Point(Center.X,Center.Y-13.0f));
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

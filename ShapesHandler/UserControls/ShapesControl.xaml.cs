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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShapesHandler.Models;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using ShapesHandler.Utilities;

namespace ShapesHandler.UserControls
{
    /// <summary>
    /// Interaction logic for ShapesControl.xaml
    /// </summary>
    public partial class ShapesControl : UserControl
    {

        public string selectedShapeName { get; set; }
        private ShapeType selectedShapeType;
        private string shapeName;
        private float firstValue; //f.e. width
        private float secondValue; //f.e height
        private Point center;
        private bool first = true;
        private bool toDelete = false;
        private int counter;
        public Shape selectShape;
        public Shape shapeToDelete;
        public SolidColorBrush CurrentShapeColor {get;set;}
        bool captured = false;
        bool panning = false;
        double x_pan, y_pan, x_canvas, y_canvas, x_trans, y_trans;

        Point[] trianglePoints;
        int triangleCountPoint;
        Point[] linePoints;
        int lineCountPoint;
        

        TextBlock tb;
        TextBox tb0;
        TextBox tb1;
        TextBox tb2;

        public TextBlock TB
        {
            get { return tb; }
            set { tb = value; }
        }

        public TextBox TB0
        {
            get { return tb0; }
            set { tb0 = value; }
        }

        public TextBox TB1
        {
            get { return tb1; }
            set { tb1 = value; }
        }

        public TextBox TB2
        {
            get { return tb2; }
            set { tb2 = value; }
        }

        public ShapesControl()
        {
            InitializeComponent();
            TB = new TextBlock();
            TB0 = new TextBox();
            TB1 = new TextBox();
            TB2 = new TextBox();
        }

        public void canvasField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedShapeName==null)
            {
                MessageBox.Show("Choose ShapeType from ListBox");
                first = true;
                return;
            }
            Enum.TryParse<ShapeType>(selectedShapeName, out selectedShapeType);

            if (selectedShapeType.Equals(ShapeType.Triangle))
            {
                
                if (triangleCountPoint ==0) {
                    trianglePoints=new Point[3];
                } 
                //trianglePoints[0,1,2]
                trianglePoints[triangleCountPoint] = Mouse.GetPosition(canvasField1);
                if (triangleCountPoint == 2)
                {
                    CustomShape triangle = new Triangle(tb0.Text, trianglePoints);
                    triangle.Stroke = Brushes.Black;
                    triangleCountPoint = 0;
                    UIElement tempPoint0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                    UIElement tempPoint1 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint1");
                    canvasField1.Children.Remove(tempPoint0); canvasField1.Children.Remove(tempPoint1);
                    canvasField1.Children.Add(triangle);
                    AddShapeToPane(triangle);//////

                }
                else
                {
                    if (triangleCountPoint == 1)
                    {
                        deleteTemporaryInputForPointy(canvasField1);
                        if (String.IsNullOrEmpty(tb0.Text))
                        {
                            MessageBox.Show("The Name is empty");
                            UIElement tempPoint0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                            canvasField1.Children.Remove(tempPoint0);
                            triangleCountPoint = 0;
                            return;
                        }
                        if (Char.IsDigit(tb0.Text[0]) || Char.IsPunctuation(tb0.Text[0]))
                        {
                            MessageBox.Show("Incorrect Name - started with digit or punctuation");
                            UIElement tempPoint0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                            canvasField1.Children.Remove(tempPoint0);
                            triangleCountPoint = 0;
                            return;
                        }
                    }
                    EllipseGeometry tempCircle = new EllipseGeometry(trianglePoints[triangleCountPoint], 2.0f, 2.0f);
                    System.Windows.Shapes.Path tempPoint = new System.Windows.Shapes.Path();
                    tempPoint.Data = tempCircle;
                    tempPoint.Name = "tempPoint" + triangleCountPoint;
                    tempPoint.Stroke = Brushes.Gray;
                    canvasField1.Children.Add(tempPoint);
                    if (triangleCountPoint == 0) createTemporaryInputForPointy(canvasField1, trianglePoints[triangleCountPoint],ShapeType.Triangle);
                    triangleCountPoint++;
                }
                return;
            }

            if (selectedShapeType.Equals(ShapeType.Line))
            {
                if (lineCountPoint==0)
                {
                    linePoints = new Point[2];
                }
                //linePoints[0,1]
                linePoints[lineCountPoint] = Mouse.GetPosition(canvasField1);
                if (lineCountPoint == 1)
                {
                    deleteTemporaryInputForPointy(canvasField1);
                    if (String.IsNullOrEmpty(tb0.Text))
                    {
                        MessageBox.Show("The Name is empty");
                        UIElement tempPoint00 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                        canvasField1.Children.Remove(tempPoint00);
                        lineCountPoint = 0;
                        return;
                    }
                    if (Char.IsDigit(tb0.Text[0]) || Char.IsPunctuation(tb0.Text[0]))
                    {
                        MessageBox.Show("Incorrect Name - started with digit or punctuation");
                        UIElement tempPoint00 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                        canvasField1.Children.Remove(tempPoint00);
                        lineCountPoint = 0;
                        return;
                    }
                    CustomShape line = new ShapesHandler.Models.Line(tb0.Text, linePoints);
                    line.Stroke = Brushes.Black;
                    lineCountPoint = 0;
                    UIElement tempPoint0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canvasField1, "tempPoint0");
                    canvasField1.Children.Remove(tempPoint0);
                    canvasField1.Children.Add(line);
                    AddShapeToPane(line);//////

                }
                else
                {
                    EllipseGeometry tempCircle = new EllipseGeometry(linePoints[0], 2.0f, 2.0f);
                    System.Windows.Shapes.Path tempPoint = new System.Windows.Shapes.Path();
                    tempPoint.Data = tempCircle;
                    tempPoint.Name = "tempPoint0";
                    tempPoint.Stroke = Brushes.Gray;
                    canvasField1.Children.Add(tempPoint);
                    createTemporaryInputForPointy(canvasField1, linePoints[0], ShapeType.Line);
                    lineCountPoint = 1;
                }
                return;
            }


            CustomShape rendershape = null;

            if (!first)
            {
                try
                {
                    deleteTemporaryInput(canvasField1);
                }
                catch (FormatException fe) {
                    MessageBox.Show("Incorrect format:" + fe.Message);
                    first = true;
                    return;
                }

                switch (selectedShapeType)
                {
                    case ShapeType.Rectangle:
                        rendershape = new ShapesHandler.Models.Rectangle(shapeName, center, firstValue, secondValue);
                        break;

                    case ShapeType.Rhomb:
                        rendershape = new Rhomb(shapeName, center, firstValue, secondValue);
                        break;

                    case ShapeType.Circle:
                        rendershape = new Circle(shapeName, center, firstValue);
                        break;

                    case ShapeType.Hexagon:
                        rendershape = new Hexagon(shapeName, center, firstValue);
                        break;

                    default:
                        break;

                }
                if (rendershape is Hexagon)
                {
                    var hex = rendershape as Hexagon;
                }

                rendershape.Stroke = Brushes.Black;
                canvasField1.Children.Add(rendershape);
                AddShapeToPane(rendershape);//////
                canvasField1.Children.OfType<UIElement>().Where(s => s is Shape).ToList<UIElement>().ForEach(sh => Console.WriteLine(" " + ((Shape)sh).Name));
                first = true;
            }

            else
            {
                center = Mouse.GetPosition(canvasField1);
                first = false;
                createTemporaryInput(tb, tb1, tb2, canvasField1, center, selectedShapeType);
            }
        }

        private void AddShapeToPane(Shape currentShape)
        {
            
            CustomShape copyShape = (CustomShape)XamlReader.Parse(XamlWriter.Save(currentShape));
            if (currentShape is Triangle)
            {
                Point translation = ((Triangle)currentShape).FindTranslationDimension();
                Point[] oldVertexes = ((Triangle)currentShape).vertexes;
                Point[] newVertexes = new Point[3];
                for (int i=0; i<3; i++)
                {
                    newVertexes[i].X = oldVertexes[i].X - translation.X;
                    newVertexes[i].Y = oldVertexes[i].Y - translation.Y;
                }
                ((Triangle)copyShape).vertexes = newVertexes;
                ((Triangle)copyShape).Center = ((Triangle)copyShape).FindCentrePoint();
            }
            else if (currentShape is ShapesHandler.Models.Line)
            {
                Point translation = ((ShapesHandler.Models.Line)currentShape).FindTranslationDimension();
                Point[] oldEnds = ((ShapesHandler.Models.Line)currentShape).ends;
                Point[] newEnds = new Point[2];
                for (int i = 0; i < 2; i++)
                {
                    newEnds[i].X = oldEnds[i].X - translation.X;
                    newEnds[i].Y = oldEnds[i].Y - translation.Y;
                }
                ((ShapesHandler.Models.Line)copyShape).ends = newEnds;
                ((ShapesHandler.Models.Line)copyShape).Center = new Point((newEnds[0].X + newEnds[1].X) / 2, (newEnds[0].Y + newEnds[1].Y) / 2);
            }
            else copyShape.Center = new Point(copyShape.WidthC / 2 + 5, copyShape.HeightC / 2 + 5);
            copyShape.IsCopy = true;
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            //mainWindow.InnerShapes.Add(copyShape);
            mainWindow.CustomShapes.Add(new ShapeWrapperControl(copyShape));
        }

        private void createTemporaryInputForPointy(Canvas canva, Point point, ShapeType selected)
        {   //Input for Shapes created with Points - Mouse Clicks
            tb.Name = "tb";
            tb.Text = selected.getTextLabel();
            tb0.Name = "tb0"; tb0.Width = 80; tb0.Clear();
            Canvas.SetLeft(tb, point.X - 20.0f);
            Canvas.SetTop(tb, point.Y - 30.0f);
            canva.Children.Add(tb);
            Canvas.SetLeft(tb0, point.X);
            Canvas.SetTop(tb0, point.Y - 16.0f);
            canva.Children.Add(tb0);
        }

        private void createTemporaryInput(TextBlock tb, TextBox tb1, TextBox tb2,
             Canvas canva, Point point, ShapeType selected)
        {   //Input for Shapes created with Center Point and Input for Parameters - Length, Radius, etc
            tb.Name = "tb";
            tb.Text = selected.getTextLabel();
            tb0.Name = "tb0"; tb0.Width = 80; tb0.Clear();
            tb1.Name = "tb1"; tb1.Width = 30; tb1.Clear();
            tb2.Name = "tb2"; tb2.Width = 30; tb2.Clear();
            if (selected.Equals(ShapeType.Hexagon) || selected.Equals(ShapeType.Circle))
            {
                tb2.Visibility = Visibility.Collapsed;
            }
            Canvas.SetLeft(tb, point.X-20.0f);
            Canvas.SetTop(tb, point.Y - 30.0f);
            canva.Children.Add(tb);
            Canvas.SetLeft(tb0, point.X);
            Canvas.SetTop(tb0, point.Y - 16.0f);
            canva.Children.Add(tb0);
            Canvas.SetLeft(tb1, point.X);
            Canvas.SetTop(tb1, point.Y + 4.0f);
            canva.Children.Add(tb1);
            Canvas.SetLeft(tb2, point.X);
            Canvas.SetTop(tb2, point.Y + 20.0f);
            canva.Children.Add(tb2);
            EllipseGeometry tempCircle = new EllipseGeometry(point, 2.0f, 2.0f);
            System.Windows.Shapes.Path tempCentre = new System.Windows.Shapes.Path();
            tempCentre.Data = tempCircle;
            tempCentre.Name = "tempCentre";
            tempCentre.Stroke = Brushes.Gray;
            canva.Children.Add(tempCentre);
            
        }

        

        private void deleteTemporaryInputForPointy(Canvas canva)
        {

            UIElement tb = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb");
            UIElement tb0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb0");
            canva.Children.Remove(tb); canva.Children.Remove(tb0);
        }

        private void deleteTemporaryInput(Canvas canva)
        {
            
            UIElement tb = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb");
            UIElement tb0 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb0");
            UIElement tb1 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb1");
            UIElement tb2 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb2");
            UIElement tempCentre = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tempCentre");
            canva.Children.Remove(tb); canva.Children.Remove(tb0);
            canva.Children.Remove(tb1); canva.Children.Remove(tb2);
            canva.Children.Remove(tempCentre);
            if (tb2.Visibility.Equals(Visibility.Collapsed)) tb2.Visibility = Visibility.Visible;
            else
            {
                secondValue = (float)Convert.ToDouble(((TextBox)tb2).Text);
                if (secondValue < 2) throw new FormatException("Entered dimension without real value");
            }
            firstValue = (float)Convert.ToDouble(((TextBox)tb1).Text);
            if (firstValue<2) throw new FormatException("Entered dimension without real value");
            shapeName = ((TextBox)tb0).Text;
            if (String.IsNullOrEmpty(shapeName)) throw new FormatException("Name is empty");
            if (Char.IsDigit(((TextBox)tb0).Text[0]) || Char.IsPunctuation(((TextBox)tb0).Text[0]))
                throw new FormatException("Incorrect Name - started with digit or punctuation");
        }

        public void canvasField_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.OriginalSource is Shape && e.ClickCount == 1) //Is Shape
            {
                selectShape = (Shape)e.OriginalSource;
                if (shapeToDelete!=null) return;
                Mouse.Capture(selectShape);
                captured = true;
                x_canvas = e.GetPosition(canvasField1).X;
                y_canvas = e.GetPosition(canvasField1).Y;
                Transform transOriginal = selectShape.RenderTransform;
                x_trans = transOriginal.Value.OffsetX;
                y_trans = transOriginal.Value.OffsetY;
                CurrentShapeColor = (SolidColorBrush)selectShape.Stroke; /////
                selectShape.Stroke = Brushes.Gray;
            }

            if (e.OriginalSource is Shape && e.ClickCount == 2) //Is Shape
            {
                if (!toDelete)
                {
                    shapeToDelete = (Shape)e.OriginalSource;
                    CurrentShapeColor = (SolidColorBrush)shapeToDelete.Stroke; /////
                    shapeToDelete.Stroke = Brushes.Brown;
                    Mouse.Capture(null);
                    toDelete = true;
                }
                else
                {
                    if (shapeToDelete != null) shapeToDelete.Stroke = CurrentShapeColor; /////
                    shapeToDelete = null;
                    Mouse.Capture(null);
                    toDelete = false;
                }
                captured = false;
            }

            if (e.OriginalSource is Canvas && e.ClickCount == 2) //Is Canvas
            {
                panning = true;
                var selectCanvas = (Canvas)e.OriginalSource;
                Mouse.Capture(selectCanvas);
                captured = true;
                x_pan = e.GetPosition(this).X;
                y_pan = e.GetPosition(this).Y;
            }
        }

        public void canvasField_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (captured)
            {
                Mouse.Capture(null);
                captured = false;
                if (!panning)
                {
                    selectShape.Stroke = CurrentShapeColor; /////
                }
                else panning = false; ////
            }
        }

        public void canvasField_MouseMove(object sender, MouseEventArgs e)
        {

            if (captured && !panning)
            {  // a Shape has mouse capture
                double x = e.GetPosition(canvasField1).X;
                double y = e.GetPosition(canvasField1).Y;
                double x_diff = x - x_canvas;
                double y_diff = y - y_canvas;
                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = x_diff+x_trans;
                translateTransform.Y = y_diff+y_trans;
                Point center = ((CustomShape)selectShape).Center;
                ((CustomShape)selectShape).XLabel = (float)(center.X + translateTransform.X);
                ((CustomShape)selectShape).YLabel = (float)(center.Y + translateTransform.Y);
                selectShape.RenderTransform = translateTransform;
                counter++;
            }

            if (captured && panning) ////panning
            {
                Point newPos = e.GetPosition(this);
                double x_diff = newPos.X - x_pan;
                double y_diff = newPos.Y - y_pan;
                TranslateTransform panTranslateTransform = new TranslateTransform();
                panTranslateTransform.X = x_diff;
                panTranslateTransform.Y = y_diff;
                canvasField1.RenderTransform = panTranslateTransform;
            }
        }


        public void canvasField_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("thisFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void canvasField_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("thisFormat"))
            {
                CustomShape droppedShape = e.Data.GetData("thisFormat") as CustomShape;
                if (!(droppedShape is Triangle || droppedShape is ShapesHandler.Models.Line))
                {
                    droppedShape.Center = new Point(droppedShape.WidthC, droppedShape.HeightC / 2);
                    droppedShape.XLabel = (float)droppedShape.WidthC;
                    droppedShape.YLabel = (float)droppedShape.HeightC / 2;
                }
                droppedShape.IsCopy = false;
                canvasField1.Children.Add(droppedShape);
            }
        }

    }
}

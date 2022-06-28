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
using System.Diagnostics;
using ShapesHandler.Models;

namespace ShapesHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ShapeType selectedShape;
        private float widthC;
        private float heightC;
        private bool first=true;


        public MainWindow()
        {
            InitializeComponent();
        }

         private void canvasField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string shapeSelected=null;
             try
            {
                shapeSelected = ((canvasField.FindName("shapeTypes") as ListBox).SelectedValue as ListBoxItem).Content.ToString();
            }
            catch (Exception ex) {
                Trace.WriteLine("ShS " + shapeSelected);
                MessageBox.Show("Choose ShapeType from ListBox"); return;
            }
            Trace.WriteLine("ShS " + shapeSelected);
            Enum.TryParse<ShapeType>(shapeSelected, out selectedShape);
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            Point center = Mouse.GetPosition(canvasField);
            Trace.WriteLine("pnt" + center);

            if (!first)
            {
                Shape Rendershape = null;
                switch (selectedShape)
                {
                    case ShapeType.Rectangle:
                        Rendershape = new ShapesHandler.Models.Rectangle("Rect1", center, 140, 200);
                        break;

                    case ShapeType.Circle:
                        Rendershape = new Ellipse() { Height = 40, Width = 40 };
                        break;

                    default:
                        break;

                }
                Trace.WriteLine("D " + widthC + " " + heightC);
                Trace.WriteLine("RS" + Rendershape);
                Rendershape.Stroke = blackBrush;
                //Trace.WriteLine("R0" + (Rendershape as ShapesHandler.Models.Rectangle).Center);
                Trace.WriteLine("R0" + Rendershape.Width);
                deleteTemporaryInput(canvasField);
                //Canvas.SetLeft(Rendershape, e.GetPosition(canvasField).X);
                //Canvas.SetTop(Rendershape, e.GetPosition(canvasField).Y);
                //Canvas.SetLeft(Rendershape, 0);
                //Canvas.SetTop(Rendershape, 0);
                canvasField.Children.Add(Rendershape);
                first = true;
            }

            else
            {
                TextBlock tb = new TextBlock();
                TextBox tb1 = new TextBox();
                TextBox tb2 = new TextBox();
                createTemporaryInput(tb, tb1, tb2, canvasField, center);
                Trace.WriteLine("D0 " + widthC + " " + heightC);
                first = false;
            }

        }

         private void createTemporaryInput(TextBlock tb, TextBox tb1, TextBox tb2,
             Canvas canva, Point point)
         {
             tb.Name = "tb"; 
             tb.Text = "Type the Width and Height";
             tb1.Name = "tb1"; tb1.Width = 30; 
             tb2.Name = "tb2"; tb2.Width = 30;
             Canvas.SetLeft(tb, point.X);
             Canvas.SetTop(tb, point.Y-5.0f);
             canva.Children.Add(tb);
             Canvas.SetLeft(tb1, point.X);
             Canvas.SetTop(tb1, point.Y + 4.0f);
             canva.Children.Add(tb1);
             Canvas.SetLeft(tb2, point.X);
             Canvas.SetTop(tb2, point.Y + 19.0f);
             canva.Children.Add(tb2);
             //tb1.LostFocus+=tb1_LostFocus;
             //tb2.LostFocus+= tb2_LostFocus;
             tb1.KeyDown += tb1_KeyDown;
             tb2.KeyDown += tb2_KeyDown;
         }

         private void tb1_LostFocus(object sender, RoutedEventArgs e)
         {
             this.widthC = (float)Convert.ToDouble((sender as TextBox).Text);
         }

         private void tb2_LostFocus(object sender, RoutedEventArgs e)
         {
             this.heightC = (float)Convert.ToDouble((sender as TextBox).Text);
         }

         private void tb1_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.Key == Key.Return)
             {
                 this.widthC = (float)Convert.ToDouble((sender as TextBox).Text);
             }
         }

         private void tb2_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.Key == Key.Return)
             {
                 this.heightC = (float)Convert.ToDouble((sender as TextBox).Text);
             }
         }

         private void deleteTemporaryInput(Canvas canva)
         {
             UIElement tb = (UIElement)LogicalTreeHelper.FindLogicalNode(canva,"tb");
             UIElement tb1 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb1");
             UIElement tb2 = (UIElement)LogicalTreeHelper.FindLogicalNode(canva, "tb2");
             Trace.WriteLine("TB="+tb+" "+tb1+" "+tb2);
             Trace.WriteLine("Count " + canva.Children.Count);
             canva.Children.Remove(tb); canva.Children.Remove(tb1); canva.Children.Remove(tb2);
             Trace.WriteLine("Count " + canva.Children.Count);
         }
           

         private void canvasField_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
         {

         }
    }
}

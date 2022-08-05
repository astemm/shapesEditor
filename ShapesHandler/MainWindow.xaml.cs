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
using System.IO;
using System.Windows.Markup;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.ObjectModel;
using ShapesHandler.Models;
using ShapesHandler.UserControls;

namespace ShapesHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<Shape> InnerShapes { get; set; }
        public ObservableCollection<ShapeWrapperControl> CustomShapes { get; set; }
        private Point startPointInList;
        public ObservableCollection<string> FoundShapeNames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
           //InnerShapes = new ObservableCollection<Shape>();
            CustomShapes = new ObservableCollection<ShapeWrapperControl>();
            FoundShapeNames = new ObservableCollection<string>();
            DataContext = this;
        }

         private void canvasField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
        }
           
         private void canvasField_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
         {

         }

         private void canvasField_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
         {
            
         }

         private void canvasField_MouseMove(object sender, MouseEventArgs e)
         {
            
         }

         private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
             // Store the mouse position
             startPointInList = e.GetPosition(null);
         }

         private void List_MouseMove(object sender, MouseEventArgs e)
         {

             if (shapeList.Items.Count == 0 || shapeList.SelectedItem == null) return;
             // Get the current mouse position
             Point mousePos = e.GetPosition(null);
             Vector diff = startPointInList - mousePos;

             if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
             {
                 // Get the dragged ListBoxItem
                 ListBox listBox = sender as ListBox;
                 Trace.WriteLine("Dragged "+listBox.SelectedItem);
                 Trace.WriteLine("Dragged1 " + listBox.SelectedItem.GetType());
                 //Shape selectedShape = listBox.SelectedItem as Shape;
                 Shape selectedShape = (listBox.SelectedItem as ShapeWrapperControl).InnerShape;
                 Shape draggedShape = XamlReader.Parse(XamlWriter.Save(selectedShape)) as Shape;
                 //Shape draggedShape = ((StackPanel)customShape.Content).Children[0] as Shape;
                 listBox.SelectedItem = null;
                 // Initialize the drag & drop operation
                 DataObject dragData = new DataObject("thisFormat", draggedShape);
                 DragDrop.DoDragDrop(listBox, dragData, DragDropEffects.Move);

             }
         }

         public void btnAdd_Click(object sender, RoutedEventArgs e)
         {
             shapesPopup.IsOpen = !shapesPopup.IsOpen;
         }

         public void btnAddBackGround_Click(object sender, RoutedEventArgs e)
         {
             OpenFileDialog openFileDialog = new OpenFileDialog();
             if (openFileDialog.ShowDialog() == true)
             {
                 ImageBrush ib = new ImageBrush();
                 ib.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                 UC.canvasField1.Background = ib;
             }
             else UC.canvasField1.Background = Brushes.AliceBlue;
         }

         public void btnSearchShape_Click(object sender, RoutedEventArgs e)
         {
             FoundShapeNames = new ObservableCollection<string>(UC.canvasField1.Children.OfType<CustomShape>().
                 Where(c => c.Name.IndexOf(searchedText.Text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                 .Select(s => s.Name).ToList<string>());
             foundShapesList.ItemsSource= FoundShapeNames;
         }

         public void btnColorSelector_Click(object sender, RoutedEventArgs e)
         {
             colorsPopup.IsOpen = !colorsPopup.IsOpen;
         }

         public void btnRemove_Click(object sender, RoutedEventArgs e)
         {
             UC.canvasField1.Children.Remove(UC.shapeToDelete);
             UC.shapeToDelete = null;
         }

         public void lbShapeTypesPopup_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             ListBoxItem selected=null;
             if (lbShapeTypesPopup.SelectedItem != null)
                 selected = lbShapeTypesPopup.SelectedItem as ListBoxItem;
             Trace.WriteLine("SEL_shape " + selected.Content is string);
             UC.selectedShapeName = selected.Content as string;
         }

         public void foundShapesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             string selectedName = null;
             if (foundShapesList.SelectedItem != null)
                 selectedName = foundShapesList.SelectedItem as string;
             Trace.WriteLine("SEL_name " + selectedName);
             UC.canvasField1.Children.OfType<CustomShape>().
                 Where(c => c.Name.Equals(selectedName)).ToList<CustomShape>()
                 .ForEach(s => s.Stroke = Brushes.Green);

         }

         public void btnSaveXml_Click(object sender, RoutedEventArgs e)
         {
             SaveFileDialog saveFileDialog = new SaveFileDialog();
             if (saveFileDialog.ShowDialog() == true)
             {
                 FileStream fs = File.Open(saveFileDialog.FileName, FileMode.Create);
                 UC.canvasField1.Name = "canvasField2";
                 XamlWriter.Save(UC, fs);
                 fs.Close();
             }
         }

         public void btnLoadXml_Click(object sender, RoutedEventArgs e)
         {
             OpenFileDialog openFileDialog = new OpenFileDialog();
             if (openFileDialog.ShowDialog() == true)
             {
                 FileStream fs = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                 Grid parentGrid = UC.Parent as Grid;
                 ShapesControl savedControl = null;
                 try
                 {
                     savedControl = XamlReader.Load(fs) as ShapesControl;
                 }
                 catch (XamlParseException xe)
                 {
                     string[] exceptionMessage = xe.Message.Split('.');
                     MessageBox.Show(string.Join(".", exceptionMessage[0], exceptionMessage[1], "Change this name to make it unique in the file"));
                     return;
                 }
                 parentGrid.Children.Remove(UC);
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).MouseLeftButtonDown += savedControl.canvasField_MouseLeftButtonDown;
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).MouseRightButtonDown += savedControl.canvasField_MouseRightButtonDown;
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).MouseRightButtonUp += savedControl.canvasField_MouseRightButtonUp;
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).MouseMove += savedControl.canvasField_MouseMove;
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).Drop += savedControl.canvasField_Drop;
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).DragEnter += savedControl.canvasField_DragEnter;
                 fs.Close();
                 ((Canvas)((ScrollViewer)savedControl.Content).Content).Name = "canvasField1";
                 savedControl.canvasField1 = (Canvas)((ScrollViewer)savedControl.Content).Content;
                 parentGrid.Children.Insert(1,savedControl);
                 UC = savedControl;
             }
         }
    }
}

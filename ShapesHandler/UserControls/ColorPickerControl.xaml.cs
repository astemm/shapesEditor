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
using System.Reflection;

namespace ShapesHandler.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPickerControl : UserControl
    {
        public ColorPickerControl()
        {
            InitializeComponent();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            this.colorList.ItemsSource = typeof(Brushes).GetProperties();
        }

        private void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            rtlfill.Fill = selectedColor;
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            if (mainWindow.UC.shapeToDelete != null)
            {
                mainWindow.UC.shapeToDelete.Stroke = selectedColor;
                mainWindow.UC.CurrentShapeColor = (SolidColorBrush)selectedColor;
            }
        }
    }
}

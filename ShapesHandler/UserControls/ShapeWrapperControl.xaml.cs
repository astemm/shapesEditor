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
using System.Windows.Markup;
using ShapesHandler.Models;

namespace ShapesHandler.UserControls
{
    /// <summary>
    /// Interaction logic for CustomShape.xaml
    /// </summary>
    public partial class ShapeWrapperControl : UserControl
    {

        public static readonly DependencyProperty InnerShapeProperty =
        DependencyProperty.Register(
            "InnerShape",
            typeof(Shape),
            typeof(ShapeWrapperControl));

        public Shape InnerShape
        {
            get { return (Shape)GetValue(InnerShapeProperty); }
            set { SetValue(InnerShapeProperty, value); }
        }

        public void UC_Loaded (object sender, RoutedEventArgs e) 
        {  //event in alternative listbox usage (not here) when binding Shape in ItemTemplate/DataTemplate
            Trace.WriteLine("not called");
                    try
                    {
                        CustomShape copyShape = (CustomShape)XamlReader.Parse(XamlWriter.Save(InnerShape));
                        ((StackPanel)this.Content).Children.Add(copyShape);
                    }
                    catch (Exception ex)
                    { 
 
                    }
        }

        public ShapeWrapperControl(Shape innerShape)
        {
            InitializeComponent();
            InnerShape = innerShape;
            ((StackPanel)this.Content).Children.Add(InnerShape);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace ShapesHandler.Models
{
    public abstract class CustomShape : Shape
    {
        public ShapeType Type { get; set; }
        public Point Center { get; set; }
        public float? XLabel { get; set; }
        public float? YLabel { get; set; }
        public bool IsCopy { get; set; }
        public float WidthC { get; set; } //Custom Width 
        public float HeightC { get; set; } //Custom Height 
    }
}

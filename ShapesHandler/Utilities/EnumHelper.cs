using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapesHandler.Models;

namespace ShapesHandler.Utilities
{
    public static class EnumHelper
    {
        public static string getTextLabel(this ShapeType shapeType)
        {
            string label;
            switch (shapeType)
            {
                case ShapeType.Rectangle: label= "Type Name, the Width and Height";
                    break;
                case ShapeType.Rhomb: label = "Type Name, the Diag1 and Diag2 values";
                    break;
                case ShapeType.Hexagon: label = "Type Name and Side length";
                    break;
                case ShapeType.Circle: label = "Type Name and Radius value";
                    break;
                case ShapeType.Triangle: label = "Type Name and Click triangle points";
                    break;
                case ShapeType.Line: label = "Type Name and Click line points";
                    break;
               default: label = "Type Name and dimensions";
                    break;
            }
            return label;
        }
    }
}

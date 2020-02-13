using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Paint
{
    [Serializable]
    public class Shape
    {
        public enum ShapeType
        {
            Rectangle = 0,
            Ellipse,
            Line
        };

        public Shape() { }

        public Shape(double _width, double _hieght, double _xi, double _xf,
            double _yi, double _yf, double _left, double _top, Color _stroke, Color _fill, ShapeType _shapevalue, 
            double _shape) 
        {
            Width = _width; Height = _hieght; Xi = _xi; Xf = _xf; Yi = _yi; Yf = _yf; Left = _left; 
            Top = _top; Stroke = _stroke; Fill = _fill; ShapeValue = _shapevalue; Border = _shape;
        }

        public Shape(Shape shape) : this(shape.Width, shape.Height, shape.Xi, shape.Xf, 
            shape.Yi, shape.Yf, shape.Left, shape.Top, shape.Stroke, shape.Fill, shape.ShapeValue, shape.Border) 
        {}

        public double Width { get; set; }
        public double Height { get; set; }
        public double Xi { get; set; }
        public double Xf { get; set; }
        public double Yi { get; set; }
        public double Yf { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Border { get; set; }
        public Color Stroke { get; set; }
        public Color Fill { get; set; }
        public ShapeType ShapeValue { get; set; }

        public UIElement GetUIElement() {

            if (ShapeValue == ShapeType.Rectangle)
            {
                Rectangle element = new Rectangle();
                element.Width = Width;
                element.Height = Height;
                element.Stroke = new SolidColorBrush(Stroke);
                element.StrokeThickness = Border;
                element.Fill = new SolidColorBrush(Fill);

                return element;
            }
            else if (ShapeValue == ShapeType.Ellipse)
            {
                Ellipse element = new Ellipse();
                element.Width = Width;
                element.Height = Height;
                element.StrokeThickness = Border;
                element.Stroke = new SolidColorBrush(Stroke);
                element.Fill = new SolidColorBrush(Fill);

                return element;
            }
            else if (ShapeValue == ShapeType.Line)
            {
                Line element = new Line();
                element.X1 = Xi;
                element.X2 = Xf;
                element.Y1 = Yi;
                element.Y2 = Yf;
                element.StrokeThickness = Border;
                element.Stroke = new SolidColorBrush(Stroke);
                element.Fill = new SolidColorBrush(Fill);

                return element;
            }

            return null;
        }
    }
}

﻿using System;
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
using System.Xml.Serialization;
using System.IO;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //Variables
        Point start;                //Mouse Start Position
        Point end;                  //Mouse Final Position
        Color last_Fill;            //Last Fill Color
        Color last_border;          //Last Border Color
        List<Shape> shapes;         //List of shapes for serialization
        Shape shape;                //Current Shape

        public MainWindow()
        {
            InitializeComponent();
        }

        /* Purpose: Used to get the position of the mouse over the canvas on pressing down*/
        private void myCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //For Debugging
            //Console.WriteLine(e.GetPosition(myCanvas));
            start = e.GetPosition(myCanvas);
        }

        /* Purpose: Used to get the position of the mouse over the canvas on releasing the mouse button
           Calls the function DrawShape after getting mouse position data. */
        private void myCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //For Debugging
            //Console.WriteLine(e.GetPosition(myCanvas));
            end = e.GetPosition(myCanvas);
            DrawShape();
        }

        /* Purpose: Called on instantiation of the tool bar, removes unwanted formatting */
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        /* Purpose: Main function used to draw a shape object onto the canvas */
        private void DrawShape()
        {
            //Verify X Position of non line shape
            if (start.X > end.X && shape.ShapeValue != Shape.ShapeType.Line)
            {
                Double temp = start.X;
                start.X = end.X;
                end.X = temp;
            }

            //Verify Y Position of non line shape
            if (start.Y > end.Y && shape.ShapeValue != Shape.ShapeType.Line)
            {
                Double temp = start.Y;
                start.Y = end.Y;
                end.Y = temp;
            }

            //Set Props
            shape.Width = end.X - start.X;
            shape.Height = end.Y - start.Y;
            shape.Border = sizeSlider.Value + 1;
            shape.Xi = start.X;
            shape.Xf = end.X;
            shape.Yi = start.Y;
            shape.Yf = end.Y;

            //new Ui element from shape
            UIElement element = shape.GetUIElement();

            //Set Position of non line shape
            if (shape.ShapeValue != Shape.ShapeType.Line)
            {
                Canvas.SetLeft(element, start.X);
                Canvas.SetTop(element, start.Y);
            }

            //Remember Positioning
            shape.Top = Canvas.GetTop(element);
            shape.Left = Canvas.GetLeft(element);

            //Add Shape to canvas
            myCanvas.Children.Add(element);

            //Add Shape to Serialization List
            shapes.Add(new Shape(shape));
        }

        /* Purpose: Tool bar option to select what type of shape the shape object will become */
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Line;
        }
        /* Purpose: Tool bar option to select what type of shape the shape object will become */
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Ellipse;
        }
        /* Purpose: Tool bar option to select what type of shape the shape object will become */
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Rectangle;
        }

        /* Purpose: Tool bar option to select what color the shape will fill with */
        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            ColorBox colorWindow = new ColorBox(last_Fill);
            colorWindow.Owner = this;
            colorWindow.ShowDialog();

            last_Fill = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            shape.Fill = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            FillColorButton.Fill = new SolidColorBrush(Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue));
        }

        /* Purpose: Tool bar option to select what type of color the border of the shape will become */
        private void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            ColorBox colorWindow = new ColorBox(last_border);
            colorWindow.Owner = this;
            colorWindow.ShowDialog();

            last_border = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            shape.Stroke = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            BrushColorButton.Fill = new SolidColorBrush(Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue));
        }

        /* Purpose: Menu option to close the application */
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /* Purpose: Menu option to clear the canvas of all shapes */
        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }

        /* Purpose: Menu option to save the current canvas by serializing shapes into xml */
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.SaveFileDialog saveFileDialog =
                new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.InitialDirectory = "";
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "xml";

            if (saveFileDialog.ShowDialog() == true)
                filename = saveFileDialog.FileName;
            else
                return;

            var serializer = new XmlSerializer(shapes.GetType());
            TextWriter writer = File.CreateText(filename);
            serializer.Serialize(writer, shapes);

            writer.Close();
        }

        /* Purpose: Menu option to open the about window */
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();

            about.Owner = this;
            about.ShowDialog();
        }

        /* Purpose: Menu option to open a current canvas by deserializing from xml*/
        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog openFileDialog =
                new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = "";
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == true)
                filename = openFileDialog.FileName;
            else
                return;

            var serializer = new XmlSerializer(shapes.GetType());
            FileStream fs = new FileStream(filename, FileMode.Open);

            try
            {
                shapes = (List<Shape>)serializer.Deserialize(fs);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Unable to open file, non xml format.");
            }
            

            fs.Close();

            myCanvas.Children.Clear();

            foreach(Shape to_load in shapes.ToList())
            {
                //Return the UI element
                UIElement element = to_load.GetUIElement();

                //Set Position of non line shape with remembered pos
                if (to_load.ShapeValue != Shape.ShapeType.Line)
                {
                    Canvas.SetLeft(element, to_load.Left);
                    Canvas.SetTop(element, to_load.Top);
                }

                //Add UiElement
                myCanvas.Children.Add(element);
            }

        }

        /* Purpose: Called on instantiation of the window object */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            shape = new Shape();
            shape.Stroke = Color.FromRgb(0,0,0);
            shape.Border = 1;
            last_border = new Color();
            last_border.R = 0; last_border.B = 0; last_border.G = 0;
            last_Fill = new Color();
            last_Fill.R = 255; last_Fill.B = 255; last_Fill.G = 255;       
            shapes = new List<Shape>();
        }

        /* Purpose: used to update the thickness of a shapes stroke */
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BorderSizeText.Text = (sizeSlider.Value + 1).ToString();
            shape.Border = sizeSlider.Value + 1;
        }
    }
}

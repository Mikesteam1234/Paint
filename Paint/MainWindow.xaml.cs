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
        List<Shape> shapes;         //List of shapes for serialization
        Shape shape;                //Current Shape

        public MainWindow()
        {
            InitializeComponent();
        }

        private void myCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //For Debugging
            Console.WriteLine(e.GetPosition(myCanvas));
            start = e.GetPosition(myCanvas);
        }

        private void myCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //For Debugging
            Console.WriteLine(e.GetPosition(myCanvas));
            end = e.GetPosition(myCanvas);
            DrawShape();
        }

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

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Line;
        }
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Ellipse;
        }
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            shape.ShapeValue = Shape.ShapeType.Rectangle;
        }

        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            ColorBox colorWindow = new ColorBox();
            colorWindow.Owner = this;
            colorWindow.ShowDialog();

            shape.Fill = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            FillColorButton.Background = new SolidColorBrush(Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue));
        }

        private void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            ColorBox colorWindow = new ColorBox();
            colorWindow.Owner = this;
            colorWindow.ShowDialog();

            shape.Stroke = Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue);
            BrushColorButton.Background = new SolidColorBrush(Color.FromRgb(colorWindow.red, colorWindow.green, colorWindow.blue));
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
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
            TextWriter writer = File.CreateText(filename);
            serializer.Serialize(writer, shapes);

            writer.Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox about = new AboutBox();

            about.Owner = this;
            about.ShowDialog();
        }

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
            shapes = (List<Shape>)serializer.Deserialize(fs);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            shape = new Shape();
            shape.Stroke = Color.FromRgb(0,0,0);
            shapes = new List<Shape>();
        }
    }
}
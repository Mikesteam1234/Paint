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
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for ColorBox.xaml
    /// </summary>
    /// 
    public partial class ColorBox : Window
    {
        public byte red { get; set; }
        public byte green { get; set; }
        public byte blue { get; set; }

        public ColorBox()
        {
            InitializeComponent();
        }

        /* Purpose: one arg constructor that is used to set the preview and current colors */
        public ColorBox(Color color)
        {
            InitializeComponent();
            red = color.R;
            green = color.G;
            blue = color.B;
            Green.Text = green.ToString();
            Red.Text = red.ToString();
            Blue.Text = blue.ToString();
            Preview.Fill = new SolidColorBrush(color);
        }

        /* Purpose: Called on click of the pick button */
        private void Pick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                red = Byte.Parse(Red.Text);
                green = Byte.Parse(Green.Text);
                blue = Byte.Parse(Blue.Text);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Values must be between 0-255");
            }
        }

        /* Purpose: on deselection of the byte editor text box of any color
         updates the preview Fill color, and does error checking */
        private void Color_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Preview.Fill = new SolidColorBrush(Color.FromRgb(Byte.Parse(Red.Text), green = Byte.Parse(Green.Text), blue = Byte.Parse(Blue.Text)));
            }
            catch (Exception)
            {
                //Log issue
            }
        }

        /* Purpose: Select a preset option for the different colors*/
        private void Preset_Click(object sender, RoutedEventArgs e)
        {
            red = ((SolidColorBrush)((Rectangle)((Button)sender).Content).Fill).Color.R;
            blue = ((SolidColorBrush)((Rectangle)((Button)sender).Content).Fill).Color.B;
            green = ((SolidColorBrush)((Rectangle)((Button)sender).Content).Fill).Color.G;
            Green.Text = green.ToString();
            Blue.Text = blue.ToString();
            Red.Text = red.ToString();
            Preview.Fill = ((Rectangle)((Button)sender).Content).Fill;
        }
    }
}

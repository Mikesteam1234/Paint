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

        private void Pick_Click(object sender, RoutedEventArgs e)
        {
            red = Byte.Parse(Red.Text);
            green = Byte.Parse(Green.Text);
            blue = Byte.Parse(Blue.Text);
            this.Close();
        }
    }
}

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

namespace ants
{
    /// <summary>
    /// Interaction logic for Food.xaml
    /// </summary>
    public partial class Food : UserControl
    {
        static Random r = new Random();
        public int Amount = 10;
        public int xMax = 1400;
        public int yMax = 800;
        private int _xPos;
        public int xPos
        {
            get => _xPos;
            set
            {
                _xPos = value;
                Canvas.SetLeft(this, value);
            }
        }
        private int _yPos;
        public int yPos
        {
            get => _yPos;
            set
            {
                _yPos = value;
                Canvas.SetTop(this, value);
            }
        }

        public Food()
        {
            InitializeComponent();
            xPos = r.Next(1395);
            yPos = r.Next(795);
        }
    }
}

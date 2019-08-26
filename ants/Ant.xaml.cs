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
    /// Interaction logic for Ant.xaml
    /// </summary>
    public partial class Ant : UserControl
    {
        private int SenseRange = 5;
        private bool _hasFood = false;
        public bool HasFood
        {
            get => _hasFood;
            set
            {
                _hasFood = value;
                ellipse.Fill = _hasFood ? Brushes.Green : Brushes.Black;
            }
        }
        public double xMax = 1400;
        public double yMax = 800;
        private double _xPos = 700;
        public double xPos
        {
            get => _xPos;
            set
            {
                _xPos = value;
                Canvas.SetLeft(this, value);
            }
        }
        private double _yPos = 400;
        public double yPos
        {
            get => _yPos;
            set
            {
                _yPos = value;
                Canvas.SetTop(this, value);
            }
        }
        public Ant()
        {
            InitializeComponent();
        }

        Random r = new Random();
        public void Move()
        {
            if (HasFood)
            {
                if (xPos >= 690 && xPos <= 710 && yPos <= 410 && yPos >= 390)
                {
                    HasFood = false;
                }
                else
                {
                    MainWindow.Trails.Add(new Trail(xPos, yPos));
                    if (Math.Abs(xPos - 700) / 700 > Math.Abs(yPos - 400) / 400)
                    {
                        if (xPos > 700) xPos -= 1;
                        else xPos += 1;
                    }
                    else
                    {
                        if (yPos > 400) yPos -= 1;
                        else yPos += 1;
                    }
                }
            }
            else
            {
                var trail = MainWindow.Trails.FirstOrDefault(a => (Math.Abs(a.X - xPos) < SenseRange && Math.Abs(a.Y - yPos) < SenseRange));
                if (trail != null)
                {
                    var nextTrail = MainWindow.Trails.FirstOrDefault(a =>
                            ((Math.Abs(a.X - trail.X) == 1 && Math.Abs(a.Y - trail.Y) == 0)
                                || (Math.Abs(a.X - trail.X) == 0 && Math.Abs(a.Y - trail.Y) == 1))
                            && Math.Sqrt(Math.Pow(a.X - 400, 2) + Math.Pow(a.Y - 700, 2)) > Math.Sqrt(Math.Pow(trail.X - 400, 2) + Math.Pow(trail.Y - 700, 2)));

                    if (nextTrail != null)
                    {
                        xPos = nextTrail.X;
                        yPos = nextTrail.Y;
                        return;
                    }
                    else
                    {
                        MoveTowards(trail.X, trail.Y);
                        return;
                    }
                }
                tryAgain:
                var d = r.Next(4);
                switch (d)
                {
                    case 0:
                        if (xPos + 1 > xMax) goto tryAgain;
                        xPos += 1;
                        break;
                    case 1:
                        if (xPos - 1 < 0) goto tryAgain;
                        xPos -= 1;
                        break;
                    case 2:
                        if (yPos + 1 > yMax) goto tryAgain;
                        yPos += 1;
                        break;
                    case 3:
                        if (yPos - 1 < 0) goto tryAgain;
                        yPos -= 1;
                        break;
                }
            }
        }

        public void MoveTowards(double x, double y)
        {
            if (xPos < x) xPos++;
            else if (xPos > x) xPos--;
            else if (yPos < x) yPos++;
            else if (yPos > x) yPos--;
        }
    }
}

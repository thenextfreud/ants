using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
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
using System.Windows.Threading;
using System.Threading;

namespace ants
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ConcurrentBag<Trail> Trails = new ConcurrentBag<Trail>();
        public ConcurrentBag<Ant> Ants = new ConcurrentBag<Ant>();

        public MainWindow()
        {
            InitializeComponent();

            Canvas.SetLeft(AntHill, (1400 - 20) / 2);
            Canvas.SetTop(AntHill, (800 - 20) / 2);

            for (int i = 0; i < 40; i++)
            {
                Thread.Sleep(30);
                var ant = new Ant();
                Ants.Add(ant);
                Canvas.Children.Add(ant);

                for (int j = 0; j < 5; j++)
                {
                    Thread.Sleep(30);
                    var food = new Food();
                    FoodItems.Add(food);
                    Canvas.Children.Add(food);
                }
            }

            RunLooper();
        }
        

        public int loopTime = 0;
        public ConcurrentBag<Food> FoodItems = new ConcurrentBag<Food>();

        public async void RunLooper()
        {
            while (true)
            {
                var t = new Task(() => loop());
                t.Start();
                await t;
            }
        }

        public void loop()
        {
            Thread.Sleep(loopTime);
            foreach(var ant in Ants)
            {
                ant.Dispatcher.Invoke(() =>
                {
                    ant.Move();
                    var food = FoodItems.FirstOrDefault(a => Canvas.GetLeft(ant) == Canvas.GetLeft(a)
                                                          && Canvas.GetTop(ant) == Canvas.GetTop(a));
                    if(food != null)
                    {
                        ant.HasFood = true;
                        food.Amount--;
                        if(food.Amount == 0)
                        {
                            Canvas.Children.Remove(food);
                            FoodItems = FoodItems.Without(food);
                        }
                    }
                });
            }
            foreach(Trail t in Trails)
            {
                t.TimeSpan -= TimeSpan.FromMilliseconds(1);
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var ant = new Ant();
            Ants.Add(ant);
            Canvas.Children.Add(ant);
        }

        private void Food_Click(object sender, RoutedEventArgs e)
        {
            var food = new Food();
            FoodItems.Add(food);
            Canvas.Children.Add(food);
        }
    }


    public class Trail
    {
        public Trail(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X;
        public double Y;
        private TimeSpan _timeSpan = TimeSpan.FromSeconds(10);
        public TimeSpan TimeSpan
        {
            get => _timeSpan;
            set
            {
                if(value == TimeSpan.FromSeconds(0))
                {
                    MainWindow.Trails = MainWindow.Trails.Without(this);
                }
                else
                {
                    _timeSpan = value;
                }
            }
        }
    }

    public static class ext
    {
        public static ConcurrentBag<T> Without<T>(this ConcurrentBag<T> Items, T item) where T : class
        {
            List<T> items2 = new List<T>();
            items2.Add(item);
            return new ConcurrentBag<T>(Items.Except(items2));
        }
    }
}

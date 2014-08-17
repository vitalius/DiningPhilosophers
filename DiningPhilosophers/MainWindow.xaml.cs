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

namespace DiningPhilosophers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Ellipse> philosophers = new List<Ellipse>();
        public static List<Label> philLabels = new List<Label>();
        public static List<Ellipse> forks = new List<Ellipse>();
        public static List<TextBox> foodText = new List<TextBox>();

        public MainWindow()
        {
            InitializeComponent();

            philosophers.Add(LayoutRoot.FindName("Philosopher1") as Ellipse);
            philosophers.Add(LayoutRoot.FindName("Philosopher2") as Ellipse);
            philosophers.Add(LayoutRoot.FindName("Philosopher3") as Ellipse);
            philosophers.Add(LayoutRoot.FindName("Philosopher4") as Ellipse);
            philosophers.Add(LayoutRoot.FindName("Philosopher5") as Ellipse);

            forks.Add(LayoutRoot.FindName("Fork1") as Ellipse);
            forks.Add(LayoutRoot.FindName("Fork2") as Ellipse);
            forks.Add(LayoutRoot.FindName("Fork3") as Ellipse);
            forks.Add(LayoutRoot.FindName("Fork4") as Ellipse);
            forks.Add(LayoutRoot.FindName("Fork5") as Ellipse);

            philLabels.Add(LayoutRoot.FindName("PhilosopherFood1") as Label);
            philLabels.Add(LayoutRoot.FindName("PhilosopherFood2") as Label);
            philLabels.Add(LayoutRoot.FindName("PhilosopherFood3") as Label);
            philLabels.Add(LayoutRoot.FindName("PhilosopherFood4") as Label);
            philLabels.Add(LayoutRoot.FindName("PhilosopherFood5") as Label);

            foodText.Add(LayoutRoot.FindName("FoodText1") as TextBox);
            foodText.Add(LayoutRoot.FindName("FoodText2") as TextBox);
            foodText.Add(LayoutRoot.FindName("FoodText3") as TextBox);
            foodText.Add(LayoutRoot.FindName("FoodText4") as TextBox);
            foodText.Add(LayoutRoot.FindName("FoodText5") as TextBox);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var t = new PhilosophersTable(philosophers, forks, philLabels, foodText);
            t.Run();
        }
    }
}

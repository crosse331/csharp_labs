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
using Newtonsoft.Json;
using System.IO;
using Shapes;
using OxyPlot;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SaveData data;

        public MainWindow()
        {
            InitializeComponent();

            var ls = new OxyPlot.LineSeries();
            for (int i=0;i<10;i++)
            {
                ls.Points.Add(new DataPoint(i, -i));
            }
            this.AddChild(ls);
            //panel = this.GetTemplateChild("panel") as StackPanel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                string fileName = openDialog.FileName;

                using (StreamReader fs = new StreamReader(fileName))
                {
                    string file = fs.ReadToEnd();

                    var shapes = JsonConvert.DeserializeObject<SaveData>(file);

                    //foreach (var s in shapes)
                    //{
                    //    Console.WriteLine(s.GetType());
                    //}
                    data = shapes;
                }

                foreach (var c in data.circles)
                {
                    DrawCircle(c);
                }
                foreach (var ell in data.ellipses)
                {
                    DrawEllipse(ell);
                }

                
            }
        }

        private void DrawCircle(CircleInfo c)
        {
            //var stPanel = new StackPanel();
            var ell = new System.Windows.Shapes.Ellipse();
            ell.StrokeThickness = 2;
            ell.Stroke = Brushes.Black;
            ell.Height = 100;
            ell.Width = 100;
            //ell.Margin
            panel.Children.Add(ell);

            //this.Content = stPanel;
        }

        private void DrawEllipse(EllipseInfo e)
        {
            var ell = new System.Windows.Shapes.Ellipse();
            ell.StrokeThickness = 2;
            ell.Stroke = Brushes.Black;
            Shapes.Ellipse myEll = new Shapes.Ellipse(new Vec2f(e.f1[0], e.f1[1]),
                new Vec2f(e.f2[0], e.f2[1]), e.bigO);
            ell.Height = myEll.a * 2 * 100;
            ell.Width = myEll.b * 2 * 100;

            panel.Children.Add(ell);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

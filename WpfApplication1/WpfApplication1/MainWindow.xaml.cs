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
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                string fileName = openDialog.FileName;

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ShapeInfo[]), new List<Type>() { typeof(ShapeInfo), typeof(CircleInfo), typeof(EllipseInfo), typeof(PolygonInfo) });

                    var shapes = new ShapeInfo[100];
                    shapes = (ShapeInfo[])serializer.ReadObject(fs);

                    foreach(var s in shapes)
                    {
                        Console.WriteLine(s.GetType());
                    }
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

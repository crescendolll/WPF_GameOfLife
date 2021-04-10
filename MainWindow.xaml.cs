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

//https://youtu.be/yEjRK54-1EU?t=2085

namespace WPF_GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            const int anzahlZellenHoch = 50;
            const int anzahlZellenBreit = 50;

            //Canvas cnv_1 = new Canvas();

            //cnv_Spielfeld.Width = anzahlZellenBreit * 15;
            //cnv_Spielfeld.Height = anzahlZellenHoch * 15;

            //double z1 = cnv_Spielfeld.Width;
            //double z2 = cnv_Spielfeld.Height;

            //cnv_Spielfeld.Background = Brushes.Black;
            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    Rectangle zelle = new Rectangle();
                    zelle.Width = cnv_Spielfeld.ActualWidth / anzahlZellenBreit;// - 2.0;
                    zelle.Height = cnv_Spielfeld.ActualHeight / anzahlZellenHoch;// - 2.0;
                    zelle.Stroke = Brushes.Gray;
                    zelle.StrokeThickness = 0.5;
                    zelle.Fill = Brushes.White;
                    cnv_Spielfeld.Children.Add(zelle);
                    Canvas.SetLeft(zelle, j * cnv_Spielfeld.ActualWidth / anzahlZellenBreit);
                    Canvas.SetTop(zelle, i * cnv_Spielfeld.ActualHeight / anzahlZellenHoch);
                    zelle.MouseDown += Zelle_MouseDown;
                }
            }
        }

        private void Zelle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Rectangle)sender).Fill == Brushes.White)
            {
                ((Rectangle)sender).Fill = Brushes.Black;
            } else
            {
                ((Rectangle)sender).Fill = Brushes.White;
            }
        }
    }
}

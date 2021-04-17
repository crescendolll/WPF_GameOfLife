using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int anzahlZellenHoch;// = 50;
        public static int anzahlZellenBreit;// = 50;
        public double generationenDauer = 0.1;
        public Rectangle[,] zellen;// = new Rectangle[anzahlZellenHoch, anzahlZellenBreit];
        public DispatcherTimer generationenTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            generationenTimer.Interval = TimeSpan.FromSeconds(generationenDauer);
            generationenTimer.Tick += GenerationenTimer_Tick;
            lbl_Infotext.Content = "Welcome to Conways\nGame of Life. \n\nRegeln und Info:\nhttps://w.wiki/3CWR";
        }

        private void GenerationenTimer_Tick(object sender, EventArgs e)
        {
            btn_Next.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {

            int eingabeHoch;
            int eingabeBreit;
            bool isParsableHoch = Int32.TryParse(tbx_ZellenHoch.Text, out eingabeHoch);
            bool isParsableBreit = Int32.TryParse(tbx_ZellenBreit.Text, out eingabeBreit);

            if ((isParsableHoch && (eingabeHoch < 51 && eingabeHoch > 0)) && (isParsableBreit && (eingabeBreit < 51 && eingabeBreit > 0)))
            {
                anzahlZellenHoch = eingabeHoch;
                anzahlZellenBreit = eingabeBreit;
                zellen = new Rectangle[anzahlZellenHoch, anzahlZellenBreit];
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

                        zellen[i, j] = zelle;
                    }
                }
                lbl_Infotext.Content = "Das Spiel wurde \nerstellt.";
            }
            else
            {
                lbl_Infotext.Content = "Bitte ganze Zahl \n1-50 eingeben.";
            }                

            btn_Next.IsEnabled = true;
            btn_Reset.IsEnabled = true;
            btn_Start.IsEnabled = true;
            btn_Stop.IsEnabled = true;
            btn_Random.IsEnabled = true;

        }

        private void Zelle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lbl_Infotext.Content = "";

            if (((Rectangle)sender).Fill == Brushes.White)
            {
                ((Rectangle)sender).Fill = Brushes.Black;
            } else
            {
                ((Rectangle)sender).Fill = Brushes.White;
            }
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            btn_Create.IsEnabled = false;

            int[,] listeLebendige = new int[anzahlZellenHoch, anzahlZellenBreit];

            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    int above = i - 1;
                    int below = i + 1;
                    int left = j - 1;
                    int right = j + 1;

                    if (above < 0)
                    {
                        above = anzahlZellenHoch - 1;
                    }
                    if (below >= anzahlZellenHoch)
                    {
                        below = 0;
                    }
                    if (left < 0)
                    {
                        left = anzahlZellenBreit - 1;
                    }
                    if (right >= anzahlZellenBreit)
                    {
                        right = 0;
                    }

                    if (zellen[above,left].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[i , left].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[below, left].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[below, j].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[below, right].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[i, right].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[above, right].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                    if (zellen[above, j].Fill == Brushes.Black)
                    {
                        listeLebendige[i, j]++;
                    }
                }
            }

            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    if (zellen[i,j].Fill == Brushes.Black)
                    {
                        if (listeLebendige[i,j] < 2 || listeLebendige[i, j] > 3)
                        {
                            zellen[i, j].Fill = Brushes.White;
                        }

                    } else if (listeLebendige[i,j] == 3)
                    {
                        zellen[i, j].Fill = Brushes.Black;
                    }


                }
            }                 

        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            lbl_Infotext.Content = "Das Spiel läuft \nautomatisch";
            generationenTimer.Start();
            btn_Create.IsEnabled = false;
            btn_Random.IsEnabled = false;
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            lbl_Infotext.Content = "Das Spiel wurde \ngestoppt";
            generationenTimer.Stop();
            btn_Random.IsEnabled = true;
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            lbl_Infotext.Content = "Das Spiel wurde \nzurückgesetzt";
            generationenTimer.Stop();
            btn_Create.IsEnabled = true;
            btn_Random.IsEnabled = true;
            tbx_ZellenBreit.IsEnabled = true;
            tbx_ZellenHoch.IsEnabled = true;
            foreach (Rectangle zelle in zellen)
            {
                zelle.Fill = Brushes.White;
            }
        }

        private void btn_Random_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            foreach (Rectangle zelle in zellen)
            {
                double live = rand.NextDouble();
                if (live <= 0.5)
                {
                    zelle.Fill = Brushes.Black;
                } else
                {
                    zelle.Fill = Brushes.White;
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            generationenDauer = e.NewValue;
            generationenTimer.Interval = TimeSpan.FromSeconds(generationenDauer);
            String angabe = ((float) generationenDauer).ToString();
            lbl_Infotext.Content = angabe + "ist die Dauer";
        }

        private void tbx_ZellenHoch_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;

            bool isParsable = Int32.TryParse(tbx_ZellenHoch.Text, out number);
            if (isParsable && (number < 51 && number > 0))
                lbl_Infotext.Content = "";
            else
                lbl_Infotext.Content = "Bitte ganze Zahl \n1-50 eingeben.";

        }

        private void tbx_ZellenBreit_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;

            bool isParsable = Int32.TryParse(tbx_ZellenBreit.Text, out number);
            if (isParsable && (number < 51 && number > 0))
                lbl_Infotext.Content = "";
            else
                lbl_Infotext.Content = "Bitte ganze Zahl \n1-50 eingeben.";
        }
    }
}

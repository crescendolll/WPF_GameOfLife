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
        public MainWindow()
        {
            InitializeComponent();
            generationenTimer.Interval = TimeSpan.FromSeconds(0.01);
            generationenTimer.Tick += GenerationenTimer_Tick;//(sender, e) => btn_Next_Click(sender,(RoutedEventArgs) e);
            //generationenDauer.Start();
        }

        private void GenerationenTimer_Tick(object sender, EventArgs e)
        {
            btn_Next.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        public static int anzahlZellenHoch = 50;
        public static int anzahlZellenBreit = 50;
        public Rectangle[,] zellen = new Rectangle[anzahlZellenHoch, anzahlZellenBreit];
        public DispatcherTimer generationenTimer = new DispatcherTimer();

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {


            if (zellen[0, 0] == null)
            {
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
                lbl_Infotext.Content = "Das Spiel wurde \nerstellt";
            } else
            {
                lbl_Infotext.Content = "Das Spiel ist bereits \nerstellt";
            }

            btn_Next.IsEnabled = true;
            btn_Reset.IsEnabled = true;
            btn_Start.IsEnabled = true;
            btn_Stop.IsEnabled = true;

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
            //lbl_Infotext.Content = "Die nächste Generation \nwurde gespielt.";
            btn_Create.IsEnabled = false;

            int[,] listeLebendige = new int[anzahlZellenHoch, anzahlZellenBreit];

            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    //int lebendige = 0;
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


                    //listeLebendige[i, j]++;
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
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            lbl_Infotext.Content = "Das Spiel wurde \ngestoppt";
            generationenTimer.Stop();
            btn_Create.IsEnabled = true;
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            lbl_Infotext.Content = "Das Spiel wurde \nzurückgesetzt";
            generationenTimer.Stop();
            btn_Create.IsEnabled = true;
            foreach (Rectangle zelle in zellen)
            {
                zelle.Fill = Brushes.White;
            }
        }
    }
}

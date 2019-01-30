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

namespace GameWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Label> tilesListLabels = new List<Label>();
        static List<Char> tilesListChars = new List<char>();
        //char[,] tilesListChars = new char[10, 10];
        static Label bufferFirstButton = null;
        Random rnd = new Random();
        int scoreInt = 0;
        int ballsCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            linesForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GenerateField();
            GenerateNextBalls();           
        }

        private void CheckLineBalls()
        {
            // создаем двумерный массив символов из листа
            char[,] field = new char[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    field[y, x] = tilesListChars[y * 10 + x];
                }
            }
            // проверка совпадений по горизонтали
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (field[y, x] == field[y, x + 1] && field[y, x] == field[y, x + 2] && field[y, x] == field[y, x + 3] && field[y, x] == field[y, x + 4] && field[y, x] != '0')
                    {
                        scoreInt += 5;
                        score.Content = "Your score: " + scoreInt;
                        for (int i = 0; i < 5; i++)
                        {
                            tilesListChars[y * 10 + (x + i)] = '0';
                            tilesListLabels[y * 10 + (x + i)].Background = GetImageBrush("bg1.png");
                        }
                    }
                }
            }
            // проверка совпадений по вертикали
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (field[y, x] == field[y + 1, x] && field[y, x] == field[y + 2, x] && field[y, x] == field[y + 3, x] && field[y, x] == field[y + 4, x] && field[y, x] != '0')
                    {
                        scoreInt += 5;
                        score.Content = "Your score: " + scoreInt;
                        for (int i = 0; i < 5; i++)
                        {
                            tilesListChars[(y + i) * 10 + x] = '0';
                            tilesListLabels[(y + i) * 10 + x].Background = GetImageBrush("bg1.png");
                        }
                    }
                }
            }
        }

        private static ImageBrush GetImageBrush(string fileName)
        {
            ImageBrush imgBrush = new ImageBrush();
            Label titleLabel = new Label();
            BitmapImage bmi = new BitmapImage();
            string fullPath = @"Images\" + fileName;

            bmi.BeginInit();
            bmi.UriSource = new Uri(fullPath, UriKind.Relative);
            bmi.EndInit();

            imgBrush.ImageSource = bmi;

            return imgBrush;
        }

        public void GenerateField()
        {
            for (int y = 1; y < 11; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    tilesListChars.Add('0');
                    Label tile = new Label()
                    {
                        Background = GetImageBrush("bg1.png")
                    };
                    Grid.SetColumn(tile, x);
                    Grid.SetRow(tile, y);
                    tile.MouseDown += tile_MouseDown;

                    tilesListLabels.Add(tile);
                    this.mainGrid.Children.Add(tile);
                }
            }
        }

        void tile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            button1_Click(sender, e);
            CheckLineBalls(); 
        }

        private void GenerateNextBalls()
        {
            CheckBallsCount();
            for (int i = 0; i < 3; i++)
            {
                if (ballsCount == 100)
                {
                    MessageBox.Show("Game over");
                    Close();
                    return;
                }
                int number = rnd.Next(100);
                int color = rnd.Next(5);
                if (tilesListChars[number] != '0')
                {
                    i--;
                }
                else
                {
                    switch (color)
                    {
                        case 0:
                            {
                                tilesListChars[number] = 'R';
                                tilesListLabels[number].Background = GetImageBrush("r1.png");
                                break;
                            }
                        case 1:
                            {
                                tilesListChars[number] = 'G';
                                tilesListLabels[number].Background = GetImageBrush("g1.png");
                                break;
                            }
                        case 2:
                            {
                                tilesListChars[number] = 'B';
                                tilesListLabels[number].Background = GetImageBrush("b1.png");
                                break;
                            }
                        case 3:
                            {
                                tilesListChars[number] = 'O';
                                tilesListLabels[number].Background = GetImageBrush("y1.png");
                                break;
                            }
                        case 4:
                            {
                                tilesListChars[number] = 'P';
                                tilesListLabels[number].Background = GetImageBrush("p1.png");
                                break;
                            }
                        default:
                            break;
                    }
                    ballsCount++;
                }
            }
        }

        private void CheckBallsCount()
        {
            ballsCount = 0;
            for (int i = 0; i < 100; i++)
            {
                if (tilesListChars[i] != '0')
                {
                    ballsCount++;
                }
            }
        }
        /// <summary>
        /// Change button background
        /// </summary>
        /// <param name="button">sender</param>
        /// <param name="selection">true - selected; false - unselected</param>
        private void SelectButton(Label button, bool selection)
        {
            char selectedChar;
            if (selection)
            {
                selectedChar = '2';
            }
            else
            {
                selectedChar = '1';
            }

            for (int i = 0; i < 100; i++)
            {
                if (button.Equals(tilesListLabels[i]))
                {
                    // swap tiles
                    if (selectedChar == '1')
                    {
                        for (int z = 0; z < 100; z++)
                        {
                            if (bufferFirstButton.Equals(tilesListLabels[z]))
                            {
                                tilesListChars[i] = tilesListChars[z];
                                tilesListChars[z] = '0';
                            }
                        }
                    }
                    // swich tile
                    switch (tilesListChars[i])
                    {
                        case 'R':
                            {
                                tilesListLabels[i].Background = GetImageBrush("r" + selectedChar + ".png");
                                return;
                            }
                        case 'G':
                            {
                                tilesListLabels[i].Background = GetImageBrush("g" + selectedChar + ".png");
                                return;
                            }
                        case 'B':
                            {
                                tilesListLabels[i].Background = GetImageBrush("b" + selectedChar + ".png");
                                return;
                            }
                        case 'O':
                            {
                                tilesListLabels[i].Background = GetImageBrush("y" + selectedChar + ".png");
                                return;
                            }
                        case 'P':
                            {
                                tilesListLabels[i].Background = GetImageBrush("p" + selectedChar + ".png");
                                return;
                            }
                        default:
                            {
                                if (selection)
                                {
                                    button.Background = GetImageBrush("bg2.png");
                                }
                                else button.Background = GetImageBrush("bg1.png");
                                return;
                            }
                    }
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Label button = new Label();
            button = (Label)sender;
            if (bufferFirstButton == null)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (button.Equals(tilesListLabels[i]))
                    {
                        if (tilesListChars[i] == '0')
                        {
                            return;
                        }
                    }
                }

                bufferFirstButton = (Label)sender;
                SelectButton(bufferFirstButton, true);
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    if (button.Equals(tilesListLabels[i]))
                    {
                        if (tilesListChars[i] != '0')
                        {
                            return;
                        }
                    }
                }

                SelectButton(button, false);
                bufferFirstButton.Background = GetImageBrush("bg1.png");

                for (int i = 0; i < 100; i++)
                {
                    if (bufferFirstButton.Equals(tilesListLabels[i]))
                    {
                        tilesListChars[i] = '0';
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    if (button.Equals(tilesListLabels[i]))
                    {
                        tilesListChars[i] = tilesListChars[i];
                    }
                }

                button.Content = bufferFirstButton.Content;
                bufferFirstButton.Content = "";

                bufferFirstButton = null;
                GenerateNextBalls();
            }
        }

    }
}

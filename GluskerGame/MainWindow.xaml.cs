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

namespace GluskerGame
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Cell[,] Map = new Cell[4,4];

        public MainWindow()
        {
            InitializeComponent();
            CreateCell();
            CreateCell();
        }

        private Cell CreateCell(int? x = null, int? y = null, int val = 2)
        {
            Cell cell = null;

            if (!(x == null || y == null))
            {//генерить не рандомно
                if (Map[(int)y, (int)x] == null)
                {
                    cell = new Cell(Map, val, (int)x, (int)y, this);
                    cell.VerticalAlignment = VerticalAlignment.Top;
                    cell.HorizontalAlignment = HorizontalAlignment.Left;
                    cell.Margin = new Thickness(70 * (int)x, 70 * (int)y, 0, 0);
                    Map[(int)y, (int)x] = cell;
                    MainGrid.Children.Add(cell);
                }
            }
            else
            { // рандомно
                Random rand = new Random(DateTime.Now.Millisecond % 100000);
                int countOfTryes = 0; // если с тысечного раза не попал, то уж точно проиграл
                bool @continue = true;
                do
                {
                    int cellX = rand.Next(0, 4);
                    int celly = rand.Next(0, 4);

                    if (Map[celly, cellX] == null)
                    {
                        @continue = false;
                        cell = new Cell(Map, val, cellX, celly, this);
                        cell.Margin = new Thickness(70 * cellX, 70 * celly, 0, 0);
                        cell.HorizontalAlignment = HorizontalAlignment.Left;
                        cell.VerticalAlignment = VerticalAlignment.Top;
                        Map[celly, cellX] = cell;
                        MainGrid.Children.Add(cell);

                    }
                    else
                        countOfTryes++;
                } while (@continue && countOfTryes < 1000);

            }

            int count = 0;
            for (int nY = 0; nY < 4; nY++)
            {
                for (int nX = 0; nX < 4; nX++)
                {
                    if (Map[nY, nX] != null)
                        count++;
                }
            }

            if (count == 16)
            {
                MessageBoxResult result = MessageBox.Show("Мне кажется или вы проигрываете?\n" +
                                                          "Может начнём сначала?", "Заново?", 
                                                          MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    Restart();
            }   
            
            return cell;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool isChanged = false;

            switch (e.Key)
            {
                case Key.A: MoveCellLeft(ref isChanged); break;
                case Key.D: MoveCellRight(ref isChanged); break;
                case Key.W: MoveCellUp(ref isChanged); break;
                case Key.S: MoveCellDown(ref isChanged); break;
                case Key.R: Restart(); break;
            }
            
            if (isChanged)
                CreateCell();
        }


        private void Restart()
        {
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    try
                    {
                        MainGrid.Children.Remove(Map[y, x]);
                        Map[y, x] = null;
                    }
                    catch { }
                }
            CreateCell();
            CreateCell();

        }
        private void MoveCellUp(ref bool isChanged)
        {
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if (Map[y, x] != null && Map[y, x].Y > 0)
                    {
                        int steps = 0;
                        Cell c = Map[y, x];
                        while (c.CanMove(c.X, c.Y - 1) && c.Y > 0 && !c.Changed)
                        {
                            isChanged = true;
                            c.Move(0,-1);
                            steps++;
                        }
                        c.Changed = false;
                    }
                }
        }

        private void MoveCellDown(ref bool isChanged)
        {
            //перебрать все ячейки
            for (int y = 3; y >= 0; y--)
                for (int x = 3; x >= 0; x--)
                {
                    //если ячейка не пуста, и не самая нижняя
                    if (Map[y, x] != null && Map[y, x].Y < 3)
                    {
                        int steps = 0;
                        Cell c = Map[y, x];
                        while (c.CanMove(c.X, c.Y + 1) && c.Y != 3 && !c.Changed) //пока можно двигать вниз, и не нижняя линия
                        {
                            isChanged = true;
                            c.Move(0, 1);
                            steps++;
                        }
                        c.Changed = false;
                    }
                }
        }


        private void MoveCellRight(ref bool isChanged)
        {
            //перебрать все ячейки
            for (int y = 3; y >= 0; y--)
                for (int x = 3; x >= 0; x--)
                {
                    //если ячейка не пуста, и не самая правая
                    if (Map[y, x] != null && Map[y, x].X < 3)
                    {
                        int steps = 0;
                        Cell c = Map[y, x];
                        while (c.CanMove(c.X + 1, c.Y) && c.X < 3 && !c.Changed) //пока можно двигать вправо
                        {
                            isChanged = true;
                            c.Move(1,0);
                            steps++;
                        }

                        c.Changed = false;
                    }
                }

        }
        private void MoveCellLeft(ref bool isChanged)
        {
            //перебрать все ячейки
            for (int y = 3; y >= 0; y--)
                for (int x = 0; x <= 3; x++)
                {
                    //если ячейка не пуста, и не самая левая
                    if (Map[y, x] != null && Map[y, x].X > 0)
                    {
                        int steps = 0;
                        Cell c = Map[y, x];
                        while (c.CanMove(c.X - 1, c.Y) && c.X > 0 && !c.Changed) //пока можно двигать вниз, и не  левая
                        {
                            isChanged = true;
                            c.Move(-1,0);
                            steps++;
                        }
                        c.Changed = false;
                    }
                }


        }

    }  
}

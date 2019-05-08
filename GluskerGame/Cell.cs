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
    class Cell : Grid
    {


        private Label numContent;
        private Border border;
        private MainWindow rootWin;

        private Cell[,] Map;
        
        public int X{ get; private set; }
        public int Y{ get; private set; }

        public bool Changed { get; set; } = false;

        private const int defHeight = 70;
        private const int defWidht  = 70;

        public int Content
        {
            get { return (int)numContent.Content; }
            set
            {
                switch (value)
                {
                    case 2:    Background = Brushes.Blue; break;
                    case 4:    Background = Brushes.Red; break;
                    case 8:    Background = Brushes.Green; break;
                    case 16:   Background = Brushes.Yellow; break;
                    case 32:   Background = Brushes.Gray; break;
                    case 64:   Background = Brushes.Pink; break;
                    case 128:  Background = Brushes.Brown; break;
                    case 256:  Background = Brushes.AliceBlue; break;
                    case 512:  Background = Brushes.Beige; break;
                    case 1024: Background = Brushes.BlueViolet; break;
                    case 2048: rootWin.Hide();MessageBox.Show("U Win"); rootWin.Close(); break;
                }
                numContent.Content = value;
            }
        }



        public Cell(Cell[,] setMap, int val, int x,int y,MainWindow root)
        {
            X = x;
            Y = y;
            rootWin= root;
            numContent = new Label();
            border = new Border();
            Content = val;
            Map = setMap;

            #region init
            numContent.VerticalAlignment = VerticalAlignment.Center;
            numContent.HorizontalAlignment = HorizontalAlignment.Center;

            Height = defHeight;
            Width = defWidht;

            border.Height = defHeight;
            border.Width = defWidht;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);

            #endregion

            Children.Add(border);
            Children.Add(numContent);

        }



        public bool CanMove(int x, int y)
        {
            bool answer = false;
            try
            {
                answer =(Map[y, x] == null || Map[y, x].Content == Content);
            }
            //???
            catch { } 
            return answer;
        }


        public void Move(int x, int y)
        {

            if (Map[Y + y, X + x] != null && Map[Y+ y, X + x].Content == Content)
            {
                //удалить ячейку с окна
                rootWin.MainGrid.Children.Remove(Map[Y + y, X + x]);
                //забыть про ячейку
                Map[Y+y, X + x] = null;
                Content *= 2;

                // передвинуть текущую ячейку
                Map[Y + y, X + x] = this;
                //забыть старую ссылку на текущую ячейку
                Map[Y, X] = null;
                Margin = new Thickness(70 * (X + x), 70 * (Y + y), 0, 0);
                X += x;
                Y += y;
                Changed = true;
            }
            else
            {

                Map[Y + y, X + x] = this;
                Map[Y, X] = null;
                Margin = new Thickness(70 * (X + x), 70 * (Y + y), 0, 0);
                X += x;
                Y += y;

            }

        }

        


    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tygrysy_i_Byki
{
    class Board
    {
        public Board()
        {
            fields = new ObservableCollection<ObservableCollection<Field>>();
            for(int i = 0; i < BOARD_HIGHT; i++)
            {
                fields.Add(new ObservableCollection<Field>());
                for (int j = 0; j < BOARD_WIDTH; j++)
                    fields[i].Add(new Field(null, i, j));
            }

            activeFields = new Stack<Point>();
            activeAnimal = new Point(-1, -1);
            
            predatorImage = new BitmapImage(new Uri("obrazki/tiger.png", UriKind.Relative));
            herbivoreImage = new BitmapImage(new Uri("obrazki/bull.png", UriKind.Relative));
            emptyImage = new BitmapImage(new Uri("obrazki/emptyImage.png", UriKind.Relative));
            
        }

        private const int BOARD_WIDTH = 4;
        private const int BOARD_HIGHT = 5;

        public ObservableCollection<ObservableCollection<Field>> fields { get; private set; }
        private ImageSource predatorImage;
        private ImageSource herbivoreImage;
        private ImageSource emptyImage;
        private Point activeAnimal; // Jezeli aktywnt to wskazuje na pozycje na planszy
                                    // w.p.p. (-1, x)
        private Stack<Point> activeFields;

        public void resetBoard()
        {
            activeAnimal.X = -1; activeAnimal.Y = -1;
            activeFields.Clear();

            // Pierwsze dwa rzedy (Oponent)
            int i;
            for (i = 0; i < 2; i++)
                foreach (var j in fields[i])
                    j.Image = herbivoreImage;

            // Puste rzedy
            for (; i < BOARD_HIGHT - 1; i++)
                foreach (var j in fields[i])
                    j.Image = emptyImage;

            // Ostatni rzad
            {
                int j;
                for (j = 0; j < BOARD_WIDTH / 2 - 1; j++)
                    fields[i][j].Image = emptyImage;
                fields[BOARD_HIGHT - 1][j++].Image = predatorImage;
                fields[BOARD_HIGHT - 1][j++].Image = predatorImage;
                for (; j < BOARD_WIDTH; j++)
                    fields[BOARD_HIGHT - 1][j].Image = emptyImage;
            }

            foreach (var k in fields)
                foreach (var j in k)
                    j.Active = FieldState.Normal;
        }

        private void changeStateOfField(int x, int y, FieldState fieldState)
        {
            fields[x][y].Active = fieldState;
            activeFields.Push(new Point(x, y));
        }

        private void colorFieldsToMove(int x, int y, bool isPredator)
        {
            //   ===========>
            //  ||         Y
            //  ||
            //  ||
            //  || X
            //  \/
            
            changeStateOfField(x, y, FieldState.Move);
            activeAnimal.X = x; activeAnimal.Y = y;

            // Left
            if (y - 1 >= 0 && fields[x][y - 1].Image == emptyImage)
            {
                changeStateOfField(x, y - 1, FieldState.Move);
                if (isPredator == true && y - 2 >= 0 && fields[x][y - 2].Image == herbivoreImage)
                    changeStateOfField(x, y - 2, FieldState.Attack);
            }
            // Right
            if (y + 1 < BOARD_WIDTH && fields[x][y + 1].Image == emptyImage)
            {
                changeStateOfField(x, y + 1, FieldState.Move);
                if (isPredator == true && y + 2 < BOARD_WIDTH && fields[x][y + 2].Image == herbivoreImage)
                    changeStateOfField(x, y + 2, FieldState.Attack);
            }
            // Up
            if (x - 1 >= 0 && fields[x - 1][y].Image == emptyImage)
            {
                changeStateOfField(x - 1, y, FieldState.Move);
                if (isPredator == true && x - 2 >= 0 && fields[x - 2][y].Image == herbivoreImage)
                    changeStateOfField(x - 2, y , FieldState.Attack);
            }
            // Down
            if (x + 1 < BOARD_HIGHT  && fields[x + 1][y].Image == emptyImage)
            {
                changeStateOfField(x + 1, y, FieldState.Move);
                if (isPredator == true && x + 2 < BOARD_HIGHT && fields[x + 2][y].Image == herbivoreImage)
                    changeStateOfField(x + 2, y , FieldState.Attack);
            }
        }

        private void clearColorFieldsToMove()
        {
            while(activeFields.Count > 0)
            {
                Point p = activeFields.Pop();
                fields[p.X][p.Y].Active = FieldState.Normal;
            }
        }

        public bool action(int x, int y, bool predatorRound)
        {
            // Przesuniecie
            if(activeAnimal.X != -1 && fields[x][y].Active != FieldState.Normal && (activeAnimal.X != x || activeAnimal.Y != y))
            {
                clearColorFieldsToMove();
                fields[activeAnimal.X][activeAnimal.Y].Image = emptyImage;
                fields[x][y].Image = predatorRound ? predatorImage : herbivoreImage;
                return true;
            }

            if (fields[x][y].Image == predatorImage && predatorRound)
            {
                if (activeAnimal.X == x && activeAnimal.Y == y) // Ten sam (odznacz)
                {
                    clearColorFieldsToMove();
                    activeAnimal.X = -1;
                }
                else
                {
                    clearColorFieldsToMove();
                    colorFieldsToMove(x, y, true);
                }
            }
            else if(fields[x][y].Image == herbivoreImage && predatorRound == false)
            {
                if (activeAnimal.X == x && activeAnimal.Y == y) // Ten sam (odznacz)
                {
                    clearColorFieldsToMove();
                    activeAnimal.X = -1;
                }
                else
                {
                    clearColorFieldsToMove();
                    colorFieldsToMove(x, y, false);
                }
            }

            return false;
        }

        /// <summary>
        /// 0 - empty
        /// 1 - predator
        /// 2 - herbivore
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void setField(int x, int y, int value)
        {
            switch (value)
            {
                case 0: fields[x][y].Image = emptyImage;
                    break;
                case 1: fields[x][y].Image = predatorImage;
                    break;
                case 2: fields[x][y].Image = herbivoreImage;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}

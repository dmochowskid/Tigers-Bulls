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
            for (int i = 0; i < BOARD_HIGHT; i++)
            {
                fields.Add(new ObservableCollection<Field>());
                for (int j = 0; j < BOARD_WIDTH; j++)
                    fields[i].Add(new Field(null, i, j));
            }

            activeFields = new Stack<Point>();
            activeAnimal = new Point(-1, -1);

            settingsWindow = SettingsWindow.getInstance();
        }

        public const int BOARD_WIDTH = 4;
        public const int BOARD_HIGHT = 5;

        public ObservableCollection<ObservableCollection<Field>> fields { get; private set; }
        public Point activeAnimal; // Jezeli aktywnt to wskazuje na pozycje na planszy
                                   // w.p.p. (-1, x)
        private Stack<Point> activeFields;
        private SettingsWindow settingsWindow;

        public void resetBoard()
        {
            activeAnimal.X = -1; activeAnimal.Y = -1;
            clearColorFieldsToMove();

            // Pierwsze dwa rzedy (Oponent)
            int i;
            for (i = 0; i < 2; i++)
                foreach (var j in fields[i])
                    j.fieldType = FieldType.Herbivore;

            // Puste rzedy
            for (; i < BOARD_HIGHT - 1; i++)
                foreach (var j in fields[i])
                    j.fieldType = FieldType.Empty;

            // Ostatni rzad
            {
                int j;
                for (j = 0; j < BOARD_WIDTH / 2 - 1; j++)
                    fields[i][j].fieldType = FieldType.Empty;
                fields[BOARD_HIGHT - 1][j++].fieldType = FieldType.Predator;
                fields[BOARD_HIGHT - 1][j++].fieldType = FieldType.Predator;
                for (; j < BOARD_WIDTH; j++)
                    fields[BOARD_HIGHT - 1][j].fieldType = FieldType.Empty;
            }

            foreach (var k in fields)
                foreach (var j in k)
                {
                    j.Active = FieldState.Normal;
                    if (j.fieldType == FieldType.Predator)
                        j.Image = settingsWindow.PredatorImage;
                    else if (j.fieldType == FieldType.Herbivore)
                        j.Image = settingsWindow.HerbivoreImage;
                    else
                        j.Image = settingsWindow.EmptyImage;
                }
        }

        public List<Point> herbivoresPosition()
        {
            List<Point> herbivoresPosition = new List<Point>(); ;
            for (int i = 0; i < fields.Count; i++)
                for (int j = 0; j < fields[i].Count; j++)
                    if (fields[i][j].Image == SettingsWindow.getInstance().HerbivoreImage)
                        herbivoresPosition.Add(new Point(i, j));
            return herbivoresPosition;
        }

        internal void refreshImages()
        {
            foreach (var i in fields)
                foreach (var j in i)
                {
                    if (j.fieldType == FieldType.Herbivore)
                        j.Image = settingsWindow.HerbivoreImage;
                    else if (j.fieldType == FieldType.Predator)
                        j.Image = settingsWindow.PredatorImage;
                }
        }

        public int herbivoreCount()
        {
            int result = 0;

            foreach (var i in fields)
                foreach (var j in i)
                    if (j.Image == settingsWindow.HerbivoreImage)
                        result++;

            return result;
        }

        private void changeStateOfField(int x, int y, FieldState fieldState)
        {
            fields[x][y].Active = fieldState;
            activeFields.Push(new Point(x, y));
        }

        public void colorFieldsToMove(int x, int y, bool isPredator)
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
            if (y - 1 >= 0 && fields[x][y - 1].Image == settingsWindow.EmptyImage)
            {
                changeStateOfField(x, y - 1, FieldState.Move);
                if (isPredator == true && y - 2 >= 0 && fields[x][y - 2].Image == settingsWindow.HerbivoreImage)
                    changeStateOfField(x, y - 2, FieldState.Attack);
            }
            // Right
            if (y + 1 < BOARD_WIDTH && fields[x][y + 1].Image == settingsWindow.EmptyImage)
            {
                changeStateOfField(x, y + 1, FieldState.Move);
                if (isPredator == true && y + 2 < BOARD_WIDTH && fields[x][y + 2].Image == settingsWindow.HerbivoreImage)
                    changeStateOfField(x, y + 2, FieldState.Attack);
            }
            // Up
            if (x - 1 >= 0 && fields[x - 1][y].Image == settingsWindow.EmptyImage)
            {
                changeStateOfField(x - 1, y, FieldState.Move);
                if (isPredator == true && x - 2 >= 0 && fields[x - 2][y].Image == settingsWindow.HerbivoreImage)
                    changeStateOfField(x - 2, y, FieldState.Attack);
            }
            // Down
            if (x + 1 < BOARD_HIGHT && fields[x + 1][y].Image == settingsWindow.EmptyImage)
            {
                changeStateOfField(x + 1, y, FieldState.Move);
                if (isPredator == true && x + 2 < BOARD_HIGHT && fields[x + 2][y].Image == settingsWindow.HerbivoreImage)
                    changeStateOfField(x + 2, y, FieldState.Attack);
            }
        }

        public void clearColorFieldsToMove()
        {
            while (activeFields.Count > 0)
            {
                Point p = activeFields.Pop();
                fields[p.X][p.Y].Active = FieldState.Normal;
            }
        }

        public List<Point> predatorsPosition()
        {
            List<Point> predatorsPosition = new List<Point>(); ;
            for (int i = 0; i < fields.Count; i++)
                for (int j = 0; j < fields[i].Count; j++)
                    if (fields[i][j].Image == SettingsWindow.getInstance().PredatorImage)
                        predatorsPosition.Add(new Point(i, j));
            return predatorsPosition;
        }

        public bool predatorCanMove()
        {
            for (int i = 0; i < fields.Count; i++)
                for (int j = 0; j < fields[i].Count; j++)
                    if (fields[i][j].Image == settingsWindow.PredatorImage)
                        if (0 < i && fields[i - 1][j].Image == settingsWindow.EmptyImage ||
                            i + 1 < BOARD_HIGHT && fields[i + 1][j].Image == settingsWindow.EmptyImage ||
                            0 < j && fields[i][j - 1].Image == settingsWindow.EmptyImage ||
                            j + 1 < BOARD_WIDTH && fields[i][j + 1].Image == settingsWindow.EmptyImage)
                            return true;
            return false;
        }

        private void move(int fromX, int fromY, int ToX, int ToY, bool predatorRound)
        {
            clearColorFieldsToMove();
            fields[fromX][fromY].Image = settingsWindow.EmptyImage;
            fields[fromX][fromY].fieldType = FieldType.Empty;
            fields[ToX][ToY].Image = predatorRound ? settingsWindow.PredatorImage : settingsWindow.HerbivoreImage;
            fields[ToX][ToY].fieldType = predatorRound ? FieldType.Predator : FieldType.Herbivore;
            activeAnimal.X = -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="predatorRound"></param>
        /// <returns>true - wykonal ruch</returns>
        public bool action(int x, int y, bool predatorRound)
        {
            if (x < 0 || x >= BOARD_HIGHT || y < 0 || y >= BOARD_WIDTH)
                return false;
            // Przesuniecie
            if (activeAnimal.X != -1 && fields[x][y].Active != FieldState.Normal && (activeAnimal.X != x || activeAnimal.Y != y))
            {
                move(activeAnimal.X, activeAnimal.Y, x, y, predatorRound);
                return true;
            }

            if (fields[x][y].Image == settingsWindow.PredatorImage && predatorRound)
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
            else if (fields[x][y].Image == settingsWindow.HerbivoreImage && predatorRound == false)
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
                case 0:
                    fields[x][y].Image = settingsWindow.EmptyImage;
                    break;
                case 1:
                    fields[x][y].Image = settingsWindow.PredatorImage;
                    break;
                case 2:
                    fields[x][y].Image = settingsWindow.HerbivoreImage;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
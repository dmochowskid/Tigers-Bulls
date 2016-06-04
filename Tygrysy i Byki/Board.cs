using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Tygrysy_i_Byki
{
    class Board
    {
        public Board()
        {
            fields = new ObservableCollection<ObservableCollection<MyImage>>();
            predatorImage = new MyImage();
            herbivoreImage = new MyImage();
            emptyImage = new MyImage();

            predatorImage.Image = new BitmapImage(new Uri("obrazki/tiger.png", UriKind.Relative));
            herbivoreImage.Image = new BitmapImage(new Uri("obrazki/bull.png", UriKind.Relative));
            emptyImage.Image = new BitmapImage(new Uri("obrazki/emptyImage.png", UriKind.Relative));
            
        }

        private const int BOARD_WIDTH = 4;
        private const int BOARD_HIGHT = 5;

        public ObservableCollection<ObservableCollection<MyImage>> fields { get; private set; }
        private MyImage predatorImage;
        private MyImage herbivoreImage;
        private MyImage emptyImage;

        public void resetBoard()
        {
            // Pierwsze dwa rzedy (Oponent)
            for (int i = 0; i < 2; i++)
            {
                fields.Add(new ObservableCollection<MyImage>());
                for (int j = 0; j < BOARD_WIDTH; j++)
                    fields[i].Add(herbivoreImage);
            }

            // Puste rzedy
            for (int i = 2; i < BOARD_HIGHT - 1; i++)
            {
                fields.Add(new ObservableCollection<MyImage>());
                for (int j = 0; j < BOARD_WIDTH; j++)
                    fields[i].Add(emptyImage);
            }

            // Ostatni rzad
            fields.Add(new ObservableCollection<MyImage>());
            for (int j = 0; j < BOARD_WIDTH / 2 - 1; j++)
                fields[BOARD_HIGHT - 1].Add(emptyImage);
            fields[BOARD_HIGHT - 1].Add(predatorImage);
            fields[BOARD_HIGHT - 1].Add(predatorImage);
            for (int j = BOARD_WIDTH / 2 + 1; j < BOARD_WIDTH; j++)
                fields[BOARD_HIGHT - 1].Add(emptyImage);
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
                case 0: fields[x][y] = emptyImage;
                    break;
                case 1: fields[x][y] = predatorImage;
                    break;
                case 2: fields[x][y] = herbivoreImage;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tygrysy_i_Byki
{
    class Computer
    {
        public Computer(Board board)
        {
            this.board = board;
        }

        private Board board;
        Random rand = new Random();

        public void move()
        {
            if (attackMove() == true)
                return;
            if (blockMove() == true)
                return;
            randMove();
        }

        private bool attackMove()
        {
            // 'Klikniecie' na tygrysy
            for (int i = 0; i < board.fields.Count; i++)
                for (int j = 0; j < board.fields[i].Count; j++)
                    if (board.fields[i][j].Image == SettingsWindow.getInstance().PredatorImage)
                        board.colorFieldsToMove(i, j, true);

            // Dodanie zagrozonych
            List<Point> dangerousHerbivore = new List<Point>();
            for (int i = 0; i < board.fields.Count; i++)
                for (int j = 0; j < board.fields[i].Count; j++)
                    if (board.fields[i][j].Active == FieldState.Attack)
                        dangerousHerbivore.Add(new Point(i, j));
            board.clearColorFieldsToMove();

            // Przesuniecie zagrozonych
            foreach (var i in dangerousHerbivore)
            {
                for (int j = 0; j < 4; j++)
                    if (randWayMove(i.X, i.Y))
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Nie musi sie udać (Nawet jezeli jest taka mozliwosc z danego pola)
        /// (toX, toY) - Punkt w ktorym powinni kierowac sie zwierzeta
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns></returns>
        private bool randWayMove(int fromX, int fromY, int toX = -1, int toY = -1)
        {
            board.colorFieldsToMove(fromX, fromY, false);

            int x = rand.Next(3) - 1;
            int y = 0;
            while (x == 0 && y == 0)
                y = rand.Next(3) - 1;
            if (toX != -1 && (Math.Abs(toX - fromX - x) + Math.Abs(toY - fromY - y) >= 2))
                return false;
            if (board.action(fromX + x, fromY + y, false) == true)
            {
                board.clearColorFieldsToMove();
                board.activeAnimal.X = -1;
                return true;
            }

            board.clearColorFieldsToMove();
            return false;
        }

        /// <summary>
        /// Poruszenie byka oddalonego o 2 od tygrysa w stronę tygrysa
        /// </summary>
        /// <returns></returns>
        private bool blockMove()
        {
            List<Point> predatorsPosition = board.predatorsPosition();
            List<Point> herbivoresPosition = board.herbivoresPosition();

            foreach(var i in herbivoresPosition)
                foreach(var j in predatorsPosition)
                    if(Math.Abs(i.X - j.X) + Math.Abs(i.Y - j.Y) == 2) // oddalone o 2
                        if (randWayMove(i.X, i.Y, j.X, j.Y) == true)
                            return true;

            return false;
        }

        private void randMove()
        {
            int chosenX = -1;
            int chosenY = -1;

            while (true)
            {
                // Znalezienie zwierzaka
                while (true)
                {
                    chosenX = rand.Next(Board.BOARD_HIGHT);
                    chosenY = rand.Next(Board.BOARD_WIDTH);
                    if (board.fields[chosenX][chosenY].Image == SettingsWindow.getInstance().HerbivoreImage)
                        break;
                }

                if (randWayMove(chosenX, chosenY) == true)
                    return;
            }
        }
    }
}

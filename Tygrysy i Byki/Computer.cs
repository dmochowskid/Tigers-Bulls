using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tygrysy_i_Byki
{
    class Computer
    {
        private Board board;

        public Computer(Board board)
        {
            this.board = board;
        }

        public void move()
        {
            Random rand = new Random();
            int chosenX = -1;
            int chosenY = -1;

            while (true)
            {
                // Znalezienie zwierzaka
                while (true)
                {
                    chosenX = rand.Next(Board.BOARD_HIGHT);
                    chosenY = rand.Next(Board.BOARD_WIDTH);

                    if (board.fields[chosenX][chosenY].Image == SettingsWindow.getInstance().EmptyImage)
                        break;
                }

                board.colorFieldsToMove(chosenX, chosenY, false);

                // Wykonanie ruchu
                int x = rand.Next(3) - 1;
                int y = 0;
                while (x == 0 && y == 0)
                    y = rand.Next(3) - 1;
                if (0 <= chosenX + x && chosenX + x < Board.BOARD_HIGHT &&
                    0 <= chosenY + y && chosenY + y < Board.BOARD_WIDTH &&
                    board.action(chosenX + x, chosenY + y, false) == true)
                {
                    board.activeAnimal.X = -1;
                    return;
                }

                board.clearColorFieldsToMove();
                board.activeAnimal.X = -1;
            }
        }
    }
}

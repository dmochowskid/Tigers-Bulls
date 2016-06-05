﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tygrysy_i_Byki
{
    class Game
    {
        public Game()
        {
            board = new Board();
            settingWindow = SettingsWindow.getInstance();
            resetGame();
        }

        public Board board { get; set; }
        private SettingsWindow settingWindow;
        private bool predatorRound;

        public void resetGame()
        {
            predatorRound = true;
            board.resetBoard();
        }

        private void endGame(bool predatorWins, int moves)
        {
            MessageBox.Show((predatorWins ? "Drapieżniki" : "Roślinożercy") + " wygraly w " + moves + " ruchach", "Gratulacje", MessageBoxButton.OK);
            resetGame();
        }

        public void action(int x, int y)
        {
            if (board.action(x, y, predatorRound) == true)
                predatorRound = !predatorRound;

            if (predatorRound == false && settingWindow.withComputer == true)
                comupterMove();

            ifEndGame();
        }

        private void ifEndGame()
        {
            if (predatorRound == true)
                if (board.predatorCanMove() == false)
                    endGame(false, 0);
           else
                if (board.herbivoreCount() <= 2)
                    endGame(true, 0);
        }

        private void comupterMove()
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

                    if (board.fields[chosenX][chosenY].Image == settingWindow.EmptyImage)
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
                    predatorRound = !predatorRound;
                    return;
                }

                board.clearColorFieldsToMove();
                board.activeAnimal.X = -1;
            }
        }
    }
}

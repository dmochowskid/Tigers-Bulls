using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tygrysy_i_Byki
{
    class Game : INotifyPropertyChanged
    {
        public Game(Image minCurrentPlayer)
        {
            board = new Board();
            settingWindow = SettingsWindow.getInstance();
            this.minCurrentPlayer = minCurrentPlayer;
            computer = new Computer(board);
            resetGame();
        }

        public Board board { get; set; }
        private Image minCurrentPlayer;
        private SettingsWindow settingWindow;
        private Computer computer;
        private bool predatorRound;

        private int counter;
        public int Counter
        {
            get
            {
                return counter / 2;
            }
            set
            {
                counter = value;
                OnPropertyChanged("Counter");
            }
        }

        public void resetGame()
        {
            predatorRound = true;
            minCurrentPlayer.DataContext = settingWindow.PredatorImage;
            Counter = 0;
            board.resetBoard();
        }

        private void endGame(bool predatorWins, int moves)
        {
            MessageBox.Show((predatorWins ? "Drapieżniki" : "Roślinożercy") + " wygraly w " + moves + " ruchach", "Gratulacje", MessageBoxButton.OK);
            resetGame();
        }

        private void changeRound()
        {
            predatorRound = !predatorRound;
            minCurrentPlayer.DataContext = (predatorRound) ? settingWindow.PredatorImage : settingWindow.HerbivoreImage;
            Counter = counter + 1;
        }

        public void action(int x, int y)
        {
            // Gracz
            if (board.action(x, y, predatorRound) == true)
            {
                if (ifEndGame() == true)
                    return;
                changeRound();
            }

            // Komputer
            if (settingWindow.withComputer == true && predatorRound == false)
            {
                computer.move();
                if (ifEndGame() == true)
                    return;
                changeRound();
            }
        }

        private bool ifEndGame()
        {
            if (board.predatorCanMove() == false)
            {
                endGame(false, Counter);
                return true;
            }
            if (board.herbivoreCount() <= 2)
            {
                endGame(true, Counter);
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

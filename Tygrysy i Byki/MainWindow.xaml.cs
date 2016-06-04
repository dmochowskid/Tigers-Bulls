using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Tygrysy_i_Byki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            board = new Board();
            itemControlBoard.ItemsSource = board.fields;

            startGame();
        }

        private Board board;

        private void startGame()
        {
            board.resetBoard();
        }

        private void Field_Click(object sender, RoutedEventArgs e)
        {
            board.setField(0, 0, 0);
        }
    }
}
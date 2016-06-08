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

            game = new Game(iAnimalRound);
            settingsWindow = SettingsWindow.getInstance();

            lPoints.DataContext = game;
            itemControlBoard.ItemsSource = game.board.fields;
        }

        private Game game;
        private SettingsWindow settingsWindow;

        private void Field_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            Field field = clicked.DataContext as Field;
            game.action(field.x, field.y);
        }

        private void MainMenu_NewGame(object sender, RoutedEventArgs e)
        {
            game.resetGame();
        }

        private void MainMenu_Ustawienia(object sender, RoutedEventArgs e)
        {
            settingsWindow.ShowDialog();
            game.board.refreshImages();
            game.refreshMinImage();
        }
        
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gPoints.Width = Math.Min(200, canvas.ActualWidth - Canvas.GetRight(gPoints));
            
            if(canvas.ActualHeight > 40)
                gPoints.Height = Math.Min(150, canvas.ActualHeight - Canvas.GetTop(gPoints));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            settingsWindow.Close();
        }
    }
}
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


            ImageSource imageSource = new BitmapImage(new Uri("obrazki/byk.png", UriKind.Relative));
            image00.Source = imageSource;

            startGame();
        }



        private void startGame()
        {
            MyImage myImage = new MyImage();
            myImage.image = new BitmapImage(new Uri("obrazki/byk.png", UriKind.Relative));
            image00.DataContext = myImage;
            image01.DataContext = myImage;
            image10.DataContext = myImage;
            image11.DataContext = myImage;
            image20.DataContext = myImage;
            image21.DataContext = myImage;
            image30.DataContext = myImage;
            image31.DataContext = myImage;
            image33.DataContext = myImage;
            image32.DataContext = myImage;

            MyImage myImage2 = new MyImage();
            myImage2.image = new BitmapImage(new Uri("obrazki/tygrys.png", UriKind.Relative));
            image14.DataContext = myImage2;
            image24.DataContext = myImage2;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            image30.DataContext = null;
        }
    }

    class MyImage
    {
        public ImageSource image { get; set; }
    }
    
}

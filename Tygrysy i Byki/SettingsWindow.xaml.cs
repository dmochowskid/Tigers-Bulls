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
using System.Windows.Shapes;

namespace Tygrysy_i_Byki
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private SettingsWindow()
        {
            InitializeComponent();

            createAnimalList();

            currentPredatorIndex = 10; // Lion
            currentHerbivoreIndex = 2; // Bull

            PredatorImage = new BitmapImage(new Uri(getFilePath(currentPredatorIndex, true), UriKind.Relative));
            HerbivoreImage = new BitmapImage(new Uri(getFilePath(currentHerbivoreIndex, false), UriKind.Relative));
            EmptyImage = new BitmapImage(new Uri("obrazki/emptyImage.png", UriKind.Relative));

            iHerbivore.DataContext = this;
            iPredator.DataContext = this;

            rbPlayerVsComp.IsChecked = true;

            withComputer = true;
        }

        private List<string> predatorNames;
        private List<string> herbivoreNames;

        private int currentPredatorIndex;
        private int currentHerbivoreIndex;

        private ImageSource predatorImage;
        public ImageSource PredatorImage
        {
            get
            {
                return predatorImage;
            }
            set
            {
                predatorImage = value;
                OnPropertyChanged("PredatorImage");
            }
        }

        private ImageSource herbivoreImage;
        public ImageSource HerbivoreImage
        {
            get
            {
                return herbivoreImage;
            }
            set
            {
                herbivoreImage = value;
                OnPropertyChanged("HerbivoreImage");
            }
        }

        public bool withComputer;

        public ImageSource EmptyImage { get; set; }

        private static SettingsWindow instance;

        public static SettingsWindow getInstance()
        {
            if (instance == null)
                instance = new SettingsWindow();
            return instance;
        }

        private void createAnimalList()
        {
            predatorNames = new List<string>();
            predatorNames.Add("alligator");
            predatorNames.Add("ant");
            predatorNames.Add("bat");
            predatorNames.Add("bear");
            predatorNames.Add("bulldog");
            predatorNames.Add("crocodile");
            predatorNames.Add("dog");
            predatorNames.Add("eagle");
            predatorNames.Add("fox");
            predatorNames.Add("insect");
            predatorNames.Add("lion");
            predatorNames.Add("rhino");
            predatorNames.Add("snake");
            predatorNames.Add("tiger");

            herbivoreNames = new List<string>();
            herbivoreNames.Add("bee");
            herbivoreNames.Add("bird");
            herbivoreNames.Add("bull");
            herbivoreNames.Add("butterfly");
            herbivoreNames.Add("cat");
            herbivoreNames.Add("chicken");
            herbivoreNames.Add("cow");
            herbivoreNames.Add("crab");
            herbivoreNames.Add("deer");
            herbivoreNames.Add("donkey");
            herbivoreNames.Add("duck"); //10
            herbivoreNames.Add("elephant");
            herbivoreNames.Add("frog");
            herbivoreNames.Add("giraffe");
            herbivoreNames.Add("gorilla");
            herbivoreNames.Add("hippo");
            herbivoreNames.Add("horse");
            herbivoreNames.Add("monkey");
            herbivoreNames.Add("moose");
            herbivoreNames.Add("mouse");
            herbivoreNames.Add("owl");
            herbivoreNames.Add("panda");
            herbivoreNames.Add("penguin");
            herbivoreNames.Add("pig");
            herbivoreNames.Add("rabbit");
            herbivoreNames.Add("rooster");
            herbivoreNames.Add("sheep");
            herbivoreNames.Add("turkey");
            herbivoreNames.Add("turtle");
        }

        private string getFilePath(int index, bool isPredator)
        {
            return "obrazki/" + ((isPredator) ? predatorNames[index] : herbivoreNames[index]) + ".png";
        }

        private int nextIndex(int index, bool isPredator)
        {
            return ++index % (isPredator ? predatorNames.Count : herbivoreNames.Count);
        }

        private int prevIndex(int index, bool isPredator)
        {
            return (--index >= 0) ? index : (isPredator ? predatorNames.Count - 1 : herbivoreNames.Count - 1);
        }

        private void bOK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void bHerbivorePrev_Click(object sender, RoutedEventArgs e)
        {
            currentHerbivoreIndex = prevIndex(currentHerbivoreIndex, false);
            HerbivoreImage = new BitmapImage(new Uri(getFilePath(currentHerbivoreIndex, false), UriKind.Relative));
        }

        private void bHerbivoreNext_Click(object sender, RoutedEventArgs e)
        {
            currentHerbivoreIndex = nextIndex(currentHerbivoreIndex, false);
            HerbivoreImage = new BitmapImage(new Uri(getFilePath(currentHerbivoreIndex, false), UriKind.Relative));
        }

        private void bPredatorPrev_Click(object sender, RoutedEventArgs e)
        {
            currentPredatorIndex = prevIndex(currentPredatorIndex, true);
            PredatorImage = new BitmapImage(new Uri(getFilePath(currentPredatorIndex, true), UriKind.Relative));
        }

        private void bPredatorNext_Click(object sender, RoutedEventArgs e)
        { 
            currentPredatorIndex = nextIndex(currentPredatorIndex, true);
            PredatorImage = new BitmapImage(new Uri(getFilePath(currentPredatorIndex, true), UriKind.Relative));
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void rbPlayerVsComp_Checked(object sender, RoutedEventArgs e)
        {
            withComputer = true;
        }

        private void rbPlayerVsPlayer_Checked(object sender, RoutedEventArgs e)
        {
            withComputer = false;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tygrysy_i_Byki
{
    enum FieldState
    {
        Normal,
        Move,
        Attack
    }

    class Field : INotifyPropertyChanged
    {
        private ImageSource image;
        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        
        private FieldState active;
        public FieldState Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                OnPropertyChanged("Active");
            }
        }

        public int x { get; set; }
        public int y { get; set; }

        public Field(ImageSource image, int x, int y)
        {
            this.image = image;
            this.x = x;
            this.y = y;
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

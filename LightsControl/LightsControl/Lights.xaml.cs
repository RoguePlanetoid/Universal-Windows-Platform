using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LightsControl
{
    public sealed partial class Lights : UserControl
    {
        public Lights()
        {
            this.InitializeComponent();
        }

        public class Item : System.ComponentModel.INotifyPropertyChanged
        {
            private bool _isOn;
            private Windows.UI.Color _colour;

            public virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            public bool IsOn
            {
                get { return _isOn; }
                set
                {
                    _isOn = value;
                    OnPropertyChanged("IsOn");
                    OnPropertyChanged("Off");
                }
            }

            public Visibility Off
            {
                get { return IsOn ? Visibility.Collapsed : Visibility.Visible; }
            }

            public Windows.UI.Color Colour
            {
                get { return _colour; }
                set { _colour = value; OnPropertyChanged("Colour"); }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<Item> _items =
                new System.Collections.ObjectModel.ObservableCollection<Item>();

        private Orientation _orientation = Orientation.Vertical;

        public System.Collections.ObjectModel.ObservableCollection<Item> Items
        {
            get { return _items; }
            set { _items = value; Display.ItemsSource = Items; }
        }

        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        private void Display_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
    }
}

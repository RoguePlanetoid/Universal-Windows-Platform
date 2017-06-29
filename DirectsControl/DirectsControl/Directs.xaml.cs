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

namespace DirectsControl
{
    public sealed partial class Directs : UserControl
    {
        public Directs()
        {
            this.InitializeComponent();
        }

        public enum Directions
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        public delegate void DirectionEvent(object sender, Directions direction);
        public event DirectionEvent Direction;

        private void Pad_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void Pad_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint point = e.GetCurrentPoint(Pad);
            bool fire = (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse) ?
                point.Properties.IsLeftButtonPressed : point.IsInContact;
            if (fire)
            {
                Windows.UI.Xaml.Shapes.Path path = ((Windows.UI.Xaml.Shapes.Path)sender);
                if (Direction != null)
                {
                    this.Direction(path, (Directions)Enum.Parse(typeof(Directions), path.Name));
                }
            }
        }
    }
}

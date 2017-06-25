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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightsControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private const int red = 0;
        private const int orange = 1;
        private const int green = 2;

        private async System.Threading.Tasks.Task<bool> Delay(int seconds = 2)
        {
            await System.Threading.Tasks.Task.Delay(seconds * 1000);
            return true;
        }

        private void Display_Loaded(object sender, RoutedEventArgs e)
        {
            Display.Items = new System.Collections.ObjectModel.ObservableCollection<Lights.Item>() {
                new Lights.Item() { Colour = Windows.UI.Colors.Red },
                new Lights.Item() { Colour = Windows.UI.Colors.Orange },
                new Lights.Item() { Colour = Windows.UI.Colors.Green, IsOn = true }
            };
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            Display.Items[red].IsOn = false;
            Display.Items[orange].IsOn = false;
            Display.Items[green].IsOn = true;
            await Delay();
            Display.Items[green].IsOn = false;
            await Delay();
            Display.Items[orange].IsOn = true;
            await Delay();
            Display.Items[orange].IsOn = false;
            await Delay();
            Display.Items[red].IsOn = true;
            await Delay();
            Display.Items[red].IsOn = true;
            await Delay();
            Display.Items[orange].IsOn = true;
            await Delay();
            Display.Items[red].IsOn = false;
            Display.Items[orange].IsOn = false;
            Display.Items[green].IsOn = true;
            await Delay();
        }
    }
}

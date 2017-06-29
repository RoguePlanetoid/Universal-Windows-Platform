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

namespace StackedControl
{
    public sealed partial class Stacked : UserControl
    {
        public Stacked()
        {
            this.InitializeComponent();
        }

        private List<Windows.UI.Color> _palette = new List<Windows.UI.Color>();
        private List<double> _items = new List<double>();

        private List<double> Percentages()
        {
            List<double> results = new List<double>();
            double total = _items.Sum();
            foreach (double item in _items)
            {
                results.Add((item / total) * 100);
            }
            return results.OrderBy(o => o).ToList();
        }

        private Windows.UI.Xaml.Shapes.Rectangle GetRectangle(Windows.UI.Color colour, int column)
        {
            Windows.UI.Xaml.Shapes.Rectangle rectangle = new Windows.UI.Xaml.Shapes.Rectangle()
            {
                Height = 10,
                Stretch = Stretch.UniformToFill,
                Fill = new SolidColorBrush(colour)
            };
            rectangle.SetValue(Grid.ColumnProperty, column);
            return rectangle;
        }

        private void Layout()
        {
            List<double> percentages = Percentages();
            Display.ColumnDefinitions.Clear();
            for (int index = 0; index < percentages.Count(); index++)
            {
                double percentage = percentages[index];
                ColumnDefinition column = new ColumnDefinition()
                {
                    Width = new GridLength(percentage, GridUnitType.Star)
                };
                Display.ColumnDefinitions.Add(column);
                Windows.UI.Color colour = (index < _palette.Count())
                    ? _palette[index] : Windows.UI.Colors.Black;
                Display.Children.Add(GetRectangle(colour, index));
            }
        }

        public List<Windows.UI.Color> Palette
        {
            get { return _palette; }
            set { _palette = value; }
        }

        public List<double> Items
        {
            get { return _items; }
            set { _items = value; Layout(); }
        }
    }
}

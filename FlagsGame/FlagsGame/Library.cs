using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Flag
{
    public enum Designs
    {
        HorizonalStripes = 1,
        VerticalStripes = 2
    };
    public Designs Design { get; set; }
    public string Name { get; set; }
    public string[] Colours { get; set; }
}

public class Library
{
    private const int size = 3;

    private List<Flag> flags = new List<Flag>()
    {
        new Flag() {
            Name = "Armenia",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE1018", "002984", "EF7b21" }
        },
        new Flag()
        {
            Name = "Austria",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE1018", "FFFFFF", "DE1018" }
        },
        new Flag()
        {
            Name = "Belgium",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "000000", "FFEF08", "DE1018" }
        },
        new Flag()
        {
            Name = "Bulgaria",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "FFFFFF", "109452", "DE1018" }
        },
        new Flag()
        {
            Name = "Estonia",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "295AA5", "000000", "FFFFFF" }
        },
        new Flag()
        {
            Name = "France",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "080863", "FFFFFF", "D60818" }
        },
        new Flag() {
            Name = "Gabon",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "109452", "FFEF08", "002984" }
        },
        new Flag()
        {
            Name = "Germany",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "000000", "DE0008", "FFDE08" }
        },
        new Flag()
        {
            Name = "Guinea",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "DE1018", "FFEF08", "109452" }
        },
        new Flag()
        {
            Name = "Hungary",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE0008", "FFFFFF", "087B39" }
        },
        new Flag()
        {
            Name = "Ireland",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "007339", "FFFFFF", "E76300" }
        },
        new Flag()
        {
            Name = "Italy",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "109452", "FFFFFF", "DE1018" }
        },
        new Flag()
        {
            Name = "Lithuania",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "FFEF08", "109452", "DE1018" }
        },
        new Flag()
        {
            Name = "Luxembourg",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE1018", "FFFFFF", "2984B5" }
        },
        new Flag()
        {
            Name = "Mali",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "109452", "FFEF08", "DE1018" }
        },
        new Flag()
        {
            Name = "Netherlands",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE1018", "FFFFFF", "002984" }
        },
        new Flag()
        {
            Name = "Nigeria",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "087B39", "FFFFFF", "087B39" }
        },
        new Flag()
        {
            Name = "Romania",
            Design = Flag.Designs.VerticalStripes,
            Colours = new string[] { "002984", "FFEF08", "DE1018" }
        },
        new Flag()
        {
            Name = "Russia",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "FFFFFF", "0852A5", "DE1018" }
        },
        new Flag()
        {
            Name = "Sierra Leone",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "5AB521", "FFFFFF", "0852A5" }
        },
        new Flag()
        {
            Name = "Yemen",
            Design = Flag.Designs.HorizonalStripes,
            Colours = new string[] { "DE1018", "FFFFFF", "000000" }
        }
    };

    private Random random = new Random((int)DateTime.Now.Ticks);
    private List<int> indexes = new List<int>();
    private List<int> choices = new List<int>();
    private int turns = 0;
    private string country = string.Empty;
    private bool lost = false;

    public static Color ConvertHexToColor(string hex)
    {
        hex = hex.Remove(0, 1);
        byte a = hex.Length == 8 ? Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) : (byte)255;
        byte r = Byte.Parse(hex.Substring(hex.Length - 6, 2), NumberStyles.HexNumber);
        byte g = Byte.Parse(hex.Substring(hex.Length - 4, 2), NumberStyles.HexNumber);
        byte b = Byte.Parse(hex.Substring(hex.Length - 2), NumberStyles.HexNumber);
        return Color.FromArgb(a, r, g, b);
    }

    private List<int> Shuffle(int start, int total)
    {
        return Enumerable.Range(start, total).OrderBy(r => random.Next(start, total)).ToList();
    }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private Viewbox GetFlag(ref string name, int index)
    {
        int pos = indexes[index];
        Flag flag = flags[pos];
        name = flag.Name;
        bool horizontal = flag.Design == Flag.Designs.HorizonalStripes;
        StackPanel panel = new StackPanel()
        {
            Height = 120,
            Width = 120,
            Orientation = horizontal ? Orientation.Vertical : Orientation.Horizontal
        };
        for (int f = 0; f < 3; f++)
        {
            Rectangle rect = new Rectangle()
            {
                Width = horizontal ? 120 : 40,
                Height = horizontal ? 40 : 120,
                Fill = new SolidColorBrush(ConvertHexToColor($"#FF{flag.Colours[f]}"))
            };
            panel.Children.Add(rect);
        }
        Viewbox view = new Viewbox()
        {
            Height = 40,
            Width = 40,
            Child = panel
        };
        return view;
    }

    private void SetButton(ref Grid grid, string name, bool show)
    {
        Button button = (Button)grid.FindName(name);
        button.Opacity = show ? 1 : 0;
    }

    private void Add(Grid grid, TextBlock text, int row, int column, int index)
    {
        string name = string.Empty;
        Viewbox view = GetFlag(ref name, index);
        Button button = new Button()
        {
            Name = name,
            Height = 75,
            Width = 75,
            Content = view
        };
        button.Click += (object sender, RoutedEventArgs e) =>
        {
            if (!lost)
            {
                string current = ((Button)sender).Name;
                if (country == current)
                {
                    SetButton(ref grid, current, false);
                    if (turns < 9)
                    {
                        Choose(ref grid, ref text);
                    }
                    else
                    {
                        text.Text = string.Empty;
                        Show("You Won!", "Flags Game");
                    }
                }
                else
                {
                    lost = true;
                }
            }
            if (lost)
            {
                Show("Game Over!", "Flags Game");
            }
        };
        button.SetValue(Grid.ColumnProperty, column);
        button.SetValue(Grid.RowProperty, row);
        grid.Children.Add(button);
    }

    private void Layout(ref Grid grid, ref TextBlock text)
    {
        text.Text = string.Empty;
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        // Setup Grid
        for (int layout = 0; layout < size; layout++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        // Setup Board
        int index = 0;
        for (int column = 0; column < size; column++)
        {
            for (int row = 0; row < size; row++)
            {
                Add(grid, text, row, column, index);
                index++;
            }
        }
    }

    private void Choose(ref Grid grid, ref TextBlock text)
    {
        int choice = choices[turns];
        int index = indexes[choice];
        country = flags[index].Name;
        text.Text = country;
        turns++;
    }

    public void New(ref Grid grid, ref TextBlock text)
    {
        lost = false;
        turns = 0;
        text.Text = string.Empty;
        indexes = Shuffle(0, flags.Count);
        choices = Shuffle(0, 9);
        Layout(ref grid, ref text);
        Choose(ref grid, ref text);
    }
}

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

namespace MatrixControl
{
    public sealed partial class Matrix : UserControl
    {
        public Matrix()
        {
            this.InitializeComponent();
        }

        private readonly byte[][] table =
        {
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 0
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,0,0,1,1,0,0,0,
            0,1,1,1,1,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,0,0,0,0,0
            }, // 1 
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 2
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 3
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,0,0,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 4
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 5
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 6
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 7
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 8
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,1,1,1,1,1,1,0,
            0,1,1,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,1,1,0,
            0,1,1,1,1,1,1,0,
            0,0,0,0,0,0,0,0
            }, // 9
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0
            }, // None
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,0,0,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,1,1,0,0,0,
            0,0,0,0,0,0,0,0
            }, // Colon
            new byte[] {
            0,0,0,0,0,0,0,0,
            0,0,0,0,0,1,1,0,
            0,0,0,0,1,1,0,0,
            0,0,0,1,1,0,0,0,
            0,0,1,1,0,0,0,0,
            0,1,1,0,0,0,0,0,
            0,0,0,0,0,0,0,0
            } // Slash
        };
        private readonly List<char> symbols =
            new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', ':', '/' };
        private const int columns = 8;
        private const int rows = 7;
        private const int size = 10;
        private byte[,] board = null;
        private int count = 0;
        private string _output = string.Empty;

        private byte[,] AsTwoDimensionalArray(byte[] input, int height, int width)
        {
            byte[,] output = new byte[height, width];
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    output[r, c] = input[r * width + c];
                }
            }
            return output;
        }

        private int SetBoard(char[] characters)
        {
            int length = characters.Count();
            board = new byte[rows, (columns * length)];
            int offset = 0;
            foreach (char character in characters)
            {
                int index = symbols.IndexOf(character);
                byte[,] symbol = AsTwoDimensionalArray(table[index], rows, columns);
                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        board[row, column + offset] = symbol[row, column];
                    }
                }
                offset += columns;
            }
            return length;
        }

        private Windows.UI.Xaml.Shapes.Rectangle GetPixel(int row, int column)
        {
            Windows.UI.Xaml.Shapes.Rectangle rect = 
                new Windows.UI.Xaml.Shapes.Rectangle()
            {
                Height = size,
                Width = size,
                Fill = Foreground,
                RadiusX = 2,
                RadiusY = 2,
                Opacity = 0,
                Margin = new Thickness(2)
            };
            rect.SetValue(Grid.ColumnProperty, column);
            rect.SetValue(Grid.RowProperty, row);
            return rect;
        }

        private void Setup()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    Windows.UI.Xaml.Shapes.Rectangle rect = 
                        Display.Children.Cast<Windows.UI.Xaml.Shapes.Rectangle>()
                        .FirstOrDefault(
                        f => Grid.GetRow(f) == row 
                        && Grid.GetColumn(f) == column);
                    rect.Opacity = board[row, column];
                }
            }
        }

        private void Layout(int length)
        {
            int total = (columns * length);
            Display.Children.Clear();
            Display.RowDefinitions.Clear();
            Display.ColumnDefinitions.Clear();
            // Grid
            for (int row = 0; row < rows; row++)
            {
                Display.RowDefinitions.Add(new RowDefinition());
            }
            for (int column = 0; column < total; column++)
            {
                Display.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Layout
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < total; column++)
                {
                    Display.Children.Add(GetPixel(row, column));
                }
            }
            Display.Height = Display.RowDefinitions.Count() * (size + 10);
            Display.Width = (Display.ColumnDefinitions.Count() * (size + 20)) / 2;
        }

        private void SetOutput(string content)
        {
            char[] characters = content.ToCharArray();
            int length = SetBoard(characters);
            if (characters.Length != count)
            {
                Layout(characters.Length);
            }
            Setup();
        }

        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    SetOutput(_output);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Library
{
    private const int size = 4;
    private const int on = 1;
    private const int off = 0;

    private readonly Color lightOn = Colors.White;
    private readonly Color lightOff = Colors.Black;

    private int[,] board = new int[size, size];
    private DispatcherTimer timer = null;
    private Random random = new Random((int)DateTime.Now.Ticks);
    private List<int> positions = new List<int>();
    private int counter = 0;
    private int turns = 0;
    private int hits = 0;
    private int wait = 0;
    private bool waiting = false;
    private bool lost = false;

    private List<int> Shuffle(int start, int end, int total)
    {
        return Enumerable.Range(start, total).OrderBy(r => random.Next(start, end)).ToList();
    }

    private Rectangle Get(Color foreground)
    {
        Rectangle rect = new Rectangle()
        {
            Height = 50,
            Width = 50,
            Fill = new SolidColorBrush(foreground)
        };
        return rect;
    }

    private void Set(ref Grid grid, int row, int column)
    {
        Button button = (Button)grid.Children.Single(
                       w => Grid.GetRow((Button)w) == row
                       && Grid.GetColumn((Button)w) == column);
        button.Content = Get(board[row, column] == on ? lightOn : lightOff);
    }

    private void Add(Grid grid, TextBlock text, int row, int column, int index)
    {
        string name = string.Empty;
        Button button = new Button()
        {
            Height = 75,
            Width = 75,
            Content = Get(lightOff)
        };
        button.Click += (object sender, RoutedEventArgs e) =>
        {
            if (!lost)
            {
                button = (Button)sender;
                row = (int)button.GetValue(Grid.RowProperty);
                column = (int)button.GetValue(Grid.ColumnProperty);
                if (board[row, column] == on)
                {
                    board[row, column] = off;
                    Set(ref grid, row, column);
                    hits++;
                }
                else
                {
                    lost = true;
                }
            }
            else
            {
                text.Text = $"Game Over in {turns} Turns!";
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

    private void Reset(Grid grid)
    {
        for (int column = 0; column < size; column++)
        {
            for (int row = 0; row < size; row++)
            {
                board[row, column] = off;
                Set(ref grid, row, column);
            }
        }
        hits = 0;
        counter = 0;
        wait = Shuffle(3, 7, 1).First();
        waiting = true;
    }

    private void Choose(Grid grid)
    {
        int row = 0;
        int column = 0;
        positions = Shuffle(0, board.Length, board.Length);
        for (int i = 0; i < size; i++)
        {
            column = positions[i] % size;
            switch (positions[i])
            {
                case int n when n < 4:
                    row = 0;
                    break;
                case int n when n >= 4 && n < 8:
                    row = 1;
                    break;
                case int n when n >= 8 && n < 12:
                    row = 2;
                    break;
                case int n when n >= 12 && n <= 15:
                    row = 3;
                    break;
            }
            board[row, column] = on;
            Set(ref grid, row, column);
        }
        counter = 0;
        wait = 0;
        waiting = false;
    }

    private void Timer(Grid grid, TextBlock text)
    {
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
        timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += (object sender, object e) =>
        {
            if (!lost)
            {
                int countdown = 0;
                if (waiting)
                {
                    countdown = (wait - counter) + 1;
                    text.Text = $"Waiting {countdown}";
                    if (countdown == 0)
                    {
                        text.Text = string.Empty;
                        Choose(grid);
                    }
                }
                else
                {
                    countdown = (size - counter) + 1;
                    text.Text = $"Solve In {countdown}";
                    if (countdown == 0)
                    {
                        if (hits == size)
                        {
                            turns++;
                            text.Text = string.Empty;
                            Reset(grid);
                        }
                        else
                        {
                            lost = true;
                            text.Text = $"Game Over in {turns} Turns!";
                        }
                    }
                }
                counter++;
            }
        };
        timer.Start();
    }

    public void New(ref Grid grid, ref TextBlock text)
    {
        lost = false;
        waiting = true;
        wait = 3;
        turns = 1;
        Layout(ref grid, ref text);
        Timer(grid, text);
    }
}

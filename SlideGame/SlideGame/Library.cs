using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Piece : Grid
{
    public Piece(int index)
    {
        this.Background = new SolidColorBrush(Colors.Black);
        Rectangle rect = new Rectangle()
        {
            Stroke = new SolidColorBrush(Colors.Gray),
        };
        TextBlock text = new TextBlock()
        {
            FontSize = 30,
            Text = index.ToString(),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = new SolidColorBrush(Colors.White)
        };
        this.Children.Add(rect);
        this.Children.Add(text);
    }

    public int Row { get; set; }
    public int Column { get; set; }
}

public class Library
{
    private const int size = 4;
    private int moves = 0;
    private int[,] board = new int[size, size];
    private List<int> values;
    private Random random = new Random((int)DateTime.Now.Ticks);

    private List<int> Shuffle(int start, int total)
    {
        return Enumerable.Range(start, total).OrderBy(r => random.Next(start, total)).ToList();
    }

    public void Show(string content, string title)
    {
        IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
    }

    private bool Valid(int row, int column)
    {
        if (row < 0 || column < 0 || row > 3 || column > 3)
        {
            return false;
        }
        return (board[row, column] == 0);
    }

    private bool Check()
    {
        int previous = board[0, 0];
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                if (board[row, column] > previous)
                {
                    return false;
                }
                previous = board[row, column];
            }
        }
        return true;
    }

    private void Move(Canvas canvas, Piece piece, int row, int column)
    {
        moves++;
        board[row, column] = board[piece.Row, piece.Column];
        board[piece.Row, piece.Column] = 0;
        piece.Row = row;
        piece.Column = column;
        Layout(canvas);
        if (Check())
        {
            Show($"Correct In {moves} Moves", "Slide Game");
        }
    }

    private void Layout(Canvas canvas)
    {
        canvas.Children.Clear();
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                if (board[row, column] > 0)
                {
                    int index = board[row, column];
                    Piece piece = new Piece(index)
                    {
                        Width = canvas.Width / size,
                        Height = canvas.Height / size,
                        Row = row,
                        Column = column
                    };
                    Canvas.SetTop(piece, row * (canvas.Width / size));
                    Canvas.SetLeft(piece, column * (canvas.Width / size));
                    piece.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
                    {
                        piece = (Piece)sender;
                        if (Valid(piece.Row - 1, piece.Column))
                        {
                            Move(canvas, piece, piece.Row - 1, piece.Column);
                        }
                        else if (Valid(piece.Row, piece.Column + 1))
                        {
                            Move(canvas, piece, piece.Row, piece.Column + 1);
                        }
                        else if (Valid(piece.Row + 1, piece.Column))
                        {
                            Move(canvas, piece, piece.Row + 1, piece.Column);
                        }
                        else if (Valid(piece.Row, piece.Column - 1))
                        {
                            Move(canvas, piece, piece.Row, piece.Column - 1);
                        }
                    };
                    canvas.Children.Add(piece);
                }
            }
        }
    }

    public void New(ref Canvas canvas)
    {
        int index = 1;
        values = Shuffle(1, board.Length);
        values.Insert(0, 0);
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                board[row, column] = values[index++];
                if (index == size * size) index = 0;
            }
        }
        Layout(canvas);
    }
}

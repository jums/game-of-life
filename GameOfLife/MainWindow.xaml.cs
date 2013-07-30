using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jums.GameOfLife.CoreCSharp;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int SquareSize = 3;
        protected Game Game { get; set; }
        private Dictionary<string, Rectangle> rectangles;

        public MainWindow()
        {
            Game = new Game();
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitiateGameView();
            base.OnInitialized(e);
        }
		
		protected override void OnRender(DrawingContext drawingContext)
		{
		}

        private void InitiateGameView()
        {
            rectangles = new Dictionary<string, Rectangle>();

            for (int i = 0; i < Game.Width; i++)
            {
                int x = i * SquareSize;

                for (int j = 0; j < Game.Height; j++)
                {
                    int y = j * SquareSize;
                    var rectangle = AddRectangleToView(x, y);
                    StoreRectangleToLookup(i, j, rectangle);
                }
            }
        }

        private void StoreRectangleToLookup(int i, int j, Rectangle rectangle)
        {
            var key = GetRectangleKey(i, j);
            rectangles[key] = rectangle;
        }

        private static string GetRectangleKey(int i, int j)
        {
            return string.Format("{0},{1}", i, j);
        }

        private Rectangle AddRectangleToView(int x, int y)
        {
            var rectangle = new Rectangle {Width = SquareSize, Height = SquareSize, Fill = WorldCanvas.Background};
            WorldCanvas.Children.Add(rectangle);
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
            return rectangle;
        }

        private void DrawGame()
        {
            var state = Game.State;

            for (int i = 0; i < Game.Width; i++)
            {
                for (int j = 0; j < Game.Height; j++)
                {
                    Rectangle rectangle = GetRectangle(i, j);
                    Brush color = state[i, j] ? Brushes.GreenYellow : WorldCanvas.Background;
                    rectangle.Fill = color;
                }
            }
        }

        private Rectangle GetRectangle(int i, int j)
        {
            string key = GetRectangleKey(i, j);
            return rectangles[key];
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
            DrawGame();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Game.Next();
            DrawGame();
        }
    }
}

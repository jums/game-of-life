using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Jums.GameOfLife.CoreCSharp;

namespace Jums.GameOfLife.WindowsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Brush LifeBrush = Brushes.GreenYellow;
        private readonly Brush DeadBrush = Brushes.White;
        private const int SquareSize = 3;
        private const int SlowInterval = 400;
        private const int FastInterval = 100;
        private Game game;
        private Player player;
        private Dictionary<string, Rectangle> rectangles;
        private int? lastSeed;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            NewGame();
            PopulateWorld();
            InitiateGameView();
            RenderGame();
            base.OnInitialized(e);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
            NewGame();
            PopulateWorld();
            RenderGame();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
            RenderGame();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            Draw.IsEnabled = false;
            DrawDone.Visibility = Visibility.Visible;
            StartDrawing();
            // TODO
        }

        private void DrawDone_Click(object sender, RoutedEventArgs e)
        {
            Draw.IsEnabled = true;
            DrawDone.Visibility = Visibility.Hidden;
            StopDrawing();
            // TODO
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
            PopulateWorld(lastSeed);
            RenderGame();
        }

        private void Populate_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
            PopulateWorld();
            RenderGame();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            game.Next();
            RenderGame();
        }

        private void PlaySlow_Click(object sender, RoutedEventArgs e)
        {
            StartPlaying(TimeSpan.FromMilliseconds(SlowInterval));
        }

        private void PlayFast_Click(object sender, RoutedEventArgs e)
        {
            StartPlaying(TimeSpan.FromMilliseconds(FastInterval));
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopPlaying();
        }

        private void NewGame()
        {
            Settings settings = GetSettings();
            game = new Game(settings);
        }

        private Settings GetSettings(bool? wrapped = null)
        {
            return new Settings
            {
                Width = 250,
                Height = 150,
                Wrapped = wrapped ?? WrapWorld.IsChecked ?? false,
                FillRate = FillRate.Value
            };
        }

        private void PopulateWorld(int? seed = null)
        {
            lastSeed = game.Populate(seed);
        }

        private void RenderGame()
        {
            var state = game.State;

            for (int x = 0; x < game.Width; x++)
            {
                for (int y = 0; y < game.Height; y++)
                {
                    Rectangle rectangle = GetRectangle(x, y);
                    Brush color = state[x, y] ? LifeBrush : DeadBrush;
                    rectangle.Fill = color;
                }
            }
        }

        private void StartDrawing()
        {
            foreach (var rectangle in rectangles.Values)
            {
                rectangle.Cursor = Cursors.Pen;
                rectangle.MouseEnter += RectangleOnMouseEnter;
            }
        }

        private void StopDrawing()
        {
            foreach (var rectangle in rectangles.Values)
            {
                rectangle.Cursor = null;
                rectangle.MouseEnter -= RectangleOnMouseEnter;
            }
        }

        private Rectangle GetRectangle(int i, int j)
        {
            string key = GetRectangleKey(i, j);
            return rectangles[key];
        }

        private void StartPlaying(TimeSpan interval)
        {
            StopPlaying();
            player = new Player(RenderGame, game.Next);
            player.Play(interval);
        }

        private void StopPlaying()
        {
            if (player != null)
            {
                player.Stop();
            }
        }

        private void InitiateGameView()
        {
            WorldCanvas.Children.Clear();
            WorldCanvas.Width = game.Width*SquareSize;
            WorldCanvas.Height = game.Height*SquareSize;

            rectangles = new Dictionary<string, Rectangle>();

            for (int i = 0; i < game.Width; i++)
            {
                int x = i*SquareSize;

                for (int j = 0; j < game.Height; j++)
                {
                    int y = j*SquareSize;
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

        private void RectangleOnMouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = LifeBrush;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = DeadBrush;
            }
        }
    }
}
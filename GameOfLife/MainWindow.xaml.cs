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
        private readonly Brush lifeBrush = Brushes.GreenYellow;
        private readonly Brush deadBrush = Brushes.White;
        private const int SquareSize = 3;
        private const int SlowInterval = 400;
        private const int FastInterval = 100;
        private Game game;
        private Player player;
        private Dictionary<string, Rectangle> rectangles;
        private Dictionary<Rectangle, Tuple<int, int>> rectanglesToCoordinates;
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
            game.Clear();
            RenderGame();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            Draw.IsEnabled = false;
            DrawDone.Visibility = Visibility.Visible;
            SetNonDrawControlsEnabled(false);
            StartDrawing();
        }

        private void DrawDone_Click(object sender, RoutedEventArgs e)
        {
            Draw.IsEnabled = true;
            DrawDone.Visibility = Visibility.Hidden;
            SetNonDrawControlsEnabled(true);
            StopDrawing();
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
            Stop.IsEnabled = true;
            SetNonPlayControlsEnabled(false);
            StartPlaying(TimeSpan.FromMilliseconds(SlowInterval));
        }

        private void PlayFast_Click(object sender, RoutedEventArgs e)
        {
            Stop.IsEnabled = true;
            SetNonPlayControlsEnabled(false);
            StartPlaying(TimeSpan.FromMilliseconds(FastInterval));
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Stop.IsEnabled = false;
            SetNonPlayControlsEnabled(true);
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
                    Brush color = state[x, y] ? lifeBrush : deadBrush;
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

        private void SetNonDrawControlsEnabled(bool enabled)
        {
            var buttons = new List<Button>
            {
                Create, Stop, PlayFast, PlaySlow, Next, Reset, Populate
            };

            buttons.ForEach(b => b.IsEnabled = enabled);
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

        private void SetNonPlayControlsEnabled(bool enabled)
        {
            var buttons = new List<Button>
            {
                Create, Stop, Next, Reset, Populate, Draw, Clear
            };

            buttons.ForEach(b => b.IsEnabled = enabled);
        }

        private void InitiateGameView()
        {
            WorldCanvas.Children.Clear();
            WorldCanvas.Width = game.Width*SquareSize;
            WorldCanvas.Height = game.Height*SquareSize;

            rectangles = new Dictionary<string, Rectangle>();
            rectanglesToCoordinates = new Dictionary<Rectangle, Tuple<int, int>>();

            for (int x = 0; x < game.Width; x++)
            {
                int uiX = x*SquareSize;

                for (int y = 0; y < game.Height; y++)
                {
                    int uiY = y*SquareSize;
                    var rectangle = AddRectangleToView(uiX, uiY);
                    StoreRectangleToLookups(x, y, rectangle);
                }
            }
        }

        private void StoreRectangleToLookups(int x, int y, Rectangle rectangle)
        {
            var key = GetRectangleKey(x, y);
            rectangles[key] = rectangle;
            rectanglesToCoordinates.Add(rectangle, new Tuple<int, int>(x, y));
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
            Tuple<int, int> coordinates = rectanglesToCoordinates[rectangle];

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = lifeBrush;
                game.CreateLifeAt(coordinates.Item1, coordinates.Item2);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                rectangle.Fill = deadBrush;
                game.KillLifeAt(coordinates.Item1, coordinates.Item2);
            }
        }
    }
}
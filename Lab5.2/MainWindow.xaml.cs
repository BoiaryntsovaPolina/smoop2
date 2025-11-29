using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab5._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private GameManager game;
        private int size = 4;

        // Шлях для збереження best score
        private readonly string appDataFolder = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Game2048");
        private readonly string bestFilePath;

        // Блок
         private List<Border> tileBorders = new List<Border>();
         private List<TextBlock> tileTextBlocks = new List<TextBlock>(); 

        public MainWindow()
        {
            InitializeComponent();

            bestFilePath = System.IO.Path.Combine(appDataFolder, "bestscore.txt");

            game = new GameManager(size);
            game.BoardChanged += Game_BoardChanged;
            game.GameOver += Game_GameOver;

            SetupBoardGrid();
            RenderBoard();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBestScore();
            AdjustBoardBorderSize();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustBoardBorderSize();
        }

        private void AdjustBoardBorderSize()
        {
            double availableWidth = this.ActualWidth - 40;
            double availableHeight = this.ActualHeight - 160;

            if (availableWidth <= 0 || availableHeight <= 0) return;

            double sizeToUse = Math.Min(availableWidth, availableHeight);

            double minSize = 240;
            double maxSize = 1000;

            sizeToUse = Math.Max(minSize, Math.Min(maxSize, sizeToUse));

            BoardBorder.Width = sizeToUse;
            BoardBorder.Height = sizeToUse;
        }

        private void SetupBoardGrid()
        {
            BoardGrid.Children.Clear();
            tileBorders.Clear();
            tileTextBlocks.Clear();

            for (int i = 0; i < size * size; i++)
            {
                var border = new Border
                {
                    Style = (Style)FindResource("TileBorderStyle"),
                    Background = (Brush)FindResource("EmptyTileBrush"),
                    Margin = new Thickness(6)
                };

                var viewbox = new Viewbox { Stretch = Stretch.Uniform };

                var text = new TextBlock
                {
                    Text = "",
                    FontFamily = (FontFamily)FindResource("MainFont"),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                };
                
                viewbox.Child = text;
                border.Child = viewbox;

                BoardGrid.Children.Add(border);

                tileBorders.Add(border);
                tileTextBlocks.Add(text);
            }
        }

        private void Game_BoardChanged(object? sender, System.EventArgs e)
        {
            Dispatcher.Invoke(() => RenderBoard());
        }

        private void Game_GameOver(object? sender, System.EventArgs e)
        {
            Dispatcher.Invoke(() => 
            {
                MessageBox.Show($"Game over!\nScore: {game.Score}", "2048");
                TryUpdateBestScore(game.Score);
            });
            
        }

        private void RenderBoard()
        {
            ScoreText.Text = game.Score.ToString();

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    int idx = r * size + c;
                    int value = game.Board[r,c];

                    var border = tileBorders[idx];
                    var text = tileTextBlocks[idx];



                    if (value==0)
                    {
                        text.Text = "";
                        border.Background = (Brush)FindResource("EmptyTileBrush");
                        text.Foreground = Brushes.Transparent;
                    }
                    else
                    {
                        text.Text = value.ToString();
                        border.Background = GetBrushForValue(value);
                        text.Foreground = value <= 4 ? Brushes.Black : Brushes.White;
                    }
                }
            }

            TryUpdateBestScore(game.Score);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            game.NewGame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;
            switch (e.Key)
            {
                case Key.Left:
                    moved = game.Move(MoveDirection.Left);
                    break;
                case Key.Right:
                    moved = game.Move(MoveDirection.Right);
                    break;
                case Key.Up:
                    moved = game.Move(MoveDirection.Up);
                    break;
                case Key.Down:
                    moved = game.Move(MoveDirection.Down);
                    break;
            }

            if (moved) { }
        }

        // ---------- Best score: load/save/reset ----------
        private void LoadBestScore()
        {
            try
            {
                if (File.Exists(bestFilePath))
                {
                    var txt = File.ReadAllText(bestFilePath);
                    if (int.TryParse(txt, out int best))
                    {
                        BestText.Text = best.ToString();
                    }
                    else
                    {
                        BestText.Text = "0";
                    }
                }
                else
                {
                    BestText.Text = "0";
                }
            }
            catch
            {
                BestText.Text = "0";
            }
        }

        private void SaveBestScore(int best)
        {
            try
            {
                if (!Directory.Exists(appDataFolder))
                    Directory.CreateDirectory(appDataFolder);

                File.WriteAllText(bestFilePath, best.ToString());
            }
            catch
            {
            }
        }

        private void TryUpdateBestScore(int currentScore)
        {
            if (!int.TryParse(BestText.Text, out int best)) best = 0;
            if (currentScore > best)
            {
                BestText.Text = currentScore.ToString();
                SaveBestScore(currentScore);
            }
        }

        private void ResetBest_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Ви впевнені, що хочете обнулити рейтинг?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    if (File.Exists(bestFilePath))
                        File.Delete(bestFilePath);
                }
                catch
                {
                }
                BestText.Text = "0";
            }
        }

        // ---------- Обробка закриття вікна ----------
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            int currentScore = game?.Score ?? 0;
            var message = $"Ви впевнені, що хочете вийти з гри? Ваш поточний результат: {currentScore}";
            var res = MessageBox.Show(message, "Підтвердження виходу", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                if (int.TryParse(BestText.Text, out int best))
                {
                    SaveBestScore(best);
                }
            }
            catch
            {
            }
        }

        private Brush GetBrushForValue(int v)
        {
            switch(v)
            {
                case 2: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEE4DA"));
                case 4: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDE0C8"));
                case 8: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2B179"));
                case 16: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59563"));
                case 32: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F67C5F"));
                case 64: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F65E3B"));
                case 128: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDCF72"));
                case 256: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDCC61"));
                case 512: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC850"));
                case 1024: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC53F"));
                case 2048: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC22E"));
                default: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3A32"));
            }
        }

    }
}
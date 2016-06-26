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
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // This list describes the Bonus Red pieces of Food on the Canvas
        private List<Point> bonusPoints = new List<Point>();

        // This list describes the body of the snake on the Canvas
        private List<Point> snakePoints = new List<Point>();

        private Brush _snakeColor = Brushes.Green;

        private enum Size
        {
            Thin = 4,
            Normal = 6,
            Thick = 8
        }

        private enum MovingDirection
        {
            Upward = 8,
            Downward = 2,
            ToLeft = 4,
            ToRight = 6
        }

        private TimeSpan _fast = new TimeSpan(9990);
        private TimeSpan _moderate = new TimeSpan(10000);
        private TimeSpan _slow = new TimeSpan(50000);
        private TimeSpan _damnslow = new TimeSpan(500000);

        private Point _startingPoint = new Point(100,100);
        private Point _currentPosition = new Point();

        private int _direction = 0;
        private int _previousDirection = 0;
        private int _headSize = (int) Size.Thick;
        private int _length = 100;
        private int _score = 0;
        private Random _rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = _moderate;
            timer.Start(); 

            KeyDown+= new KeyEventHandler(onButtonKeyDown);
            paintSnake(_startingPoint);
            _currentPosition = _startingPoint;
            for (int n = 0; n < 10; n++)
                paintBonus(n);
        }
        private void paintSnake(Point startingPoint)
        {
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = _snakeColor;
            newEllipse.Width = _headSize;
            newEllipse.Height = _headSize;

            Canvas.SetTop(newEllipse,_currentPosition.Y);
            Canvas.SetLeft(newEllipse,_currentPosition.X);

            int count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(newEllipse);
            snakePoints.Add(_currentPosition);

            if (count > _length)
            {
                paintCanvas.Children.RemoveAt(count-_length+9);
                snakePoints.RemoveAt(count-_length);
            }
        }


        private void paintBonus(int index)
        {
            Point bonusPoint = new Point(_rnd.Next(5, 620), _rnd.Next(5, 380));
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Red;
            newEllipse.Width = _headSize;
            newEllipse.Height = _headSize;

            Canvas.SetTop(newEllipse, bonusPoint.Y);
            Canvas.SetLeft(newEllipse, bonusPoint.X);
            paintCanvas.Children.Insert(index, newEllipse);
            bonusPoints.Insert(index, bonusPoint);
        }


       

        private void timer_Tick(object sender, EventArgs e)
        {
            switch (_direction)
            {
                case (int)MovingDirection.Downward:
                    _currentPosition.Y += 1;
                    paintSnake(_currentPosition);
                    break;
                case (int)MovingDirection.Upward:
                    _currentPosition.Y -= 1;
                    paintSnake(_currentPosition);
                    break;
                case (int)MovingDirection.ToLeft:
                    _currentPosition.X -= 1;
                    paintSnake(_currentPosition);
                    break;
                case (int)MovingDirection.ToRight:
                    _currentPosition.X += 1;
                    paintSnake(_currentPosition);
                    break;
            }

            //if ((_currentPosition.X < 5) || (_currentPosition.X > 620) ||
            //        (_currentPosition.Y < 5) || (_currentPosition.Y > 380))
            //    GameOver();

            int n = 0;
            foreach (Point point in bonusPoints)
            {
                if ((Math.Abs(point.X - _currentPosition.X) < _headSize) &&
                    (Math.Abs(point.Y - _currentPosition.Y) < _headSize)
                    )
                {
                    _length += 10;
                    _score += 10;
                    bonusPoints.RemoveAt(n);
                    paintCanvas.Children.RemoveAt(n);
                    paintBonus(n);
                    break;
                }
                n++;
            }

        }

        private void GameOver()
        {
            MessageBox.Show("You Lose! Your score is " + _score.ToString(), "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            this.Close();
        }
        private void onButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (_previousDirection != (int) MovingDirection.Upward)
                        _direction = (int) MovingDirection.Downward;
                    break;
                case Key.Up:
                    if (_previousDirection != (int)MovingDirection.Downward)
                        _direction = (int)MovingDirection.Upward;
                    break;
                case Key.Left:
                    if (_previousDirection != (int)MovingDirection.ToRight)
                        _direction = (int)MovingDirection.ToLeft;
                    break;
                case Key.Right:
                    if (_previousDirection != (int)MovingDirection.ToLeft)
                        _direction = (int)MovingDirection.ToRight;
                    break;
            }
            _previousDirection = _direction;
        }
    }
}

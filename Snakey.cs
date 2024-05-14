using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimpleSnakeGame
{
    public partial class SnakeGameForm : Form
    {
        private const int TileSize = 20;
        private const int GridSize = 20;
        private Timer gameTimer;
        private List<Point> snake;
        private Point food;
        private Direction snakeDirection;
        private bool isGameOver;

        public SnakeGameForm()
        {
            InitializeComponent();

            snake = new List<Point>();
            snake.Add(new Point(GridSize / 2, GridSize / 2));
            food = GetRandomFoodPosition();
            snakeDirection = Direction.Right;
            isGameOver = false;

            gameTimer = new Timer();
            gameTimer.Interval = 1000
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            this.KeyDown += SnakeGameForm_KeyDown;
        }

        private void SnakeGameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!isGameOver)
            {
                foreach (Point segment in snake)
                {
                    canvas.FillRectangle(Brushes.Green, segment.X * TileSize, segment.Y * TileSize, TileSize, TileSize);
                }

                canvas.FillEllipse(Brushes.Red, food.X * TileSize, food.Y * TileSize, TileSize, TileSize);
            }
            else
            {
                string gameOverText = "Game Over! Press Enter to restart.";
                Font gameOverFont = new Font("Arial", 16, FontStyle.Bold);
                SizeF textSize = canvas.MeasureString(gameOverText, gameOverFont);
                PointF textPosition = new PointF((this.ClientSize.Width - textSize.Width) / 2, (this.ClientSize.Height - textSize.Height) / 2);
                canvas.DrawString(gameOverText, gameOverFont, Brushes.Black, textPosition);
            }
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (!isGameOver)
            {
                MoveSnake();
                CheckCollision();
                Invalidate();
            }
        }

        private void MoveSnake()
        {
            Point newHead = snake.First();
            switch (snakeDirection)
            {
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
            }

            snake.Insert(0, newHead);
            if (newHead != food)
            {
                snake.RemoveAt(snake.Count - 1);
            }
            else
            {
                food = GetRandomFoodPosition();
            }
        }

        private void CheckCollision()
        {
            Point head = snake.First();
            if (head.X < 0 || head.X >= GridSize || head.Y < 0 || head.Y >= GridSize)
            {
                GameOver();
                return;
            }

            if (snake.Any(p => p != head && p == head))
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            isGameOver = true;
            gameTimer.Stop();
            Invalidate();
        }

        private Point GetRandomFoodPosition()
        {
            Random random = new Random();
            return new Point(random.Next(0, GridSize), random.Next(0, GridSize));
        }

        private void SnakeGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    snake.Clear();
                    snake.Add(new Point(GridSize / 2, GridSize / 2));
                    food = GetRandomFoodPosition();
                    snakeDirection = Direction.Right;
                    isGameOver = false;
                    gameTimer.Start();
                    Invalidate();
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (snakeDirection != Direction.Down)
                            snakeDirection = Direction.Up;
                        break;
                    case Keys.Down:
                        if (snakeDirection != Direction.Up)
                            snakeDirection = Direction.Down;
                        break;
                    case Keys.Left:
                        if (snakeDirection != Direction.Right)
                            snakeDirection = Direction.Left;
                        break;
                    case Keys.Right:
                        if (snakeDirection != Direction.Left)
                            snakeDirection = Direction.Right;
                        break;
                }
            }
        }

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}


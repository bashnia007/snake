using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Snake
{
    public class GameManager
    {
        #region Privates

        private Timer _timer;
        private bool _continue;
        #endregion
        #region Properies
        public int MapSize { get; set; }
        public Direction Direction { get; set; }

        public Queue<Coords> Snake { get; set; }
        public Coords Food { get; set; }
        public int Speed { get; set; }

        public int Score { get; set; }
        #endregion
        public void Initialize()
        {
            Snake = new Queue<Coords>();
            Snake.Enqueue(new Coords(1, 1));
            Snake.Enqueue(new Coords(2, 1));
            Snake.Enqueue(new Coords(3, 1));

            Score = 0;

            WriteMap();
            CreateFood();
            _timer = new Timer();
            switch (Speed)
            {
                case 1:
                    _timer.Interval = 1000;
                    break;
                case 2:
                    _timer.Interval = 500;
                    break;
                case 3:
                    _timer.Interval = 300;
                    break;
                case 4:
                    _timer.Interval = 100;
                    break;
                default:
                    return;
            }
            
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            Direction = Direction.Right;
            _continue = true;
            Console.CursorVisible = false;
            while (_continue)
            {
                Listen();
            }
        }

        private void WriteMap()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Console.Write('.');
                }
                Console.WriteLine();
            }
            foreach (var coords in Snake)
            {
                Console.SetCursorPosition(coords.X, coords.Y);
                Console.Write('@');
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Move();
        }

        public void Listen()
        {
            var result = Console.ReadKey(true);
            if (result.Key == ConsoleKey.LeftArrow && Direction != Direction.Right)
            {
                Direction = Direction.Left;
            }
            if (result.Key == ConsoleKey.RightArrow && Direction != Direction.Left)
            {
                Direction = Direction.Right;
            }
            if (result.Key == ConsoleKey.UpArrow && Direction != Direction.Down)
            {
                Direction = Direction.Up;
            }
            if (result.Key == ConsoleKey.DownArrow && Direction != Direction.Up)
            {
                Direction = Direction.Down;
            }
        }
        
        private void Move()
        {
            var oldHead = Snake.Last();
            var newHead = new Coords();
            switch (Direction)
            {
                case Direction.Right:
                    newHead = new Coords(oldHead.X + 1, oldHead.Y);
                    break;
                case Direction.Left:
                    newHead = new Coords(oldHead.X - 1, oldHead.Y);
                    break;
                case Direction.Down:
                    newHead = new Coords(oldHead.X, oldHead.Y + 1);
                    break;
                case Direction.Up:
                    newHead = new Coords(oldHead.X, oldHead.Y - 1);
                    break;
            }
            Snake.Enqueue(newHead);
            if (!IsValid(newHead))
            {
                GameOver();
                return;
            }
            Console.SetCursorPosition(newHead.X, newHead.Y);
            Console.Write('@');
            if (newHead.X == Food.X && newHead.Y == Food.Y)
            {
                Score++;
                CreateFood();
            }
            else
            {
                var tall = Snake.Dequeue();
                Console.SetCursorPosition(tall.X, tall.Y);
                Console.Write('.');
            }
        }

        private bool IsValid(Coords head)
        {
            if (head.X < 0 || head.Y < 0 || head.X >= MapSize || head.Y >= MapSize) return false;
            if (Snake.Count(s => s.X == head.X && s.Y == head.Y) > 1) return false;
            return true;
        }
        
        private void GameOver()
        {
            _timer.Stop();
            _timer.Dispose();
            _continue = false;
            Console.Clear();
            Console.WriteLine("Игра окончена");
            Console.WriteLine($"Ваш результат: {Score}");
            Console.ReadLine();
            Console.ReadLine();
        }

        private void CreateFood()
        {
            var rnd = new Random();
            while (true)
            {
                int foodX = rnd.Next(MapSize);
                int foodY = rnd.Next(MapSize);
                try
                {
                    if (Snake.All(s => s.X != foodX && s.Y != foodY))
                    {
                        Food = new Coords(foodX, foodY);
                        Console.SetCursorPosition(foodX, foodY);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('$');
                        Console.ResetColor();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.ToString());
                }
            }
            WriteScore();
        }

        private void WriteScore()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(MapSize + 10, 29);
            Console.Write("Результат: " + Score);
            Console.ResetColor();
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Down,
        Up
    }

    public struct Coords
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

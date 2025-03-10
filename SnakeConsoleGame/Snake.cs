using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Предоставляет консольную версию игры "Змейка".
    /// </summary>
    internal class Snake
    {
        /*
            Параметр, определяющий текущее направление змейки
            0: Вверх
            1: Вправо
            2: Вниз
            3: Влево
        */
        private int _direction;
        private bool _snakeIsAlive;
        private int _score;
        private char[,] _field;
        private Queue<Point> _snake;
        private Point _currentHead;
        private Point _apple;
        private int _speed;
        private int _fieldLen;
        private int _horizontal;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Snake"/>, с указанными
        /// вертикальной и горизонтальной размерностями игрового поля
        /// и заданной скоростью змейки, чем меньше значение, тем быстрее змейка.
        /// </summary>
        /// <param name="hor">Размер поля игры по горизонтали.</param>
        /// <param name="vert">Размер поля игры по вертикали.</param>
        /// <param name="speed">Скорость змейки, чем меньше значение тем быстрее змейка.</param>
        public Snake(int hor = 10, int vert = 10, int speed = 10)
        {
            if (hor < 10) hor = 10;
            if (vert < 10) vert = 10;
            _field = new char[vert + 2, hor + 2];
            _snake = new Queue<Point>();
            _direction = 1;
            _snakeIsAlive = true;
            _score = 0;
            _speed = speed * 10;
            _fieldLen = (_field.GetLength(0) - 2) * (_field.GetLength(1) - 2);
            _horizontal = hor;
        }

        /// <summary>
        /// Создает поле по заданным значениям ширины и высоты.
        /// </summary>
        private void SetUpField()
        {
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    _field[i, j] = ' ';
                }
            }

            for (int i = 0; i < _field.GetLength(0); i++)
            {
                _field[i, 0] = '|';
                _field[i, _field.GetLength(1) - 1] = '|';
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    _field[0, j] = '-';
                    _field[_field.GetLength(0) - 1, j] = '-';
                }
            }
        }

        /// <summary>
        /// Асинхронно генерирует кадры для игры.
        /// </summary>
        /// <returns>Задача, которая отрисовывает готовое изображение в консоли.</returns>
        private async Task GenerateFrameAsync()
        {
            await Task.Run(() =>
            {
                Console.Clear();
                StringBuilder sb = new StringBuilder();
                string spaces = new string(' ', _horizontal / 2 - 2);
                sb.Append($"{spaces}Score: {_score}\n");
                for (int i = 0; i < _field.GetLength(0); i++)
                {
                    for (int j = 0; j < _field.GetLength(1); j++)
                    {
                        sb.Append(_field[i, j]);
                    }
                    sb.Append('\n');
                }
                Console.Write($"{sb}");
            });
        }

        /// <summary>
        /// Добавляет змейку в изначальное положение на поле.
        /// </summary>
        private void AddSnake()
        {
            for (int i = 3; i < 10; i++)
            {
                _currentHead = new Point(i, 6);
                _snake.Enqueue(_currentHead);
            }
        }

        /// <summary>
        /// Поиск случайной свободной точки на игровом поле.
        /// </summary>
        /// <returns>Точка, не занятая змейкой.</returns>
        private Point GetRandomEmptyPoint()
        {
            Random random = new Random();
            int EmptyPoints = _fieldLen - _snake.Count;
            int RandomIndex = random.Next(0, EmptyPoints);

            for (int i = 1; i < _field.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < _field.GetLength(1) - 1; j++)
                {
                    if (_field[i, j] == ' ')
                    {
                        if (RandomIndex == 0)
                            return new Point(i, j);
                        else
                            RandomIndex--;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Меняет координаты точки местами.
        /// </summary>
        /// <param name="point">Точка, которую нужно изменить.</param>
        /// <returns>Новая точка с измененными координатами.</returns>
        private Point SwitchCoordinates(Point point)
        {
            return new Point(point.Y, point.X);
        }

        /// <summary>
        /// Помещает на игровое поле яблоко.
        /// </summary>
        private void SpawnApple()
        {
            _apple = SwitchCoordinates(GetRandomEmptyPoint());
            _field[_apple.Y, _apple.X] = '*';
        }

        /// <summary>
        /// Добавление змейки на поле в изначальное положение.
        /// </summary>
        private void AddSnakeOnField()
        {
            foreach (Point point in _snake)
            {
                _field[point.Y, point.X] = 'O';
            }
        }

        /// <summary>
        /// Сдвиг змейки на одно деление вправо.
        /// </summary>
        private void MoveRight()
        {
            if (_currentHead.X + 1 > _field.GetLength(1) - 2)
                _currentHead.X = _currentHead.X / (_field.GetLength(1) - 2);
            else
                _currentHead.X += 1;
        }

        /// <summary>
        /// Сдвиг змейки на одно деление влево.
        /// </summary>
        private void MoveLeft()
        {
            if (_currentHead.X - 1 < 1 && _direction != 1)
                _currentHead.X = _field.GetLength(1) - 2;
            else
                _currentHead.X -= 1;
        }

        /// <summary>
        /// Сдвиг змейки на одно деление вниз.
        /// </summary>
        private void MoveDown()
        {
            if (_currentHead.Y + 1 > _field.GetLength(0) - 2)
                _currentHead.Y = _currentHead.Y / (_field.GetLength(0) - 2);
            else
                _currentHead.Y += 1;
        }

        /// <summary>
        /// Сдвиг змейки на одно деление вверх.
        /// </summary>
        private void MoveUp()
        {
            if (_currentHead.Y - 1 < 1)
                _currentHead.Y = _field.GetLength(0) - 2;
            else
                _currentHead.Y -= 1;
        }

        /// <summary>
        /// Вызов метода сдвига змейки в заданном направлении.
        /// </summary>
        private void GetNewCurrentHead()
        {
            switch (_direction)
            {
                case 0: MoveUp(); break;
                case 1: MoveRight(); break;
                case 2: MoveDown(); break;
                case 3: MoveLeft(); break;
            }
        }

        /// <summary>
        /// Проверка, что головная точка змейки совпадает с точкой тела змейки.
        /// </summary>
        /// <param name="point">Точка, с которой необходимо сравнить точки тела змейки.</param>
        /// <returns>Если координаты точек совпадают, то возвращает <c>true</c>, иначе возвращает <c>false</c>.</returns>
        private bool HasSamePoint(Point point)
        {
            Point[] points = _snake.ToArray();

            int length = points.Length - 1;

            for (int i = 0; i < length; i++)
            {
                if (point.X == points[i].X && point.Y == points[i].Y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка, что координаты головной точки змейки совпадает с координатами точки яблока на поле.
        /// </summary>
        /// <returns>Возвращает <c>true</c> если координаты совпадают, и <c>false</c>, если не совпадают.</returns>
        private bool IsAppleEaten()
        {
            if (_currentHead.X == _apple.X && _currentHead.Y == _apple.Y)
                return true;
            return false;
        }

        /// <summary>
        /// Запускает движение змейки, пока она жива.
        /// </summary>
        private async void MoveAsync()
        {
            while (_snakeIsAlive)
            {
                GetNewCurrentHead();
                _snake.Enqueue(_currentHead);

                if (IsAppleEaten())
                {
                    _score += 100;
                    SpawnApple();
                }
                else
                {
                    Point prev = _snake.Dequeue();
                    _field[prev.Y, prev.X] = ' ';
                }
                _field[_currentHead.Y, _currentHead.X] = 'O';

                if (HasSamePoint(_currentHead))
                    _snakeIsAlive = false;

                await Task.Delay(_speed);
            }
        }

        /// <summary>
        /// Отслеживает нажатия клавиш, для смены направления змейки.
        /// </summary>
        private async void ChangeDirectionAsync()
        {
            while (_snakeIsAlive)
            {
                if (Console.KeyAvailable)
                {
                    string keyPressed = Console.ReadKey(true).Key.ToString();
                    switch (keyPressed)
                    {
                        case "UpArrow": _direction = _direction == 2 ? 2 : 0; break;
                        case "RightArrow": _direction = _direction == 3 ? 3 : 1; break;
                        case "DownArrow": _direction = _direction == 0 ? 0 : 2; break;
                        case "LeftArrow": _direction = _direction == 1 ? 1 : 3; break;
                        default: break;
                    }
                }
                await Task.Delay(1);
            }
        }

        /// <summary>
        /// Запускает консольную версию игры "Змейка".
        /// </summary>
        /// <returns>Возвращает объект <c>Task</c></returns>
        public async Task RunAsync()
        {
            SetUpField();
            AddSnake();
            AddSnakeOnField();
            SpawnApple();

            ChangeDirectionAsync();
            MoveAsync();

            while (_snakeIsAlive)
            {
                await GenerateFrameAsync();
                await Task.Delay(10);
            }

            if (_score == (_fieldLen - 7) * 100)
                Console.WriteLine("\tПобеда!");
            else
                Console.WriteLine("\tПоражение!");
        }
    }
}

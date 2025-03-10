using ConsoleApp2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnakeConsoleGame
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            /*
                Параметры поля:
                    horizontal: размер поля по горизонтали
                    vertical:   размер поля по вертикали
                    speed:      скорость змейки (чем меньше скорость, тем быстрее змейка)
            */
            int horizontal = 20;
            int vertical = 15;
            int speed = 7;

            
            // Параметры из командной строки

            if (args.Length == 3)
            {
                try
                {
                    horizontal = int.Parse(args[0]);
                    vertical = int.Parse(args[1]);
                    speed = int.Parse(args[2]);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка: Неверный формат входных данных");
                    Console.ResetColor();
                    return 1;
                }
            }
            

            while (true)
            {
                Snake snake = new Snake(horizontal, vertical, speed);
                await snake.RunAsync();

                string key;

                Console.WriteLine("Нажмите R для перезапуска, или Esc для выхода");
                while (true)
                {
                    key = Console.ReadKey(true).Key.ToString();
                    if (key == "R" || key == "Escape" || key == "К")
                        break;
                }

                if (key == "Escape")
                    break;
            }

            return 0;
        }
    }
}

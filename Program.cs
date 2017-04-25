using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Привет! Это змейка, созданная мною в перерыве в работе");
            Console.WriteLine("Выбери уровень сложности и ткни ENTER:");
            Console.WriteLine("1: Тормоз");
            Console.WriteLine("2: Середнячок");
            Console.WriteLine("3: Профи");
            Console.WriteLine("4: ФЛЕШ");
            var line = Console.ReadLine();
            int choise;
            if (int.TryParse(line, out choise) && choise < 5 && choise > 0)
            {
                Console.Clear();
                var game = new GameManager
                {
                    MapSize = 15,
                    Speed = choise
                };
                game.Initialize();
            }
        }

        static void Menu()
        {
            
        }
    }
}

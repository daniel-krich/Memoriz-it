using System;

namespace MemoryCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string result;
            do
            {
                CardMemoryGame game = new CardMemoryGame();

                game.Play();

                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.Write("Do you want to play again (y/n): ");
                Console.ResetColor();

                result = Console.ReadLine();

            } while (result.ToLower() == "y");
        }
    }
}

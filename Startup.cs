using MemoryCardGame.Core;
using MemoryCardGame.IGame;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame
{
    public static class Startup
    {
        public static void Run()
        {
            string input;
            do
            {
                GameServices.Configure()
                .GetRequiredService<IGameController>()
                    .Play();


                Utils.ColoredMessage("\nDo you wish to play again? (y / n)", ConsoleColor.DarkMagenta);
                input = Console.ReadLine();

            } while (input.ToLower() == "y");
        }
    }
}

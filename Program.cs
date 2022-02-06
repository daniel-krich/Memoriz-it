using System;
using Microsoft.Extensions.DependencyInjection;
using MemoryCardGame.Core;
using MemoryCardGame.Game;
using MemoryCardGame.IGame;

namespace MemoryCardGame
{
    class Program
    {
        static void Main(string[] args) =>
            Startup.Run();
            
    }
}

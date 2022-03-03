using System;
using Microsoft.Extensions.DependencyInjection;
using MemorizIt.Core;
using MemorizIt.Game;
using MemorizIt.Contracts;

namespace MemorizIt
{
    class Program
    {
        static void Main(string[] args) =>
            Startup.Run();
            
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemorizIt.Contracts;
using MemorizIt.Game;
using MemorizIt.UI;

namespace MemorizIt.Core
{
    public static class GameServices
    {

        public static IServiceProvider Configure()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ICardCollection, CardCollection>();
            services.AddSingleton<IPlayerCollection, PlayerCollection>();
            services.AddSingleton<IDisplayer, Displayer>();
            services.AddSingleton<IGameController, GameController>();
            return services.BuildServiceProvider();
        }
    }
}

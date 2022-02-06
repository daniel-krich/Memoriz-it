using MemorizIt.Core;
using MemorizIt.IGame;
using MemorizIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorizIt.Game
{
    public class PlayerCollection : IPlayerCollection
    {
        private int _currentPlayerIndex;
        public int CurrentPlayerIndex { get => _currentPlayerIndex; }
        public List<PlayerModel> Players { get; set; }

        public PlayerCollection()
        {
            Players = new List<PlayerModel>();
        }

        public void InsertPlayer(string name)
        {
            Players.Add(new PlayerModel(name));
            _currentPlayerIndex = Utils.Random.Next(Players.Count);
        }

        public PlayerModel PlayerWithHighestScore()
        {
            return Players.Select(n => n).Max();
        }

        public int NextPlayerTurn()
        {
            if (Players is not null && (_currentPlayerIndex + 1) < Players.Count)
            {
                return ++_currentPlayerIndex;
            }
            return _currentPlayerIndex = 0;
        }
    }
}

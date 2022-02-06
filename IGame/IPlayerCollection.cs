using MemoryCardGame.Models;
using System.Collections.Generic;

namespace MemoryCardGame.IGame
{
    public interface IPlayerCollection
    {
        int CurrentPlayerIndex { get; }
        List<PlayerModel> Players { get; set; }

        void InsertPlayer(string name);
        int NextPlayerTurn();
        PlayerModel PlayerWithHighestScore();
    }
}
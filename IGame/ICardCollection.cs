using MemorizIt.Models;

namespace MemorizIt.IGame
{
    public interface ICardCollection
    {
        CardModel[,] Cards { get; set; }

        bool IsAllCardsFlipped();
        void CreateCardStack(int rows, int columns);
    }
}
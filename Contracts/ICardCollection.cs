using MemorizIt.Models;

namespace MemorizIt.Contracts
{
    public interface ICardCollection
    {
        CardModel[,] Cards { get; set; }

        bool IsAllCardsFlipped();
        void CreateCardStack(int rows, int columns);
    }
}
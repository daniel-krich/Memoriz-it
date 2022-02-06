using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryCardGame.Core;
using MemoryCardGame.IGame;
using MemoryCardGame.Models;

namespace MemoryCardGame.Game
{
    public class CardCollection : ICardCollection
    {
        private static string CardSymbols { get => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
        public CardModel[,] Cards { get; set; }

        public CardCollection()
        {

        }

        public bool IsAllCardsFlipped()
        {
            foreach(var card in Cards)
            {
                if (!card.IsFlipped)
                    return false;
            }
            return true;
        }

        public void CreateCardStack(int rows, int columns)
        {
            if (rows * columns % 2 != 0) columns++;
            if (rows > 10) rows = 10;
            if (columns > 10) columns = 10;

            Cards = new CardModel[rows, columns];
            char[] CharactersDoubles = RandomCharacters(rows * columns / 2);
            int charIndex = 0;
            int iterations = 0;
            while (charIndex < CharactersDoubles.Length)
            {
                int[] rand_card_1 = { Utils.Random.Next(0, rows), Utils.Random.Next(0, columns) };
                int[] rand_card_2 = { Utils.Random.Next(0, rows), Utils.Random.Next(0, columns) };
                if (rand_card_1[0] != rand_card_2[0] && rand_card_1[1] != rand_card_2[1])
                {
                    if (Cards[rand_card_1[0], rand_card_1[1]] == null && Cards[rand_card_2[0], rand_card_2[1]] == null)
                    {
                        Cards[rand_card_1[0], rand_card_1[1]] = new CardModel(CharactersDoubles[charIndex]);
                        Cards[rand_card_2[0], rand_card_2[1]] = new CardModel(CharactersDoubles[charIndex]);
                        charIndex++;
                    }
                }


                /*
                 * If the random int loop passes the iteration limit and goes to an endless loop to shuffle the cards, we will just go through...
                 * ...all the indexes and search for null indexes and assign them a value to exit the while loop.
                 */
                if (++iterations > rows * columns * 10)
                {

                    for (int irows = 0; irows < Cards.GetLength(0); irows++)
                    {
                        for (int icolumns = 0; icolumns < Cards.GetLength(1); icolumns++)
                        {
                            if (Cards[irows, icolumns] == null)
                            {
                                Cards[irows, icolumns] = new CardModel(CharactersDoubles[charIndex]);
                                if (iterations % 2 == 0)
                                    charIndex++;
                                iterations++;
                            }
                        }
                    }
                }
            }
        }

        private char[] RandomCharacters(int length)
        {
            char[] randomSeq = new char[length];
            for (int i = 0; i < randomSeq.Length; i++)
            {
                randomSeq[i] = CardSymbols[Utils.Random.Next(0, CardSymbols.Length)];
            }
            return randomSeq;
        }
    }
}

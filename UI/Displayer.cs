using MemoryCardGame.IGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame.UI
{
    public class Displayer : IDisplayer
    {
        private ICardCollection _cardCollection;
        private IPlayerCollection _playerCollection;

        private int cWidth = 9, cHeight = 7, cSpace = 4;
        private ConsoleColor cColor = ConsoleColor.White;
        public Displayer(ICardCollection cardCollection, IPlayerCollection playerCollection)
        {
            _cardCollection = cardCollection;
            _playerCollection = playerCollection;
        }

        /// <summary>
        /// prints current player name and some stats to console.
        /// </summary>
        private void PrintCurrentPlayerWithStats()
        {
            Console.Write("Player: {0}", _playerCollection.Players[_playerCollection.CurrentPlayerIndex].Name);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" (pairs {0})", _playerCollection.Players[_playerCollection.CurrentPlayerIndex].Score);
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Prints all the cards to the console.
        /// Cards that are not flipped will be shown as "?".
        /// Otherwise will show their symbolic value.
        /// </summary>
        public void UpdateFrame()
        {
            Console.Clear();
            PrintCurrentPlayerWithStats();

            for (int row = 0; row < _cardCollection.Cards.GetLength(0); row++)
            {
                for (int height = 0; height < cHeight; height++)
                {
                    for (int column = 0; column < _cardCollection.Cards.GetLength(1); column++)
                    {

                        /*
                         * insert spaces to make gap between the cards.
                         */
                        for (int spaces = 0; spaces < cSpace; spaces++)
                        {
                            Console.Write(" ");
                        }

                        for (int width = 0; width < cWidth; width++)
                        {
                            /*
                             * Hide the card with blank spaces, and continue the next iteration.
                             */
                            if (_cardCollection.Cards[row, column].IsHidden)
                            {
                                Console.Write(" ");
                                continue;
                            }


                            /*
                             * if we are at the middle of the card (width and height), then insert the value of the card if it's flipped...
                             * ...or the "?" symbol if the card is not flipped.                          
                             */
                            if (cWidth / 2 == width && cHeight / 2 == height)
                            {

                                if (_cardCollection.Cards[row, column].IsFlipped)
                                {
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(_cardCollection.Cards[row, column].Symbol);
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("?");
                                }
                                Console.ResetColor();
                            }
                            /*
                             * Top left corner of the card.
                             * display row if card is not flipped.
                             */
                            else if (height == 0 && width == 0 && !_cardCollection.Cards[row, column].IsFlipped)
                            {
                                Console.BackgroundColor = cColor;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(row);
                                Console.ResetColor();
                            }
                            /*
                             * bottom right corner of the card.
                             * display column if card is not flipped.
                             */
                            else if (height == cHeight - 1 && width == cWidth - 1 && !_cardCollection.Cards[row, column].IsFlipped)
                            {
                                Console.BackgroundColor = cColor;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(column);
                                Console.ResetColor();
                            }
                            /*
                             * rest of the cards body, blank spaces with the color specified (default white)
                             */
                            else
                            {
                                Console.BackgroundColor = cColor;
                                Console.Write(" ");
                                Console.ResetColor();
                            }
                        }

                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}

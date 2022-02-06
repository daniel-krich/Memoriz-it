using MemoryCardGame.Core;
using MemoryCardGame.IGame;
using MemoryCardGame.Models;
using MemoryCardGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCardGame.Game
{
    public class GameController : IGameController
    {
        private static string GameTitle { get; } = "Memoriz'it";
        private DateTime _gameStart;
        private IPlayerCollection _playerCollection;
        private ICardCollection _cardCollection;
        private IDisplayer _displayer;

        public GameController(IPlayerCollection playerCollection, ICardCollection cardCollection, IDisplayer displayer)
        {
            _playerCollection = playerCollection;
            _cardCollection = cardCollection;
            _displayer = displayer;
        }


        public void Play()
        {
            InitWnd();
            ConfigureGame();
            ConfigurePlayers();

            Utils.ColoredMessage("\nGame started.", ConsoleColor.DarkGreen);
            Thread.Sleep(1000);

            HandleGame();
        }

        private void HandleGame()
        {
            _gameStart = DateTime.UtcNow;
            while (!_cardCollection.IsAllCardsFlipped())
            {
                _displayer.UpdateFrame();
                MakeMove();
            }

            Console.Clear();

            PlayerModel winner = _playerCollection.PlayerWithHighestScore();

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nWinner! {0} flipped {1} paires ({2} fails)", winner.Name, winner.Score, winner.Fails);
            Console.ResetColor();

            foreach (PlayerModel others in _playerCollection.Players)
            {
                if (others == winner)
                    continue;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n{0} flipped {1} paires ({2} fails)", others.Name, others.Score, others.Fails);
                Console.ResetColor();
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nPlaytime: {0}:{1}", Convert.ToInt32((DateTime.UtcNow - _gameStart).TotalMinutes).ToString().PadLeft(2, '0'), Convert.ToInt32(((DateTime.UtcNow - _gameStart).TotalSeconds % 60)).ToString().PadLeft(2, '0'));
            Console.ResetColor();
        }

        private void MakeMove()
        {
            CardModel FirstCard = ChooseCardInput("Enter \"Row\" and \"Column\" for the first card (e.g: 0 0): ");
            FirstCard.Flip();

            _displayer.UpdateFrame();

            CardModel SecondCard = ChooseCardInput("Enter \"Row\" and \"Column\" for the second card (e.g: 0 0): ");
            SecondCard.Flip();

            if (FirstCard != SecondCard)
            {
                _displayer.UpdateFrame();

                if (FirstCard.Symbol == SecondCard.Symbol)
                {
                    FirstCard.Hide();
                    SecondCard.Hide();
                    _playerCollection.Players[_playerCollection.CurrentPlayerIndex].AddScore(1);

                    if (!_cardCollection.IsAllCardsFlipped())
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Goodjob, you've earned 1 point and another turn.\n");
                        Console.ResetColor();
                    }
                }
                else
                {
                    _playerCollection.Players[_playerCollection.CurrentPlayerIndex].Fail();

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Cards don't match, good luck next time.\n");
                    Console.ResetColor();

                    /*
                     * Flip the cards back if they are not equal.
                     */
                    FirstCard.Flip();
                    SecondCard.Flip();


                    _playerCollection.NextPlayerTurn();
                }
                if (!_cardCollection.IsAllCardsFlipped())
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Unpause in 2.5 sec");
                    Console.ResetColor();
                    Thread.Sleep(2500);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Game finished, waiting for results...\n");
                    Console.ResetColor();
                    Thread.Sleep(2500);
                }
            }
            else
                /*
                 * if both instances are the same, we're gonna flip only once.
                 */
                FirstCard.Flip();
        }

        private CardModel ChooseCardInput(string text, string delimiter = " ")
        {
            Thread.Sleep(250);

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(text);
            Console.ResetColor();
            string[] result = Console.ReadLine().Split(delimiter);

            int row;
            int column;
            try
            {
                row = Convert.ToInt32(result[0]);
                column = Convert.ToInt32(result[1]);
            }
            catch (Exception)
            {
                return ChooseCardInput(text, delimiter);
            }

            if (_cardCollection.Cards.GetLength(0) <= row || _cardCollection.Cards.GetLength(1) <= column)
                return ChooseCardInput(text, delimiter);

            CardModel CurrentCard = _cardCollection.Cards[row, column];

            /*
             * don't let players flip cards that already been flipped.
             * recursion occure if card is already flipped.
             */
            if (CurrentCard.IsFlipped)
                return ChooseCardInput(text, delimiter);

            Thread.Sleep(250);

            return CurrentCard;

        }

        private void InitWnd()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                Utils.ShowWindow(Utils.GetConsoleWindow(), 3);
            }
            Console.Title = GameTitle;
            Console.Clear();
        }

        private void ConfigureGame()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Enter the desired Rows and Columns (e.g 3x4, max config 10x10): ");
            Console.ResetColor();
            string[] config = Console.ReadLine().Split("x");

            int rows = 0;
            int columns = 0;

            try
            {
                rows = Convert.ToInt32(config[0]);
                columns = Convert.ToInt32(config[1]);
            }
            catch (Exception)
            {
                ConfigureGame();
                return;
            }

            _cardCollection.CreateCardStack(rows, columns);
        }

        private void ConfigurePlayers()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("\nEnter the amount of players: ");
            Console.ResetColor();

            int player_num = 0;

            try
            {
                player_num = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                ConfigurePlayers();
                return;
            }

            for (int i = 0; i < player_num; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("\nEnter a name for player {0}: ", i + 1);
                Console.ResetColor();
                string name = Console.ReadLine();
                _playerCollection.InsertPlayer(name);
            }
        }
    }
}

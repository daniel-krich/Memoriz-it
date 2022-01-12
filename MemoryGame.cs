using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace MemoryCardGame
{
    class CardMemoryGame
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static Random random = new Random();
        private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private Card[,] MatrixMemoryGame;
        private Player[] Players;

        private int CurrentPlayerMove;
        private int card_width, card_height, space_between_cards;
        private ConsoleColor cards_color;

        private DateTime GameStart;

        /// <summary>
        /// Constructor.
        /// Creates the memory game with some params that control how the cards would look in the console (width, height, spaces).
        /// </summary>
        /// <param name="card_width"> each card width "visual" in the console, has to be odd </param>
        /// <param name="card_height"> each card height "visual" in the console, has to be odd </param>
        /// <param name="space_between_cards"> spaces between the different cards </param>
        /// <param name="cards_color"> ConsoleColor type, background color of the cards</param>
        public CardMemoryGame(int card_width = 8, int card_height = 6, int space_between_cards = 3, ConsoleColor cards_color = ConsoleColor.White)
        {
            if (card_width % 2 == 0)
                card_width++;

            if (card_height % 2 == 0)
                card_height++;

            this.card_width = card_width;
            this.card_height = card_height;
            this.space_between_cards = space_between_cards;
            this.cards_color = cards_color;

        }

        /// <summary>
        /// This function starts the game.
        /// Does all of the game logic.
        /// Also determines the winner of the game.
        /// </summary>
        public void Play()
        {
            /*
             * Check if we're running on windows, and resize the console to prevent visual bugs.
             */
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                ShowWindow(GetConsoleWindow(), 3);
            }

            Console.Clear();

            Console.Title = "Cards memory game";

            ConfigureGame();
            ConfigurePlayers();

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Game has started");
            Console.ResetColor();

            GameStart = DateTime.UtcNow;

            Thread.Sleep(1000);

            while (!IsGameFinished())
            {
                PrintTable();
                MakeMove();
            }

            PrintTable();

            Player winner = Players.Select(n => n).Max();

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nWinner! {0} flipped {1} paires ({2} fails)", winner.Name, winner.Score, winner.Fails);
            Console.ResetColor();

            foreach (Player others in Players)
            {
                if (others == winner)
                    continue;
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\n{0} flipped {1} paires ({2} fails)", others.Name, others.Score, others.Fails);
                Console.ResetColor();
            }

            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nPlaytime: {0}:{1}", Convert.ToInt32((DateTime.UtcNow - GameStart).TotalMinutes).ToString().PadLeft(2, '0'), Convert.ToInt32(((DateTime.UtcNow - GameStart).TotalSeconds % 60)).ToString().PadLeft(2, '0'));
            Console.ResetColor();

        }

        /// <summary>
        /// ReadLine Input.
        /// Let the client configure the game Rows and Columns.
        /// </summary>
        public void ConfigureGame()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("How many rows and columns the card game will have (example \"4x6\"): ");
            Console.ResetColor();
            string[] config = Console.ReadLine().Split("x");

            int rows = 0;
            int columns = 0;

            try
            {
                rows = Convert.ToInt32(config[0]);
                columns = Convert.ToInt32(config[1]);
            }
            catch(Exception)
            {
                ConfigureGame();
            }

            if (rows * columns % 2 != 0)
                columns++;

            MatrixMemoryGame = GenerateMartixTable(rows, columns);
        }

        /// <summary>
        /// ReadLine Input.
        /// Lets the players make moves and flip cards.
        /// Compare 2 cards that has been flipped, if the symbolic value is equal then add +1 point and another turn to the current player.
        /// Otherwise move to the next player.
        /// </summary>
        public void MakeMove()
        {

            Card FirstCard = ChooseCardInput("Enter \"Row\" and \"Column\" for the first card (e.g: 0 0): ");
            FirstCard.Flip();

            PrintTable();

            Card SecondCard = ChooseCardInput("Enter \"Row\" and \"Column\" for the second card (e.g: 0 0): ");
            SecondCard.Flip();

            if (FirstCard != SecondCard)
            {
                PrintTable();

                if (FirstCard.symbol == SecondCard.symbol)
                {
                    FirstCard.Hide();
                    SecondCard.Hide();
                    Players[CurrentPlayerMove].AddScore(1);

                    if (!IsGameFinished())
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Goodjob, you've earned 1 point and another turn.\n");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Players[CurrentPlayerMove].Fail();

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Cards don't match, good luck next time.\n");
                    Console.ResetColor();

                    /*
                     * Flip the cards back if they are not equal.
                     */
                    FirstCard.Flip();
                    SecondCard.Flip();


                    /*
                     * Each time increasing CurrentPlayerMove by 1 to let the next player play.
                     * if CurrentPlayerMove is greater then the max index - we reset it to 0.
                     */
                    if (++CurrentPlayerMove == Players.Length)
                        CurrentPlayerMove = 0;
                }
                if (!IsGameFinished())
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

        /// <summary>
        /// ReadLine Input
        /// Gives the current player an option to choose a card.
        /// </summary>
        /// <param name="text">Text that will be shown in the console with Console.Write</param>
        /// <param name="delimiter">ReadLine input delimiter to split the params</param>
        /// <returns>returns an instance to that specific card</returns>
        public Card ChooseCardInput(string text, string delimiter = " ")
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

            if (MatrixMemoryGame.GetLength(0) <= row || MatrixMemoryGame.GetLength(1) <= column)
                return ChooseCardInput(text, delimiter);

            Card CurrentCard = MatrixMemoryGame[row, column];

            /*
             * don't let players flip cards that already been flipped.
             * recursion occure if card is already flipped.
             */
            if (CurrentCard.isFlipped)
                return ChooseCardInput(text, delimiter);

            Thread.Sleep(250);

            return CurrentCard;

        }

        /// <summary>
        /// Looping through all the cards and searching for unflipped cards.
        ///</summary>
        /// <returns>True if all cards are flipped (Game finished), False otherwise (Not finished).</returns>
        public bool IsGameFinished()
        {
            foreach(Card c in MatrixMemoryGame)
            {
                if (c.isFlipped == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ReadLine Input.
        /// Ask the client about the amount of players.
        /// Also naming the users.
        /// </summary>
        public void ConfigurePlayers()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("Enter the amount of players: ");
            Console.ResetColor();

            int player_num = 0;

            try
            {
                player_num = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception)
            {
                ConfigurePlayers();
            }

            Players = new Player[player_num];

            for (int i = 0; i < Players.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("Enter a name for player {0}: ", i + 1);
                Console.ResetColor();
                string name = Console.ReadLine();
                Players[i] = new Player(name);
            }
        }

        /// <summary>
        /// prints current player name and some stats to console.
        /// </summary>
        private void PrintCurrentPlayerWithStats()
        {
            Console.Write("Player: {0}", Players[CurrentPlayerMove].Name);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" (pairs {0})", Players[CurrentPlayerMove].Score);
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Prints all the cards to the console.
        /// Cards that are not flipped will be shown as "?".
        /// Otherwise will show their symbolic value.
        /// </summary>
        private void PrintTable()
        {
            Console.Clear();
            PrintCurrentPlayerWithStats();

            for (int row = 0; row < MatrixMemoryGame.GetLength(0); row++)
            {
                for (int height = 0; height < card_height; height++)
                {
                    for (int column = 0; column < MatrixMemoryGame.GetLength(1); column++)
                    {

                        /*
                         * insert spaces to make gap between the cards.
                         */
                        for (int spaces = 0; spaces < space_between_cards; spaces++)
                        {
                            Console.Write(" ");
                        }

                        for (int width = 0; width < card_width; width++)
                        {
                            /*
                             * Hide the card with blank spaces, and continue the next iteration.
                             */
                            if(MatrixMemoryGame[row, column].isHidden)
                            {
                                Console.Write(" ");
                                continue;
                            }


                            /*
                             * if we are at the middle of the card (width and height), then insert the value of the card...
                             * ...or the "?" symbol.
                             */
                            if (card_width / 2 == width && card_height / 2 == height)
                            {
                                    
                                if (MatrixMemoryGame[row, column].isFlipped)
                                {
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(MatrixMemoryGame[row, column].symbol);
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
                             */
                            else if(height == 0 && width == 0)
                            {
                                Console.BackgroundColor = cards_color;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(row);
                                Console.ResetColor();
                            }
                            /*
                             * bottom right corner of the card
                             */
                            else if (height == card_height-1 && width == card_width-1)
                            {
                                Console.BackgroundColor = cards_color;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(column);
                                Console.ResetColor();
                            }
                            /*
                             * rest of the cards body, blank spaces with the color specified (default white)
                             */
                            else
                            {
                                Console.BackgroundColor = cards_color;
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

        /// <summary>
        /// Generating the 2D martix table for the game.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns>2D Array of the Card class</returns>
        private Card[,] GenerateMartixTable(int rows, int columns)
        {
            Card[,] MatrixMemory = new Card[rows, columns];
            char[] CharactersDoubles = randomChars(rows * columns / 2);
            int charIndex = 0;
            int iterations = 0;
            while(charIndex < CharactersDoubles.Length)
            {
                int[] rand_card_1 = { random.Next(0, rows), random.Next(0, columns) };
                int[] rand_card_2 = { random.Next(0, rows), random.Next(0, columns) };
                if (rand_card_1[0] != rand_card_2[0] && rand_card_1[1] != rand_card_2[1])
                {
                    if (MatrixMemory[rand_card_1[0], rand_card_1[1]] == null && MatrixMemory[rand_card_2[0], rand_card_2[1]] == null)
                    {
                        MatrixMemory[rand_card_1[0], rand_card_1[1]] = new Card(CharactersDoubles[charIndex]);
                        MatrixMemory[rand_card_2[0], rand_card_2[1]] = new Card(CharactersDoubles[charIndex]);
                        charIndex++;
                    }
                }
                

                /*
                 * If the random int loop passes the iteration limit and goes to an endless loop to shuffle the cards, we will just go through...
                 * ...all the indexes and search for null indexes and assign them a value to exit the while loop.
                 */
                if(++iterations > rows * columns * 10)
                {
                    
                    for (int irows = 0; irows < MatrixMemory.GetLength(0); irows++)
                    {
                        for (int icolumns = 0; icolumns < MatrixMemory.GetLength(1); icolumns++)
                        {
                            if (MatrixMemory[irows, icolumns] == null)
                            {
                                MatrixMemory[irows, icolumns] = new Card(CharactersDoubles[charIndex]);
                                if (iterations % 2 == 0)
                                    charIndex++;
                                iterations++;
                            }
                        }
                    }
                }
            }

            return MatrixMemory;
        }

        /// <summary>
        /// returns a random sequence of chars, depends on the length param.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>random sequence of char[]</returns>
        private char[] randomChars(int length)
        {
            char[] randomSeq = new char[length];
            for (int i = 0; i < randomSeq.Length; i++)
            {
                randomSeq[i] = characters[random.Next(0, characters.Length)];
            }
            return randomSeq;
        }
    }
}

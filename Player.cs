using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame
{
    public class Player
    {
        public string Name;
        public int Score;
        public int Fails;
        public Player(string name)
        {
            Name = name;
            Score = 0;
            Fails = 0;
        }

        /// <summary>
        /// Adds a score to the user
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            Score += score;
        }

        /// <summary>
        /// override the ToString()
        /// </summary>
        /// <returns>returns player name instead of GetType()</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Increases the fails
        /// </summary>
        public void Fail()
        {
            Fails++;
        }
    }
}

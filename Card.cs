using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame
{
    public class Card
    {
        public char symbol;
        public bool isFlipped;
        public Card(char sym)
        {
            symbol = sym;
            isFlipped = false;
        }

        /// <summary>
        /// Flips the card every call.
        /// </summary>
        public void Flip()
        {
            isFlipped = !isFlipped;
        }
    }
}

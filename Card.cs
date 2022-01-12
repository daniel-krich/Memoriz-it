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
        public bool isHidden;

        /// <summary>
        /// init Card with a symbol
        /// </summary>
        /// <param name="sym">symbolic value for the card</param>
        public Card(char sym)
        {
            symbol = sym;
            isFlipped = false;
        }

        /// <summary>
        /// Hides the card
        /// </summary>
        public void Hide()
        {
            isHidden = true;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame.Models
{
    public class CardModel
    {

        public char Symbol { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsHidden { get; set; }

        public CardModel(char symbol)
        {
            Symbol = symbol;
        }
        

        public void Hide() => IsHidden = true;

        public void Flip() => IsFlipped = !IsFlipped;
    }
}
